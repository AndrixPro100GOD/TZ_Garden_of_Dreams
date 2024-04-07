using UnityEditor;

using UnityEngine;

namespace Game2D.Scripts.Editor.Windows
{
    public class ConfirmationWindow : EditorWindow
    {
        public bool Confirmed { get; private set; }

        private string message;

        // Метод для отображения окна подтверждения
        public static bool ShowWindow(string message)
        {
            ConfirmationWindow window = CreateInstance<ConfirmationWindow>();
            window.position = new Rect(Screen.width / 2, Screen.height / 2, 350, 40);
            window.titleContent = new GUIContent("Подтверждение");
            window.message = message;
            window.ShowModalUtility();
            return window.Confirmed;
        }

        private void OnGUI()
        {
            GUILayout.Label(message, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            if (GUILayout.Button("Да"))
            {
                Confirmed = true;
                Close();
            }
            GUILayout.Space(5);
            if (GUILayout.Button("Нет"))
            {
                Confirmed = false;
                Close();
            }

            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }
    }
}