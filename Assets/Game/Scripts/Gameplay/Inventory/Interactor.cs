using Game2D.Gameplay.Items;

using UnityEngine;

namespace Game2D.Gameplay.Inventory
{
    [RequireComponent(typeof(CircleCollider2D))]
    public class Interactor : MonoBehaviour
    {
        [SerializeField, HideInInspector]
        private CircleCollider2D _circleCollider2D;

#nullable enable
        private PickupableItem? _currentItem;
#nullable disable

        #region Init

#if UNITY_EDITOR

        private void OnValidate()
        {
            InitMono();

            _circleCollider2D.isTrigger = true;
        }

#endif

        private void Reset()
        {
            InitMono();
        }

        private void Awake()
        {
            InitMono();
        }

        private void InitMono()
        {
            if (_circleCollider2D == null)
                _circleCollider2D = GetComponent<CircleCollider2D>();
        }

        #endregion Init

        private void OnTriggerEnter2D(Collider2D collision)
        {
            //if (collision.TryGetComponent(out PickupableItem item))
            //  _currentItem
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
        }
    }
}