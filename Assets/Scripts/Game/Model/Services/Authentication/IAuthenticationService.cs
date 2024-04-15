using Cysharp.Threading.Tasks;
using Nakama;

namespace Game.Model.Services.Authentication
{
    public interface IAuthenticationService
    {
        AuthenticationService ID { get; }
        UniTask<ISession> AuthenticateAsync(IClient client);
    }
}