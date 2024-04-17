using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;
using Nakama;
using UnityEngine.Device;

namespace Game.Model.Services.Authentication
{
    public class DeviceAuthentication : IAuthenticationService
    {
        private readonly string deviceId = SystemInfo.deviceUniqueIdentifier;

        public AuthenticationService ID => AuthenticationService.Device;

        public async UniTask<ISession> AuthenticateAsync(
            IClient client,
            string email = null,
            string password = null,
            string username = null,
            bool create = true,
            Dictionary<string, string> vars = null,
            RetryConfiguration retryConfiguration = null,
            CancellationToken cancellationToken = default)
        {
            return await client.AuthenticateDeviceAsync(deviceId, canceller: cancellationToken);
        }
    }
}