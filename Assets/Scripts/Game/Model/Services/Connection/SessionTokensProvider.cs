using Core.Persistence;

namespace Game.Model.Services.Connection
{
    public class SessionTokensProvider
    {
        private readonly IStorageService storageService;

        public SessionTokensProvider(IStorageService storageService)
        {
            this.storageService = storageService;
        }

        public bool TryGetTokens<T>(out T tokensData) =>
            (tokensData = storageService.Load<T>()) != null;

        public void SaveTokens<T>(T tokensData) =>
            storageService.Save(tokensData);
    }
}