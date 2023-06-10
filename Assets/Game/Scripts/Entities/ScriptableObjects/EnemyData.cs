using Assets.Game.Scripts.Entities.Abilities;
using Assets.Game.Scripts.Entities.ScriptableObjects.AI;
using UnityEngine;

namespace Assets.Game.Scripts.Entities.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Data/Enemy Tank Data")]
    public class EnemyData : TankData
    {
        [Header("AI")]
        [SerializeField] private BaseAIFiringBrain primaryFiringBrainSO;
        [SerializeField] private BaseAIFiringBrain secondaryFiringBrainSO;
        [SerializeField] private BaseAIMovementBrain movementBrainSO;

        public BaseAIMovementBrain MovementBrainSO => movementBrainSO;
        public BaseAIFiringBrain PrimaryFiringBrainSO => primaryFiringBrainSO;
        public BaseAIFiringBrain SecondaryFiringBrainSO => secondaryFiringBrainSO;
    }
}