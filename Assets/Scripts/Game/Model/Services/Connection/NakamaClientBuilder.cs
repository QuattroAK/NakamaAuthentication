using Nakama;
using ILogger = Nakama.ILogger;

namespace Game.Model.Services.Connection
{
    public class NakamaClientBuilder
    {
        private string scheme;
        private string host;
        private int port;
        private string serverKey;
        private int timeout;
        private bool autoRefreshSession;

        private IHttpAdapter adapter;
        private ILogger logger;

        public NakamaClientBuilder SetScheme(string scheme)
        {
            this.scheme = scheme;
            return this;
        }

        public NakamaClientBuilder SetHost(string host)
        {
            this.host = host;
            return this;
        }

        public NakamaClientBuilder SetPort(int port)
        {
            this.port = port;
            return this;
        }

        public NakamaClientBuilder SetServerKey(string serverKey)
        {
            this.serverKey = serverKey;
            return this;
        }

        public NakamaClientBuilder AutoRefreshSession(bool autoRefreshSession)
        {
            this.autoRefreshSession = autoRefreshSession;
            return this;
        }

        public NakamaClientBuilder SetTimeout(int timeout)
        {
            this.timeout = timeout;
            return this;
        }

        public NakamaClientBuilder SetAdapter(IHttpAdapter adapter)
        {
            this.adapter = adapter;
            return this;
        }

        public NakamaClientBuilder SetLogger(ILogger logger)
        {
            this.logger = logger;
            return this;
        }

        public IClient Build() =>
            new Client(scheme, host, port, serverKey, adapter, autoRefreshSession)
            {
                Timeout = timeout,
                Logger = logger
            };
    }
}