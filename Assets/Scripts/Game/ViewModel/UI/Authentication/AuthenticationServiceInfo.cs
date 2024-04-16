using UnityEngine;

namespace Game.ViewModel.UI.Authentication
{
    public class AuthenticationServiceInfo
    {
        public string ServiceID { get; }
        public Sprite ServiceIcon { get; }

        public AuthenticationServiceInfo(string serviceID, Sprite serviceIcon)
        {
            ServiceID = serviceID;
            ServiceIcon = serviceIcon;
        }
    }
}