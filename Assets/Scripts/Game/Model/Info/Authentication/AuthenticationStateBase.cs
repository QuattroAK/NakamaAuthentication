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
        [SerializeField] private bool connectionSuccess;
        [SerializeField] private bool connectionError;
        [SerializeField] private bool connectionWaiting;
        [SerializeField] private bool authenticationError;
        [SerializeField] private Color backgroundColor;

        public bool InputEmail => inputEmail;
        public bool InputPassword => inputPassword;
        public bool BackButton => backButton;
        public bool TileText => tileText;
        public bool Cards => cards;
        public bool Enter => enter;
        public bool ConnectionSuccess => connectionSuccess;
        public bool ConnectionError => connectionError;
        public Color BackgroundColor => backgroundColor;
        public bool ConnectionWaiting => connectionWaiting;
        public bool AuthenticationError => authenticationError;
    }
}