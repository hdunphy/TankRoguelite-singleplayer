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

        public StateMachine(List<BaseState> states, GameObject parent, Type currentState = null)
        {
            states = states.Select(state => UnityEngine.Object.Instantiate(state)).ToList();
            states.ForEach(state => state.Initialize(parent));
            _states = states.ToDictionary(s => s.GetType());
            _currentState = currentState ?? _states.Keys.First();
        }

        public void Update(StateInputs inputs)
        {
            _currentState = _states[_currentState].TrySwitchStates(inputs);
            _states[_currentState].RunBehavior();
        }
    }

    //public class StateMachineHolder : ScriptableObject
    //{
    //    [SerializeField] private float advanceThreshold;
    //    [SerializeField] private float retreatThreshold;

    //    public void GetState(StateInputs inputs)
    //    {
    //        if (inputs.BulletDetected)
    //        {
    //            //return avoid bullet
    //        }
    //        if (!inputs.CanSeePlayer)
    //        {
    //            //can't see player advance to see player
    //        }

    //        if (inputs.PlayerDistance < retreatThreshold)
    //        {
    //            //player is too close retreat
    //        }
    //        else if (inputs.PlayerDistance < advanceThreshold)
    //        {
    //            //player is good distance strafe
    //        }
    //        else
    //        {
    //            //player is far away advance
    //        }
    //    }
    //}



    public class StateInputs
    {
        public Vector2 PlayerPosition { get; set; }

        public StateInputs(Vector2 playerPosition)
        {
            PlayerPosition = playerPosition;
        }
    }
}