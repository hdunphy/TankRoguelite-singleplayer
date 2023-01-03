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
            _normalizedDirection = Vector2.zero;
        }

        private void RotateTank()
        {
            var target = _normalizedDirection;
            if(target == Vector2.zero) return;

            if (Vector2.Angle(transform.up, target) > 90)
            {
                target *= -1;
            }

            var currentAngle = transform.eulerAngles.z;
            var targetAngle = Vector2.SignedAngle(Vector2.up, target);
            var angle = Mathf.MoveTowardsAngle(currentAngle, targetAngle, _movementData.TankRotationSpeed * Time.deltaTime);
            transform.eulerAngles = new Vector3(0, 0, angle);
        }
    }
}
