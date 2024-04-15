using Cysharp.Threading.Tasks;
using Nakama;

namespace Game.Model.Services.Authentication
{
    public class EmailAuthentification : IAuthenticationService
    {
        const string email = "email@example.com";
        const string password = "3bc8f72e95a9";

        public AuthenticationService ID => AuthenticationService.Email;

        public async UniTask<ISession> AuthenticateAsync(IClient client)
        {
            return await client.AuthenticateEmailAsync(email, password);
        }
    }
}