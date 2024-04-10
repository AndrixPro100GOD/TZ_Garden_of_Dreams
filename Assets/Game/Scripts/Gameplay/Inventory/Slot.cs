using Game2D.Gameplay.Items;

namespace Game2D.Gameplay.Inventory
{
    public struct Slot
    {
#nullable enable

        public int SlotIndex { get; set; }
        public IItem? SlotItem { get; set; }
        public int SlotCount { get; set; }

#nullable disable
    }
}