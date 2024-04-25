using System;
using Nakama;

namespace Game.Model.Services.Authentication
{
    public class AuthenticationResult : IAuthenticationResult
    {
        public AuthenticationResult(Exception ex)
        {
            Exception = ex;
        }
        
        public AuthenticationResult(ISession session)
        {
            Session = session;
        }

        public ISession Session { get; }
        public Exception Exception { get; }
        public string ErrorMessage => Exception.Message;
        public bool IsSuccess => Session != null;
    }
}
