using Game2D.Gameplay.Items.Scriptable;

namespace Game2D.DataManagment
{
    public static class GlobalDataManager
    {
        private static ItemsDataManager _itemsDataManager = new();

        public static IDataManager<DataGuidSaver, ItemDataBase> GetItemsDataManager => _itemsDataManager;
    }
}