using System;
using System.Collections.Generic;
using R3;
using VContainer;

namespace Game.ViewModel.UI.Authentication
{
    public interface IAuthenticationPopupModel : IDisposable
    {
        ReadOnlyReactiveProperty<AuthenticationPopupState> State { get; }
        ReadOnlyReactiveProperty<string> AuthenticationMessageError { get; }
        public IObjectResolver Container { get; }
        string ServiceId { get; }
        public IEnumerable<IAuthenticationCardModel> GetAuthenticationsCards();
        void SetAuthenticate(string serviceId, (string email, string password) inputData);
        void SetInputData((string email, string password) inputData);
        void Return();
        void Start();
    }
}