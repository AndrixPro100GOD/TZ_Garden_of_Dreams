using Game2D.Gameplay.Items.Combat.Scriptable;

using System.Collections;

using UnityEngine;

using static ProjectConfiguration.ProjectConfiguration;

namespace Game2D.Gameplay.Items.Combat
{
    [AddComponentMenu(NAME_ROOT_ITEM_COMBAT + nameof(Firearm))]
    public class Firearm : ItemBase
    {
        [SerializeField]
        private FirearmItemData m_weaponItemData;

        [SerializeField]
        private Transform m_firePoint;

        [SerializeField]
        private float bulletSpeed;

        [SerializeField]
        private float fireRate;

        public IFirearmItem GetFirearmItem => m_weaponItemData;
        public override IItemData GetItemData => m_weaponItemData;

        public bool IsReloading { get; private set; }

        private int _currentAmmo;

        private void Awake()
        {
            Initialization();
        }

        [ContextMenu("Initialization (Load data from scriptable)")]
        private void Initialization()
        {
            if (m_weaponItemData == null)
            {
                Debug.LogError("У огнестрела нету данных об предмете");
                return;
            }

            bulletSpeed = GetFirearmItem.GetBulletSpeed;
            fireRate = GetFirearmItem.GetFireRate;
            _currentAmmo = GetFirearmItem.GetAmmoInMag;//TODO: Ammo it's an item
        }

        #region Firearms Logic

        public void Shoot()
        {
            if (_currentAmmo < 1)
            {
                Reload();
                return;
            }
        }

        public void Reload()
        {
            if (!IsReloading)
            {
                _ = StartCoroutine(ReloadCorunite());
            }
        }

        private IEnumerator ReloadCorunite()
        {
            IsReloading = true;

            yield return new WaitForSeconds(GetFirearmItem.GetReloadTime);

            IsReloading = false;
        }

        #endregion Firearms Logic
    }
}