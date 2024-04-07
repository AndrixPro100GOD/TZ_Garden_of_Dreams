using Game2D.Scripts.Editor;

using UnityEditor;

using UnityEngine;

namespace Game2D.Gameplay.Items.Scriptable
{
    internal static class RunCheckerOnStartup
    {
        [InitializeOnLoadMethod]
        private static void RunOnEditorStartup()
        {
            try
            {
                CheckItemAssets();
                CheckEveryItemAsset();
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"Ошибка при запуске: {ex.Message}");
                EditorApplication.Exit(1);
            }
        }

        /// <summary>
        /// Проверяет есть ли в проекте отсутствующие предметы, которые находятсяв JSON файле
        /// </summary>
        private static void CheckItemAssets()
        {
            System.Collections.Generic.List<ItemGuidData> foundItems = ItemsDataManager.FindAndResoreData(out System.Collections.Generic.List<ItemGuidData> unfoundItems);

            if (unfoundItems.Count > 0)
            {
                Debug.LogError($"В проекте отсутствуют сохранённые предметы в количестве: {unfoundItems.Count} остальные: {foundItems.Count} найдены");

                ItemsDataManagerWindow window = ItemsDataManagerWindow.ShowWindow();
                window.CountAssets();

                EditorApplication.isPaused = true;
            }
        }

        /// <summary>
        /// Проверяет каждый предмет на наличие пустых ссылок
        /// </summary>
        private static void CheckEveryItemAsset()
        {
            // Получить все ItemDataBase в проекте
            ItemDataBase[] scriptableObjects = ExtensionScriptableObject.GetAllScriptableObjects<ItemDataBase>();

            // Проверить каждый ItemDataBase
            foreach (ItemDataBase item in scriptableObjects)
            {
                Debug.Assert(!string.IsNullOrEmpty(item.GetGUID), $"{GetNameFromObject(item)} Item ID is null", item);
                Debug.Assert(!string.IsNullOrEmpty(item.GetName), $"{GetNameFromObject(item)} Item Name is null", item);
                Debug.Assert(!string.IsNullOrEmpty(item.GetDescription), $"{GetNameFromObject(item)} Item Description is null", item);
                Debug.Assert(item.GetItem != null, $"{GetNameFromObject(item)} Item prefab is null", item);
                Debug.Assert(item.GetSprite != null, $"{GetNameFromObject(item)} Item sprite is null", item);
            }
        }

        private static string GetNameFromObject(in Object uObject)
        {
            return $"\"{ExtensionObject.GetFileNameObject(uObject)}\"";
        }
    }
}