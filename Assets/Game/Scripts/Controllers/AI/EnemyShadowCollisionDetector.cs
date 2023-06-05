using UnityEngine;

namespace Assets.Game.Scripts.Controllers.AI
{
    public class EnemyShadowCollisionDetector : MonoBehaviour
    {
        [SerializeField] private EnemyController enemyController;

        private IMovement _movement;

        private void Awake()
        {
            _movement = enemyController.GetComponent<IMovement>();
        }

        private void LateUpdate()
        {
            transform.position = (Vector2)enemyController.transform.position + _movement.Direction;
        }

        private void OnDrawGizmosSelected()
        {
            if (enemyController == null) return;

            Gizmos.color = Color.red;

            Gizmos.DrawCube(transform.position, enemyController.ColliderSize);
        }
    }
}