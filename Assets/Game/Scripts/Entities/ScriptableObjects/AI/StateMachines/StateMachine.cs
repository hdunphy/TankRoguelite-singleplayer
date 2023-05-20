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

        public Type CurrentState { get; private set; }

        public StateMachine(List<BaseState> states, GameObject parent, Blackboard blackboard, Type currentState = null)
        {
            states = states.Select(state => UnityEngine.Object.Instantiate(state)).ToList();
            states.ForEach(state => state.Initialize(parent, blackboard));
            _states = states.ToDictionary(s => s.GetType());
            CurrentState = currentState ?? _states.Keys.First();
        }

        public void Update()
        {
            var nextState = _states[CurrentState].TrySwitchStates();
            if (_states.ContainsKey(nextState))
            {
                CurrentState= nextState;
            }
            _states[CurrentState].RunBehavior();
        }
    }

    public class Blackboard
    {
        public Vector2 PlayerPosition { get; set; }
    }
}