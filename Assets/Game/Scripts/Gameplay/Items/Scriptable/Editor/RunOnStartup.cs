using UnityEditor;

using UnityEngine;

namespace Game2D.Gameplay.Items.Scriptable
{
    internal class RunOnStartup
    {
        [InitializeOnLoadMethod]
        private static void RunOnEditorStartup()
        {
            try
            {
                var foundItems = ItemsDataManager.FindAndResoreData(out var unfoundItems);

                if (unfoundItems.Count > 0)
                {
                    Debug.LogError($"В проекте отсутствуют сохранённые предметы в количестве: {unfoundItems.Count} остальные: {foundItems.Count} найдены");

                    var window = EditorWindow.GetWindow<ItemsDataWindow>();
                    window.Show();
                    window.CountAssets();

                    EditorApplication.isPaused = true;
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"Ошибка при запуске: {ex.Message}");
                EditorApplication.Exit(1);
            }
        }
    }
}