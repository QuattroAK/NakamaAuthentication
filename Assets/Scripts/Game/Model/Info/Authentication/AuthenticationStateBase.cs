using System;
using UnityEngine;

namespace Game.Model.Info.Authentication
{
    [Serializable]
    public class AuthenticationStateBase
    {
        [SerializeField] private bool inputEmail;
        [SerializeField] private bool inputPassword;
        [SerializeField] private bool backButton;
        [SerializeField] private bool tileText;
        [SerializeField] private bool cards;
        [SerializeField] private bool enter;
        [SerializeField] private Color backgroundColor;

        public bool InputEmail => inputEmail;

        public bool InputPassword => inputPassword;

        public bool BackButton => backButton;

        public bool TileText => tileText;

        public bool Cards => cards;

        public bool Enter => enter;
        public Color BackgroundColor => backgroundColor;
    }
}