using System.Linq;
using Game.Model.Services.Authentication;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Model.Info.Authentication
{
    [CreateAssetMenu(menuName = "Game/" + nameof(AuthenticationsInfo))]
    public class AuthenticationsInfo : ScriptableObject
    {
        [Header("Authentication services info")]
        [SerializeField] private AuthenticationInfo[] authenticationInfos;
        
        [Header("DefaultInfo")]
        [SerializeField] private AuthenticationInfo defaultAuthenticationInfo;

        [Header("States")] 
        [SerializeField] private AuthenticationStateBase logInState;
        [SerializeField] private AuthenticationStateBase emailState;
        [SerializeField] private AuthenticationStateBase deviceState;
        [SerializeField] private AuthenticationStateBase emailCanOpenState;
        [SerializeField] private AuthenticationStateBase connectionSuccess;
        [SerializeField] private AuthenticationStateBase connectionError;
        [SerializeField] private AuthenticationStateBase connectionWaitingState;

        public AuthenticationInfo DefaultAuthenticationInfo => defaultAuthenticationInfo;

        public AuthenticationStateBase LogInState => logInState;

        public AuthenticationStateBase EmailState => emailState;

        public AuthenticationStateBase DeviceState => deviceState;

        public AuthenticationStateBase EmailCanOpenState => emailCanOpenState;

        public AuthenticationStateBase ConnectionSuccess => connectionSuccess;

        public AuthenticationStateBase ConnectionError => connectionError;

        public AuthenticationStateBase ConnectionWaitingState => connectionWaitingState;

        public bool TryGet(AuthenticationService serviceID, out AuthenticationInfo info) =>
            (info = authenticationInfos.FirstOrDefault(info => info.ID == serviceID)) != null;
    }
}