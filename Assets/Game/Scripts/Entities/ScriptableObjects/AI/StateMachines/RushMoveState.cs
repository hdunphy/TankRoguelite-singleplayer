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
        [SerializeField] private AmmoDetectionParameters detectionParameters;
        [SerializeField] private BaseState exitState;
        [SerializeField] private int maxSearchDepth;
        [SerializeField] private float searchStepSize;

        private IPathfinding _pathFinding;
        private IMovement _movement;
        private GameObject _parent;
        private Blackboard _blackboard;

        private List<IAmmo> _dangerousObjects;
        private Vector3 _bestPosition;

        public override void Initialize(GameObject parent, Blackboard blackboard)
        {
            _parent = parent;
            _blackboard = blackboard;
            _bestPosition = _parent.transform.position;

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
            if (detectionParameters.CheckForBullets(_bestPosition).Any())
            {
                GetBestPosition();
            }

            _pathFinding.UpdatePath(_bestPosition);

            var direction = _pathFinding.GetDirection();
            _movement.SetMovementDirection(direction);

        }

        private void GetBestPosition()
        {
            var currentPos = _parent.transform.position;
            int maxDangerousObjects = int.MaxValue;
            _bestPosition = currentPos;
            for (int i = 0; i < maxSearchDepth; i++)
            {
                for (int x = -1; x <= 1; x++)
                {
                    for (int y = -1; y <= 1; y++)
                    {
                        if (x == 0 && y == 0) continue;

                        var position = new Vector3(x, y) + currentPos;
                        var dangerousObjectsCount = detectionParameters.CheckForBullets(position).Count();

                        if (dangerousObjectsCount < maxDangerousObjects)
                        {
                            maxDangerousObjects = dangerousObjectsCount;
                            _bestPosition = position;
                        }
                    }
                }
            }
        }

        public override Type TrySwitchStates()
        {
            _dangerousObjects = detectionParameters.CheckForBullets(_parent.transform.position);

            return _dangerousObjects.Any() ? typeof(AvoidBulletState) : exitState.GetType();
        }
    }

    [CreateAssetMenu(menuName = "Data/AI/StateMachine/Advance")]
    public class AdvanceMoveState : BaseState
    {
        [SerializeField] private float advanceThreshold;
        [SerializeField] private AmmoDetectionParameters detectionParameters;

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
            var dangerousObjects = detectionParameters.CheckForBullets(_parent.transform.position);
            if (dangerousObjects.Any())
            {
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

    [CreateAssetMenu(menuName = "Data/AI/StateMachine/Helpers/Bullet Detection Parameters")]
    public class AmmoDetectionParameters : ScriptableObject
    {
        [SerializeField] private float ammoDetectionRadius;
        [SerializeField] private LayerMask ammoLayerMask;

        public float AmmoDetectionRadius => ammoDetectionRadius;
        public LayerMask AmmoLayerMask => ammoLayerMask;

        /// <summary>
        /// Returns list of dangerous objects that could affect the origin point
        /// </summary>
        /// <param name="origin"></param>
        /// <returns></returns>
        public List<IAmmo> CheckForBullets(Vector3 origin)
        {
            var dangerousObjects = new List<IAmmo>();
            var collisions = Physics2D.OverlapCircleAll(origin, ammoDetectionRadius, ammoLayerMask);

            foreach (var collision in collisions)
            {
                if (collision.TryGetComponent(out IAmmo ammo) && ammo.WillDamage(origin))
                {
                    dangerousObjects.Add(ammo);
                }
            }

            return dangerousObjects;
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