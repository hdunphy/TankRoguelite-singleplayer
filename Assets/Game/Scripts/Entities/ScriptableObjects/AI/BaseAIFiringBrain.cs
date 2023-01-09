using Assets.Game.Scripts.Controllers;
using Assets.Game.Scripts.Entities.Abilities;
using UnityEngine;

namespace Assets.Game.Scripts.Entities.ScriptableObjects.AI
{
    public abstract class BaseAIFiringBrain : ScriptableObject
    {

        protected TankGunMovement _tankGunMovement;
        protected FiringController _firingController;
        protected Transform _selfTransform;

        public virtual void Initialize(BaseAIFiringBrainInput input)
        {
            _tankGunMovement = input.TankGunMovement;
            _firingController = input.FiringController;
            _selfTransform = input.SelfTransform;
        }

        public abstract void UpdateLogic(float deltaTime);

    }
    public class BaseAIFiringBrainInput
    {
        public TankGunMovement TankGunMovement { get; set; }
        public FiringController FiringController { get; set; }
        public Transform SelfTransform { get; set; }

        public BaseAIFiringBrainInput(TankGunMovement tankGunMovement, FiringController firingController, Transform selfTransform)
        {
            TankGunMovement = tankGunMovement;
            FiringController = firingController;
            SelfTransform = selfTransform;
        }
    }
}