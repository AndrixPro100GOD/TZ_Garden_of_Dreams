using UnityEditor;

using UnityEngine;

namespace Game2D.Scripts.Editor
{
    public static class ExtensionScriptableObject
    {
        public static T[] GetAllScriptableObjects<T>() where T : ScriptableObject
        {
            string[] guids = AssetDatabase.FindAssets($"t:{typeof(T).Name}");

            T[] scriptableObjects = new T[guids.Length];

            for (int i = 0; i < guids.Length; i++)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guids[i]);
                scriptableObjects[i] = AssetDatabase.LoadAssetAtPath<T>(assetPath);
            }

            return scriptableObjects;
        }
    }
}