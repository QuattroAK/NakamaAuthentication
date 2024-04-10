using System;
using UnityEngine;
using Nakama;
using VContainer.Unity;

public class ConnectionController : IInitializable, IDisposable
{
    private string scheme = "http";
    private string host = "localhost";
    private int port = 7350;
    private string serverKey = "defaultkey";

    private IClient client;
    private ISession session;
    private ISocket socket;

    private readonly ConnectionInfo _info;

    public ConnectionController(ConnectionInfo info)
    {
        this._info = info;
        Debug.Log($"<color=red>{info.Port}</color>");
    }

    private async void Start()
    {
        client = new Client(_info.Scheme, _info.Host, _info.Port, _info.ServerKey,
            UnityWebRequestAdapter.Instance);

        try
        {
            session = await client.AuthenticateDeviceAsync(SystemInfo.deviceUniqueIdentifier);
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

    private async void CloseSocket()
    {
        if (socket != null)
            await socket.CloseAsync();

        Debug.Log("Log out completed");
    }

    public void Initialize()
    {
        Start();
    }

    public void Dispose()
    {
        CloseSocket();
    }
}