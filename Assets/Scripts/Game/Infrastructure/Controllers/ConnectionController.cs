using System;
using Cysharp.Threading.Tasks;
using Game.Model.Services.Authentication;
using Game.Model.Services.Connection;
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

        private readonly ClientFactory clientFactory;
        private readonly PopupsController popups;
        private readonly AuthenticationServices authenticationServices;

        public ConnectionController(ClientFactory clientFactory, PopupsController popups,
            AuthenticationServices authenticationServices)
        {
            this.clientFactory = clientFactory;
            this.popups = popups;
            this.authenticationServices = authenticationServices;
        }

        public void Start()
        {
            Subscribe();
            client = clientFactory.GetNakamaClient(UnityWebRequestAdapter.Instance, new UnityLogger());
            popups.Show<AuthenticationPopup, AuthenticationPopupModel>(client);
        }

        private void Subscribe()
        {
            authenticationServices.OnAuthentication.AddListener(OpenSocket);
        }

        private void OpenSocket(IAuthenticationResult result)
        {
            if (!result.IsSuccess) return;

            session = result.Session;
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