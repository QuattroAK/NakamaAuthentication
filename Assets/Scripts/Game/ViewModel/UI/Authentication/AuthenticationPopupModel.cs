using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Extensions;
using Game.Model.Info.Authentication;
using Game.Model.Services.Authentication;
using Nakama;
using UnityEngine;
using UnityEngine.Events;

namespace Game.ViewModel.UI.Authentication
{
    public class AuthenticationPopupModel : IAuthenticationPopupModel
    {
        private readonly AuthenticationServices authenticationServices;
        private readonly AuthenticationsInfo authenticationsInfo;
        private readonly IClient client;
        private readonly CancellationTokenSource cancellationToken = new();

        private Dictionary<string, Sprite> authenticationsCardsInfo;
        public UnityEvent<AuthenticationPopupState> OnChangeState { get; } = new();

        private AuthenticationService currentServiceId;
        private (string email, string password) inputData;
        private bool connectionSuccess;

        public AuthenticationPopupModel(AuthenticationServices authenticationServices,
            AuthenticationsInfo authenticationsInfo, IClient client)
        {
            this.authenticationServices = authenticationServices;
            this.authenticationsInfo = authenticationsInfo;
            this.client = client;
            Subscribe();
        }

        private void Subscribe()
        {
            authenticationServices.OnAuthorization.AddListener(OnAuthorizationHandler);
        }

        public IReadOnlyDictionary<string, Sprite> GetAuthenticationsCardsInfo()
        {
            authenticationsCardsInfo = new(authenticationServices.Services.Count);
            
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
            currentServiceId = id;

            if (!IsEmailService(id))
            {
                authenticationServices.Authenticate(id, client, cancellationToken.Token).Forget();
            }
            else
            {
                if (HasInputData())
                    authenticationServices.Authenticate(id, client, cancellationToken.Token, inputData).Forget();
            }

            ChangeState(ResolveState());
        }

        public void ValidateInputData((string email, string password) inputData)
        {
            this.inputData = inputData;
            ChangeState(ResolveState());
            Debug.Log(currentServiceId);
        }

        private bool HasInputData() =>
            !string.IsNullOrEmpty(inputData.email) && !string.IsNullOrEmpty(inputData.password);

        private bool IsEmailService(AuthenticationService id) =>
            id == AuthenticationService.Email;

        private AuthenticationStateBase ResolveState() => currentServiceId switch
        {
            AuthenticationService.Email => connectionSuccess ? authenticationsInfo.ConnectionSuccess :
                authenticationServices.AuthorizationProgress ? authenticationsInfo.ConnectionWaitingState :
                authenticationServices.IsSent ? authenticationsInfo.ConnectionError :
                HasInputData() ? authenticationsInfo.EmailCanOpenState : authenticationsInfo.EmailState,

            AuthenticationService.Device => connectionSuccess ? authenticationsInfo.ConnectionSuccess :
                authenticationServices.AuthorizationProgress ? authenticationsInfo.ConnectionWaitingState :
                authenticationsInfo.ConnectionError,

            _ => authenticationsInfo.LogInState
        };

        private void ChangeState(AuthenticationStateBase state) =>
            OnChangeState?.Invoke(new AuthenticationPopupState(state));

        public void OnBack()
        {
            currentServiceId = AuthenticationService.None;
            ChangeState(ResolveState());
        }

        private void OnAuthorizationHandler(ISession session, bool success)
        {
            connectionSuccess = success;
            ChangeState(ResolveState());
        }

        public void Dispose()
        {
            Debug.Log($"Dispose - {nameof(AuthenticationPopupModel)}");
            cancellationToken?.Cancel();
            cancellationToken?.Dispose();
        }
    }
}