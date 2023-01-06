using Assets.Game.Scripts.Entities.Abilities;
using System;

namespace Assets.Game.Scripts.Entities
{
    [Serializable]
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

        public static TankModifiers operator +(TankModifiers a, TankModifiers b) => new()
        {
            PrimaryExtraAmmo = a.PrimaryExtraAmmo + b.PrimaryExtraAmmo,
            SecondaryExtraAmmo = a.SecondaryExtraAmmo + b.SecondaryExtraAmmo,
            BulletBounceNumber = a.BulletBounceNumber + b.BulletBounceNumber,
            BulletSpeedModifier = a.BulletSpeedModifier * b.BulletSpeedModifier,
            PrimaryFireAbilityModifier = a.PrimaryFireAbilityModifier + b.PrimaryFireAbilityModifier,
            SecondaryFireAbilityModifier = a.SecondaryFireAbilityModifier + b.SecondaryFireAbilityModifier,
            TankSpeedModifier = a.TankSpeedModifier * b.TankSpeedModifier,
        };
    }
}