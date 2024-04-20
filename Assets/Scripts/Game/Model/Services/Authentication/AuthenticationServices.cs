using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Nakama;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Model.Services.Authentication
{
    public class AuthenticationServices
    {
        private ISession session;
        public IReadOnlyList<IAuthenticationService> Services { get; }

        public readonly UnityEvent<ISession, bool> OnAuthorization = new();

        public AuthenticationServices(IReadOnlyList<IAuthenticationService> services)
        {
            Services = services;
        }

        public async UniTask Authenticate(
            AuthenticationService id,
            IClient client,
            CancellationToken ct,
            (string email, string password) inputData = default)
        {
            var service = Services.First(service => service.ID == id);
            Debug.LogError($"Start {id}");

            try
            {
                session = await service.AuthenticateAsync(client, inputData, cancellationToken: ct);
            }
            catch (ApiResponseException ex)
            {
                Debug.LogFormat("Error authenticating: {0}:{1}", ex.StatusCode, ex.Message);
            }
            finally
            {
                OnAuthorization?.Invoke(session, session != null);
                if (session != null)
                    Debug.Log($"Authenticated with COMPLETED - {session.UserId}");
            }
        }
    }
}