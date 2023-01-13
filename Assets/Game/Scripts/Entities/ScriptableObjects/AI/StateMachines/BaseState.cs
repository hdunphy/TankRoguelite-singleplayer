using System;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Game.Scripts.Entities.ScriptableObjects.AI.StateMachines
{
    public abstract class BaseState : ScriptableObject
    {
        protected UnityEvent<StateInputs> trySwitchState;

        public abstract void Initialize(GameObject parent);
        public abstract Type TrySwitchStates(StateInputs inputs);
        public abstract void RunBehavior();
    }
}