﻿using Assets.Game.Scripts.Controllers;
using Assets.Game.Scripts.Controllers.AI.Pathfinding;
using System;
using System.ComponentModel;
using UnityEngine;

namespace Assets.Game.Scripts.Entities.ScriptableObjects.AI.StateMachines
{

    [CreateAssetMenu(menuName = "Data/AI/StateMachine/Rush State"), DisplayName("Rush")]
    public class RushMoveState : BaseState
    {
        private IPathfinding _pathFinding;
        private IMovement _movement;
        private PlayerController _player;

        public override void Initialize(GameObject parent, Blackboard blackboard)
        {
            base.Initialize(parent, blackboard);
            _player = FindObjectOfType<PlayerController>(); //might not be the best way but whatever

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
            _blackboard.DebugData.StateName = "Rush";
            if (_player == null) return;

            _pathFinding.UpdatePath(_player.transform.position);

            var direction = _pathFinding.GetDirection();
            _movement.SetMovementDirection(direction);
        }

        public override Type TrySwitchStates() => GetType();
    }
}