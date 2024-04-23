using System;
using Game.Model.Services.Authentication;
using UnityEngine;

namespace Game.Model.Info.Authentication
{
    [Serializable]
    public class AuthenticationCardInfo
    {
        [SerializeField] private AuthenticationService id;
        [SerializeField] private Sprite icon;

        public AuthenticationService ID => id;
        public Sprite Icon => icon;
    }
}