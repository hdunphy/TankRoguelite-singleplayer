using Assets.Game.Scripts.Entities;
using UnityEngine;

namespace Assets.Game.Scripts.Controllers
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class RBMovement : MonoBehaviour, IMovement
    {
        private MovementData _movementData;
        private Rigidbody2D _rigidbody2D;
        private Vector2 _normalizedDirection;

        void Awake()
        {
            _rigidbody2D= GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            RotateTank();
        }

        public void SetMovementData(MovementData movementData) => _movementData = movementData;

        public void SetMovementDirection(Vector2 movementDirection)
        {
            _normalizedDirection = movementDirection.normalized;
            _rigidbody2D.velocity = _normalizedDirection * _movementData.MoveSpeed;
        }

        public void StopMoving()
        {
            _normalizedDirection = transform.up;
        }

        private void RotateTank()
        {
            var target = _normalizedDirection;
            if(target == Vector2.zero) return;

            var angle = Vector2.Angle(transform.up, target);
            if (angle > 90)
            {
                target *= -1;
            }
            transform.up = Vector2.MoveTowards(transform.up, target, _movementData.TankRotationSpeed * Time.deltaTime);
        }
    }
}
