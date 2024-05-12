using System.IO;
using UnityEngine;

namespace Core.Persistence
{
    public static class PersistenceUtility
    {
        public static void DeleteDataDirectory()
        {
            Directory.Delete(Application.persistentDataPath, true);
        }
    }
}