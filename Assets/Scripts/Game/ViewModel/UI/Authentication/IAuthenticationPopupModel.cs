using System;
using System.Collections.Generic;
using R3;
using UnityEngine;
using UnityEngine.Events;

namespace Game.ViewModel.UI.Authentication
{
    public interface IAuthenticationPopupModel : IDisposable
    {
        public ReadOnlyReactiveProperty<AuthenticationPopupState> State { get; }
        UnityEvent<string> AuthenticationMessageError { get; }
        IReadOnlyDictionary<string, Sprite> GetAuthenticationsCardsInfo();
        void SetAuthenticate(string serviceId, (string email, string password) inputData);
        void SetInputData((string email, string password) inputData);
        void Return();
        void Start();
    }
}