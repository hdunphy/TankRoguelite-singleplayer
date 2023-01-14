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
        private BaseAIMovementBrain _movementBrain;

        private void Awake()
        {
            _firingBrain = Instantiate(firingBrainSO);
            _movementBrain = Instantiate(movementBrainSO);
        }

        // Use this for initialization
        void Start()
        {
            _firingBrain.Initialize(new(tankGunMovement, primaryFiringController, transform));
            _movementBrain.Initialize(gameObject, FindObjectOfType<PlayerController>().transform);
            
            primaryFiringController.SetTankData(tankData);
            tankGunMovement.SetGunRotationSpeed(tankData.GunRotationSpeed);
            if (TryGetComponent(out IMovement movement))
                movement.SetTankData(tankData);
        }

        // Update is called once per frame
        void Update()
        {
            _firingBrain.UpdateLogic(Time.deltaTime);
            _movementBrain.UpdateLogic(Time.deltaTime);
        }
    }
}