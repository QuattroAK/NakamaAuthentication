using System;
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

        public readonly UnityEvent<IAuthenticationResult> OnAuthentication = new();
        public bool AuthorizationProgress { get; private set; } = false;
        public bool IsSent { get; private set; } = false;

        private IAuthenticationResult result;

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
            Debug.Log($"Start - {id}, timeout - {client.Timeout}");

            AuthorizationProgress = true;
            IsSent = true;

            var retryConfiguration = new RetryConfiguration(
                500,
                5,
                (x, y) => Debug.LogError($"Attempt to connect to the server - {x}, {y}"));

            try
            {
                session = await service.AuthenticateAsync(client, inputData, retryConfiguration: retryConfiguration,
                    cancellationToken: ct);
                result = new AuthenticationResult(session);
            }
            catch (ApiResponseException ex)
            {
                result = new AuthenticationResult(ex);
                Debug.LogError($"Error authenticating: {ex}");
            }
            catch (Exception ex)
            {
                result = new AuthenticationResult(ex);
                Debug.LogError($"Error: {ex.Message}");
            }
            finally
            {
                AuthorizationProgress = false;
                OnAuthentication?.Invoke(result);
                IsSent = false;

                if (session != null)
                    Debug.Log($"<color=green>Authenticated with COMPLETED - {session.UserId}</color>");
                else
                    Debug.LogError($"Failed");
            }
        }
    }
}