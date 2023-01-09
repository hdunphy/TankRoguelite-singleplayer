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

    public class BestShotAIFiringBrain : BaseAIFiringBrain
    {
        [SerializeField, Min(1)] private int numberOfShotsToCheck;
        public override void UpdateLogic(float deltaTime)
        {
            float step = 360 / numberOfShotsToCheck;
            for(int i = 0; i < numberOfShotsToCheck; i++)
            {
                var degreeOffset = i * step;

                //get look direction;
            }
        }
    }

    [CreateAssetMenu(menuName = "Data/AI/Random Firing Brain")]
    public class RandomAIFiringBrain : BaseAIFiringBrain
    {
        [SerializeField] private float timeBetweenMoves = 0.5f;
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

            Vector2 targetVector = Vector2.one;
            var hitsAlly = shootAbility.CheckShot(_firingController.FirePoint.position, _lookDirection, 0, 100, ref targetVector, shotLayerMask, "Enemy", debug);
            _firingController.SetAbilityButtonPressed(!hitsAlly);
        }

        private void FindNextLookPoint()
        {
            _lookDirection = Random.insideUnitCircle;
            _tankGunMovement.SetLookPoint(_lookDirection * 2 + (Vector2)_selfTransform.position);
        }
    }
}