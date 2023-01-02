using Assets.Game.Scripts.Entities;
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

            _movement.SetMovementData(tankData.MovementData);
            gunMovement.SetGunRotationSpeed(tankData.GunRotationSpeed);
        }

        private void OnDestroy()
        {
            _controls.Player.Movement.performed -= HandleMovement;
            _controls.Player.Movement.canceled -= HandleMovement;
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

        private void HandleMovement(CallbackContext ctx)
        {
            _movement.SetMovementDirection(ctx.ReadValue<Vector2>());

            if (ctx.canceled)
            {
                _movement.StopMoving();
            }
        }
    }
}
