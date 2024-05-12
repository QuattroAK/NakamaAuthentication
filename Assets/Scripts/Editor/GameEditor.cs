using Core.Persistence;
using UnityEditor;

namespace Editor
{
#if UNITY_EDITOR
    public static class GameEditor
    {
        [MenuItem("GameSettings/Persistence/ClearData")]
        public static void ClearData()
        {
            if (EditorUtility.DisplayDialog("Clear Data", "Are you sure?", "Ok"))
                PersistenceUtility.DeleteDataDirectory();
        }
    }
}
#endif