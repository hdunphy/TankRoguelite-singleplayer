using Assets.Game.Scripts.Entities;
using Assets.Game.Scripts.Entities.Abilities;
using Assets.Scripts.Inputs;
using System;
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

            _movement.SetMovementData(tankData.MovementData);
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
            gunMovement.SetLookPoint(worldPos);
        }

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

        private void SetIsFiring(FiringController firingController, bool isPressed)
        {
            if(firingController is not null && firingController.enabled)
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
