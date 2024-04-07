using Game2D.Gameplay.Items;

using UnityEngine;
using UnityEngine.Events;

namespace Game2D.Gameplay.Interactions
{
    [RequireComponent(typeof(IItem))]
    public class InteractableItem : MonoBehaviour, IInteractableItem
    {
        [SerializeField]
        private UnityEvent m_OnInteract;

        [SerializeField]
        private UnityEvent m_OnInteractAlt;

        [SerializeField]
        private UnityEvent m_OnDropItem;

        [SerializeField]
        private UnityEvent m_OnDestroyItem;

        public IItemData GetItemData => GetComponent<IItemData>();

        public ItemBase GetItem => GetComponent<ItemBase>();

        public void Interacting()
        {
            m_OnInteract.Invoke();
        }

        public void InteractingAlt()
        {
            m_OnInteractAlt.Invoke();
        }

        public void DropingItem()
        {
            m_OnDropItem.Invoke();
        }

        public void DestroyItem()
        {
            m_OnDestroyItem.Invoke();
        }
    }
}