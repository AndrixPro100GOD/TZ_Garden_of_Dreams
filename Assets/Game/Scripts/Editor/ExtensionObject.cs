using UnityEditor;

namespace Game2D.Editor
{
    public static class ExtensionObject
    {
        public static string GetFileNameObject<T>(in T uObject) where T : UnityEngine.Object
        {
            return System.IO.Path.GetFileNameWithoutExtension(AssetDatabase.GetAssetPath(uObject));
        }

        /// <summary>
        /// Находит все Asset пути в проекте/>
        /// </summary>
        /// <returns>Все найденые пути</returns>
        public static string[] FindAssetsPath<T>() where T : UnityEngine.Object
        {
            // Поиск Asset по типу класса в проекте
            string[] assetPathsGUID = AssetDatabase.FindAssets($"t:{nameof(T)}", new string[] { "Assets" });

            string[] assetFilePaths = new string[assetPathsGUID.Length];

            for (int i = 0; i < assetPathsGUID.Length; i++)
            {
                //Поучаем path
                assetFilePaths[i] = AssetDatabase.GUIDToAssetPath(assetPathsGUID[i]);
            }

            return assetFilePaths;
        }
    }
}