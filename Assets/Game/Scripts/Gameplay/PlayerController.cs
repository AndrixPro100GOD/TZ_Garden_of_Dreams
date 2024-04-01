using System;

using UnityEngine;

namespace Game2D.Gameplay
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField]
        private CharacterManager m_character;

        public Vector2 PlayerInputMovement { get; private set; }

        // Use this for initialization
        private void Start()
        {
            if (m_character != null)
            {
                InitializeCharacter();
            }

            Debug.Assert(m_character != null, "У игрока нету персонажа");
        }

        // Update is called once per frame
        private void Update()
        {
            _ = ReadInputMovement();

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