using Game2D.Gameplay.Items.Scriptable;

using UnityEditor;

namespace Game2D.DataManagment
{
    [CustomEditor(typeof(ItemDataBase), true)]
    public class ItemsDataBaseEditor : UnityEditor.Editor
    {
        private SerializedProperty _itemGUIDProperty;
        private SerializedProperty _itemNameProperty;

        private string _previousItemName;

        private void OnEnable()
        {
            _itemGUIDProperty = serializedObject.FindProperty("dataGuid");
            _itemNameProperty = serializedObject.FindProperty("dataName");

            if (string.IsNullOrEmpty(_itemGUIDProperty.stringValue))
            {
                _itemGUIDProperty.stringValue = System.Guid.NewGuid().ToString();//Let's hope it's will not hit a jackpot
                _itemNameProperty.stringValue = $"New Item {_itemGUIDProperty.stringValue}";
                _ = serializedObject.ApplyModifiedProperties();

                GlobalDataManager.GetItemsDataManager.SaveData(GlobalDataManager.GetItemsDataManager.Convert((ItemDataBase)target));
            }

            _previousItemName = _itemNameProperty.stringValue;
        }

        public override void OnInspectorGUI()
        {
            DisplayGUID();

            base.OnInspectorGUI();

            serializedObject.Update();

            CheckName();

            _ = serializedObject.ApplyModifiedProperties();
        }

        private void DisplayGUID()
        {
            _ = EditorGUILayout.BeginHorizontal();

            EditorGUILayout.LabelField("GUID:", EditorStyles.boldLabel);

            EditorGUILayout.LabelField(_itemGUIDProperty.stringValue, EditorStyles.miniBoldLabel);

            EditorGUILayout.EndHorizontal();
        }

        private void CheckName()
        {
            if (_itemNameProperty.stringValue != _previousItemName)
            {
                if (string.IsNullOrEmpty(_itemNameProperty.stringValue))
                {
                    _itemNameProperty.stringValue = _previousItemName;
                }

                GlobalDataManager.GetItemsDataManager.SaveData(GlobalDataManager.GetItemsDataManager.Convert((ItemDataBase)target));

                _previousItemName = _itemNameProperty.stringValue;
            }
        }
    }
}