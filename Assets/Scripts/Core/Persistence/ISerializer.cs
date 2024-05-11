namespace Core.Persistence
{
    public interface ISerializer
    {
        string Serialize<T>(T obj);
        T Deserialize<T>(string value);
    }
}