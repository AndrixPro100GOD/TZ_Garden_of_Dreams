//Это всё можно сделать ассетом, для того что бы управлять значениями в редактаре, но на данный момент так быстрее
#if UNITY_EDITOR

using UnityEditor;

#endif

using System.IO;

using UnityEngine;

using static ProjectConfiguration.ResourcesPath;

namespace ProjectConfiguration
{
    public static class ProjectNames
    {
        public const string NAME_ROOT = "2D Game/";
        public const string NAME_ROOT_CHARACTER = NAME_ROOT + "Character/";
        public const string NAME_ROOT_PLAYER = NAME_ROOT + "Player/";
        public const string NAME_ROOT_ITEM = NAME_ROOT + "Item/";
        public const string NAME_ROOT_ITEM_COMBAT = NAME_ROOT_ITEM + "Combat/";
    }

    public static class ResourcesPath
    {
        public const string PATH_ROOT_RESOURCES = "Assets/Recurces/";

        public static class ItemsDataPath
        {
            public const string FILENAME_SAVE = "ItemsData.json";
            public const string PATH_ITEMS = "Items/";
            public const string PATH_ITEMS_SAVE = PATH_ITEMS + FILENAME_SAVE;
        }

        public static class InventoriesDataPath
        {
            public const string FILENAME_SAVE = "InventoryStorageData.json";
            public const string PATH_INVENTORY = "Inventory/";
            public const string PATH_INVENTORY_SAVE = PATH_INVENTORY + FILENAME_SAVE;
        }

        public static class ImagesPaths
        {
            public const string PATH_IMAGES = "Images/";
            public const string PATH_IMAGE_ITEM_STOCK = PATH_IMAGES + "Item stock.png";
        }
    }

    public static class ResourcesUtility//Addressable may improve this
    {
        public static Sprite GetSpriteItemStock()
        {
            FileUtility.RestoreDirectoryPath(PATH_ROOT_RESOURCES + ImagesPaths.PATH_IMAGES);

            return Resources.Load<Sprite>(ImagesPaths.PATH_IMAGE_ITEM_STOCK);
        }

        private static void SaveText(in string json, in string dir, in string fileName)
        {
            FileUtility.RestoreDirectoryPath(dir);

            File.WriteAllText(dir + fileName, json);

#if UNITY_EDITOR
            AssetDatabase.Refresh();
#endif
        }

        private static string LoadTextAsset(in string path)
        {
            if (!File.Exists(PATH_ROOT_RESOURCES + path))
            {
                Debug.LogError($"Can't load text asset to this path: \"{path}\"");
                return string.Empty;
            }

            if (Application.isPlaying)
                return Resources.Load<TextAsset>(path).text;
            else
                return File.ReadAllText(PATH_ROOT_RESOURCES + path);
        }

        public static class ItemsDataUtil
        {
            public static void SaveItemsInResurces(in string json)
            {
                SaveText(json, PATH_ROOT_RESOURCES + ItemsDataPath.PATH_ITEMS, ItemsDataPath.FILENAME_SAVE);
            }

            public static string LoadItemsFromResurces()
            {
                return LoadTextAsset(ItemsDataPath.PATH_ITEMS_SAVE);
            }
        }

        public static class InventoriesDataUtil
        {
            public static void SaveInventoryInResurces(in string json)
            {
                SaveText(json, PATH_ROOT_RESOURCES + InventoriesDataPath.PATH_INVENTORY, InventoriesDataPath.FILENAME_SAVE);
            }

            public static string LoadInventoryFromResurces()
            {
                return LoadTextAsset(InventoriesDataPath.PATH_INVENTORY_SAVE);
            }
        }
    }

    public static class FileUtility
    {
        public static void RestoreDirectoryPath(in string pathDirectory)
        {
            if (Directory.Exists(pathDirectory))
                return;

            Directory.CreateDirectory(pathDirectory);
#if UNITY_EDITOR
            AssetDatabase.Refresh();
#endif
        }
    }
}