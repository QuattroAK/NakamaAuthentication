using Game.Model.Info.Connection;
using Nakama;

namespace Game.Model.Services.Connection
{
    public class ClientFactory
    {
        private readonly NakamaClientInfo nakamaClient;

        public ClientFactory(NakamaClientInfo nakamaClient)
        {
            this.nakamaClient = nakamaClient;
        }

        public IClient GetNakamaClient(IHttpAdapter adapter, ILogger logger)
        {
            var builder = new NakamaClientBuilder();

            return builder
                .SetScheme(nakamaClient.Scheme)
                .SetHost(nakamaClient.Host)
                .SetPort(nakamaClient.Port)
                .SetServerKey(nakamaClient.ServerKey)
                .AutoRefreshSession(nakamaClient.AutoRefreshSession)
                .SetAdapter(adapter)
                .SetTimeout(nakamaClient.ConnectionTimeout)
                .SetLogger(logger)
                .Build();
        }
    }
}