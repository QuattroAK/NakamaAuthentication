namespace Core.Persistence
{
    public interface IStorageService
    {
        void Save<T>(T data);
        T Load<T>();
        void Delete<T>();
        void DeleteAll();
    }
}