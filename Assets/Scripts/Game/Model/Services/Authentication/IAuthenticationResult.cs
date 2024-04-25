using System;
using Nakama;

namespace Game.Model.Services.Authentication
{
    public interface IAuthenticationResult
    {
        ISession Session { get; }
        Exception Exception { get; }
        string ErrorMessage { get; }
        bool IsSuccess { get; }
    }
}