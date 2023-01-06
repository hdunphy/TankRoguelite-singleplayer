using UnityEngine;

namespace Assets.Game.Scripts.Entities
{
    [CreateAssetMenu(menuName = "Data/Tank Data")]
    public class TankData : ScriptableObject
    {
        [Header("Combat")]
        [SerializeField] private int health;
        [SerializeField] private int startingPrimaryAmmo;
        [SerializeField] private int startingSecondaryAmmo;
        [Header("Movement")]
        [SerializeField] private float moveSpeed;
        [SerializeField] private float tankRotationSpeed;
        [SerializeField] private float gunRotationSpeed;

        public int Health { get => health; }
        public int TotalPrimaryAmmo { get => startingPrimaryAmmo + TankModifiers.PrimaryExtraAmmo; }
        public int TotalSecondaryAmmo { get => startingSecondaryAmmo + TankModifiers.SecondaryExtraAmmo; }
        public float GunRotationSpeed { get => gunRotationSpeed; }
        public float MoveSpeed { get => moveSpeed * TankModifiers.TankSpeedModifier; }
        public float TankRotationSpeed { get => tankRotationSpeed; }
        public TankModifiers TankModifiers { get; private set; } = new();

        public void ResetModifier()
        {
            TankModifiers = new();
        }

        public void AddModifiers(TankModifiers modifiers)
        {
            TankModifiers += modifiers;
        }
    }
}