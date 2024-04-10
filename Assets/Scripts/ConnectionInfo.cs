using UnityEngine;

[CreateAssetMenu(menuName = "Game/" + nameof(ConnectionInfo))]
public class ConnectionInfo : ScriptableObject
{
    [SerializeField] string scheme;
    [SerializeField] private string host;
    [SerializeField] private int port;
    [SerializeField] private string serverKey;

    public string Scheme => scheme;
    public string Host => host;
    public int Port => port;
    public string ServerKey => serverKey;
}