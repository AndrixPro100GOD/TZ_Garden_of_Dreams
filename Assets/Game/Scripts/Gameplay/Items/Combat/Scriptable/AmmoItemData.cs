using Game2D.Gameplay.Items.Scriptable;

using UnityEngine;

using static ProjectConfiguration.ProjectNames;

namespace Game2D.Gameplay.Items.Combat.Scriptable
{
    [CreateAssetMenu(menuName = NAME_ROOT_ITEM + "Ammo item", fileName = "Ammo Item")]
    public class AmmoItemData : ItemDataBase
    {
        [SerializeField]
        private float ammoDamage = 1;

        [SerializeField]
        private AmmoTypes ammoType = AmmoTypes.Mm9;

        [SerializeField]
        private AmmoEffectTypes ammoEffect = AmmoEffectTypes.Kinematic;

        public float GetDamage => ammoDamage;
        public AmmoTypes GetAmmoType => ammoType;
        public AmmoEffectTypes GetAmmoEffect => ammoEffect;
    }
}