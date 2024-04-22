using System;
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
    public class AuthenticationPopupModel : IAuthenticationPopupModel, IDisposable
    {
        private readonly AuthenticationServices authenticationServices;
        private readonly AuthenticationsInfo authenticationsInfo;
        private readonly IClient client;
        private readonly CancellationTokenSource cancellationToken = new();

        private List<AuthenticationServiceInfo> authenticationsServiceInfos;
        public UnityEvent<AuthenticationPopupState> OnChangeState { get; } = new();

        private AuthenticationService currentServiceId;

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

        public IReadOnlyList<AuthenticationServiceInfo> GetAuthenticationsServiceInfos()
        {
            authenticationsServiceInfos =
                new List<AuthenticationServiceInfo>(authenticationServices.Services.Count);

            foreach (var service in authenticationServices.Services)
            {
                authenticationsServiceInfos.Add(authenticationsInfo.TryGet(service.ID, out var serviceInfo)
                    ? new AuthenticationServiceInfo(serviceInfo.ID.ToString(), serviceInfo.Icon)
                    : new AuthenticationServiceInfo(service.ID.ToString(),
                        authenticationsInfo.DefaultAuthenticationInfo.Icon));
            }

            return authenticationsServiceInfos;
        }

        public void SetAuthenticate(string serviceId, (string email, string password) inputData)
        {
            if (!serviceId.TryEnum(out AuthenticationService id)) return;
            currentServiceId = id;

            ChangeState(ResolveState(id, inputData));

            if (!IsEmailService(id))
            {
                authenticationServices.Authenticate(id, client, cancellationToken.Token).Forget();
                return;
            }

            if (HasInputData(inputData))
                authenticationServices.Authenticate(id, client, cancellationToken.Token, inputData).Forget();
        }

        public void ValidateInputData((string email, string password) inputData)
        {
            ChangeState(ResolveState(currentServiceId, inputData));
            Debug.LogError(currentServiceId);
        }

        private bool HasInputData((string email, string password) inputData) =>
            !string.IsNullOrEmpty(inputData.email) && !string.IsNullOrEmpty(inputData.password);

        private bool IsEmailService(AuthenticationService id) =>
            id == AuthenticationService.Email;

        private AuthenticationStateBase ResolveState(AuthenticationService id,
            (string email, string password) inputData) => id switch
        {
            AuthenticationService.Email => HasInputData(inputData)
                ? authenticationsInfo.EmailCanOpenState
                : authenticationsInfo.EmailState,
            AuthenticationService.Device => authenticationsInfo.DeviceState,
            _ => authenticationsInfo.LogInState
        };

        private void ChangeState(AuthenticationStateBase state) =>
            OnChangeState?.Invoke(new AuthenticationPopupState(state));

        public void OnBack()
        {
            currentServiceId = AuthenticationService.None;
            ChangeState(authenticationsInfo.LogInState);
        }

        private void OnAuthorizationHandler(ISession session, bool success)
        {
            ChangeState(success ? authenticationsInfo.ConnectionSuccess : authenticationsInfo.ConnectionError);
        }

        public void Dispose()
        {
            Debug.LogError($"Dispose - {nameof(AuthenticationPopupModel)}");
            cancellationToken?.Cancel();
            cancellationToken?.Dispose();
        }
    }
}