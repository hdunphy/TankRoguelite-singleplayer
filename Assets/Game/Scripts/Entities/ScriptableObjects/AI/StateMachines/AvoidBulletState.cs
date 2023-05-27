using Assets.Game.Scripts.Controllers;
using Assets.Game.Scripts.Entities.Interfaces;
using Assets.Game.Scripts.Entities.ScriptableObjects.AI.StateMachines.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Assets.Game.Scripts.Entities.ScriptableObjects.AI.StateMachines
{

    [CreateAssetMenu(menuName = "Data/AI/StateMachine/Avoid Bullet"), DisplayName("Avoid")]
    public class AvoidBulletState : BaseState
    {
        [SerializeField] private MoveStrafeAvoidSMParameters parameters;
        [SerializeField] private BaseState exitState;

        private IMovement _movement;
        private List<IAmmo> _dangerousObjects;

        public override void Initialize(GameObject parent, Blackboard blackboard)
        {
            base.Initialize(parent, blackboard);
            _dangerousObjects = new List<IAmmo>();

            if (!parent.TryGetComponent(out _movement))
            {
                Debug.LogError("Missing Movement Component");
            }
        }

        public override void RunBehavior()
        {
            _blackboard.DebugData.StateName = "Avoid";

            _movement.SetMovementDirection(Vector2.zero);
            //try switch state told us there we do have dangerous objects
            //Keep this just to ignore errors
            if (!_dangerousObjects.Any()) return;

            Vector2 currentPosition = _parent.transform.position;
            var closestObject = _dangerousObjects.OrderByDescending(a => Vector2.Distance(currentPosition, a.Position)).First();

            Vector2 direction = closestObject.GetSafeDirection(currentPosition);

            _movement.SetMovementDirection(direction);
        }

        public override Type TrySwitchStates()
        {
            _dangerousObjects = parameters.CheckForBullets(_parent.transform.position, _parent);

            return _dangerousObjects.Any() ? GetType() : exitState.GetType();
        }
    }
}