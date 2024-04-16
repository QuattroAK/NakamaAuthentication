using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Game.Model.Info;
using Game.Model.Services.Authentication;
using Nakama;

namespace Game.ViewModel.UI.Authentication
{
    public class AuthenticationPopupModel : IAuthenticationPopupModel
    {
        private readonly AuthenticationServices authenticationServices;
        private readonly AuthenticationsInfo authenticationsInfo;
        private readonly IClient client;

        private List<AuthenticationServiceInfo> authenticationsServiceInfos;

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

        public void SetAuthenticate(string serviceID)
        {
            if (Enum.TryParse(serviceID, out AuthenticationService ID))
            {
                authenticationServices.Authenticate(ID, client).Forget();
            }
        }
    }
}