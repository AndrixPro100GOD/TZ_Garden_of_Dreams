using UnityEngine;

using static ProjectConfiguration.ProjectNames;

namespace Game2D.Gameplay.Inventory.UI
{
    [AddComponentMenu(NAME_ROOT_ITEM + "Inventory controller UI")]
    public class InventoryControllerUI : MonoBehaviour
    {
        [SerializeField]
        private InventoryStorage m_inventory;

        [SerializeField]
        public void Show()
        {
        }
    }
}