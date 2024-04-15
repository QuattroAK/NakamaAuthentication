using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Game.View.UI.Authentication;
using Game.ViewModel.UI.Authentication;
using Nakama;
using UnityEngine;
using VContainer.Unity;

namespace Game.Infrastructure.Controllers
{
    public class ConnectionController : IAsyncStartable, IDisposable
    {
        private IClient client;
        private ISession session;
        private ISocket socket;

        private readonly ConnectionInfo connection;
        private readonly PopupsController popups;

        public ConnectionController(ConnectionInfo connection, PopupsController popups)
        {
            this.connection = connection;
            this.popups = popups;
        }

        async UniTask IAsyncStartable.StartAsync(CancellationToken ct)
        {
            Debug.LogError($"<color=green>Connection start</color>");
            client = new Client(connection.Scheme, connection.Host, connection.Port, connection.ServerKey,
                UnityWebRequestAdapter.Instance);

            popups.Show<AuthenticationPopup, AuthenticationPopupModel>(client);

            try
            {
                session = await client.AuthenticateDeviceAsync(SystemInfo.deviceUniqueIdentifier, canceller: ct);
            }
            catch (ApiResponseException ex)
            {
                Debug.LogFormat("Error authenticating device: {0}:{1}", ex.StatusCode, ex.Message);
            }

            Debug.Log($"Authenticated with Device ID COMPLETED - {session.UserId}");
            socket = client.NewSocket();
            await socket.ConnectAsync(session);

            Debug.Log(client);
            Debug.Log(socket);
        }

        private async UniTaskVoid CloseSocket()
        {
            if (socket != null)
                await socket.CloseAsync();
            else
                Debug.LogError("Socket was not created");

            Debug.Log("Log out completed");
        }

        public void Dispose() =>
            CloseSocket().Forget();
    }
}