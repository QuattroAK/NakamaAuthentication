using System.Collections.Generic;
using System.Linq;
using Game.Model.Services.Authentication;
using UnityEngine;

namespace Game.Model.Info
{
    [CreateAssetMenu(menuName = "Game/" + nameof(AuthenticationsInfo))]
    public class AuthenticationsInfo : ScriptableObject
    {
        [SerializeField] private AuthenticationInfo[] authenticationInfos;

        public IReadOnlyList<AuthenticationInfo> AuthenticationInfos => authenticationInfos;

        public bool TryGet(AuthenticationService authenticationService, out AuthenticationInfo info) =>
            (info = authenticationInfos.FirstOrDefault(info => info.ID == authenticationService)) != null;
    }
}