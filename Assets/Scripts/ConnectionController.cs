using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Nakama;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using Object = UnityEngine.Object;

public class ConnectionController : IAsyncStartable, IDisposable
{
    private IClient client;
    private ISession session;
    private ISocket socket;

    private readonly ConnectionInfo connectionInfo;
    private readonly IObjectResolver container;

    public ConnectionController(ConnectionInfo connectionInfo, IObjectResolver container)
    {
        this.connectionInfo = connectionInfo;
        this.container = container;
    }

    async UniTask IAsyncStartable.StartAsync(CancellationToken ct)
    {
        Debug.LogError($"<color=green>Connection start</color>");
        client = new Client(connectionInfo.Scheme, connectionInfo.Host, connectionInfo.Port, connectionInfo.ServerKey,
            UnityWebRequestAdapter.Instance);

        await UniTask.Delay(TimeSpan.FromSeconds(2f), cancellationToken: ct);

        var scope = Show<AuthenticationPopup, AuthenticationPopupModel>();

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

    private IObjectResolver Show<TComponent, RObject1>(params object[] arguments)
        where TComponent : Component
    {
        var view = Object.Instantiate(connectionInfo.Prefab.gameObject);

        var scope = container
            .CreateScope(builder =>
            {
                builder.Register<RObject1>(Lifetime.Scoped).AsSelf().AsImplementedInterfaces();
                foreach (var argument in arguments)
                    builder.RegisterInstance(argument).AsImplementedInterfaces();
            });

        scope.InjectGameObject(view);

        if (view.TryGetComponent(out TComponent c))
        {
            Debug.LogError($"Instantiate {nameof(TComponent)}");
        }

        return scope;
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