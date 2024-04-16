using System.Linq;
using Game.Model.Services.Authentication;
using UnityEngine;

namespace Game.Model.Info
{
    [CreateAssetMenu(menuName = "Game/" + nameof(AuthenticationsInfo))]
    public class AuthenticationsInfo : ScriptableObject
    {
        [SerializeField] private AuthenticationInfo[] authenticationInfos;

        [SerializeField] private AuthenticationInfo defaultAuthenticationInfo;

        public AuthenticationInfo DefaultAuthenticationInfo => defaultAuthenticationInfo;

        public bool TryGet(AuthenticationService serviceID, out AuthenticationInfo info) =>
            (info = authenticationInfos.FirstOrDefault(info => info.ID == serviceID)) != null;
    }
}