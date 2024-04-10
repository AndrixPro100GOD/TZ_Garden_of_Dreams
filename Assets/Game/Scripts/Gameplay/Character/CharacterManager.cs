using Game2D.Gameplay.Inventory;

using UnityEngine;

using static ProjectConfiguration.ProjectNames;

namespace Game2D.Gameplay.Character
{
    [AddComponentMenu(NAME_ROOT_CHARACTER + "Character manager")]
    [DisallowMultipleComponent]
    [RequireComponent(typeof(CharacterMovementController), typeof(CharacterInventory))]
    public class CharacterManager : MonoBehaviour, ICharacter
    {
        [SerializeField]
        private CharacterMovementController m_characterMovementController;

        [SerializeField]
        private CharacterItemController m_characterItemController;

        [SerializeField]
        private CharacterInventory m_characterInventory;

        public CharacterMovementController GetCharacterMovement => m_characterMovementController;

        public IInventoryStorage InventoryStorage => m_characterInventory.GetInventoryStorage;

        #region Init

#if UNITY_EDITOR

        private void OnValidate()
        {
            Initialize();
        }

#endif

        private void Reset()
        {
            Initialize();
        }

        private void Awake()
        {
            Initialize();
        }

        private void Initialize()
        {
            if (m_characterMovementController == null)
            {
                m_characterMovementController = GetComponent<CharacterMovementController>();
            }
        }

        #endregion Init
    }
}