using Assets.Game.Scripts.Controllers;
using Assets.Game.Scripts.Controllers.AI.Pathfinding;
using System;
using UnityEngine;

namespace Assets.Game.Scripts.Entities.ScriptableObjects.AI.StateMachines
{
    [CreateAssetMenu(menuName = "Data/AI/StateMachine/Rush State")]
    public class RushMoveState : BaseState
    {
        private IPathfinding _pathFinding;
        private IMovement _movement;
        private PlayerController _player;

        public override void Initialize(GameObject parent)
        {
            _player = FindObjectOfType<PlayerController>();

            if (!parent.TryGetComponent(out _pathFinding))
            {
                Debug.LogError("Missing Pathfinding Component");
            }
            if (!parent.TryGetComponent(out _movement))
            {
                Debug.LogError("Missing Movement Component");
            }
        }

        public override void RunBehavior()
        {
            if (_player is null) return;

            _pathFinding.UpdatePath(_player.transform.position);

            var direction = _pathFinding.GetDirection();
            _movement.SetMovementDirection(direction);
        }

        public override Type TrySwitchStates(StateInputs inputs) => GetType();
    }
}