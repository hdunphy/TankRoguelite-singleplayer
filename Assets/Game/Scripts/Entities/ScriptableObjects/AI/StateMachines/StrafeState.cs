using Assets.Game.Scripts.Controllers;
using Assets.Game.Scripts.Controllers.AI;
using Assets.Game.Scripts.Entities.ScriptableObjects.AI.StateMachines.Helpers;
using Assets.Game.Scripts.Utilities;
using System;
using System.ComponentModel;
using System.Linq;
using UnityEngine;

namespace Assets.Game.Scripts.Entities.ScriptableObjects.AI.StateMachines
{
    [CreateAssetMenu(menuName = "Data/AI/StateMachine/Strafe"), DisplayName("Strafe")]
    public class StrafeState : BaseState
    {
        private const float SIZE_RADIUS = 1f;
        [SerializeField] private LayerMask obstacleLayerMask;
        [SerializeField] private float moveCheckDistance;
        [SerializeField] private MoveStrafeAvoidSMParameters parameters;

        private IMovement _movement;
        private bool _isNegativeYDireciton;
        private EnemyShadowCollisionDetector _enemyShadow;
        private float distance;

        public Vector2 MovementDirection { get; private set; }

        public override void Initialize(GameObject parent, Blackboard blackboard)
        {
            base.Initialize(parent, blackboard);

            if (!parent.TryGetComponent(out _movement))
            {
                Debug.LogError("Missing Movement Component");
            }
            _enemyShadow = parent.GetComponentInChildren<EnemyShadowCollisionDetector>();
            if (_enemyShadow == null)
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

            //Do I need to change the radius ?
            var hit = Physics2D.CircleCast(position, SIZE_RADIUS, MovementDirection, moveCheckDistance, obstacleLayerMask);
            // Check for obstacles in the way
            if (hit.collider != null)
            {
                // Reverse strafing direction if there's an obstacle
                MovementDirection = -MovementDirection;
                _isNegativeYDireciton = !_isNegativeYDireciton;
            }

            float moveToThresholdPercentage = (parameters.AdvanceThreshold - distance) / parameters.AdvanceThreshold;
            var directionFromPlayer = (Vector2)_parent.transform.position - _blackboard.PlayerPosition;
            var shiftedDirection = MovementDirection.RotateTowards(directionFromPlayer, moveToThresholdPercentage);

            var hit2 = Physics2D.CircleCast(position, SIZE_RADIUS, shiftedDirection, moveCheckDistance, obstacleLayerMask);
            // Check for obstacles in the way
            if (hit2.collider == null)
            {
                MovementDirection = shiftedDirection;
            }
            _movement.SetMovementDirection(MovementDirection);
        }

        public override Type TrySwitchStates()
        {
            //if bullet coming towards AI then switch to Avoid Bullets
            var dangerousObjects = parameters.CheckForBullets(_enemyShadow.transform.position, _parent);
            if (dangerousObjects.Any())
            {
                return typeof(AvoidBulletState);
            }

            //if player is closer than threshold then strafe
            distance = Vector2.Distance(_blackboard.PlayerPosition, _parent.transform.position);
            _blackboard.DebugData.Message = distance.ToString("F2");

            if (distance > parameters.AdvanceThreshold + parameters.AdvanceThresholdBuffer)
            {
                return typeof(AdvanceMoveState);
            }

            return GetType();
        }
    }
}