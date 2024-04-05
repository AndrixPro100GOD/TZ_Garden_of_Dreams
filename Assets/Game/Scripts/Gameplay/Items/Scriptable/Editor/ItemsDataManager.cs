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

        public static void SaveItemGUID(ItemBase item)
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
        /// Находит все пути в проекте с Asset <see cref="ItemBase"/>
        /// </summary>
        /// <returns>Все найденые пути</returns>
        public static string[] FindItemAssetsFilePathInProject()
        {
            // Поиск Item Asset по типу класса в проекте
            string[] assetPathsGUID = AssetDatabase.FindAssets($"t:{nameof(ItemBase)}", new string[] { "Assets" });

            string[] assetFilePaths = new string[assetPathsGUID.Length];

            for (int i = 0; i < assetPathsGUID.Length; i++)
            {
                //Поучаем path Item у которого есть GUID и Name
                assetFilePaths[i] = AssetDatabase.GUIDToAssetPath(assetPathsGUID[i]);
            }

            return assetFilePaths;
        }

        /// <summary>
        /// Загружает предметы по пути указанных в <paramref name="assetsFilePaths"/> для того, чтобы обновить JSON файл с предметами
        /// </summary>
        /// <param name="assetsFilePaths"></param>
        /// <returns>Все найденные в проекте ассеты с <see cref="ItemBase"/> по указаному пути <paramref name="assetsFilePaths"/></returns>
        public static List<ItemGuidData> RestoreData(string[] assetsFilePaths)
        {
#nullable enable

            List<ItemGuidData> itemsData = LoadItemGUIDs();//Загружаем текущие Items

            foreach (string path in assetsFilePaths)
            {
                ItemBase? item = AssetDatabase.LoadAssetAtPath<ItemBase>(path);// Получаем всю информацию об ассете в качестве типа данных

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
            List<ItemGuidData> fountItems = RestoreData(FindItemAssetsFilePathInProject());
            unfoundItems = fountItems.FindAll(item => !item.IsAssetExist);

            return fountItems;
        }
    }
}