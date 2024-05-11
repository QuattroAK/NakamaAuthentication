using Core.Persistence;

namespace Game.Model.Services.Connection
{
    public class SessionTokenRefresher
    {
        private readonly IStorageService storageService;

        public SessionTokenRefresher(IStorageService storageService)
        {
            this.storageService = storageService;
        }

        public bool TryGetSessionTokens<T>(out T data) =>
            (data = storageService.Load<T>()) != null;
    }
}