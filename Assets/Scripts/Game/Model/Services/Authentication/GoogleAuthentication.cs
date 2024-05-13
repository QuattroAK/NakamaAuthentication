using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using Nakama;
using UnityEngine;

namespace Game.Model.Services.Authentication
{
    public class GoogleAuthentication : IAuthenticationService
    {
        public AuthenticationService ID => AuthenticationService.Google;
        private CancellationToken cancellationToken;

        public GoogleAuthentication()
        {
            PlayGamesPlatform.Activate();
            PlayGamesPlatform.DebugLogEnabled = true;
        }

        public async UniTask<ISession> AuthenticateAsync(
            IClient client,
            (string email, string password) inputData = default,
            string username = null,
            bool create = true,
            Dictionary<string, string> vars = null,
            RetryConfiguration retryConfiguration = null,
            CancellationToken cancellationToken = default)
        {
            this.cancellationToken = cancellationToken;

            var authenticationToken = await GetGooglePlayGameAuthenticationTokenAsync();

            if (string.IsNullOrEmpty(authenticationToken))
                return null;

            return await client.AuthenticateGoogleAsync(authenticationToken, username, vars: vars,
                retryConfiguration: retryConfiguration,
                canceller: cancellationToken);
        }

        private async UniTask<string> GetGooglePlayGameAuthenticationTokenAsync()
        {
            var newToken = string.Empty;
            PlayGamesPlatform.Instance.Authenticate(status =>
            {
                if (status == SignInStatus.Success)
                {
                    RequestAuthenticationToken(true, SetToken);
                }
                else
                {
                    Debug.LogError($"Google log in failed, try manually");
                    PlayGamesPlatform.Instance.ManuallyAuthenticate(newStatus =>
                    {
                        if (newStatus == SignInStatus.Success)
                        {
                            RequestAuthenticationToken(true, SetToken);
                        }
                        else
                        {
                            if (!cancellationToken.IsCancellationRequested)
                                throw new Exception("Google manually authenticate is failed");
                        }
                    });
                }

                void SetToken(string token)
                {
                    newToken = token;
                    Debug.Log($"Log in successful - {newToken}");
                }
            });

            await UniTask.WaitUntil(() => !string.IsNullOrEmpty(newToken), cancellationToken: cancellationToken);

            return newToken;
        }

        private void RequestAuthenticationToken(bool forceRefreshToken, Action<string> callback) =>
            PlayGamesPlatform.Instance.RequestServerSideAccess(forceRefreshToken, callback);
    }
}