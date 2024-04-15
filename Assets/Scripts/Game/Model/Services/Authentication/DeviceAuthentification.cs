using Cysharp.Threading.Tasks;
using System;
using Nakama;

namespace Game.Model.Services.Authentication
{
    public class DeviceAuthentification : IAuthenticationService
    {
        private readonly string deviceId = Guid.NewGuid().ToString();

        public AuthenticationService ID => AuthenticationService.Device;

        public async UniTask<ISession> AuthenticateAsync(IClient client)
        {
            return await client.AuthenticateDeviceAsync(deviceId);
        }
    }
}