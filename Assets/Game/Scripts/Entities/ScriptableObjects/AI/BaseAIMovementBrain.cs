using Assets.Game.Scripts.Entities.ScriptableObjects.AI.StateMachines;
using Assets.Game.Scripts.Entities.ScriptableObjects.AI.StateMachines.Helpers;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Game.Scripts.Entities.ScriptableObjects.AI
{
    public abstract class BaseAIMovementBrain : ScriptableObject
    {
        [SerializeField] private List<BaseState> states;

        private StateMachine _stateMachine;
        private Blackboard _blackboard;
        private GameObject _parent;
        private Transform _target;

        protected abstract SMParameters StateMachineParams { get; }

        public Blackboard Blackboard => _blackboard;

        public virtual void Initialize(GameObject parent, Transform target)
        {
            _target = target;
            _parent = parent;
            _blackboard = new Blackboard();
            _stateMachine = new(states, _parent, _blackboard, StateMachineParams);
        }

        public virtual void UpdateLogic(float deltaTime)
        {
            _blackboard.PlayerPosition = _target.position;
            _stateMachine.Update();
        }
    }
}