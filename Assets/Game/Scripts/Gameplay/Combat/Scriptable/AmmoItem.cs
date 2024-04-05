using UnityEngine;

using static ProjectConfiguration.ProjectConfiguration;

namespace Game2D.Gameplay.Combat.Scriptable
{
    [CreateAssetMenu(menuName = NAME_ROOT_ITEM + "Ammo item", fileName = "Ammo Item")]
    public class AmmoItem : ScriptableObject
    {
        [SerializeField]
        private float ammoDamage = 1;

        [SerializeField]
        private AmmoTypes ammoType = AmmoTypes.Mm9;

        [SerializeField]
        private AmmoEffect ammoEffect = AmmoEffect.Kinematic;

        public float GetDamage => ammoDamage;
        public AmmoTypes GetAmmoType => ammoType;
        public AmmoEffect GetAmmoEffect => ammoEffect;
    }
}