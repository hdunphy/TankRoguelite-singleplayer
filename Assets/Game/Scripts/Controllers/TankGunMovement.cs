using UnityEngine;

namespace Assets.Game.Scripts.Controllers
{
    public class TankGunMovement : MonoBehaviour
    {
        [SerializeField] private Transform rotationPoint;

        private float _gunRotationSpeed;
        private Vector2 _gunDirection;

        private void FixedUpdate()
        {
            RotateGun();
        }

        private void RotateGun()
        {
            rotationPoint.right = Vector2.MoveTowards(rotationPoint.right, _gunDirection, _gunRotationSpeed * Time.deltaTime);
        }

        public void SetGunRotationSpeed(float gunRotationSpeed) => _gunRotationSpeed = gunRotationSpeed;
        public void SetLookPoint(Vector2 point)
        {
            Vector2 direction = point - (Vector2)transform.position;
            Debug.DrawRay(transform.position, direction, Color.black, .1f);
            
            _gunDirection = direction;
        }
    }
}
