using UnityEngine;

namespace Assets.Game.Scripts.Entities.Abilities
{
    public class EmptyAbility : Ability
    {
        public override void Activate(GameObject parent) { }

        public override void BeginCooldown(GameObject parent) { }

        public override void CancelAbility(GameObject parent) { }
    }
}