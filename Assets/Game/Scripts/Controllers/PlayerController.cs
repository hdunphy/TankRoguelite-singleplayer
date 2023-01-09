using Assets.Game.Scripts.Entities;
using Assets.Game.Scripts.Entities.Abilities;
using Assets.Scripts.Inputs;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

namespace Assets.Game.Scripts.Controllers
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Data")]
        [SerializeField] private TankData tankData;
        [Header("Controls")]
        [SerializeField] private TankGunMovement gunMovement;
        [Header("Inputs")]
        [SerializeField] private PlayerTargeter playerTargeter;
        [SerializeField] private FiringController primaryFiringController;
        [SerializeField] private FiringController secondaryFiringController;

        private IMovement _movement;
        private Controls _controls;

        private void Awake()
        {
            _movement= GetComponent<IMovement>();

            _controls = new();

            //TODO: might need to fix this depending on how I handle room transitions
            tankData.ResetModifier();

            primaryFiringController.SetTankData(tankData);
            secondaryFiringController.SetTankData(tankData);
        }

        private void OnEnable()
        {
            _controls.Enable();
        }

        private void OnDisable()
        {
            _controls.Disable();
        }

        private void Start()
        {
            _controls.Player.Movement.performed += HandleMovement;
            _controls.Player.Movement.canceled += HandleMovement;

            _controls.Player.PrimaryFire.performed += HandlePrimaryFire;
            _controls.Player.PrimaryFire.canceled += HandlePrimaryFire;

            _controls.Player.SecondaryFire.performed += HandleSecondaryFire;
            _controls.Player.SecondaryFire.canceled += HandleSecondaryFire;

            _movement.SetTankData(tankData);
            gunMovement.SetGunRotationSpeed(tankData.GunRotationSpeed);
        }

        private void OnDestroy()
        {
            _controls.Player.Movement.performed -= HandleMovement;
            _controls.Player.Movement.canceled -= HandleMovement;
            _controls.Player.PrimaryFire.performed -= HandlePrimaryFire;
            _controls.Player.PrimaryFire.canceled -= HandlePrimaryFire;
            _controls.Player.SecondaryFire.performed -= HandleSecondaryFire;
            _controls.Player.SecondaryFire.canceled -= HandleSecondaryFire;
        }

        private void FixedUpdate()
        {
            FindLookPoint();
        }

        private void FindLookPoint()
        {
            var point = playerTargeter.TargetTransform.position;
            var worldPos = Camera.main.ScreenToWorldPoint(point);
            Vector2 direction = worldPos - transform.position;
            gunMovement.SetLookDirection(direction);
        }

        #region Pickups
        public void AddAbility(Ability ability, bool isPrimaryAbility)
        {
            if (isPrimaryAbility)
            {
                primaryFiringController.enabled = true;
                primaryFiringController.SetAbility(ability);
            }
            else if (!isPrimaryAbility)
            {
                secondaryFiringController.enabled = true;
                secondaryFiringController.SetAbility(ability);
            }
        }

        public void AddUpgrade(TankModifiers modifiers)
        {
            tankData.AddModifiers(modifiers);
        }
        #endregion

        private void SetIsFiring(FiringController firingController, bool isPressed)
        {
            if(firingController.enabled)
            {
                firingController.SetAbilityButtonPressed(isPressed);
            }
        }

        #region Input Callbacks
        private void HandleMovement(CallbackContext ctx)
        {
            _movement.SetMovementDirection(ctx.ReadValue<Vector2>());

            if (ctx.canceled)
            {
                _movement.StopMoving();
            }
        }

        private void HandlePrimaryFire(CallbackContext ctx) => SetIsFiring(primaryFiringController, ctx.performed);

        private void HandleSecondaryFire(CallbackContext ctx) => SetIsFiring(secondaryFiringController, ctx.performed);
        #endregion
    }
}
