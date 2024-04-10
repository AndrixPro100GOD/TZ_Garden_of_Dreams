using Game2D.DataManagment;
using Game2D.Scripts.Editor.Windows;

using System.Collections.Generic;
using System.Linq;

using UnityEditor;

using UnityEngine;

namespace Game2D.Gameplay.Items.Scriptable
{
    public class ItemsDataManagerWindow : EditorWindow
    {
        private List<(DataGuidSaver data, string assetPath)> foundData = new();
        private List<DataGuidSaver> unfoundData = new();
        private List<(ItemDataBase assetData, string assetPath)> unfoundAssetData = new();

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

        private void Awake()
        {
            CountAssets();
        }

        private void OnDestroy()
        {
#pragma warning disable S2696 // Instance members should not write to "static" fields
            _currentWindow = null;
#pragma warning restore S2696 // Instance members should not write to "static" fields
        }

        private void OnGUI()
        {
            DisplayCount();
            DisplayUnfoundData();
            DisplayUnfoundAssetsData();

            if (GUILayout.Button("Refresh"))
            {
                CountAssets();
            }

            DisplayAssetPaths(foundData);
        }

        public void CountAssets()
        {
            foundData = GlobalDataManager.GetItemsDataManager.GetDataWithAssetPaths().ToList();
            unfoundData = GlobalDataManager.GetItemsDataManager.GetUnfoundData();
            unfoundAssetData = GlobalDataManager.GetItemsDataManager.GetUnfoundAssetsData();
        }

        private void DisplayCount()
        {
            GUILayout.Box("Количество", new GUIStyle(GUI.skin.box)
            {
                fontStyle = FontStyle.Bold,
                alignment = TextAnchor.MiddleCenter
            }, GUILayout.ExpandWidth(true));

            GUILayout.BeginVertical(GUI.skin.box);

            GUILayout.Label($"Найденные предметы в проекте: {foundData.Count}");
            GUILayout.Label($"Ненайденные предметы в проекте: {unfoundData.Count}",
                new GUIStyle(GUI.skin.label)
                {
                    normal = { textColor = unfoundData.Count > 0 ? Color.red : Color.green },
                    fontStyle = FontStyle.Bold
                });
            GUILayout.Label($"Найденные неизвестные предметы в проекте: {unfoundAssetData.Count}");

            GUILayout.EndVertical();

            GUILayout.Space(2.5f);

            GUILayout.Box("", new GUIStyle(GUI.skin.box)
            {
                normal = new()
                {
                    background = EditorGUIUtility.whiteTexture,
                    textColor = Color.red,
                }
            }, GUILayout.ExpandWidth(true), GUILayout.Height(1));

            GUILayout.Space(2.5f);
        }

        private void DisplayUnfoundData()
        {
            if (unfoundData.Count < 1)
            {
                return;
            }

            //Это данные из базы данных, которые не нашли скриптабельные объекты в проекте, на которые они ссылались по GUID
            GUILayout.Label($"Найденых неизвестных данных: {unfoundData.Count}");

            scrollPosition = GUILayout.BeginScrollView(scrollPosition);

            foreach (DataGuidSaver item in unfoundData)
            {
                GUILayout.Label($"Name: `{item.DataName}` GUID: `{item.DataGuid}`",
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
                        GlobalDataManager.GetItemsDataManager.DeleteData(item);
                        CountAssets();
                    }
#pragma warning restore S1066 // Mergeable "if" statements should be combined
                }
                GUILayout.Space(15);
            }

            GUILayout.EndScrollView();//Ошибка
        }

        private void DisplayUnfoundAssetsData()
        {
            if (unfoundAssetData.Count < 1)
            {
                return;
            }
            GUILayout.BeginHorizontal();

            //Это скриптабельные объекты, о которых нет информации в базе данных
            GUILayout.Label($"Найденых неизвесных ассетов: {unfoundAssetData.Count}");

            if (GUILayout.Button("Восстановить ассеты в базу данных"))
            {
                GlobalDataManager.GetItemsDataManager.RestoreDatabase();
                CountAssets();
            }
            GUILayout.EndHorizontal();
        }

        private void DisplayAssetPaths(List<(DataGuidSaver data, string assetPath)> dataWithAssetPath)
        {
            if (dataWithAssetPath.Count < 1)
            {
                return;
            }

            scrollPosition = GUILayout.BeginScrollView(scrollPosition);

            for (int i = 0; i < dataWithAssetPath.Count; i++)
            {
                GUILayout.BeginHorizontal();

                string assetPath = dataWithAssetPath[i].assetPath;
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
                        GlobalDataManager.GetItemsDataManager.DeleteData(dataWithAssetPath[i].data);

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