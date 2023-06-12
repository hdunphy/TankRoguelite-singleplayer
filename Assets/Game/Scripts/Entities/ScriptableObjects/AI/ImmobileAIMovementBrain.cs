using Assets.Game.Scripts.Entities.ScriptableObjects.AI.StateMachines.Helpers;
using UnityEngine;

namespace Assets.Game.Scripts.Entities.ScriptableObjects.AI
{
    [CreateAssetMenu(menuName = "Data/AI/Movment/Immobile")]
    public class ImmobileAIMovementBrain : BaseAIMovementBrain
    {
        protected override SMParameters StateMachineParams => CreateInstance<EmptySMParameters>();

        public override void Initialize(GameObject parent, Transform target) => _blackboard = new();

        public override void UpdateLogic(float deltaTime) { }
    }
}