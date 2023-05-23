using Assets.Game.Scripts.Controllers;
using Assets.Game.Scripts.Controllers.AI.Pathfinding;
using Assets.Game.Scripts.Entities.Interfaces;
using Assets.Game.Scripts.Entities.ScriptableObjects.AI.StateMachines.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Game.Scripts.Entities.ScriptableObjects.AI.StateMachines
{
    public class PerfectAvoidBulletState : BaseState
    {
        [SerializeField] private MoveStrafeAvoidSMParameters parameters;
        [SerializeField] private BaseState exitState;
        [SerializeField] private int maxSearchDepth;
        [SerializeField] private float searchStepSize;

        private IPathfinding _pathFinding;
        private IMovement _movement;
        private GameObject _parent;

        private List<IAmmo> _dangerousObjects;
        private Vector3 _bestPosition;

        public override void Initialize(GameObject parent, Blackboard blackboard)
        {
            _parent = parent;
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
            if (parameters.CheckForBullets(_bestPosition, _parent).Any())
            {
                GetBestPosition();
                Debug.Log($"<color=geen>Bullet Detected, Moving to {_bestPosition}</color>");
            }

            //var currentPosition = _parent.transform.position;
            //if(Vector2.Distance(currentPosition, _bestPosition) < 0.1)
            //{
            //    _bestPosition= currentPosition;
            //    return;
            //}

            //Debug.Log($"Best pos: {_bestPosition}");
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

                        var position = new Vector3(x, y) * (i * searchStepSize) + currentPos;
                        var dangerousObjectsCount = parameters.CheckForBullets(position, _parent).Count();

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
            _dangerousObjects = parameters.CheckForBullets(_parent.transform.position, _parent);

            return _dangerousObjects.Any() ? GetType() : exitState.GetType();
        }

    }
}