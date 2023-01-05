using Assets.Game.Scripts.Controllers;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Game.Scripts.Entities.Pickups
{
    [RequireComponent(typeof(BoxCollider2D))]
    public abstract class CollectablePickup : MonoBehaviour
    {
        [SerializeField] private UnityEvent OnPickupEvent; //Triggers on a successful pickup


        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out PlayerController controller))
            {
                OnPickup(controller);
            }
        }

        public abstract void OnPickup(PlayerController controller);
    }
}