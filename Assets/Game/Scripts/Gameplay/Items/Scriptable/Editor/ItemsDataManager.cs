using Game2D.DataManagment;
using Game2D.Gameplay.DataManagment;

using static ProjectConfiguration.ResourcesUtility;

namespace Game2D.Gameplay.Items.Scriptable
{
    public class ItemsDataManager : AssetDataManagerBase<DataGuidSaver, ItemDataBase>
    {
        protected override void SaveJson(in string json)
        {
            ItemsDataUtil.SaveItemsInResurces(json);
        }

        protected override string LoadJson()
        {
            return ItemsDataUtil.LoadItemsFromResurces();
        }

        public override DataGuidSaver Convert(ItemDataBase guidData)
        {
            return new(guidData);
        }
    }
}