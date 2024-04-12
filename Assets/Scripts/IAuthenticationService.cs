using Cysharp.Threading.Tasks;
using Nakama;

public interface IAuthenticationService
{
    UniTask<ISession> AuthenticateAsync(IClient client);
}