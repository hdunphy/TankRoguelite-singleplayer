using Assets.Game.Scripts.Entities.Abilities;

namespace Assets.Game.Scripts.Entities
{
    public class TankModifiers
    {
        public int PrimaryExtraAmmo { get; set; }
        public int SecondaryExtraAmmo { get; set; }
        public int BulletBounceNumber { get; set; }
        public float BulletSpeedModifier { get; set; }
        public AbilityModifier PrimaryFireAbilityModifier { get; set; }
        public AbilityModifier SecondaryFireAbilityModifier { get; set; }
        public float TankSpeedModifier { get; set; }

        public TankModifiers() 
        {
            PrimaryExtraAmmo = 0;
            SecondaryExtraAmmo = 0;
            BulletBounceNumber = 1;
            BulletSpeedModifier = 1;
            PrimaryFireAbilityModifier = new();
            SecondaryFireAbilityModifier = new();
            TankSpeedModifier = 1;
        }
    }
}