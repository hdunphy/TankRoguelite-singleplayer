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

        private void Awake()
        {
            //TODO: Implement movementbrain
            _firingBrain = Instantiate(firingBrainSO);
            primaryFiringController.SetTankData(tankData);
        }

        // Use this for initialization
        void Start()
        {
            tankGunMovement.SetGunRotationSpeed(tankData.GunRotationSpeed);
            _firingBrain.Initialize(new(tankGunMovement, primaryFiringController, transform));
        }

        // Update is called once per frame
        void Update()
        {
            _firingBrain.UpdateLogic(Time.deltaTime);
        }
    }
}