using Assets.Game.Scripts.Entities;
using Assets.Game.Scripts.Entities.ScriptableObjects.AI;
using UnityEngine;

namespace Assets.Game.Scripts.Controllers.AI
{

    [RequireComponent(typeof(TankGunMovement), typeof(FiringController))]
    public class EnemyController : MonoBehaviour
    {
        [Header("Inputs")]
        [SerializeField] private BaseAIFiringBrain firingBrainSO;
        [SerializeField] private BaseAIMovementBrain movementBrainSO;
        [SerializeField] private TankData tankData;

        [Header("Controllers")]
        [SerializeField] private TankGunMovement tankGunMovement;
        [SerializeField] private FiringController primaryFiringController;


        private BaseAIFiringBrain _firingBrain;
        public BaseAIMovementBrain MovementBrain { get; private set; }
        public Vector2 ColliderSize { get; private set; }


        private void Awake()
        {
            _firingBrain = Instantiate(firingBrainSO);
            MovementBrain = Instantiate(movementBrainSO);
            ColliderSize = GetComponent<BoxCollider2D>().size;
        }

        // Use this for initialization
        void Start()
        {
            _firingBrain.Initialize(new(tankGunMovement, primaryFiringController, transform));
            MovementBrain.Initialize(gameObject, FindObjectOfType<PlayerController>().transform);

            primaryFiringController.SetTankData(tankData);
            tankGunMovement.SetGunRotationSpeed(tankData.GunRotationSpeed);
            if (TryGetComponent(out IMovement movement))
                movement.SetTankData(tankData);
        }

        // Update is called once per frame
        void Update()
        {
            _firingBrain.UpdateLogic(Time.deltaTime);
            MovementBrain.UpdateLogic(Time.deltaTime);
        }
    }
}