using UnityEngine;

namespace Assets.Game.Scripts.Entities.ScriptableObjects.AI
{
    [CreateAssetMenu(menuName = "Data/AI/Empty Firing Brain")]
    public class EmptyFiringBrain : BaseAIFiringBrain
    {
        public override void UpdateLogic(float deltaTime) { }
    }
}