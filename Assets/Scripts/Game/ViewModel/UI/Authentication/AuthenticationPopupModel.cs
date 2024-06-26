using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Core.Extensions;
using Cysharp.Threading.Tasks;
using Game.Model.Info.Authentication;
using Game.Model.Services.Authentication;
using Game.Model.Services.Connection;
using Nakama;
using R3;
using UnityEngine;
using VContainer;

namespace Game.ViewModel.UI.Authentication
{
    public class AuthenticationPopupModel : IAuthenticationPopupModel
    {
        private readonly AuthenticationServices authenticationServices;
        private readonly AuthenticationsInfo authenticationsInfo;
        private readonly SessionDataProvider sessionDataProvider;
        private readonly IClient client;
        private readonly CancellationTokenSource cancellationToken = new();
        private readonly ReactiveProperty<AuthenticationPopupState> state = new(new AuthenticationPopupState());
        private readonly ReactiveProperty<AuthenticationService> serviceId = new(AuthenticationService.None);
        private readonly ReactiveProperty<(string email, string password)> inputData = new();
        private readonly ReactiveProperty<string> authenticationMessageError = new(string.Empty);
        private readonly CompositeDisposable disposables = new();

        private IAuthenticationResult authenticationResult;
        private Dictionary<string, Sprite> authenticationsCardsInfo;
        private bool connectionSuccess;

        public IObjectResolver Container { get; }
        public string ServiceId => serviceId.Value.ToString();
        public ReadOnlyReactiveProperty<AuthenticationPopupState> State => state;
        public ReadOnlyReactiveProperty<string> AuthenticationMessageError => authenticationMessageError;

        public AuthenticationPopupModel(AuthenticationServices authenticationServices,
            AuthenticationsInfo authenticationsInfo, IClient client, SessionDataProvider sessionDataProvider,
            IObjectResolver container)
        {
            this.authenticationServices = authenticationServices;
            this.authenticationsInfo = authenticationsInfo;
            this.sessionDataProvider = sessionDataProvider;
            Container = container;
            this.client = client;
        }

        public void Start()
        {
            Subscribe();
            
            if (TryGetSessionData(out var sessionData))
                AuthenticateRestore(sessionData);
        }

        private void Subscribe()
        {
            authenticationServices.OnAuthentication.AddListener(OnAuthenticationHandler);
            serviceId.Subscribe(ResolveState).AddTo(disposables);
            inputData
                .Subscribe(_ => ResolveState(serviceId.CurrentValue))
                .AddTo(disposables);

            authenticationServices.AuthorizationProgress
                .Subscribe(_ => ResolveState(serviceId.CurrentValue))
                .AddTo(disposables);

            authenticationMessageError
                .Subscribe(_ => ResolveState(serviceId.CurrentValue))
                .AddTo(disposables);
        }

        private void AuthenticateRestore(SessionData sessionData)
        {
            if (!sessionData.ServiceId.TryEnum(out AuthenticationService currentServiceId)) return;

            authenticationServices.AuthenticateRestoreAsync(client, sessionData.AuthToken, sessionData.RefreshToken,
                cancellationToken.Token).Forget();

            serviceId.Value = currentServiceId;
        }
        
        public IEnumerable<IAuthenticationCardModel> GetAuthenticationsCards()
        {
            return authenticationServices.Services.Select(service => Container.CreateScope(builder =>
            {
                builder.Register<AuthenticationCardModel>(Lifetime.Scoped)
                    .WithParameter(authenticationsInfo.TryGet(service.ID, out var serviceInfo)
                        ? serviceInfo.Icon
                        : authenticationsInfo.MockAuthenticationCardInfo.Icon)
                    .WithParameter(service.ID.ToString()).AsImplementedInterfaces();
                    
            }).Resolve<IAuthenticationCardModel>());
        }

        public void SetAuthenticate(string serviceId, (string email, string password) inputData)
        {
            if (!serviceId.TryEnum(out AuthenticationService id)) return;

            this.serviceId.Value = id;

            if (IsEmailService())
            {
                if (HasInputData())
                    authenticationServices.AuthenticateAsync(id, client, cancellationToken.Token, inputData).Forget();

                return;
            }

            authenticationServices.AuthenticateAsync(id, client, cancellationToken.Token).Forget();
        }

        public void SetInputData((string email, string password) inputData) =>
            this.inputData.Value = inputData;

        private bool HasInputData() =>
            !string.IsNullOrEmpty(inputData.Value.email) && !string.IsNullOrEmpty(inputData.Value.password);

        private bool TryGetSessionData(out SessionData sessionData) =>
            sessionDataProvider.TryGetData(out sessionData);

        private bool IsEmailService() =>
            serviceId.Value == AuthenticationService.Email;

        private void ResolveState(AuthenticationService serviceId)
        {
            var newState =  serviceId switch
            {
                AuthenticationService.Email => connectionSuccess ? authenticationsInfo.ConnectionSuccess :
                    authenticationServices.AuthorizationProgress.CurrentValue ? authenticationsInfo.ConnectionWaitingState :
                    authenticationServices.IsSent ? authenticationResult.Exception is ApiResponseException ?
                        authenticationsInfo.AuthenticationError : authenticationsInfo.ConnectionError :
                    HasInputData() ? authenticationsInfo.EmailCanOpenState : authenticationsInfo.EmailState,

                AuthenticationService.Device or AuthenticationService.Google => connectionSuccess ?
                    authenticationsInfo.ConnectionSuccess : authenticationServices.AuthorizationProgress.CurrentValue ? 
                        authenticationsInfo.ConnectionWaitingState: authenticationsInfo.ConnectionError,

                _ => authenticationsInfo.LogInState
            };
            
            ChangeState(newState);
        }

        private void ChangeState(AuthenticationStateBase newState) =>
            state.Value = new AuthenticationPopupState(newState);

        public void Return() =>
            serviceId.Value = AuthenticationService.None;

        private void OnAuthenticationHandler(IAuthenticationResult result)
        {
            if (result.IsSuccess)
            {
                sessionDataProvider.SetData(new SessionData
                {
                    AuthToken = result.Session.AuthToken,
                    RefreshToken = result.Session.RefreshToken,
                    ServiceId = serviceId.CurrentValue.ToString()
                });
            }

            connectionSuccess = result.IsSuccess;
            authenticationResult = result;
            authenticationMessageError.Value = result.ErrorMessage;
        }
    
        public void Dispose()
        {
            authenticationServices.OnAuthentication.RemoveListener(OnAuthenticationHandler);
            disposables.Dispose();
            cancellationToken?.Cancel();
            cancellationToken?.Dispose();
        }
    }
}