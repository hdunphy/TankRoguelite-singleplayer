using Assets.Game.Scripts.Controllers;
using Assets.Game.Scripts.Entities.Abilities;
using UnityEngine;

namespace Assets.Game.Scripts.Entities.ScriptableObjects.AI
{
    public class RandomAIFiringBrainInput : BaseAIFiringBrainInput
    {
        public RandomAIFiringBrainInput(TankGunMovement tankGunMovement, FiringController firingController, Transform selfTransform) 
            : base(tankGunMovement, firingController, selfTransform)
        {
        }
    }

    [CreateAssetMenu(menuName = "Data/AI/Random Firing Brain")]
    public class RandomAIFiringBrain : BaseAIFiringBrain
    {
        [SerializeField] private float timeBetweenMoves = 0.5f;
        [SerializeField] private float visionDistance = 100f;
        [SerializeField] private LayerMask shotLayerMask;
        [SerializeField] private bool debug;

        private bool hasNextShot = false;
        private float _remainingTime = 0f;

        private Vector2 _lookDirection;

        public override void Initialize(BaseAIFiringBrainInput input)
        {
            base.Initialize(input);

            FindNextLookPoint();
        }

        public override void UpdateLogic(float deltaTime)
        {
            _remainingTime -= deltaTime;
            if (hasNextShot && _remainingTime < 0)
            {
                hasNextShot = false;
                FindNextLookPoint();
            }
            else if (_tankGunMovement.IsAligned && !hasNextShot)
            {
                _remainingTime = timeBetweenMoves;
                hasNextShot = true;
            }

            var hitsAlly = shootAbility.CheckShot(_firingController.FirePoint.position, _lookDirection, visionDistance, shotLayerMask, out CheckShotOutput output, "Enemy", debug);
            _firingController.SetAbilityButtonPressed(!hitsAlly);

            if (output.RaycastHit.collider != null && output.RaycastHit.collider.CompareTag("Player"))
            {
                Debug.Log($"Fire at player: {hitsAlly}");
            }
        }

        private void FindNextLookPoint()
        {
            _lookDirection = Random.insideUnitCircle;
            _tankGunMovement.SetLookDirection(_lookDirection);
        }
    }
}