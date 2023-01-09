using Assets.Game.Scripts.Controllers;
using Assets.Game.Scripts.Controllers.AI;
using Assets.Game.Scripts.Utilities;
using UnityEngine;

namespace Assets.Game.Scripts.Entities.ScriptableObjects.AI
{
    [CreateAssetMenu(menuName = "Data/AI/Best Shot Firing Brain")]
    public class BestShotAIFiringBrain : BaseAIFiringBrain
    {
        [Header("Parameters")]
        [SerializeField, Min(1)] private int numberOfShotsToCheck;
        [SerializeField, Min(0)] private float coolDownBetweenFindShot;
        [SerializeField] private LayerMask shootingLayerMask;
        [SerializeField] private bool shootWhenIsAligned;
        [SerializeField] private bool isDebug;

        [Header("Shot Values")]
        [SerializeField] private float hitPlayerValue;
        [SerializeField] private float hitEnemyValue;
        [SerializeField] private float hitProjectileValue;
        [Header("Modifiers")]
        [SerializeField] private float degreeOffsetModifier;
        [SerializeField] private float playerDistanceModifier;
        [SerializeField] private float enemyDistanceModifier;
        [SerializeField] private float projectileDistanceModifier;
        [SerializeField] private float bouncesModifier;

        private float _nextShot;

        public override void Initialize(BaseAIFiringBrainInput input)
        {
            base.Initialize(input);

            _nextShot = coolDownBetweenFindShot;
        }

        public override void UpdateLogic(float deltaTime)
        {
            if((_nextShot -= deltaTime) < 0)
            {
                _nextShot = coolDownBetweenFindShot;
                FindNextShot();
            }

            if (_tankGunMovement.IsAligned && shootWhenIsAligned)
            {
                _firingController.SetAbilityButtonPressed(true);
            }
            else if (!shootWhenIsAligned && shootAbility.CheckShot(_firingController.FirePoint.position, _firingController.FirePointDirection, 100, shootingLayerMask, out CheckShotOutput _))
            {
                _firingController.SetAbilityButtonPressed(true);
            }
            else
            {
                _firingController.SetAbilityButtonPressed(false);
            }
        }

        private void FindNextShot()
        {
            float maxScore = float.MinValue;
            float targetAngle = 0;

            int step = 360 / numberOfShotsToCheck;
            for (int i = -180; i < 180; i += step)
            {
                float currentScore = GetShotScore(i);

                if (currentScore > maxScore)
                {
                    targetAngle = i;
                    maxScore = currentScore;
                }
            }

            Vector2 direction = _firingController.FirePointDirection.Rotate(targetAngle);
            _tankGunMovement.SetLookDirection(direction);
            if (isDebug)
            {
                Debug.DrawRay(_selfTransform.position, direction, Color.blue);
            }
        }

        private float GetShotScore(float degreeOffset)
        {
            //get look direction;
            var lookDirection = _firingController.FirePointDirection.Rotate(degreeOffset);

            float currentScore = Mathf.Abs(degreeOffset) * degreeOffsetModifier;
            if (shootAbility.CheckShot(_firingController.FirePoint.position, lookDirection, 100, shootingLayerMask, out CheckShotOutput output, ProjectileData.ANY_TAG))
            {
                if (output.RaycastHit.collider.TryGetComponent(out PlayerController _))
                {
                    currentScore += hitPlayerValue / output.TravelDistance * playerDistanceModifier;
                }
                else if (output.RaycastHit.collider.TryGetComponent(out EnemyController _))
                {
                    currentScore += hitEnemyValue / output.TravelDistance * enemyDistanceModifier;
                }
                else if (output.RaycastHit.collider.TryGetComponent(out Projectile _))
                {
                    currentScore += hitProjectileValue / output.TravelDistance * projectileDistanceModifier;
                }

                currentScore += output.NumberOfBounces * bouncesModifier;
            }

            return currentScore;
        }
    }
}