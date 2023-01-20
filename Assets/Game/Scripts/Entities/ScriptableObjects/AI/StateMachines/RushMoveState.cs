using Assets.Game.Scripts.Controllers;
using Assets.Game.Scripts.Controllers.AI.Pathfinding;
using Assets.Game.Scripts.Entities.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Game.Scripts.Entities.ScriptableObjects.AI.StateMachines
{
    [CreateAssetMenu(menuName = "Data/AI/StateMachine/Strafe")]
    public class StrafeState : BaseState
    {
        public override void Initialize(GameObject parent, Blackboard blackboard)
        {
            throw new NotImplementedException();
        }

        public override void RunBehavior()
        {
            throw new NotImplementedException();
        }

        public override Type TrySwitchStates()
        {
            throw new NotImplementedException();
        }
    }

    [CreateAssetMenu(menuName = "Data/AI/StateMachine/Avoid Bullet")]
    public class AvoidBulletState : BaseState
    {
        public override void Initialize(GameObject parent, Blackboard blackboard)
        {
            throw new NotImplementedException();
        }

        public override void RunBehavior()
        {
            throw new NotImplementedException();
        }

        public override Type TrySwitchStates()
        {
            throw new NotImplementedException();
        }
    }

    [CreateAssetMenu(menuName = "Data/AI/StateMachine/Advance")]
    public class AdvanceMoveState : BaseState
    {
        [SerializeField] private float advanceThreshold;
        [SerializeField] private float bulletDetectionRadius;
        [SerializeField] private LayerMask bulletLayerMask;

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

            _pathFinding.SetIsAtTarget((position, target) => Vector2.Distance(position, target) < advanceThreshold);
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
            var collisions = Physics2D.OverlapCircleAll(_parent.transform.position, bulletDetectionRadius, bulletLayerMask);
            List<IAmmo> dangerousObjects = new();

            foreach(var collision in collisions)
            {
                if(collision.TryGetComponent(out IAmmo ammo) && ammo.WillDamage(_parent.transform.position))
                {
                    dangerousObjects.Add(ammo);
                }
            }
            if (dangerousObjects.Any())
            {
                _blackboard.DangerousObjects = dangerousObjects;
                return typeof(AvoidBulletState);
            }

            //if player is closer than threshold then strafe
            var distance = Vector2.Distance(_blackboard.PlayerPosition, _parent.transform.position);
            if (distance < advanceThreshold)
            {
                return typeof(StrafeState);
            }

            return GetType();
        }
    }

    [CreateAssetMenu(menuName = "Data/AI/StateMachine/Rush State")]
    public class RushMoveState : BaseState
    {
        private IPathfinding _pathFinding;
        private IMovement _movement;
        private PlayerController _player;

        public override void Initialize(GameObject parent, Blackboard blackboard)
        {
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
            if (_player == null) return;

            _pathFinding.UpdatePath(_player.transform.position);

            var direction = _pathFinding.GetDirection();
            _movement.SetMovementDirection(direction);
        }

        public override Type TrySwitchStates() => GetType();
    }
}