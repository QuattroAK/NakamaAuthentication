using System.Collections.Generic;
using Game.Model.Info;
using Game.Model.Services.Authentication;

namespace Game.ViewModel.UI.Authentication
{
    public interface IAuthenticationPopupModel
    {
        public int ServicesCount { get; }
        IReadOnlyList<AuthenticationInfo> AuthenticationsInfo { get; }
        void SetAuthenticate(AuthenticationService service);
    }
}