using UnityEngine;

namespace Game2D.Gameplay.Items
{
    public abstract class ItemBase : MonoBehaviour, IItem
    {
        public abstract IItemData GetItemData { get; }

        public ItemBase GetItem => this;
    }
}