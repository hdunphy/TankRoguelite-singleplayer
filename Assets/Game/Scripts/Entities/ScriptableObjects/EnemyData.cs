using Assets.Game.Scripts.Entities.Abilities;
using Assets.Game.Scripts.Entities.ScriptableObjects.AI;
using UnityEngine;

namespace Assets.Game.Scripts.Entities.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Data/Enemy Tank Data")]
    public class EnemyData : TankData
    {
        [Header("AI")]
        [SerializeField] private BaseAIFiringBrain firingBrainSO;
        [SerializeField] private BaseAIMovementBrain movementBrainSO;
        [SerializeField] private Ability[] abilities;

        public BaseAIMovementBrain MovementBrainSO => movementBrainSO;
        public BaseAIFiringBrain FiringBrainSO => firingBrainSO;
        public Ability[] Abilities => abilities;
    }
}