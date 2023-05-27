using Assets.Game.Scripts.Entities.ScriptableObjects.AI.StateMachines;
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

        public Blackboard Blackboard => _blackboard;

        public virtual void Initialize(GameObject parent, Transform target)
        {
            _target = target;
            _parent = parent;
            _blackboard = new Blackboard();
            _stateMachine = new(states, _parent, _blackboard);
        }

        public virtual void UpdateLogic(float deltaTime)
        {
            _blackboard.PlayerPosition = _target.position;
            _stateMachine.Update();
        }
    }
}