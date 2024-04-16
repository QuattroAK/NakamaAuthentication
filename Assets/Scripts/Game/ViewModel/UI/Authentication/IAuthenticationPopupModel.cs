using System.Collections.Generic;

namespace Game.ViewModel.UI.Authentication
{
    public interface IAuthenticationPopupModel
    {
        IReadOnlyList<AuthenticationServiceInfo> GetAuthenticationsServiceInfos();
        void SetAuthenticate(string serviceID);
    }
}