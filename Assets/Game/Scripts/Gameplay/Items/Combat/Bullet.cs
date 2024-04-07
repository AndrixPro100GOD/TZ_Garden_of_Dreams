using UnityEngine;

namespace Game2D.Gameplay.Items.Combat
{
    [RequireComponent(typeof(Collider2D))]
    public class Bullet : MonoBehaviour
    {
        [SerializeField]
        private float damage;

        [SerializeField]
        private Collider2D m_collider;

        [SerializeField]
        private Rigidbody2D m_rigidbody2D;

        #region Init

#if UNITY_EDITOR

        private void OnValidate()
        {
            InitMono();

            m_collider.isTrigger = false;

            m_rigidbody2D.isKinematic = true;
            //m_rigidbody2D.collisionDetectionMode
        }

#endif

        private void Reset()
        {
            InitMono();
        }

        private void InitMono()
        {
            if (m_collider == null)
                m_collider = GetComponent<Collider2D>();

            if (m_rigidbody2D == null)
                m_rigidbody2D = GetComponent<Rigidbody2D>();
        }

        #endregion Init

        // Use this for initialization
        private void Start()
        {
        }

        // Update is called once per frame
        private void Update()
        {
        }
    }
}