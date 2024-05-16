using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Core.Extensions;
using Game.Model.Info.Authentication;
using Game.Model.Services.Authentication;
using Game.Model.Services.Connection;
using Nakama;
using R3;
using UnityEngine;
using UnityEngine.Events;

namespace Game.ViewModel.UI.Authentication
{
    public class AuthenticationPopupModel : IAuthenticationPopupModel
    {
        private readonly AuthenticationServices authenticationServices;
        private readonly AuthenticationsInfo authenticationsInfo;
        private readonly SessionDataProvider sessionDataProvider;
        private readonly CancellationTokenSource cancellationToken = new();
        private readonly IClient client;
        private readonly ReactiveProperty<AuthenticationPopupState> state = new(new AuthenticationPopupState());
        private readonly ReactiveProperty<AuthenticationService> authenticationId = new(AuthenticationService.None);

        private (string email, string password) inputData;
        private IAuthenticationResult authenticationResult;
        private Dictionary<string, Sprite> authenticationsCardsInfo;
        private bool connectionSuccess;
        
        public string AuthenticationId => authenticationId.Value.ToString();
        public ReadOnlyReactiveProperty<AuthenticationPopupState> State => state;
        public UnityEvent<string> AuthenticationMessageError { get; } = new();

        public AuthenticationPopupModel(AuthenticationServices authenticationServices,
            AuthenticationsInfo authenticationsInfo, IClient client, SessionDataProvider sessionDataProvider)
        {
            this.authenticationServices = authenticationServices;
            this.authenticationsInfo = authenticationsInfo;
            this.sessionDataProvider = sessionDataProvider;
            this.client = client;
        }

        public void Start()
        {
            Subscribe();
            if (HasSessionData(out var sessionData))
                AuthenticateRestore(sessionData);
        }

        private void Subscribe()
        {
            authenticationServices.OnAuthentication.AddListener(OnAuthenticationHandler);
            authenticationId.Subscribe(ResolveState);
            authenticationServices.AuthorizationProgress.Subscribe(ResolveState);
        }

        private void AuthenticateRestore(SessionData sessionData)
        {
            if (!sessionData.AuthenticationId.TryEnum(out AuthenticationService currentServiceId)) return;

            authenticationServices.AuthenticateRestoreAsync(client, sessionData.AuthToken, sessionData.RefreshToken,
                cancellationToken.Token).Forget();

            authenticationId.Value = currentServiceId;
        }

        public IReadOnlyDictionary<string, Sprite> GetAuthenticationsCardsInfo()
        {
            authenticationsCardsInfo = new Dictionary<string, Sprite>(authenticationServices.Services.Count);

            foreach (var service in authenticationServices.Services)
            {
                authenticationsCardsInfo[service.ID.ToString()] =
                    authenticationsInfo.TryGet(service.ID, out var serviceInfo)
                        ? serviceInfo.Icon
                        : authenticationsInfo.MockAuthenticationCardInfo.Icon;
            }

            return authenticationsCardsInfo;
        }

        public void SetAuthenticate(string serviceId, (string email, string password) inputData)
        {
            if (!serviceId.TryEnum(out AuthenticationService id)) return;

            this.inputData = inputData;
            authenticationId.Value = id;

            if (!IsEmailService())
            {
                authenticationServices.AuthenticateAsync(id, client, cancellationToken.Token).Forget();
            }
            else
            {
                if (HasInputData())
                    authenticationServices.AuthenticateAsync(id, client, cancellationToken.Token, inputData).Forget();
            }
        }

        public void SetInputData((string email, string password) inputData)
        {
            this.inputData = inputData;
            ResolveState(authenticationId.CurrentValue);
        }

        private bool HasInputData() =>
            !string.IsNullOrEmpty(inputData.email) && !string.IsNullOrEmpty(inputData.password);

        private bool HasSessionData(out SessionData sessionData) =>
            sessionDataProvider.TryGetData(out sessionData);

        private bool IsEmailService() =>
            authenticationId.Value == AuthenticationService.Email;

        private void ResolveState(AuthenticationService serviceId)
        {
            var newState =  serviceId switch
            {
                AuthenticationService.Email => connectionSuccess ? authenticationsInfo.ConnectionSuccess :
                    authenticationServices.AuthorizationProgress.CurrentValue ? authenticationsInfo.ConnectionWaitingState :
                    authenticationServices.IsSent ? authenticationResult.Exception is ApiResponseException
                        ? authenticationsInfo.AuthenticationError
                        : authenticationsInfo.ConnectionError :
                    HasInputData() ? authenticationsInfo.EmailCanOpenState : authenticationsInfo.EmailState,

                AuthenticationService.Device => connectionSuccess ? authenticationsInfo.ConnectionSuccess :
                    authenticationServices.AuthorizationProgress.CurrentValue ? authenticationsInfo.ConnectionWaitingState :
                    authenticationsInfo.ConnectionError,

                AuthenticationService.Google => connectionSuccess ? authenticationsInfo.ConnectionSuccess :
                    authenticationServices.AuthorizationProgress.CurrentValue ? authenticationsInfo.ConnectionWaitingState :
                    authenticationsInfo.ConnectionError,

                _ => authenticationsInfo.LogInState
            };
            
            ChangeState(newState);
        }

        private void ChangeState(AuthenticationStateBase newState) =>
            state.Value = new AuthenticationPopupState(newState);

        public void Return() =>
            authenticationId.Value = AuthenticationService.None;

        private void OnAuthenticationHandler(IAuthenticationResult result)
        {
            connectionSuccess = result.IsSuccess;
            authenticationResult = result;

            if (result.IsSuccess)
            {
                sessionDataProvider.SetData(new SessionData
                {
                    AuthToken = result.Session.AuthToken,
                    RefreshToken = result.Session.RefreshToken,
                    AuthenticationId = authenticationId.CurrentValue.ToString()
                });
            }

            if (!authenticationResult.IsSuccess && authenticationResult.Exception is ApiResponseException)
                AuthenticationMessageError?.Invoke(authenticationResult.ErrorMessage);

            ResolveState(authenticationId.CurrentValue);
        }

        private void ResolveState(bool _) =>
            ResolveState(authenticationId.CurrentValue);

        public void Dispose()
        {
            authenticationServices.OnAuthentication.RemoveListener(OnAuthenticationHandler);
            cancellationToken?.Cancel();
            cancellationToken?.Dispose();
        }
    }
}