using System;
using UnityEngine;

namespace Assets.Game.Scripts.Entities.ScriptableObjects.AI.StateMachines
{
    public abstract class BaseState : ScriptableObject
    {
        protected Blackboard _blackboard;
        protected GameObject _parent;
        public virtual void Initialize(GameObject parent, Blackboard blackboard)
        {
            _blackboard = blackboard;
            _parent = parent;
        }

        public abstract Type TrySwitchStates();
        public abstract void RunBehavior();
    }
}