using Game2D.Gameplay.Items.Scriptable;

using UnityEngine;

using static ProjectConfiguration.ProjectNames;

namespace Game2D.Gameplay.Items.Combat.Scriptable
{
    /// <summary>
    /// Характеристики огнестрельного оружия.
    /// </summary>
    [CreateAssetMenu(menuName = NAME_ROOT_ITEM + "Firearm item", fileName = "Firearm Item")]
    public class FirearmItemData : ItemDataBase, IFirearmItem
    {
        [Min(0.1f)]
        [SerializeField]
        private float weaponDamageAmmoMultiplyer = 1f;

        [Min(1)]
        [SerializeField]
        private int weaponAmmoInMag = 30;

        [Min(0.01f)]
        [SerializeField]
        private float weaponReloadTime = 1f;

        [Min(0.01f)]
        [SerializeField]
        private float weaponFireRate = .5f;

        [Min(0.01f)]
        [SerializeField]
        private float weaponBulletSpeed = 10;

        [SerializeField]
        [Tooltip("Какой тип патрона использует оружие")]
        private AmmoTypes weaponUsesAmmoType = AmmoTypes.Mm9;

        public float GetDamageAmmoMultiplyer => weaponDamageAmmoMultiplyer;
        public int GetAmmoInMag => weaponAmmoInMag;
        public float GetReloadTime => weaponReloadTime;
        public float GetFireRate => weaponFireRate;
        public float GetBulletSpeed => weaponBulletSpeed;
        public AmmoTypes GetAmmoType => weaponUsesAmmoType;
    }
}