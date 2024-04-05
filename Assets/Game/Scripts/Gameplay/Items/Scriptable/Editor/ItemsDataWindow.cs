using System.Collections.Generic;

using UnityEditor;

using UnityEngine;

namespace Game2D.Gameplay.Items.Scriptable
{
    public class ItemsDataWindow : EditorWindow
    {
        private List<ItemGuidData> foundAssets, unfoundItems = new();
        private Vector2 scrollPosition;

        [MenuItem("Tools/Items Data Manager")]
        private static void ShowWindow()
        {
            ItemsDataWindow window = GetWindow<ItemsDataWindow>("Items Data Manager");
            window.minSize = new Vector2(400, 100);
        }

        private void OnGUI()
        {
            if (HasAssetsSearched())
            {
                DisplayCount();
                DisplayUnfoundItems();
            }

            if (GUILayout.Button("Refresh and Restore items data"))
            {
                CountAssets();
            }

            if (HasAssetsSearched())
            {
                GUILayout.Label("Найденные Assets в проекте:");
                DisplayAssetPaths(foundAssets);
            }
        }

        private bool HasAssetsSearched()
        {
            return foundAssets != null && foundAssets.Count > 0;
        }

        public void CountAssets()
        {
            foundAssets = ItemsDataManager.FindAndResoreData(out unfoundItems);
        }

        private void DisplayCount()
        {
            GUILayout.Label($"Found\t Assets in project: {foundAssets.Count}");
            GUILayout.Label($"Unfound Assets in Project: {unfoundItems.Count}",
                new GUIStyle(GUI.skin.label)
                {
                    normal = { textColor = unfoundItems.Count > 0 ? Color.red : Color.green },
                    fontStyle = FontStyle.Bold
                });
        }

        private void DisplayUnfoundItems()
        {
            if (unfoundItems.Count < 1)
            {
                return;
            }

            GUILayout.Label($"Unfound items: {unfoundItems.Count}");

            scrollPosition = GUILayout.BeginScrollView(scrollPosition);

            foreach (ItemGuidData item in unfoundItems)
            {
                GUILayout.Label($"Name: `{item.ItemName}` GUID: `{item.ItemGUID}`",
                    new GUIStyle(GUI.skin.label)
                    {
                        normal = new GUIStyleState() { background = Texture2D.grayTexture, textColor = Color.white },
                        border = new RectOffset(32, 32, 32, 32)
                    });
            }

            GUILayout.EndScrollView();
        }

        private void DisplayAssetPaths(List<ItemGuidData> itemsWithAssetPath)
        {
            scrollPosition = GUILayout.BeginScrollView(scrollPosition);

            for (int i = 0; i < itemsWithAssetPath.Count; i++)
            {
                string assetPath = itemsWithAssetPath[i].AssetPath;
                if (GUILayout.Button(assetPath, EditorStyles.miniButton))
                {
                    Selection.activeObject = AssetDatabase.LoadAssetAtPath<Object>(assetPath);
                }
            }
            GUILayout.EndScrollView();
        }
    }
}