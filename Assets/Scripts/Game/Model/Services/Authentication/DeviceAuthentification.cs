using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using Nakama;
using UnityEngine.Device;

namespace Game.Model.Services.Authentication
{
    public class DeviceAuthentification : IAuthenticationService
    {
        private readonly string deviceId = SystemInfo.deviceUniqueIdentifier;

        public AuthenticationService ID => AuthenticationService.Device;

        public async UniTask<ISession> AuthenticateAsync(IClient client, CancellationToken ct)
        {
            return await client.AuthenticateDeviceAsync(deviceId, canceller: ct);
        }
    }
}