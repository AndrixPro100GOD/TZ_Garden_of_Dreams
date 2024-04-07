using System.Collections.Generic;
using System.IO;
using System.Linq;

using Unity.Plastic.Newtonsoft.Json;

using UnityEditor;

namespace Game2D.Gameplay.Items.Scriptable
{
    public static class ItemsDataManager
    {
        private static readonly string _filePath = "Assets/Resources/Items/ItemsDataGuid.json";

        #region ItemGUID Save Load Remove

        /// <summary>
        /// Сохраняет <paramref name="item"/> добавляя или перезаписывая его в JSON файле, не изменяя список с остальными предметами
        /// </summary>
        /// <param name="item">новый предмет</param>
        public static void SaveItemGUID(ItemDataBase item)
        {
#nullable enable
            List<ItemGuidData> itemsData = LoadItemGUIDs();
            ItemGuidData? existingItem = itemsData.Find(itemData => itemData.ItemGUID == item.GetGUID);

            if (existingItem != null)
            {
                existingItem.ItemName = item.GetName;
            }
            else
            {
                itemsData.Add(new ItemGuidData(item.GetGUID, item.GetName));
            }
            SaveItemGuidData(itemsData);

#nullable disable
        }

        /// <summary>
        /// Сохраняет все предметы, полностью перезаписывая весь список
        /// </summary>
        /// <param name="itemsData">Список предметов</param>
        public static void SaveItemGuidData(List<ItemGuidData> itemsData)
        {
            string json = JsonConvert.SerializeObject(itemsData, typeof(List<ItemGuidData>), null);
            File.WriteAllText(_filePath, json);
            AssetDatabase.Refresh();
        }

        /// <summary>
        /// Считает все предметы хранившиеся в JSON файле
        /// </summary>
        /// <returns>Список всех предметов, которые когда либо были созданы</returns>
        public static List<ItemGuidData> LoadItemGUIDs()
        {
            if (!File.Exists(_filePath))
            {
                return new List<ItemGuidData>();
            }

            string json = File.ReadAllText(_filePath);

            return string.IsNullOrEmpty(json) ? new List<ItemGuidData>() : JsonConvert.DeserializeObject<List<ItemGuidData>>(json).ToList();
        }

        /// <summary>
        /// Удаляет <see cref="ItemGuidData"/> из JSON файла и удаляет Asset <see cref="ItemDataBase"/> из проекта, на который ссылался <paramref name="itemGuidData"/>
        /// </summary>
        /// <param name="itemGuidData">Предмет на удаление</param>
        /// <param name="onlyJsonMessage">Выводить ли в консоль уведомление только об JSON удалении</param>
        /// <returns></returns>
        public static bool RemoveItem(ItemGuidData itemGuidData, bool onlyJsonMessage = false)//TODO: Сделать возмозжность записи истоии и откатов Ctrl + Z
        {
            bool isRemovedJSON = RemoveItemGUID(itemGuidData);
            bool isDeletedAsset = RemoveItemAsset(itemGuidData);

            if (!isRemovedJSON)
            {
                UnityEngine.Debug.Log($"Не удалось удалить \"{itemGuidData.ItemName}\" из JSON списка");
            }

            if (!isDeletedAsset)
            {
                UnityEngine.Debug.Log($"Не удалось удалить \"{itemGuidData.ItemName}\" из проекта (все Assets)");
            }

            if (isRemovedJSON && isDeletedAsset)
            {
                UnityEngine.Debug.Log($" \"{itemGuidData.ItemName}\" удалён из проекта (все Assets) и JSON файла");
            }

            return isRemovedJSON || isDeletedAsset;
        }

        private static bool RemoveItemGUID(ItemGuidData itemGuidData)
        {
            List<ItemGuidData> list = LoadItemGUIDs();

            ItemGuidData itemFound = list.Find(item => item.ItemGUID == itemGuidData.ItemGUID);
            bool result = list.Remove(itemFound);

            SaveItemGuidData(list);

            return result;
        }

        private static bool RemoveItemAsset(in ItemGuidData itemGuidData)
        {
            string[] assetPathsGUID = AssetDatabase.FindAssets($"t:{nameof(ItemDataBase)}", new string[] { "Assets" });

            foreach (string assetPathGUID in assetPathsGUID)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(assetPathGUID);
                ItemDataBase itemData = AssetDatabase.LoadAssetAtPath<ItemDataBase>(assetPath);

                // Проверяем, совпадает ли GUID предмета с GUID ассета
                if (itemData.GetGUID == itemGuidData.ItemGUID)
                {
                    bool result = AssetDatabase.DeleteAsset(assetPath);
                    AssetDatabase.Refresh();
                    return result;
                }
            }

            return false; // Если не удалось найти ассет с указанным GUID
        }

        #endregion ItemGUID Save Load Remove

        #region ItemData

#nullable enable

        public static ItemDataBase? GetItemData(ItemGuidData itemGuidData, out string assetPath)
        {
            string[] assetPathsGUID = AssetDatabase.FindAssets($"t:{nameof(ItemDataBase)}", new string[] { "Assets" });

            foreach (string assetPathGUID in assetPathsGUID)
            {
                assetPath = AssetDatabase.GUIDToAssetPath(assetPathGUID);
                ItemDataBase itemData = AssetDatabase.LoadAssetAtPath<ItemDataBase>(assetPath);

                // Проверяем, совпадает ли GUID предмета с GUID ассета
                if (itemData.GetGUID == itemGuidData.ItemGUID)
                {
                    return itemData;
                }
            }
            assetPath = string.Empty;
            return null; // Если не удалось найти ассет с указанным GUID
        }

#nullable disable

        /// <summary>
        /// Находит все пути в проекте с Asset <see cref="ItemDataBase"/>
        /// </summary>
        /// <returns>Все найденые пути</returns>
        public static string[] FindItemDataAssetsFilePathInProject()
        {
            // Поиск Item Asset по типу класса в проекте
            string[] assetPathsGUID = AssetDatabase.FindAssets($"t:{nameof(ItemDataBase)}", new string[] { "Assets" });

            string[] assetFilePaths = new string[assetPathsGUID.Length];

            for (int i = 0; i < assetPathsGUID.Length; i++)
            {
                //Поучаем path Item у которого есть GUID и Name
                assetFilePaths[i] = AssetDatabase.GUIDToAssetPath(assetPathsGUID[i]);
            }

            return assetFilePaths;
        }

        #endregion ItemData

        /// <summary>
        /// Загружает предметы по пути указанных в <paramref name="assetsFilePaths"/> для того, чтобы обновить JSON файл с предметами
        /// </summary>
        /// <param name="assetsFilePaths"></param>
        /// <returns>Все найденные в проекте ассеты с <see cref="ItemDataBase"/> по указаному пути <paramref name="assetsFilePaths"/></returns>
        public static List<ItemGuidData> RestoreData(string[] assetsFilePaths)
        {
#nullable enable

            List<ItemGuidData> itemsData = LoadItemGUIDs();//Загружаем текущие Items

            foreach (string path in assetsFilePaths)
            {
                ItemDataBase? item = AssetDatabase.LoadAssetAtPath<ItemDataBase>(path);// Получаем всю информацию об ассете в качестве типа данных

                if (item == null)
                {
                    continue;//Если не получилось получить Data, то пропускаем
                }

                ItemGuidData foundItemData = itemsData.Find(itemData => itemData.ItemGUID == item.GetGUID); //Пытаемся найти такой же Item в базе данных

                if (foundItemData != null)//Если Item существует в базе данных и в проекте в виде Asset, то отмечаем его существование
                {
                    foundItemData.AssetPath = path;
                }
                else//Если Asset не был раньше в базе данных, то тогда добовляем его
                {
                    itemsData.Add(new ItemGuidData(item) { AssetPath = path });
                }
            }

            SaveItemGuidData(itemsData);// Сохраняем данные об ассетах в JSON файл
            return itemsData;
#nullable disable
        }

        /// <summary>
        /// Находит все ассеты с предметами в проекте и добавляет их в JSON файл
        /// </summary>
        /// <param name="unfoundItems">Список всех ненайденых ассетов с предметами в проекте</param>
        /// <returns>Список всех найденых ассетов с предметами в проекте</returns>
        public static List<ItemGuidData> FindAndResoreData(out List<ItemGuidData> unfoundItems)
        {
            //Находим все предметы в проекте
            List<ItemGuidData> fountItems = RestoreData(FindItemDataAssetsFilePathInProject());

            //Создаём список ненайденных предметов
            List<ItemGuidData> unfoundItemsFromList = fountItems.FindAll(item => !item.IsAssetExist);

            //Удаляем из списка "всех найденных предметов" все ненайденные
            _ = fountItems.RemoveAll(item => unfoundItemsFromList.Contains(item));

            //Для модификатора out, так как он не может быть предикатом при использовании RemoveAll
            unfoundItems = unfoundItemsFromList;

            return fountItems;
        }
    }
}