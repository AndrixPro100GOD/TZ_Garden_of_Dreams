using UnityEditor;

namespace Game2D.Scripts.Editor
{
    public static class ExtensionObject
    {
        public static string GetFileNameObject<T>(in T uObject) where T : UnityEngine.Object
        {
            return System.IO.Path.GetFileNameWithoutExtension(AssetDatabase.GetAssetPath(uObject));
        }
    }
}