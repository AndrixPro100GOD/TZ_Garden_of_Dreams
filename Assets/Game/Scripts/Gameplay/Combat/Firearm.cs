using Game2D.Gameplay.Combat.Scriptable;

using UnityEngine;
using UnityEngine.Events;

using static ProjectConfiguration.ProjectConfiguration;

namespace Game2D.Gameplay.Combat
{
    [AddComponentMenu(NAME_ROOT_COMBAT + nameof(Firearm))]
    public class Firearm : MonoBehaviour
    {
        [SerializeField]
        private WeaponItem m_weaponItemData;

        [SerializeField]
        private Transform m_firePoint;

        public UnityEvent OnEmplyMag;

        private readonly int _currentAmmo;

        public void Shoot()
        {
            if (_currentAmmo < 1)
            {
                OnEmplyMag?.Invoke();
                return;
            }
        }

        public float forceSpeed = 30, transateSpeed = 3f;

        public void Reload()
        {
        }
    }
}