using Game2D.Gameplay.Character;
using Game2D.Gameplay.Inventory;

using System;

using UnityEngine;

using static ProjectConfiguration.ProjectNames;

namespace Game2D.Gameplay.Players
{
    [AddComponentMenu(NAME_ROOT_PLAYER + "Player Controller")]
    public class PlayerController : MonoBehaviour, IInventorySaver
    {
        public const string ID_SAVE = "Player";

        [SerializeField]
        private CharacterManager m_character;

        public Vector2 PlayerInputMovement { get; private set; }
        public Vector2 PlayerCursorPosition { get; private set; }

        public string IdSave => ID_SAVE;

        public IInventoryStorage InventoryStorage => throw new NotImplementedException();

        private Camera _mainCamera;

        // Use this for initialization
        private void Start()
        {
            if (m_character != null)
            {
                InitializeCharacter();
            }

            _mainCamera = Camera.main;

            Debug.Assert(m_character != null, "У игрока нету персонажа");
        }

        // Update is called once per frame
        private void Update()
        {
            if (m_character == null)
            {
                return;
            }
        }

        #region Player Input

        /// <summary>
        /// Считывает весь ввод игрока для перемещения
        /// </summary>
        [Obsolete("Should use UnityInputSystem for future")]//Для экономии времени использовал Legacy Input Manager
        private Vector2 ReadInputMovement()
        {
            return PlayerInputMovement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        }

        private Vector2 ReadMousePosition()
        {
            return (Vector2)_mainCamera.ScreenToWorldPoint(Input.mousePosition);
        }

        #endregion Player Input

        public void SetCharacter(in CharacterManager character)
        {
            DeinitializeCharacter();

            m_character = character;

            InitializeCharacter();
        }

        private void InitializeCharacter()
        {
            if (m_character != null)
            {
                m_character.GetCharacterMovement.SetDirectionDelegate(ReadInputMovement, 0);
            }

            Debug.Assert(m_character != null, "Перонаж не установлен");
        }

        private void DeinitializeCharacter()
        {
            if (m_character != null)
            {
                m_character.GetCharacterMovement.ClearDirectionDelegate();
            }
        }
    }
}