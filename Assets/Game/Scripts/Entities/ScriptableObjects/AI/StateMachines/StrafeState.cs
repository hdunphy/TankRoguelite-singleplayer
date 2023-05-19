using Assets.Game.Scripts.Controllers;
using Assets.Game.Scripts.Entities.ScriptableObjects.AI.StateMachines.Helpers;
using System;
using System.Linq;
using UnityEngine;

namespace Assets.Game.Scripts.Entities.ScriptableObjects.AI.StateMachines
{
    [CreateAssetMenu(menuName = "Data/AI/StateMachine/Strafe")]
    public class StrafeState : BaseState
    {
        [SerializeField] private LayerMask obstacleLayerMask;
        [SerializeField] private float moveCheckDistance;
        [SerializeField] private MoveStrafeAvoidSMParameters parameters;

        private IMovement _movement;
        private GameObject _parent;
        private Blackboard _blackboard;
        private int _direction;

        public override void Initialize(GameObject parent, Blackboard blackboard)
        {
            _direction = 1;
            _parent = parent;
            _blackboard = blackboard;

            if (!parent.TryGetComponent(out _movement))
            {
                Debug.LogError("Missing Movement Component");
            }
        }

        public override void RunBehavior()
        {
            Vector2 position = _parent.transform.position;
            Vector2 direction = _blackboard.PlayerPosition - position;
            direction.Normalize();

            Vector2 desiredPosition = _blackboard.PlayerPosition + direction * moveCheckDistance;
            Vector2 movementDirection = (desiredPosition - position) * _direction;
            movementDirection.Normalize();

            var hit = Physics2D.Raycast(position, movementDirection, moveCheckDistance, obstacleLayerMask);
            // Check for obstacles in the way
            if (hit.collider != null)
            {
                // Reverse strafing direction if there's an obstacle
                movementDirection = -movementDirection;
                _direction = -_direction;
            }

            _movement.SetMovementDirection(movementDirection);
        }

        public override Type TrySwitchStates()
        {
            //if bullet coming towards AI then switch to Avoid Bullets
            var dangerousObjects = parameters.CheckForBullets(_parent.transform.position);
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