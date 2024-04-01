using UnityEngine;

namespace Game2D
{
    public class FrameRateLocker : MonoBehaviour
    {
        [SerializeField]
        private int m_frameLock = 60;

        [SerializeField]
        private bool m_isEnable = true;

#if UNITY_EDITOR

        private void OnValidate()
        {
            if (Application.isPlaying && m_isEnable)
            {
                UpdateFrameLock();
            }
        }

#endif

        private void Awake()
        {
            if (m_isEnable)
            {
                UpdateFrameLock();
            }
        }

        private void UpdateFrameLock()
        {
            Application.targetFrameRate = m_frameLock;
        }
    }
}