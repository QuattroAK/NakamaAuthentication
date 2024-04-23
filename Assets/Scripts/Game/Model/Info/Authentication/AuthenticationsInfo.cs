using System.Collections.Generic;
using Game.Model.Services.Authentication;
using UnityEngine;

namespace Game.Model.Info.Authentication
{
    [CreateAssetMenu(menuName = "Game/" + nameof(AuthenticationsInfo))]
    public class AuthenticationsInfo : ScriptableObject
    {
        [Header("Authentication cards info")]
        [SerializeField] private AuthenticationCardInfo[] authenticationCardsInfo;

        [Header("Mock info")] 
        [SerializeField] private AuthenticationCardInfo mockAuthenticationCardInfo;

        [Header("Popup states")]
        [SerializeField] private AuthenticationStateBase logInState;

        [SerializeField] private AuthenticationStateBase emailState;
        [SerializeField] private AuthenticationStateBase emailCanOpenState;
        [SerializeField] private AuthenticationStateBase connectionSuccess;
        [SerializeField] private AuthenticationStateBase connectionError;
        [SerializeField] private AuthenticationStateBase connectionWaitingState;

        private Dictionary<AuthenticationService, AuthenticationCardInfo> authenticationCardsCache;

        public AuthenticationCardInfo MockAuthenticationCardInfo => mockAuthenticationCardInfo;
        public AuthenticationStateBase LogInState => logInState;
        public AuthenticationStateBase EmailState => emailState;
        public AuthenticationStateBase EmailCanOpenState => emailCanOpenState;
        public AuthenticationStateBase ConnectionSuccess => connectionSuccess;
        public AuthenticationStateBase ConnectionError => connectionError;
        public AuthenticationStateBase ConnectionWaitingState => connectionWaitingState;

        public bool TryGet(AuthenticationService serviceID, out AuthenticationCardInfo cardInfo) =>
            authenticationCardsCache.TryGetValue(serviceID, out cardInfo);

        private void OnEnable() =>
            Recache();

        private void Recache()
        {
            authenticationCardsCache = new(authenticationCardsInfo.Length);

            foreach (var cardInfo in authenticationCardsInfo)
            {
                if (!authenticationCardsCache.TryAdd(cardInfo.ID, cardInfo))
                    Debug.LogError($"{cardInfo.ID} is duplicated", this);
            }
        }
    }
}