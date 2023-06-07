using Assets.Game.Scripts.Controllers;
using Assets.Game.Scripts.Controllers.AI;
using Assets.Game.Scripts.Entities.Interfaces;
using Assets.Game.Scripts.Entities.ScriptableObjects.AI.StateMachines.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using TMPro.EditorUtilities;
using UnityEngine;

namespace Assets.Game.Scripts.Entities.ScriptableObjects.AI.StateMachines
{

    [CreateAssetMenu(menuName = "Data/AI/StateMachine/Avoid Bullet"), DisplayName("Avoid")]
    public class AvoidBulletState : BaseState
    {
        [SerializeField] private MoveStrafeAvoidSMParameters parameters;
        [SerializeField] private BaseState exitState;
        [SerializeField] private float avoidanceSeconds = .5f;

        private IMovement _movement;
        private List<IAmmo> _dangerousObjects;
        private EnemyShadowCollisionDetector _enemyShadow;
        private float secondsSafe;

        public override void Initialize(GameObject parent, Blackboard blackboard)
        {
            base.Initialize(parent, blackboard);
            _dangerousObjects = new List<IAmmo>();

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
            _blackboard.DebugData.StateName = "Avoid";

            if (!_dangerousObjects.Any())
            {
                secondsSafe += Time.deltaTime;
                return;
            }

            secondsSafe = 0f;

            Vector2 currentPosition = _parent.transform.position;
            var closestObject = _dangerousObjects.OrderByDescending(a => Vector2.Distance(currentPosition, a.Position)).First();

            Vector2 direction = closestObject.GetSafeDirection(currentPosition);

            _movement.SetMovementDirection(direction);
        }

        public override Type TrySwitchStates()
        {
            _dangerousObjects = parameters.CheckForBullets(_enemyShadow.transform.position, _parent);
            _blackboard.DebugData.Message = $"Safe: {secondsSafe}";

            bool isSafe = !_dangerousObjects.Any() && secondsSafe >= avoidanceSeconds;
            return isSafe ? exitState.GetType() : GetType();
        }
    }
}