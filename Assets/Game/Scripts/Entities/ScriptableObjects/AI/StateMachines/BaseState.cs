using Assets.Game.Scripts.Entities.ScriptableObjects.AI.StateMachines.Helpers;
using System;
using UnityEngine;

namespace Assets.Game.Scripts.Entities.ScriptableObjects.AI.StateMachines
{
    public abstract class BaseState : ScriptableObject
    {
        protected Blackboard _blackboard;
        protected GameObject _parent;
        protected SMParameters _stateMachineParams;
        public virtual void Initialize(GameObject parent, Blackboard blackboard, SMParameters stateMachineParams)
        {
            _blackboard = blackboard;
            _parent = parent;
            _stateMachineParams = stateMachineParams;
        }

        public abstract Type TrySwitchStates();
        public abstract void RunBehavior();
    }
}