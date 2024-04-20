using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Nakama;

namespace Game.Model.Services.Authentication
{
    public class EmailAuthentication : IAuthenticationService
    {
        public AuthenticationService ID => AuthenticationService.Email;

        public async UniTask<ISession> AuthenticateAsync(
            IClient client,
            (string email, string password) inputData = default,
            string username = null,
            bool create = true,
            Dictionary<string, string> vars = null,
            RetryConfiguration retryConfiguration = null,
            CancellationToken cancellationToken = default)
        {
            return await client.AuthenticateEmailAsync(
                inputData.email,
                inputData.password,
                canceller: cancellationToken);
        }
    }
}