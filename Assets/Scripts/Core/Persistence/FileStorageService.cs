using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Core.Persistence
{
    public class FileStorageService : IStorageService
    {
        private readonly ISerializer serializer;

        private readonly string dataPath = Application.persistentDataPath;
        private const string fileExtension = ".json";

        public FileStorageService(ISerializer serializer)
        {
            this.serializer = serializer;
        }

        public void Save<T>(T data)
        {
            var filePath = GetPathToFile(typeof(T).Name);
            File.WriteAllText(filePath, serializer.Serialize(data));
        }

        public T Load<T>()
        {
            var filePath = GetPathToFile(typeof(T).Name);
            return !File.Exists(filePath) ? default : serializer.Deserialize<T>(File.ReadAllText(filePath));
        }

        public void Delete<T>()
        {
            var filePath = GetPathToFile(typeof(T).Name);

            if (File.Exists(filePath))
                File.Delete(filePath);
        }

        public void DeleteAll()
        {
            foreach (var filePath in Directory.GetFiles(dataPath))
                File.Delete(filePath);
        }

        private string GetPathToFile(string fileName) =>
            Path.Combine(dataPath, string.Concat(fileName, fileExtension));

        public IEnumerable<string> GetSavedFiles()
        {
            foreach (var path in Directory.EnumerateFiles(dataPath))
                if (Path.GetExtension(path) == fileExtension)
                    yield return Path.GetFileNameWithoutExtension(path);
        }
    }
}