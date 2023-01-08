using Assets.Game.Scripts.Entities;
using Assets.Game.Scripts.Entities.ScriptableObjects.AI;
using UnityEngine;

namespace Assets.Game.Scripts.Controllers.AI
{
    public class EnemyController : MonoBehaviour
    {
        [Header("Inputs")]
        [SerializeField] private BaseAIFiringBrain firingBrain;
        [SerializeField] private TankData tankData;
        [Header("Controllers")]
        [SerializeField] private TankGunMovement tankGunMovement;
        [SerializeField] private FiringController primaryFiringController;

        private void Awake()
        {
            primaryFiringController.SetTankData(tankData);
        }

        // Use this for initialization
        void Start()
        {
            tankGunMovement.SetGunRotationSpeed(tankData.GunRotationSpeed);
            firingBrain.Initialize(new(tankGunMovement, primaryFiringController, transform));
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}