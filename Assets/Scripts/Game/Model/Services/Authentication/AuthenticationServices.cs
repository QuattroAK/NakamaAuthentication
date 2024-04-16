using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Nakama;
using UnityEngine;
using VContainer.Unity;

namespace Game.Model.Services.Authentication
{
    public class AuthenticationServices
    {
        public IReadOnlyList<IAuthenticationService> Services { get; }

        public AuthenticationServices(IReadOnlyList<IAuthenticationService> services)
        {
            Services = services;
        }

        public async UniTask Authenticate(AuthenticationService id, IClient client)
        {
            // var service = services.FirstOrDefault(x => x.ID == id);
            //
            // if (service != null) 
            //     await service.AuthenticateAsync(client);
            await UniTask.Yield();
            Debug.LogError($"Start {id}");
        }
    }
}