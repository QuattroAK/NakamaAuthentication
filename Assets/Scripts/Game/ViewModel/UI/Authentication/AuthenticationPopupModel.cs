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

        public AuthenticationPopupModel(AuthenticationServices authenticationServices,
            AuthenticationsInfo authenticationsInfo, IClient client)
        {
            this.authenticationServices = authenticationServices;
            this.authenticationsInfo = authenticationsInfo;
            this.client = client;
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

        public void SetAuthenticate(string serviceId)
        {
            if (!serviceId.TryEnum(out AuthenticationService id)) return;

            ChangeState(ResolveState(id));

            if (!IsEmailService(id))
                authenticationServices.Authenticate(id, client, cancellationToken.Token).Forget();
        }

        private bool IsEmailService(AuthenticationService id) =>
            id == AuthenticationService.Email;

        private AuthenticationStateBase ResolveState(AuthenticationService id) => id switch
        {
            AuthenticationService.None => authenticationsInfo.LogInState,
            AuthenticationService.Device => authenticationsInfo.DeviceState,
            AuthenticationService.Email => authenticationsInfo.EmailState,
            _ => authenticationsInfo.LogInState
        };

        private void ChangeState(AuthenticationStateBase state) =>
            OnChangeState?.Invoke(new AuthenticationPopupState(state));

        public void OnBack() =>
            ChangeState(authenticationsInfo.LogInState);

        public void Dispose()
        {
            Debug.LogError($"Dispose - {nameof(AuthenticationPopupModel)}");
            cancellationToken?.Cancel();
            cancellationToken?.Dispose();
        }
    }
}