using Game2D.DataManagment;
using Game2D.Gameplay.Inventory.Storage.Slots;

using static ProjectConfiguration.ResourcesUtility;

namespace Game2D.Gameplay.Inventory.Storage
{
    public class InvnetoryDataManager : DataManagerBase<InventoryStorageSaver>
    {
        protected override string LoadJson()
        {
            return InventoriesDataUtil.LoadInventoryFromResurces();
        }

        protected override void SaveJson(in string json)
        {
            InventoriesDataUtil.SaveInventoryInResurces(json);
        }
    }
}