using System;
using UnityEngine;

namespace Assets.Game.Scripts.Entities.Abilities
{
    [Serializable]
    public class AbilityModifier
    {
        public float ActionTimeModifer { get; set; }
        public float CoolDownTimeModifer { get; set; }

        public AbilityModifier()
        {
            ActionTimeModifer = 1;
            CoolDownTimeModifer = 1;
        }

        public static AbilityModifier operator +(AbilityModifier a, AbilityModifier b) => new()
        {
            ActionTimeModifer = a.ActionTimeModifer * b.ActionTimeModifer,
            CoolDownTimeModifer = a.CoolDownTimeModifer * b.CoolDownTimeModifer,
        };
    }

    public abstract class Ability : ScriptableObject
    {
        [SerializeField, Tooltip("How long it takes for the ability to complete")] private float actionTime;
        [SerializeField, Tooltip("How long before you can use the ability again")] private float cooldownTime;
        [SerializeField] private Sprite sprite;

        public AbilityModifier AbilityModifier { get; set; } = new();
        public bool HasUse { get; set; } //Keeps track if this ability is enable/disabled. True if can be used
        public bool IsButtonPressed { get; set; } //Keep track if input is being pressed
        public float ActionTime { get => actionTime; } //public member to get the action time
        public float CooldownTime { get => cooldownTime; } //public memeber to get the cooldown time
        public Sprite Sprite { get => sprite; } //The sprite to display for this ability
        public object Parameter { get; set; } //Used for additional info required

        /// <summary>
        /// Want the default value of hasUse to be true
        /// </summary>
        private void OnEnable()
        {
            HasUse = true;
        }

        /// <summary>
        /// Abstract function called on ability activation
        /// </summary>
        /// <param name="parent">the game object that this ability is connected to</param>
        public abstract void Activate(GameObject parent);

        /// <summary>
        /// Once ability is done and the cooldown timer starts, this function gets called
        /// </summary>
        /// <param name="parent">the game object that this ability is connected to</param>
        public abstract void BeginCooldown(GameObject parent);

        /// <summary>
        /// Call to cancel the current ability
        /// </summary>
        /// <param name="parent">the game object that this ability is connected to</param>
        public abstract void CancelAbility(GameObject parent);
    }
}