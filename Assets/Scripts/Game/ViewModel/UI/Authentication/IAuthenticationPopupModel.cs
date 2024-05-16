using System;
using System.Collections.Generic;
using R3;
using UnityEngine;

namespace Game.ViewModel.UI.Authentication
{
    public interface IAuthenticationPopupModel : IDisposable
    {
        ReadOnlyReactiveProperty<AuthenticationPopupState> State { get; }
        ReadOnlyReactiveProperty<string> AuthenticationMessageError { get; }
        string ServiceId { get; }
        IReadOnlyDictionary<string, Sprite> GetAuthenticationsCardsInfo();
        void SetAuthenticate(string serviceId, (string email, string password) inputData);
        void SetInputData((string email, string password) inputData);
        void Return();
        void Start();
    }
}