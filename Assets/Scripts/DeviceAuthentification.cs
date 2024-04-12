using System;
using Cysharp.Threading.Tasks;
using Nakama;

public class DeviceAuthentification : IAuthenticationService
{
    private readonly string deviceId = Guid.NewGuid().ToString();

    public async UniTask<ISession> AuthenticateAsync(IClient client)
    {
        return await client.AuthenticateDeviceAsync(deviceId);
    }
}