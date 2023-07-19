using Assets.Game.Scripts.Controllers.Manager;
using Assets.Game.Scripts.Controllers.Tanks;
using Assets.Game.Scripts.Entities.ScriptableObjects;
using Assets.Game.Scripts.Entities.ScriptableObjects.AI;
using UnityEngine;

namespace Assets.Game.Scripts.Controllers.AI
{

    [RequireComponent(typeof(TankGunMovement), typeof(FiringController))]
    public class EnemyController : MonoBehaviour
    {
        [Header("Inputs")]
        [SerializeField] private EnemyData enemyData;

        [Header("Controllers")]
        [SerializeField] private TankGunMovement tankGunMovement;
        [SerializeField] private FiringController primaryFiringController;
        [SerializeField] private FiringController secondaryFiringController;
        [SerializeField] private TankColorManipulator tankColorManipulator;

        private BaseAIFiringBrain _primaryFireingBrain;
        private BaseAIFiringBrain _secondaryFireingBrain;
        public BaseAIMovementBrain MovementBrain { get; private set; }
        public Vector2 ColliderSize { get; private set; }

        private EnemyData _enemyData;

        public void SetEnemyData(EnemyData enemyData)
        {
            _enemyData = enemyData;
            SetupFiring();
            SetUpMovement();
        }

        private void Awake()
        {
            ColliderSize = GetComponent<BoxCollider2D>().size;

            if (enemyData == null) return;

            SetEnemyData(enemyData);
        }

        private void SetUpMovement()
        {
            MovementBrain = Instantiate(_enemyData.MovementBrainSO);

            MovementBrain.Initialize(gameObject, FindObjectOfType<PlayerController>().transform);

            tankGunMovement.SetGunRotationSpeed(_enemyData.GunRotationSpeed);
            if (TryGetComponent(out IMovement movement))
                movement.SetTankData(_enemyData);

            tankColorManipulator.ChangeSpriteColor(_enemyData.PrimaryColor);
        }

        private void SetupFiring()
        {
            _primaryFireingBrain = Instantiate(_enemyData.PrimaryFiringBrainSO);
            _secondaryFireingBrain = Instantiate(_enemyData.SecondaryFiringBrainSO);

            _primaryFireingBrain.Initialize(new(tankGunMovement, primaryFiringController, transform));
            _secondaryFireingBrain.Initialize(new(tankGunMovement, secondaryFiringController, transform));

            primaryFiringController.SetTankData(_enemyData);
            secondaryFiringController.SetTankData(_enemyData);
        }

        void Update()
        {
            _primaryFireingBrain.UpdateLogic(Time.deltaTime);
            _secondaryFireingBrain.UpdateLogic(Time.deltaTime);
            MovementBrain.UpdateLogic(Time.deltaTime);
        }
    }
}