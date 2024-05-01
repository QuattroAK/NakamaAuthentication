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
        private string token;
        private CancellationToken cancellationToken;

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

            try
            {
                PlayGamesPlatform.Instance.Authenticate(ProcessAuthentication);
            }
            catch (Exception ex)
            {
                Debug.LogError(ex.Message);
            }

            await UniTask.WaitUntil(() => !string.IsNullOrEmpty(token),
                cancellationToken: this.cancellationToken);

            Debug.Log($"Token pre start - {token}");
            Debug.Log($"Start - {token}");

            if (string.IsNullOrEmpty(token))
                return null;

            return await client.AuthenticateGoogleAsync(token, username, vars: vars,
                retryConfiguration: retryConfiguration,
                canceller: cancellationToken);
        }

        private void ProcessAuthentication(SignInStatus status)
        {
            if (status == SignInStatus.Success)
            {
                PlayGamesPlatform.Instance.RequestServerSideAccess(true, tokenId =>
                {
                    token = tokenId;
                    Debug.Log($"log in Successful");
                    Debug.Log($"Token - {token}");
                });
            }
            else
            {
                Debug.LogError($"Failed, try Manually");
                PlayGamesPlatform.Instance.ManuallyAuthenticate(ProcessManuallyAuthentication);
            }
        }

        private void ProcessManuallyAuthentication(SignInStatus status)
        {
            if (status == SignInStatus.Success)
            {
                PlayGamesPlatform.Instance.RequestServerSideAccess(true, tokenId =>
                {
                    token = tokenId;
                    Debug.Log($"log in Successful");
                    Debug.Log($"Token - {token}");
                });
            }
            else
            {
                Debug.LogError($"Manually is failed");
                if (!cancellationToken.IsCancellationRequested)
                    throw new OperationCanceledException(token);
            }
        }
    }
}