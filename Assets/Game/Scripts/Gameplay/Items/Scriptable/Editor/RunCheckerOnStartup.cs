using Game2D.DataManagment;
using Game2D.Editor;

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
                Debug.LogError($"Ошибка при запуске \"проверки предметов\": {ex.Message}");
            }
        }

        private static void CheckItemAssets()
        {
            System.Collections.Generic.List<DataGuidSaver> unfoundItems = GlobalDataManager.GetItemsDataManager.GetUnfoundData();

            if (unfoundItems.Count > 0)
            {
                Debug.LogError($"В проекте отсутствуют сохранённые предметы в количестве: {unfoundItems.Count}");

                ItemsDataManagerWindow window = ItemsDataManagerWindow.ShowWindow();
                window.CountAssets();
            }
        }

        private static void CheckEveryItemAsset()
        {
            // Получить все TAssetData в проекте
            ItemDataBase[] scriptableObjects = Game2D.Scripts.Editor.ExtensionScriptableObject.GetAllScriptableObjects<ItemDataBase>();

            // Проверить каждый TAssetData
            foreach (ItemDataBase item in scriptableObjects)
            {
                Debug.Assert(!string.IsNullOrEmpty(item.DataGuid), $"{GetNameFromObject(item)} Item ID is null", item);
                Debug.Assert(!string.IsNullOrEmpty(item.DataName), $"{GetNameFromObject(item)} Item Name is null", item);
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