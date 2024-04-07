using Game2D.Gameplay.Items.Scriptable;

using Unity.Plastic.Newtonsoft.Json;

using UnityEngine;

namespace Game2D.Gameplay.Items
{
    [System.Serializable]
    public class ItemGuidData
    {
        [JsonConstructor]
        public ItemGuidData(string itemGUID, string itemName)
        {
            ItemGUID = itemGUID;
            ItemName = itemName;
        }

        public ItemGuidData(ItemDataBase item)
        {
            ItemGUID = item.GetGUID;
            ItemName = item.GetName;
        }

        [JsonProperty]
        public string ItemGUID { get; set; }

        [JsonProperty]
        public string ItemName { get; set; }

#if UNITY_EDITOR

        [JsonIgnore]
        [Tooltip("Проверка существует ли такой Asset в проекте, нужен для отображения в отдельном окне")]
        public bool IsAssetExist => !string.IsNullOrEmpty(AssetPath);

        [JsonIgnore]
        [Tooltip("Путь к файлу этого Asset, нужен для отображения в отдельном окне")]
        public string AssetPath { get; set; }

#endif
    }
}