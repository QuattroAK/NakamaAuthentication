using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Game.ViewModel.UI.Authentication
{
    public interface IAuthenticationPopupModel : IDisposable
    {
        UnityEvent<AuthenticationPopupState> OnChangeState { get; }
        UnityEvent<string> authenticationMessageError { get; }
        IReadOnlyDictionary<string, Sprite> GetAuthenticationsCardsInfo();
        void SetAuthenticate(string serviceId, (string email, string password) inputData);
        void ValidateInputData((string email, string password) inputData);
        void OnBack();
    }
}