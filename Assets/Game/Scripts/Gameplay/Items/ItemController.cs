using Game2D.Gameplay.Interactions;
using Game2D.Gameplay.Items;

using UnityEngine;

namespace Game2D.Assets.Game.Scripts.Gameplay.Items
{
    public class ItemController : MonoBehaviour
    {
#nullable enable

        public IItem? CurrentItem { get => _currentItem; set => SetItem(value); }
        public bool HasItem => _currentItem != null;
        public bool IsItemInteractable => _currentItemInteractable != null;

        private Transform _myTransform;

        private IItem? _currentItem;
        private IInteractableItem? _currentItemInteractable;
        private Transform? _currentItemTransform;

        #region Monobehavior

        private void Awake()
        {
            _myTransform = transform;
        }

        private void Update()
        {
            if (!HasItem) return;

            UpdatePosition();
        }

        #endregion Monobehavior

        public void SetItem(in IItem? item)
        {
            if (item == null)
            {
                _currentItem = null;
                _currentItemInteractable = null;
                _currentItemTransform = null;
                return;
            }

            //Set the current item

            _currentItem = item;

            if (item is IInteractableItem interactable)
            {
                _currentItemInteractable = interactable;
            }

            _currentItemTransform = item.GetItem.transform;
        }

        public void UpdatePosition()
        {
            if (_currentItemTransform == null) return;

            _currentItemTransform.position = _myTransform.position;
        }

        public void IteractionMain()
        {
            if (_currentItemInteractable == null)
            {
                Debug.Log("Not Interactable");//Допустим издаёт звук
            }
            else
                _currentItemInteractable.Interacting();
        }

        public void InteractionAlternative()
        {
            if (_currentItemInteractable == null)
            {
                Debug.Log("Not Interactable");//Допустим издаёт звук
            }
            else
                _currentItemInteractable.InteractingAlt();
        }

        public void DropItem()
        {
            if (!HasItem) return;

            if (_currentItemInteractable != null)
                _currentItemInteractable.DropingItem();

            SetItem(null);
        }
    }

#nullable disable
}