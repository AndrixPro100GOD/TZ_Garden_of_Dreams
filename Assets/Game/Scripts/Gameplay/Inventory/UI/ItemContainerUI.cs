using Game2D.Gameplay.Items;

using UnityEngine;
using UnityEngine.UI;

using static ProjectConfiguration.ProjectNames;

namespace Game2D.Gameplay.Inventory.UI
{
    [AddComponentMenu(NAME_ROOT_ITEM + "Item conteiner UI")]
    public class ItemContainerUI : MonoBehaviour
    {
        [SerializeField]
        private Image m_image;

        public int StackCountMax { get; set; }
        private IItem ContainerItem { get; set; }

        [SerializeField]
        public void SetItem(IItem item)
        {
            ContainerItem = item;
            m_image.sprite = item.GetItemData.GetSprite;
        }

        public bool SetStackMax()
        {
            return true;
        }
    }
}