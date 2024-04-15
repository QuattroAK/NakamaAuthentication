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
        private readonly IReadOnlyList<IAuthenticationService> authentications;
        public int AuthenticationsCount => authentications.Count;

        public AuthenticationServices(IReadOnlyList<IAuthenticationService> authentications)
        {
            Debug.LogError($"<color=yellow>Invoke ctor {nameof(AuthenticationServices)}</color>");
            this.authentications = authentications;
        }

        public async UniTask Authenticate(AuthenticationService id, IClient client)
        {
            // var service = authentications.FirstOrDefault(x => x.ID == id);
            //
            // if (service != null) 
            //     await service.AuthenticateAsync(client);
            await UniTask.Yield();
            Debug.LogError($"Start {id}");
        }
    }
}