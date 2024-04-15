using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Game.Model.Info;
using Game.Model.Services.Authentication;
using UnityEngine;
using Nakama;

namespace Game.ViewModel.UI.Authentication
{
    public class AuthenticationPopupModel : IAuthenticationPopupModel
    {
        private readonly AuthenticationServices authenticationServices;
        private readonly AuthenticationsInfo authenticationsInfo;
        private readonly IClient client;

        public int ServicesCount => authenticationServices.AuthenticationsCount;

        public IReadOnlyList<AuthenticationInfo> AuthenticationsInfo =>
            authenticationsInfo.AuthenticationInfos;

        public AuthenticationPopupModel(AuthenticationServices authenticationServices,
            AuthenticationsInfo authenticationsInfo, IClient client)
        {
            Debug.LogError($"<color=yellow>Invoke ctor {nameof(AuthenticationPopupModel)}</color>");
            this.authenticationServices = authenticationServices;
            this.authenticationsInfo = authenticationsInfo;
            this.client = client;
        }

        public void SetAuthenticate(AuthenticationService serviceID)
        {
            authenticationServices.Authenticate(serviceID, client).Forget();
        }
    }
}