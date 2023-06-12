using Assets.Game.Scripts.Entities.ScriptableObjects.AI.StateMachines.Helpers;
using UnityEngine;

namespace Assets.Game.Scripts.Entities.ScriptableObjects.AI
{
    [CreateAssetMenu(menuName = "Data/AI/Movment/Normal")]
    public class AIMovementBrain : BaseAIMovementBrain
    {
        [SerializeField] private SMParameters movementSMParameters;
        protected override SMParameters StateMachineParams => movementSMParameters;
    }
}