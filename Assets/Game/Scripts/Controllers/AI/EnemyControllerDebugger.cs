using System.ComponentModel;
using System.Reflection;
using TMPro;
using UnityEngine;

namespace Assets.Game.Scripts.Controllers.AI
{
    public class EnemyControllerDebugger : MonoBehaviour
    {
        [SerializeField] private EnemyController controller;

        [Header("Debug")]
        [SerializeField] private TMP_Text stateText;

        private Quaternion initialRotation;

        private void Start()
        {
            initialRotation = transform.rotation;
        }

        private void Update()
        {
            stateText.text = controller.MovementBrain.CurrentState.GetCustomAttribute<DisplayNameAttribute>().DisplayName;
        }

        private void LateUpdate()
        {
            transform.rotation = initialRotation;
        }
    }
}