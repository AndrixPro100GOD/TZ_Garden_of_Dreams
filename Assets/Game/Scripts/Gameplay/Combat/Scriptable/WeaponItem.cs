using Game2D.Gameplay.Items.Scriptable;

using UnityEngine;

using static ProjectConfiguration.ProjectConfiguration;

namespace Game2D.Gameplay.Combat.Scriptable
{
    /// <summary>
    /// Характеристики оружия.
    /// </summary>
    [CreateAssetMenu(menuName = NAME_ROOT_ITEM + "Weapon item", fileName = "Weapon Item")]
    public class WeaponItem : ItemBase
    {
        [Min(0.1f)]
        [SerializeField]
        private float weaponDamageAmmoMultiplyer = 1f;

        [Min(1)]
        [SerializeField]
        private int weaponAmmoInMag = 30;

        [SerializeField]
        [Tooltip("Какой тип патрона использует оружие")]
        private AmmoTypes weaponUsesAmmoType = AmmoTypes.Mm9;

        public float GetDamageAmmoMultiplyer => weaponDamageAmmoMultiplyer;
        public int GetAmmoInMag => weaponAmmoInMag;
        public AmmoTypes GetAmmoType => weaponUsesAmmoType;
    }
}