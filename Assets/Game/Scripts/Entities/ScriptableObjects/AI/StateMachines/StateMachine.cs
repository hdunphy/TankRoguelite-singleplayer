using Assets.Game.Scripts.Entities.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Game.Scripts.Entities.ScriptableObjects.AI.StateMachines
{
    public class StateMachine
    {
        private Dictionary<Type, BaseState> _states;

        private Type _currentState;

        public StateMachine(List<BaseState> states, GameObject parent, Blackboard blackboard, Type currentState = null)
        {
            states = states.Select(state => UnityEngine.Object.Instantiate(state)).ToList();
            states.ForEach(state => state.Initialize(parent, blackboard));
            _states = states.ToDictionary(s => s.GetType());
            _currentState = currentState ?? _states.Keys.First();
        }

        public void Update()
        {
            _currentState = _states[_currentState].TrySwitchStates();
            _states[_currentState].RunBehavior();
        }
    }

    public class Blackboard
    {
        public Vector2 PlayerPosition { get; set; }
    }
}