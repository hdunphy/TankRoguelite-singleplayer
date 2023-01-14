using UnityEngine;

namespace Assets.Game.Scripts.Entities.ScriptableObjects.AI
{
    [CreateAssetMenu(menuName = "Data/AI/Movment/Immobile")]
    public class ImmobileAIMovementBrain : BaseAIMovementBrain
    {
        public override void Initialize(GameObject parent, Transform target) { }

        public override void UpdateLogic(float deltaTime) { }
    }
}