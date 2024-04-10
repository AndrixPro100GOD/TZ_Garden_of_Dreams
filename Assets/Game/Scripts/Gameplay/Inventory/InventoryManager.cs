using UnityEngine;

using static ProjectConfiguration.ProjectNames;

namespace Game2D.Gameplay.Inventory
{
    [AddComponentMenu(NAME_ROOT_ITEM + "In/// Получает все ненайденные <typeparamref name=\"TData\"/> данные при найденных ссылающих на них <typeparamref name=\"TAssetData\"/>ventoryStorage")]
    public class InventoryManager : MonoBehaviour
    {
        public static InventoryManager Instance { get; private set; }

        #region Init

        private void Awake()
        {
            if (Instance != null)
            {
                Debug.LogError($"В игре уже присувствует {nameof(InventoryManager)}");
                Destroy(this);
                return;
            }

            Instance = this;
            Initialize();
        }

        private void OnDisable()
        {
#if UNITY_EDITOR
            if (Application.isPlaying)
            {
                Debug.LogError($"{nameof(InventoryManager)} был отключен");
            }
#endif
        }

        private void Initialize()
        {
        }

        #endregion Init

        public void SaveInventory(string saveId, IInventoryStorage inventory)
        {
            //string json = ProjectConfiguration.ResourcesUtility.LoadInventoryFromResurces();

            //var inventory = string.IsNullOrEmpty(json) ? new List<ItemGuidData>() : JsonConvert.DeserializeObject<List<ItemGuidData>>(json).ToList();

            //string json = JsonConvert.SerializeObject(new InventorySaveData(saveId, inventory));
            // ProjectConfiguration.ResourcesUtility.SaveInventoryInResurces(json);
        }

        /*
        public IInventoryStorage LoadInventory(string saveId)
        {
            string json = ProjectConfiguration.RecurcesUtility.LoadInventoryFromResurces();
        }
        */
    }
}