using Game.Model.Info.Authentication;
using UnityEngine;

namespace Game.ViewModel.UI.Authentication
{
    public class AuthenticationPopupState
    {
        public AuthenticationPopupState()
        {
        }
        
        public AuthenticationPopupState(AuthenticationStateBase state)
        {
            InputEmail = state.InputEmail;
            InputPassword = state.InputPassword;
            BackButton = state.BackButton;
            TileText = state.TileText;
            Cards = state.Cards;
            BackgroundColor = state.BackgroundColor;
            Enter = state.Enter;
            ConnectionError = state.ConnectionError;
            ConnectionSuccess = state.ConnectionSuccess;
            ConnectionWaiting = state.ConnectionWaiting;
            AuthenticationError = state.AuthenticationError;
        }

        public bool ConnectionWaiting { get; }
        public bool ConnectionSuccess { get; }
        public bool ConnectionError { get; }
        public bool InputEmail { get; }
        public bool InputPassword { get; }
        public bool BackButton { get; }
        public bool TileText { get; }
        public bool Cards { get; }
        public Color BackgroundColor { get; }
        public bool Enter { get; }
        public bool AuthenticationError { get; }
    }
}