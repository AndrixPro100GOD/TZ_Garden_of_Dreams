using Game2D.Gameplay.Interactions;
using Game2D.Gameplay.Items;

using UnityEngine;

namespace Game2D.Assets.Game.Scripts.Gameplay.Items
{
    public class ItemController : MonoBehaviour
    {
#nullable enable
        public Vector2 ItemLookPosition { get => _itemLookPosition; set => SetLookPosition(value); }
        public IItem? CurrentItem { get => _currentItem; set => SetItem(value); }
        public bool HasItem => _currentItem != null;
        public bool IsItemInteractable => _currentItemInteractable != null;

        private Transform _myTransform;

        private IItem? _currentItem;
        private Vector2 _itemLookPosition;
        private IInteractableItem? _currentItemInteractable;
        private Transform? _currentItemTransform;

        #region Monobehavior

        private void Awake()
        {
            _myTransform = transform;
        }

        private void Update()
        {
            if (!HasItem)
            {
                return;
            }

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

        private void UpdatePosition()
        {
            if (_currentItemTransform == null)
            {
                return;
            }

            _currentItemTransform.position = _myTransform.position;
        }

        public void SetLookPosition(in Vector2 position)
        {
            _itemLookPosition = position;

            if (_currentItemTransform == null)
            {
                return;
            }

            _currentItemTransform.rotation = Quaternion.LookRotation((Vector2)_currentItemTransform.position - position, Vector3.right);
        }

        public void IteractionMain()
        {
            if (_currentItemInteractable == null)
            {
                Debug.Log("Not Interactable");//Допустим издаёт звук
            }
            else
            {
                _currentItemInteractable.Interacting();
            }
        }

        public void InteractionAlternative()
        {
            if (_currentItemInteractable == null)
            {
                Debug.Log("Not Interactable");//Допустим издаёт звук
            }
            else
            {
                _currentItemInteractable.InteractingAlt();
            }
        }

        public void DropItem()
        {
            if (!HasItem)
            {
                return;
            }

            _currentItemInteractable?.DropingItem();

            SetItem(null);
        }
    }

#nullable disable
}