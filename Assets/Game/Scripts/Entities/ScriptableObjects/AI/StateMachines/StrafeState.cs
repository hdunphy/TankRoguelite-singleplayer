using Assets.Game.Scripts.Controllers;
using Assets.Game.Scripts.Controllers.AI;
using Assets.Game.Scripts.Entities.ScriptableObjects.AI.StateMachines.Helpers;
using System;
using System.ComponentModel;
using System.Linq;
using UnityEngine;

namespace Assets.Game.Scripts.Entities.ScriptableObjects.AI.StateMachines
{
    [CreateAssetMenu(menuName = "Data/AI/StateMachine/Strafe"), DisplayName("Strafe")]
    public class StrafeState : BaseState
    {
        [SerializeField] private LayerMask obstacleLayerMask;
        [SerializeField] private float moveCheckDistance;
        [SerializeField] private MoveStrafeAvoidSMParameters parameters;

        private IMovement _movement;
        private bool _isNegativeYDireciton;
        private EnemyShadowCollisionDetector _enemyShadow;

        public Vector2 MovementDirection { get; private set; }

        public override void Initialize(GameObject parent, Blackboard blackboard)
        {
            base.Initialize(parent, blackboard);

            if (!parent.TryGetComponent(out _movement))
            {
                Debug.LogError("Missing Movement Component");
            }
            _enemyShadow = parent.GetComponentInChildren<EnemyShadowCollisionDetector>();
            if (_enemyShadow != null)
            {
                Debug.LogError("Missing Enemy Shadow Component");
            }
        }

        public override void RunBehavior()
        {
            _blackboard.DebugData.StateName = "Strafe";
            Vector2 position = _parent.transform.position;
            Vector2 direction = _blackboard.PlayerPosition - position;
            direction.Normalize();

            MovementDirection = _isNegativeYDireciton ? new Vector2(-direction.y, direction.x) : new Vector2(direction.y, -direction.x);
            MovementDirection.Normalize();

            var hit = Physics2D.Raycast(position, MovementDirection, moveCheckDistance, obstacleLayerMask);
            // Check for obstacles in the way
            if (hit.collider != null)
            {
                // Reverse strafing direction if there's an obstacle
                MovementDirection = -MovementDirection;
                _isNegativeYDireciton = !_isNegativeYDireciton;
            }

            _movement.SetMovementDirection(MovementDirection);
        }

        public override Type TrySwitchStates()
        {
            //if bullet coming towards AI then switch to Avoid Bullets
            var dangerousObjects = parameters.CheckForBullets(_enemyShadow.transform.position, _enemyShadow.gameObject);
            if (dangerousObjects.Any())
            {
                return typeof(AvoidBulletState);
            }

            //if player is closer than threshold then strafe
            var distance = Vector2.Distance(_blackboard.PlayerPosition, _parent.transform.position);
            if (distance > parameters.AdvanceThreshold)
            {
                return typeof(AdvanceMoveState);
            }

            return GetType();
        }
    }
}