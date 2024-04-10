using Game2D.DataManagment;

using UnityEngine;

namespace Game2D.Gameplay.Items
{
    public interface IItemData : IGuidData
    {
        public IItem GetItem { get; }
        public string GetDescription { get; }
        public Sprite GetSprite { get; }
        public int GetStackMaxCount { get; }
    }
}