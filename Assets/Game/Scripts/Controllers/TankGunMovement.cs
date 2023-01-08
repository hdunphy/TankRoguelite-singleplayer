using UnityEngine;

namespace Assets.Game.Scripts.Controllers
{
    public class TankGunMovement : MonoBehaviour
    {
        private const float ALIGNMENT_THRESHOLD = 0.1f;
        [SerializeField] private Transform rotationPoint;

        private float _gunRotationSpeed;
        private Vector2 _gunDirection;

        public bool IsAligned { get; private set; }

        private void FixedUpdate()
        {
            RotateGun();
        }

        private void RotateGun()
        {
            var targetAngle = Vector2.SignedAngle(Vector2.right, _gunDirection);
            var angle = Mathf.MoveTowardsAngle(rotationPoint.eulerAngles.z, targetAngle, _gunRotationSpeed * Time.deltaTime);
            IsAligned = Mathf.Abs(targetAngle - angle) > ALIGNMENT_THRESHOLD;

            if (!IsAligned)
            {
                rotationPoint.eulerAngles = new Vector3(0, 0, angle);
            }
        }

        public void SetGunRotationSpeed(float gunRotationSpeed) => _gunRotationSpeed = gunRotationSpeed;
        public void SetLookPoint(Vector2 point)
        {
            Vector2 direction = point - (Vector2)transform.position;
            Debug.DrawRay(transform.position, direction, Color.black, .1f);

            _gunDirection = direction.normalized;
        }
    }
}
