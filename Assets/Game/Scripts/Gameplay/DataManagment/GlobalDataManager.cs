using Game2D.Gameplay.DataManagment;
using Game2D.Gameplay.Inventory.Storage;
using Game2D.Gameplay.Items.Scriptable;

namespace Game2D.DataManagment
{
    public static class GlobalDataManager
    {
        private static readonly ItemsDataManager _itemsDataManager = new();
        private static readonly InvnetoryDataManager _inventoryDataManager = new();

        public static IAssetDataManager<DataGuidSaver, ItemDataBase> GetItemsDataManager => _itemsDataManager;
        public static IDataManager<InventoryStorageSaver> GetInventoryDataManager => _inventoryDataManager;
    }
}