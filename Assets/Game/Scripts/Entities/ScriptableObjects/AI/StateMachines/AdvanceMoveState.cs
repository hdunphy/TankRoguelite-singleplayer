﻿using Assets.Game.Scripts.Controllers;
using Assets.Game.Scripts.Controllers.AI;
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

        private MovementSMParameters _parameters;
        private IPathfinding _pathFinding;
        private IMovement _movement;
        private PlayerController _player;
        private EnemyShadowCollisionDetector _enemyShadow;

        public override void Initialize(GameObject parent, Blackboard blackboard, SMParameters stateMachineParams)
        {
            base.Initialize(parent, blackboard, stateMachineParams);

            _parameters = stateMachineParams as MovementSMParameters;
            if (_parameters == null)
            {
                Debug.LogError("State Machine Parameters of incorrect type");
            }
            _player = FindObjectOfType<PlayerController>(); //might not be the best way but whatever

            if (!parent.TryGetComponent(out _pathFinding))
            {
                Debug.LogError("Missing Pathfinding Component");
            }
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
            _blackboard.DebugData.StateName = "Advance";
            if (_player == null) return;

            _pathFinding.UpdatePath(_player.transform.position);

            var direction = _pathFinding.GetDirection();
            _movement.SetMovementDirection(direction);
        }

        public override Type TrySwitchStates()
        {
            //if bullet coming towards AI then switch to Avoid Bullets
            var dangerousObjects = _parameters.CheckForBullets(_enemyShadow.transform.position, _parent);
            if (dangerousObjects.Any())
            {
                return typeof(AvoidBulletState);
            }

            //if player is closer than threshold then strafe
            var distance = Vector2.Distance(_blackboard.PlayerPosition, _parent.transform.position);
            _blackboard.DebugData.Message = distance.ToString("F2");
            if (distance < _parameters.AdvanceThreshold - _parameters.AdvanceThresholdBuffer)
            {
                return typeof(StrafeState);
            }

            return GetType();
        }
    }
}