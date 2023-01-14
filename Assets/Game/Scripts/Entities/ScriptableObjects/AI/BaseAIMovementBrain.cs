using Assets.Game.Scripts.Entities.ScriptableObjects.AI.StateMachines;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Game.Scripts.Entities.ScriptableObjects.AI
{
    public abstract class BaseAIMovementBrain : ScriptableObject
    {
        [SerializeField] private List<BaseState> states;

        private StateMachine _stateMachine;
        private GameObject _parent;
        private Transform _target;

        public virtual void Initialize(GameObject parent, Transform target)
        {
            _target = target;
            _parent = parent;
            _stateMachine = new(states, _parent);
        }

        public virtual void UpdateLogic(float deltaTime)
        {
            //var distanceFromTarget = Vector2.Distance(_target.position, _parent.transform.position);
            _stateMachine.Update(new(_target.position));
        }
    }
}