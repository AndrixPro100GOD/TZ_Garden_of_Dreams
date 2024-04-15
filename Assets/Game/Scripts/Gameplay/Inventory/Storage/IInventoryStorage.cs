using System.Collections.Generic;

namespace Game2D.Gameplay.Inventory
{
    public interface IInventoryStorage
    {
        public List<Slot> Slots { get; }

        void SetInventory(List<Slot> slots);
    }
}