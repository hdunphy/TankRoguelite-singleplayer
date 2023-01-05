using Assets.Game.Scripts.Entities;
using UnityEngine;

namespace Assets.Game.Scripts.Controllers
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class RBMovement : MonoBehaviour, IMovement
    {
        private TankData _tankData;
        private Rigidbody2D _rigidbody2D;
        private Vector2 _normalizedDirection;

        void Awake()
        {
            _rigidbody2D= GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            if(_normalizedDirection != Vector2.zero)
            {
                _rigidbody2D.velocity = _normalizedDirection * _tankData.MoveSpeed;
            }

            RotateTank();
        }

        public void SetTankData(TankData tankData) => _tankData = tankData;

        public void SetMovementDirection(Vector2 movementDirection)
        {
            _normalizedDirection = movementDirection.normalized;
        }

        public void StopMoving() => _normalizedDirection = Vector2.zero;

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
            var angle = Mathf.MoveTowardsAngle(currentAngle, targetAngle, _tankData.TankRotationSpeed * Time.deltaTime);
            transform.eulerAngles = new Vector3(0, 0, angle);
        }
    }
}
