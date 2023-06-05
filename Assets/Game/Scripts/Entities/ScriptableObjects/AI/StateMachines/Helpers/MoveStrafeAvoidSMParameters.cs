using Assets.Game.Scripts.Entities.Interfaces;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Game.Scripts.Entities.ScriptableObjects.AI.StateMachines.Helpers
{
    [CreateAssetMenu(menuName = "Data/AI/StateMachine/Helpers/Move Strafe Avoid Parameters")]
    public class MoveStrafeAvoidSMParameters : ScriptableObject
    {
        [SerializeField] private float advanceThreshold;
        [SerializeField] private float ammoDetectionRadius;
        [SerializeField] private LayerMask ammoLayerMask;
        [SerializeField] private LayerMask shadowColliderMask;

        public float AmmoDetectionRadius => ammoDetectionRadius;
        public LayerMask AmmoLayerMask => ammoLayerMask;
        public float AdvanceThreshold => advanceThreshold;

        /// <summary>
        /// Returns list of dangerous objects that could affect the origin point
        /// </summary>
        /// <param name="origin"></param>
        /// <returns></returns>
        public List<IAmmo> CheckForBullets(Vector3 origin, GameObject targetedGameObject)
        {
            var dangerousObjects = new List<IAmmo>();
            var collisions = Physics2D.OverlapCircleAll(origin, ammoDetectionRadius, ammoLayerMask);

            foreach (var collision in collisions)
            {
                if (collision.TryGetComponent(out IAmmo ammo) && ammo.WillDamage(origin, targetedGameObject, shadowColliderMask))
                {
                    dangerousObjects.Add(ammo);
                }
            }

            return dangerousObjects;
        }
    }
}