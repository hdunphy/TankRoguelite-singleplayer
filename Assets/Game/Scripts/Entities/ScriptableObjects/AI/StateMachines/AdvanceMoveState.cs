using Assets.Game.Scripts.Controllers;
using Assets.Game.Scripts.Controllers.AI.Pathfinding;
using Assets.Game.Scripts.Entities.ScriptableObjects.AI.StateMachines.Helpers;
using System;
using System.ComponentModel;
using System.Linq;
using UnityEngine;

namespace Assets.Game.Scripts.Entities.ScriptableObjects.AI.StateMachines
{
    [CreateAssetMenu(menuName = "Data/AI/StateMachine/Advance"), DisplayName("Advance")]
    public class AdvanceMoveState : BaseState
    {
        [SerializeField] private MoveStrafeAvoidSMParameters parameters;

        private IPathfinding _pathFinding;
        private IMovement _movement;
        private PlayerController _player;
        private GameObject _parent;
        private Blackboard _blackboard;

        public override void Initialize(GameObject parent, Blackboard blackboard)
        {
            _parent = parent;
            _blackboard = blackboard;
            _player = FindObjectOfType<PlayerController>(); //might not be the best way but whatever

            if (!parent.TryGetComponent(out _pathFinding))
            {
                Debug.LogError("Missing Pathfinding Component");
            }
            if (!parent.TryGetComponent(out _movement))
            {
                Debug.LogError("Missing Movement Component");
            }

            _pathFinding.SetIsAtTarget((position, target) => Vector2.Distance(position, target) < parameters.AdvanceThreshold);
        }

        public override void RunBehavior()
        {
            if (_player == null) return;

            _pathFinding.UpdatePath(_player.transform.position);

            var direction = _pathFinding.GetDirection();
            _movement.SetMovementDirection(direction);
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
            if (distance < parameters.AdvanceThreshold)
            {
                return typeof(StrafeState);
            }

            return GetType();
        }
    }
}