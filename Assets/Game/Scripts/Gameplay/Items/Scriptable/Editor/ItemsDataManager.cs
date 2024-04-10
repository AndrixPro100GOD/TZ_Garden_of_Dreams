using Game2D.DataManagment;

using static ProjectConfiguration.ResourcesUtility;

namespace Game2D.Gameplay.Items.Scriptable
{
    public class ItemsDataManager : DataManagerBase<DataGuidSaver, ItemDataBase>
    {
        public ItemsDataManager()
        {
        }

        protected override void SaveJson(in string json)
        {
            ItemsDataUtil.SaveItemsInResurces(json);
        }

        protected override string LoadJson()
        {
            return ItemsDataUtil.LoadItemsFromResurces();
        }

        /*

        /// <summary>
        /// Загружает предметы по пути указанных в <paramref name="assetsFilePaths"/> для того, чтобы обновить JSON файл с предметами
        /// </summary>
        /// <param name="assetsFilePaths"></param>
        /// <returns>Все найденные в проекте ассеты с <see cref="TAssetData"/> по указаному пути <paramref name="assetsFilePaths"/></returns>
        public static List<TSavingData> RestoreData(string[] assetsFilePaths)
        {
#nullable enable

            List<TSavingData> itemsData = DataManagerBase.LoadDatabase<TSavingData>(ItemsDataUtil.LoadItemsFromResurces()); ;//Загружаем текущие Items

            foreach (string path in assetsFilePaths)
            {
                TAssetData? item = AssetDatabase.LoadAssetAtPath<TAssetData>(path);// Получаем всю информацию об ассете в качестве типа данных

                if (item == null)
                {
                    continue;//Если не получилось получить Data, то пропускаем
                }

                TSavingData? foundItemData = itemsData.Find(itemData => itemData.DataGuid == item.DataGuid); //Пытаемся найти такой же Item в базе данных

                if (foundItemData != null)//Если Item существует в базе данных и в проекте в виде Asset, то отмечаем его существование
                {
                    foundItemData.IsJsonExist = true;
                    foundItemData.AssetPath = path;
                }
                else//Если Asset не был раньше в базе данных, то тогда добовляем его
                {
                    itemsData.Add(new TSavingData(item) { AssetPath = path, IsJsonExist = false });
                }
            }

            DataManagerBase.SaveDataGuid(itemsData);// Сохраняем данные об ассетах в JSON файл
            return itemsData;
#nullable disable
        }

        /// <summary>
        /// Находит все ассеты с предметами в проекте и добавляет их в JSON файл
        /// </summary>
        /// <param name="unfoundData">Список всех ненайденых ассетов с предметами в проекте</param>
        /// <returns>Список всех найденых ассетов с предметами в проекте</returns>
        public static List<TSavingData> FindAndResoreData(out List<TSavingData> unfoundData)
        {
            //Находим все предметы в проекте
            List<TSavingData> fountItems = RestoreData(DataManagerBase.FindAssetsPath<TAssetData>());

            //Создаём список ненайденных предметов
            List<TSavingData> unfoundItemsFromList = fountItems.FindAll(item => !item.IsAssetExist || !item.IsJsonExist);

            //Удаляем из списка "всех найденных предметов" все ненайденные
            _ = fountItems.RemoveAll(item => unfoundItemsFromList.Contains(item));

            //Для модификатора out, так как он не может быть предикатом при использовании RemoveAll
            unfoundData = unfoundItemsFromList;

            return fountItems;
        }
        */
    }
}