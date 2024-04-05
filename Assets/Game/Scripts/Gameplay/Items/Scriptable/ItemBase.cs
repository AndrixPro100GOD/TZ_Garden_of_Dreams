using UnityEngine;

namespace Game2D.Gameplay.Items.Scriptable
{
    /// <summary>
    /// Базовое представление хранения информации о предмете так же утсанавливает собстбеный GUID при использовании <see cref="ItemBaseEditor"/>
    /// </summary>
    public abstract class ItemBase : ScriptableObject
    {
        [SerializeField, HideInInspector]
        private string itemGUID = string.Empty;

        [SerializeField]
        [Tooltip("Картинка для инвентаря")]
        private Sprite itemSprite;

        [SerializeField]
        private string itemName = "Item";

        [SerializeField]
        private string itemDescription = "The item description";

        [Min(0)]
        [SerializeField]
        private int itemStackMaxCount = 64;

        public string GetGUID => itemGUID;
        public string GetName => itemName;
        public string GetDescription => itemDescription;
        public Sprite GetSprite => itemSprite;
        public int GetStackMaxCount => itemStackMaxCount;
    }
}