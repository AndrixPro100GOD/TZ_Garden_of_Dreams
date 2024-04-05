using UnityEngine;

using static ProjectConfiguration.ProjectConfiguration;

namespace Game2D.Gameplay.Character
{
    [AddComponentMenu(NAME_ROOT_CHARACTER + "Character manager")]
    [DisallowMultipleComponent]
    [RequireComponent(typeof(CharacterMovementController))]
    public class CharacterManager : MonoBehaviour
    {
        [SerializeField]
        private CharacterMovementController m_characterMovementController;

        public CharacterMovementController GetCharacterMovement => m_characterMovementController;

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