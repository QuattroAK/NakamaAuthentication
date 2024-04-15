using Cysharp.Threading.Tasks;
using Nakama;

public interface IAuthenticationService
{
    AuthenticationService ID { get; }
    UniTask<ISession> AuthenticateAsync(IClient client);
}