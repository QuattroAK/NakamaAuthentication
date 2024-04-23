using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Nakama;
using UnityEngine;

namespace Game.Model.Services.Authentication
{
    public class GoogleAuthentication : IAuthenticationService
    {
        public AuthenticationService ID => AuthenticationService.Google;

        public UniTask<ISession> AuthenticateAsync(
            IClient client,
            (string email, string password) inputData = default,
            string username = null,
            bool create = true,
            Dictionary<string, string> vars = null,
            RetryConfiguration retryConfiguration = null,
            CancellationToken cancellationToken = default)
        {
            Debug.LogError($"{nameof(GoogleAuthentication)} is not implemented");
            return new UniTask<ISession>();
        }
    }
}