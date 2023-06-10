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


        private BaseAIFiringBrain _primaryFireingBrain;
        private BaseAIFiringBrain _secondaryFireingBrain;
        public BaseAIMovementBrain MovementBrain { get; private set; }
        public Vector2 ColliderSize { get; private set; }


        private void Awake()
        {
            _primaryFireingBrain = Instantiate(enemyData.PrimaryFiringBrainSO);
                _secondaryFireingBrain = Instantiate(enemyData.SecondaryFiringBrainSO);
            MovementBrain = Instantiate(enemyData.MovementBrainSO);
            ColliderSize = GetComponent<BoxCollider2D>().size;
        }

        // Use this for initialization
        void Start()
        {
            _primaryFireingBrain.Initialize(new(tankGunMovement, primaryFiringController, transform));
            _secondaryFireingBrain.Initialize(new(tankGunMovement, secondaryFiringController, transform));
            MovementBrain.Initialize(gameObject, FindObjectOfType<PlayerController>().transform);

            primaryFiringController.SetTankData(enemyData);
            secondaryFiringController.SetTankData(enemyData);
            tankGunMovement.SetGunRotationSpeed(enemyData.GunRotationSpeed);
            if (TryGetComponent(out IMovement movement))
                movement.SetTankData(enemyData);
        }

        // Update is called once per frame
        void Update()
        {
            _primaryFireingBrain.UpdateLogic(Time.deltaTime);
            _secondaryFireingBrain.UpdateLogic(Time.deltaTime);
            MovementBrain.UpdateLogic(Time.deltaTime);
        }
    }
}