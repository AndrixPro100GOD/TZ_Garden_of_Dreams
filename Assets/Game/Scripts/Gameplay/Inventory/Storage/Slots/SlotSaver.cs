using Game2D.DataManagment;

using Unity.Plastic.Newtonsoft.Json;

namespace Game2D.Gameplay.Inventory.Storage.Slots
{
    public class SlotSaver : DataGuidSaver
    {
        [JsonConstructor]
        public SlotSaver(string dataGuid, string dataName, int slotIndex, int slotCount, string slotDataGuid) : base(dataGuid, dataName)
        {
            SlotIndex = slotIndex;
            SlotCount = slotCount;
            SlotDataGuid = slotDataGuid;
        }

        public SlotSaver(string dataGuid, string dataName, Slot slot) : base(dataGuid, dataName)
        {
            if (slot.SlotItem != null)
            {
                SlotDataGuid = slot.SlotItem != null ? slot.SlotItem.GetItemData.DataGuid : string.Empty;
            }

            SlotIndex = slot.SlotIndex;
            SlotCount = slot.SlotCount;

            DataName = "Slot " + slot.SlotIndex;
        }

        public int SlotIndex { get; set; }
        public int SlotCount { get; set; }
        public string SlotDataGuid { get; set; }

        public Slot GetSlot()
        {
            return new Slot()
            {
                SlotIndex = SlotIndex,
                SlotCount = SlotCount,
                SlotItem = null// return item is GUID is not empty
            };
        }
    }
}