using Game2D.Scripts.Editor.Windows;

using System.Collections.Generic;

using UnityEditor;

using UnityEngine;

namespace Game2D.Gameplay.Items.Scriptable
{
    public class ItemsDataManagerWindow : EditorWindow
    {
        private List<ItemGuidData> foundAssets = new(), unfoundItems = new();
        private Vector2 scrollPosition;
#nullable enable
        private static ItemsDataManagerWindow? _currentWindow;

        private static bool IsWindowOpen => _currentWindow != null;

#nullable disable

        [MenuItem("Tools/Items Data Manager")]
        public static ItemsDataManagerWindow ShowWindow()
        {
            if (IsWindowOpen)
            {
                return _currentWindow;
            }

            ItemsDataManagerWindow window = GetWindow<ItemsDataManagerWindow>("Items Data Manager", false);
            window.minSize = new Vector2(400, 100);
            _currentWindow = window;

            return window;
        }

        private void OnDestroy()
        {
#pragma warning disable S2696 // Instance members should not write to "static" fields
            _currentWindow = null;
#pragma warning restore S2696 // Instance members should not write to "static" fields
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

                if (GUILayout.Button("Delete JSON item", new GUIStyle(GUI.skin.button)
                {
                    fixedWidth = 110
                }))
                {
#pragma warning disable S1066 // Mergeable "if" statements should be combined
                    if (ConfirmationWindow.ShowWindow("Удалить полностью этот Asset из проекта и JSON файла?"))
                    {
                        _ = ItemsDataManager.RemoveItem(item, true);
                        CountAssets();
                    }
#pragma warning restore S1066 // Mergeable "if" statements should be combined
                }
                GUILayout.Space(15);
            }

            GUILayout.EndScrollView();//Ошибка
        }

        private void DisplayAssetPaths(List<ItemGuidData> itemsWithAssetPath)
        {
            scrollPosition = GUILayout.BeginScrollView(scrollPosition);

            for (int i = 0; i < itemsWithAssetPath.Count; i++)
            {
                GUILayout.BeginHorizontal();

                string assetPath = itemsWithAssetPath[i].AssetPath;
                if (GUILayout.Button(assetPath, EditorStyles.miniButton))
                {
                    Selection.activeObject = AssetDatabase.LoadAssetAtPath<Object>(assetPath);
                }
                if (GUILayout.Button("Delete asset", new GUIStyle(GUI.skin.button)
                {
                    fixedWidth = 100
                }))
                {
#pragma warning disable S1066 // Mergeable "if" statements should be combined
                    if (ConfirmationWindow.ShowWindow("Удалить полностью этот Asset из проекта и JSON файла?"))
                    {
                        _ = ItemsDataManager.RemoveItem(itemsWithAssetPath[i]);
                        CountAssets();
                    }
#pragma warning restore S1066 // Mergeable "if" statements should be combined
                }
                GUILayout.EndHorizontal();
            }

            GUILayout.EndScrollView();
        }
    }
}