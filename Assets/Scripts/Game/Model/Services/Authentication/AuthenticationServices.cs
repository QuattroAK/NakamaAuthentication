using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Nakama;
using UnityEngine;
using UnityEngine.Events;
using R3;

namespace Game.Model.Services.Authentication
{
    public class AuthenticationServices
    {
        private readonly RetryConfiguration retryConfiguration;
        private readonly ReactiveProperty<bool> authorizationProgress = new();
        
        private IAuthenticationResult result;
        private ISession session;
        
        public IReadOnlyList<IAuthenticationService> Services { get; }
        public ReadOnlyReactiveProperty<bool> AuthorizationProgress => authorizationProgress;
        public bool IsSent { get; private set; }
        public readonly UnityEvent<IAuthenticationResult> OnAuthentication = new();

        public AuthenticationServices(IReadOnlyList<IAuthenticationService> services)
        {
            Services = services;
            retryConfiguration = new RetryConfiguration(
                500,
                5,
                (x, y) => Debug.LogError($"Attempt to connect to the server - {x}, {y}"));
        }

        public async UniTask AuthenticateRestoreAsync(IClient client, string authToken, string refreshToken,
            CancellationToken ct)
        {
            IsSent = true;
            authorizationProgress.Value = true;
            session = Session.Restore(authToken, refreshToken);

            try
            {
                if (session.HasExpired(DateTime.UtcNow.AddDays(1)))
                {
                    session = await client.SessionRefreshAsync(session, retryConfiguration: retryConfiguration,
                        canceller: ct);
                }

                result = new AuthenticationResult(session);
            }
            catch (ApiResponseException ex)
            {
                result = new AuthenticationResult(ex);
                Debug.LogError($"Error refresh authenticating: {ex}");
            }
            catch (Exception ex)
            {
                result = new AuthenticationResult(ex);
                session = null;
                Debug.LogError($"Error: {ex.Message}");
            }
            finally
            {
                OnAuthentication?.Invoke(result);

                if (session != null)
                    Debug.Log($"<color=green>Authenticated with COMPLETED - {session.UserId}</color>");
                else
                    Debug.LogError($"Failed");

                authorizationProgress.Value = false;
                IsSent = false;
            }
        }

        public async UniTask AuthenticateAsync(
            AuthenticationService id,
            IClient client,
            CancellationToken ct,
            (string email, string password) inputData = default)
        {
            var service = Services.First(service => service.ID == id);
            Debug.Log($"Start - {id}, timeout - {client.Timeout}");

            IsSent = true;
            authorizationProgress.Value = true;

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
                OnAuthentication?.Invoke(result);

                if (session != null)
                    Debug.Log($"<color=green>Authenticated with COMPLETED - {session.UserId}</color>");
                else
                    Debug.LogError($"Failed");

                authorizationProgress.Value = false;
                IsSent = false;
            }
        }
    }
}