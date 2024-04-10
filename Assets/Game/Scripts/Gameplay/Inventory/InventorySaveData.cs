using System.Collections.Generic;

using Unity.Plastic.Newtonsoft.Json;

namespace Game2D.Gameplay.Inventory
{
    public class InventorySaveData
    {
        [JsonConstructor]
        public InventorySaveData(string id, List<Slot> slots)
        {
            Id = id;
            Slots = slots;
        }

        public InventorySaveData(string id, IInventoryStorage inventory)
        {
            Id = id;
            Slots = inventory.Slots;
        }

        public string Id { get; set; }

        public List<Slot> Slots { get; set; }
    }
}