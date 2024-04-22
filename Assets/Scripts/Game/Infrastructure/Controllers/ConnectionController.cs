using System;
using Cysharp.Threading.Tasks;
using Game.Model.Services.Authentication;
using Game.View.UI.Authentication;
using Game.ViewModel.UI.Authentication;
using Nakama;
using UnityEngine;
using VContainer.Unity;

namespace Game.Infrastructure.Controllers
{
    public class ConnectionController : IStartable, IDisposable
    {
        private IClient client;
        private ISession session;
        private ISocket socket;

        private readonly ConnectionInfo connection;
        private readonly PopupsController popups;
        private readonly AuthenticationServices authenticationServices;

        public ConnectionController(ConnectionInfo connection, PopupsController popups,
            AuthenticationServices authenticationServices)
        {
            this.connection = connection;
            this.popups = popups;
            this.authenticationServices = authenticationServices;
        }

        public void Start()
        {
            Debug.LogError($"<color=green>Connection start</color>");
            Subscribe();

            client = new Client(connection.Scheme, connection.Host, connection.Port, connection.ServerKey,
                UnityWebRequestAdapter.Instance);

            client.Logger = new UnityLogger();;
            client.Timeout = 10;

            popups.Show<AuthenticationPopup, AuthenticationPopupModel>(client);
        }

        private void Subscribe()
        {
            authenticationServices.OnAuthorization.AddListener(OnAuthorizationHandler);
        }

        private void OnAuthorizationHandler(ISession newSession, bool success)
        {
            if (!success) return;

            session = newSession;
            OpenSocketAsync().Forget();
        }

        private async UniTaskVoid OpenSocketAsync()
        {
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