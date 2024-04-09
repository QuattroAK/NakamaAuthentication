using UnityEngine;
using Nakama;

public class ConnectionController : MonoBehaviour
{
    private string scheme = "http";
    private string host = "localhost";
    private int port = 7350;
    private string serverKey = "defaultkey";

    private IClient client;
    private ISession session;
    private ISocket socket;

    private async void Start()
    {
        client = new Client(scheme, host, port, serverKey, UnityWebRequestAdapter.Instance);

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

    private async void OnApplicationQuit()
    {
        await socket.CloseAsync();
        Debug.Log("Log out completed");
    }
}