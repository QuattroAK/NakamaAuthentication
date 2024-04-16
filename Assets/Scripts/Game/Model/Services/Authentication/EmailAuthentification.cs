using System.Threading;
using Cysharp.Threading.Tasks;
using Nakama;

namespace Game.Model.Services.Authentication
{
    public class EmailAuthentification : IAuthenticationService
    {
        const string email = "email@example.com";
        const string password = "3333";

        public AuthenticationService ID => AuthenticationService.Email;

        public async UniTask<ISession> AuthenticateAsync(IClient client, CancellationToken ct)
        {
            return await client.AuthenticateEmailAsync(email, password, canceller: ct);
        }
    }
}