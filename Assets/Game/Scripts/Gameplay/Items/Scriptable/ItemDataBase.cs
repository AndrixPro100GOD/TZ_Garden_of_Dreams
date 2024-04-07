using UnityEngine;

namespace Game2D.Gameplay.Items.Scriptable
{
    /// <summary>
    /// Базовое представление хранения информации о предмете так же утсанавливает собстбеный GUID при использовании <see cref="ItemDataBaseEditor"/>
    /// </summary>
    public abstract class ItemDataBase : ScriptableObject, IItemData
    {
        [SerializeField, HideInInspector]
        private string itemGUID = string.Empty;

        [SerializeField]
        private ItemBase itemPrefab;

        [SerializeField]
        [Tooltip("Картинка для ячейки  в инвентаре")]
        private Sprite itemSprite;

        [SerializeField]
        private string itemName = "Item";

        [SerializeField]
        private string itemDescription = "The item description";

        [Min(1)]
        [SerializeField]
        private int itemStackMaxCount = 64;

        public IItem GetItem => itemPrefab;
        public string GetGUID => itemGUID;
        public string GetName => itemName;
        public string GetDescription => itemDescription;
        public Sprite GetSprite => itemSprite;
        public int GetStackMaxCount => itemStackMaxCount;
    }
}