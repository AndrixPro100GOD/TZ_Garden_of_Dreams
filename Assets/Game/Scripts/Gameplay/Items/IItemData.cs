using UnityEngine;

namespace Game2D.Gameplay.Items
{
    public interface IItemData
    {
        public IItem GetItem { get; }
        public string GetGUID { get; }
        public string GetName { get; }
        public string GetDescription { get; }
        public Sprite GetSprite { get; }
        public int GetStackMaxCount { get; }
    }
}