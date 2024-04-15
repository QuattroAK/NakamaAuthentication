using System;
using Game.Model.Services.Authentication;
using UnityEngine;

namespace Game.Model.Info
{
    [Serializable]
    public class AuthenticationInfo
    {
        [SerializeField] private AuthenticationService id;
        [SerializeField] private Sprite icon;

        public AuthenticationService ID => id;
        public Sprite Icon => icon;
    }
}