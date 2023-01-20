using System;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Game.Scripts.Entities.ScriptableObjects.AI.StateMachines
{
    public abstract class BaseState : ScriptableObject
    {
        public abstract void Initialize(GameObject parent, Blackboard blackboard);
        public abstract Type TrySwitchStates();
        public abstract void RunBehavior();
    }
}