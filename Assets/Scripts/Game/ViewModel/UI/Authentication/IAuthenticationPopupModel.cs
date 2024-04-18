using System.Collections.Generic;
using UnityEngine.Events;

namespace Game.ViewModel.UI.Authentication
{
    public interface IAuthenticationPopupModel
    {
        UnityEvent<AuthenticationPopupState> OnChangeState { get; }
        IReadOnlyList<AuthenticationServiceInfo> GetAuthenticationsServiceInfos();
        void SetAuthenticate(string serviceId);
        void OnBack();
    }
}