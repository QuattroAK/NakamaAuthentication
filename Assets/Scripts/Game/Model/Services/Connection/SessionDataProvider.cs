using Core.Persistence;

namespace Game.Model.Services.Connection
{
    public class SessionDataProvider
    {
        private readonly IStorageService storageService;

        public SessionDataProvider(IStorageService storageService)
        {
            this.storageService = storageService;
        }

        public bool TryGetData<T>(out T tokensData) =>
            (tokensData = storageService.Load<T>()) != null;

        public void SetData<T>(T tokensData) =>
            storageService.Save(tokensData);
    }
}