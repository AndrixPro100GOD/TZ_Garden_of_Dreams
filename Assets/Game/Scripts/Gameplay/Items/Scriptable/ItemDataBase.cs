using Game2D.DataManagment;

using UnityEngine;

namespace Game2D.Gameplay.Items.Scriptable
{
    /// <summary>
    /// Базовое представление хранения информации о предмете так же утсанавливает собстбеный GUID при использовании <see cref="ItemsDataBaseEditor"/>
    /// </summary>
    public abstract class ItemDataBase : DataGuidScriptable, IItemData
    {
        [SerializeField]
        private ItemBase itemPrefab;

        [SerializeField]
        [Tooltip("Картинка для отображения в игре и в виде ячейки в инвентаре")]
        private Sprite itemSprite;

        [SerializeField]
        private string itemDescription = "The item description";

        [Min(1)]
        [SerializeField]
        private int itemStackMaxCount = 64;

        public IItem GetItem => itemPrefab;
        public string GetDescription => itemDescription;
        public Sprite GetSprite => itemSprite;
        public int GetStackMaxCount => itemStackMaxCount;
    }
}