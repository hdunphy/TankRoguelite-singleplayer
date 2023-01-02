using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Game.Scripts.Controllers
{
    public class PlayerTargeter : MonoBehaviour
    {
        [SerializeField] private Transform targetIconTransform;

        public Transform TargetTransform => targetIconTransform;

        private void Awake()
        {
            transform.SetParent(null);
            transform.position = Vector3.zero;
        }

        private void Update()
        {
            targetIconTransform.position = Mouse.current.position.ReadValue();
        }
    }
}