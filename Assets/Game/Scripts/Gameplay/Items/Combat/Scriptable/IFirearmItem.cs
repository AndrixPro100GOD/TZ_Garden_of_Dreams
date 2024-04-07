namespace Game2D.Gameplay.Items.Combat.Scriptable
{
    public interface IFirearmItem : IItemData
    {
        public float GetDamageAmmoMultiplyer { get; }
        public int GetAmmoInMag { get; }
        public float GetReloadTime { get; }
        public float GetFireRate { get; }
        public float GetBulletSpeed { get; }
        public AmmoTypes GetAmmoType { get; }
    }
}