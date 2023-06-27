using Assets.Game.Scripts.Utilities.MonoBehaviours;
using System.Collections;
using UnityEngine;

namespace Assets.Game.Scripts.Controllers.Manager
{
    public class LayerMaskSingleton : Singleton<LayerMaskSingleton>
    {
        [SerializeField]
        private LayerMask wallLayer;

        [SerializeField, Tooltip("Walls and Projectiles")] 
        private LayerMask obstacleLayers;

        [SerializeField] private LayerMask shootingLayers;

        [SerializeField, Tooltip("Projectiles")] private LayerMask projectileLayers;
        [SerializeField, Tooltip("Tanks and Shadow Colliders")] private LayerMask shadowColliderLayers;
    }
}