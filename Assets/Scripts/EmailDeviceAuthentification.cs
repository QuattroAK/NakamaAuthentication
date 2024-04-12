using Cysharp.Threading.Tasks;
using Nakama;

public class EmailDeviceAuthentification : IAuthenticationService
{
    const string email = "email@example.com";
    const string password = "3bc8f72e95a9";

    public async UniTask<ISession> AuthenticateAsync(IClient client)
    {
        return await client.AuthenticateEmailAsync(email, password);
    }
}