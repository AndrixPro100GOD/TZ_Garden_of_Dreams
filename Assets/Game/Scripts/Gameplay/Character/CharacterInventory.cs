using Game2D.Gameplay.Inventory;

using UnityEngine;

using static ProjectConfiguration.ProjectNames;

namespace Game2D.Gameplay.Character
{
    [DisallowMultipleComponent]
    [AddComponentMenu(NAME_ROOT_CHARACTER + "Character inventory")]
    public class CharacterInventory : MonoBehaviour
    {
        [SerializeField]
        private bool m_isDropInventory = true;

        [SerializeField]
        private Inventory.InventoryStorage Inventory;

        public IInventoryStorage GetInventoryStorage => Inventory;
    }
}