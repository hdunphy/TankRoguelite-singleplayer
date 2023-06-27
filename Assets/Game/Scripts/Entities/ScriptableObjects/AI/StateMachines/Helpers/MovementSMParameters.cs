using Assets.Game.Scripts.Controllers.Manager;
using Assets.Game.Scripts.Entities.Interfaces;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Game.Scripts.Entities.ScriptableObjects.AI.StateMachines.Helpers
{
    [CreateAssetMenu(menuName = "Data/AI/StateMachine/Helpers/Move Strafe Avoid Parameters")]
    public class MovementSMParameters : SMParameters
    {
        [SerializeField] private float advanceThreshold;
        [SerializeField] private float advanceThresholdBuffer;
        [SerializeField] private float ammoDetectionRadius;

        public float AmmoDetectionRadius => ammoDetectionRadius;
        public float AdvanceThreshold => advanceThreshold;
        public float AdvanceThresholdBuffer => advanceThresholdBuffer;

        /// <summary>
        /// Returns list of dangerous objects that could affect the origin point
        /// </summary>
        /// <param name="origin"></param>
        /// <returns></returns>
        public List<IAmmo> CheckForBullets(Vector3 origin, GameObject targetedGameObject)
        {
            var dangerousObjects = new List<IAmmo>();
            var collisions = Physics2D.OverlapCircleAll(origin, ammoDetectionRadius, LayerMaskSingleton.Instance.ProjectileLayers);

            foreach (var collision in collisions)
            {
                if (collision.TryGetComponent(out IAmmo ammo) && ammo.WillDamage(origin, targetedGameObject, LayerMaskSingleton.Instance.ShadowColliderLayers))
                {
                    dangerousObjects.Add(ammo);
                }
            }

            return dangerousObjects;
        }
    }
}