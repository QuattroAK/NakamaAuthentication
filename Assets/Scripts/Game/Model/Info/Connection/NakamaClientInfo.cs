using UnityEngine;

namespace Game.Model.Info.Connection
{
    [CreateAssetMenu(menuName = "Game/" + nameof(NakamaClientInfo))]
    public class NakamaClientInfo : ScriptableObject
    {
        [SerializeField] string scheme;
        [SerializeField] private string host;
        [SerializeField] private int port;
        [SerializeField] private string serverKey;
        [SerializeField] private bool autoRefreshSession;
        [SerializeField] private int connectionTimeout ;

        public string Scheme => scheme;
        public string Host => host;
        public int Port => port;
        public string ServerKey => serverKey;
        public bool AutoRefreshSession => autoRefreshSession;
        public int ConnectionTimeout => connectionTimeout;
    }
}