using Assets.Game.Scripts.Entities;
using Assets.Game.Scripts.Entities.Abilities;
using Assets.Game.Scripts.Entities.Interfaces;
using Assets.Game.Scripts.Entities.ScriptableObjects;
using System.Collections;
using UnityEngine;

namespace Assets.Game.Scripts.Controllers.AI
{
    public class EnemyController : MonoBehaviour
    {

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }

    public abstract class BaseAIFiringBrain : ScriptableObject
    {

    }

    public class BaseAIFiringBrainInput
    {
        public TankGunMovement TankGunMovement { get; set; }
        public AbilityHolder AbilityHolder { get; set; }
        public Transform SelfTransform { get; set; }

        public BaseAIFiringBrainInput(TankGunMovement tankGunMovement, AbilityHolder abilityHolder, Transform selfTransform)
        {
            TankGunMovement = tankGunMovement;
            AbilityHolder = abilityHolder;
            SelfTransform = selfTransform;
        }
    }
    public class RandomAIFiringBrainInput : BaseAIFiringBrainInput
    {
        public RandomAIFiringBrainInput(TankGunMovement tankGunMovement, AbilityHolder abilityHolder, Transform selfTransform) : base(tankGunMovement, abilityHolder, selfTransform)
        {
        }
    }

    public class RandomAIFiringBrain : BaseAIFiringBrain
    {
        [SerializeField] private float timeBetweenMoves = 0.5f;
        [SerializeField] private AmmoData ammoData;
        [SerializeField] private LayerMask badTargets;

        private TankGunMovement _tankGunMovement;
        private AbilityHolder _abilityHolder;
        private Transform _selfTransform;

        private bool hasNextShot = false;
        private float _remainingTime = 0f;

        private Vector2 _lookDirection;

        public void Initialize()
        {
            FindNextLookPoint();
        }

        public void Update(float deltaTime)
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
            var hitsAlly = ammoData.CheckShot(_selfTransform.position, _lookDirection, 0, 100, ref targetVector, badTargets);
            _abilityHolder.SetAbilityButtonPressed(!hitsAlly);
        }

        private void FindNextLookPoint()
        {
            _lookDirection = Random.insideUnitCircle;
            _tankGunMovement.SetLookPoint(_lookDirection);
        }
    }
}