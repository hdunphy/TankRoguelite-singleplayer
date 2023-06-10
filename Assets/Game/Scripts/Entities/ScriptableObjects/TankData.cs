using UnityEngine;

namespace Assets.Game.Scripts.Entities.ScriptableObjects
{

    [CreateAssetMenu(menuName = "Data/Tank Data")]
    public class TankData : ScriptableObject
    {
        [Header("Combat")]
        [SerializeField] protected int health;
        [SerializeField] protected int startingPrimaryAmmo;
        [SerializeField] protected int startingSecondaryAmmo;
        [Header("Movement")]
        [SerializeField] protected float moveSpeed;
        [SerializeField] protected float tankRotationSpeed;
        [SerializeField] protected float gunRotationSpeed;

        public int Health { get => health; }
        public int TotalPrimaryAmmo { get => startingPrimaryAmmo + TankModifiers.PrimaryExtraAmmo; }
        public int TotalSecondaryAmmo { get => startingSecondaryAmmo + TankModifiers.SecondaryExtraAmmo; }
        public float GunRotationSpeed { get => gunRotationSpeed; }
        public float MoveSpeed { get => moveSpeed * TankModifiers.TankSpeedModifier; }
        public float TankRotationSpeed { get => tankRotationSpeed; }
        public TankModifiers TankModifiers { get; protected set; } = new();

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