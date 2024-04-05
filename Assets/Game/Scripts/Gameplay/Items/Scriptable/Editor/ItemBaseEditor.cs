using UnityEditor;

using static Game2D.Gameplay.Items.Scriptable.ItemsDataManager;

namespace Game2D.Gameplay.Items.Scriptable
{
    [CustomEditor(typeof(ItemBase), true)]
    public class ItemBaseEditor : Editor
    {
        private SerializedProperty _itemGUIDProperty;
        private SerializedProperty _itemNameProperty;

        private string _previousItemName;

        private void OnEnable()
        {
            _itemGUIDProperty = serializedObject.FindProperty("itemGUID");
            _itemNameProperty = serializedObject.FindProperty("itemName");

            if (string.IsNullOrEmpty(_itemGUIDProperty.stringValue))
            {
                _itemGUIDProperty.stringValue = System.Guid.NewGuid().ToString();//Let's hope it's will not hit a jackpot
                _itemNameProperty.stringValue = $"New Item {_itemGUIDProperty.stringValue}";

                SaveItemGUID((ItemBase)target);

                _ = serializedObject.ApplyModifiedProperties();
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

                SaveItemGUID((ItemBase)target);

                _previousItemName = _itemNameProperty.stringValue;
            }
        }
    }
}