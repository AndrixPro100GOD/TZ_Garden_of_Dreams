using Game2D.DataManagment;
using Game2D.Gameplay.Inventory.Storage.Slots;

using System.Collections.Generic;

using Unity.Plastic.Newtonsoft.Json;

namespace Game2D.Gameplay.Inventory.Storage
{
    public class InventoryStorageSaver : DataGuidSaver
    {
        [JsonConstructor]
        public InventoryStorageSaver(string dataGuid, string dataName, List<SlotSaver> slotSavers) : base(dataGuid, dataName)
        {
            SlotSavers = slotSavers;
        }

        [JsonProperty]
        public List<SlotSaver> SlotSavers { get; set; }
    }
}