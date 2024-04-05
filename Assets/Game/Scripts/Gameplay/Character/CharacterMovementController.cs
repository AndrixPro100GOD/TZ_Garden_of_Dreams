using System;

using UnityEngine;

using static ProjectConfiguration.ProjectConfiguration;

namespace Game2D.Gameplay.Character
{
    public delegate Vector2 MovementDirectionDelegate();

    /// <summary>
    /// ”правление телом персонажа дл€ перемещени€
    /// </summary>
    [DisallowMultipleComponent]
    [AddComponentMenu(NAME_ROOT_CHARACTER + "Character movement")]
    [RequireComponent(typeof(CircleCollider2D), typeof(Rigidbody2D))]
    public class CharacterMovementController : MonoBehaviour
    {
        [SerializeField]
        private float m_moveSpeed = 10;

        public event Action OnDestinationReached;

        private MovePoint Point;
        private Vector2 LastMoveDirection { get; set; }
#nullable enable
        private MovementDirectionDelegate? DirectionMovement { get; set; }
#nullable disable

        [SerializeField, HideInInspector]
        private Transform _characterTransform;

        [SerializeField, HideInInspector]
        private Rigidbody2D _characterRigidbody;//¬ыбрал Rigidbody2D так как, будет быстрее написать логику передвижений и заимодействи€ с миром.

        private DirectionMovementType _directionMovementType;

        #region MonoBehaviour with Init

#if UNITY_EDITOR

        private void OnValidate()
        {
            Initialize();

            if (_characterRigidbody != null)
            {
                _characterRigidbody.gravityScale = 0;
                _characterRigidbody.freezeRotation = true;
            }
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
            if (_characterTransform == null)
            {
                _characterTransform = transform;
            }

            if (_characterRigidbody == null)
            {
                _characterRigidbody = GetComponent<Rigidbody2D>();
            }
        }

        private void FixedUpdate()
        {
            switch (_directionMovementType)
            {
                case DirectionMovementType.Move:
                    Move();
                    break;

                case DirectionMovementType.MoveToPosition:
                    MoveToPoint();
                    break;
            }
        }

        #endregion MonoBehaviour with Init

        #region Initializon of MoveDirectionDelegate

        public void SetDirectionDelegate(MovementDirectionDelegate moveDireciton, DirectionMovementType directionMovement)
        {
            _directionMovementType = directionMovement;
            DirectionMovement = moveDireciton;
        }

        public void ClearDirectionDelegate()
        {
            DirectionMovement = null;
            StopMovement();
        }

        #endregion Initializon of MoveDirectionDelegate

        #region FixedUpdate Movement

        private void MoveCharacter(Vector2 toDirection)
        {
            LastMoveDirection = toDirection.normalized;
            _characterRigidbody.velocity = m_moveSpeed * Time.fixedDeltaTime * LastMoveDirection;
        }

        private void StopMovement()
        {
            _characterRigidbody.velocity = Vector2.zero;
        }

        /// <summary>
        /// ѕеремещение при помощи утсновленого делегата <see cref="MovementDirectionDelegate"/>, если отцувствует то перемещени€ не произойдЄт
        /// </summary>
        private void Move()
        {
            if (DirectionMovement == null)
            {
                return;
            }

            MoveCharacter(DirectionMovement.Invoke());
        }

        /// <summary>
        /// ѕеремещение в определенную на определенную позицию при помощи утсновленого делегата <see cref="MovementDirectionDelegate"/>, если отцувствует то перемещени€ не произойдЄт
        /// </summary>
        private void MoveToPoint()
        {
            Vector2 point;

            if (DirectionMovement != null)
            {
                point = DirectionMovement.Invoke();
            }
            else if (Point.HasPosition)
            {
                point = Point.Position;
            }
            else
            {
                return;
            }

            if (Vector2.Distance(point, _characterTransform.position) < 0.01f)
            {
                ClearDirectionDelegate();
                Point.ClearPoint();
                OnDestinationReached?.Invoke();
                StopMovement();
                return;
            }

            MoveCharacter(point - (Vector2)_characterTransform.position);
        }

        public void MoveToPosition(Vector2 position)
        {
            ClearDirectionDelegate();

            _directionMovementType = DirectionMovementType.MoveToPosition;

            Point.Position = position;
        }

        #endregion FixedUpdate Movement

        public enum DirectionMovementType : byte
        {
            Move,
            MoveToPosition
        }

        private struct MovePoint
        {
            public Vector2 Position { get; set; }

            public readonly bool HasPosition => Position != Vector2.zero;

            public void SetPosition(in Vector2 point)
            {
                Position = point;
            }

            public void ClearPoint()
            {
                Position = Vector2.zero;
            }
        }
    }
}