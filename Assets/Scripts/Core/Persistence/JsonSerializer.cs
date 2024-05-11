using Newtonsoft.Json;

namespace Core.Persistence
{
    public class JsonSerializer : ISerializer
    {
        public string Serialize<T>(T obj) =>
            JsonConvert.SerializeObject(obj, Formatting.Indented);

        public T Deserialize<T>(string value) =>
            JsonConvert.DeserializeObject<T>(value);
    }
}