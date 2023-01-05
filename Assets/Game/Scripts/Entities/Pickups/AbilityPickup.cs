using Assets.Game.Scripts.Controllers;
using Assets.Game.Scripts.Entities.Abilities;
using UnityEngine;

namespace Assets.Game.Scripts.Entities.Pickups
{
    public class AbilityPickup : CollectablePickup
    {
        /*Component attached to an ingame ability pickup*/

        [SerializeField] private Ability ability; //Ability given to player upon pickup
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private bool isPrimaryAbility;

        private void Start()
        {
            spriteRenderer.sprite = ability.Sprite;
        }

        public override void OnPickup(PlayerController controller)
        {
            controller.AddAbility(ability, isPrimaryAbility);
            Destroy(gameObject);
        }
    }
}