using Assets.Game.Scripts.Entities.ScriptableObjects.AI.StateMachines;
using TMPro;
using UnityEngine;

namespace Assets.Game.Scripts.Controllers.AI
{
    public class EnemyControllerDebugger : MonoBehaviour
    {
        [SerializeField] private EnemyController controller;

        [Header("Debug")]
        [SerializeField] private TMP_Text stateText;
        [SerializeField] private TMP_Text stateMessage;

        private Quaternion initialRotation;
        private Blackboard _blackboard => controller.MovementBrain.Blackboard;

        private void Start()
        {
            initialRotation = transform.rotation;
            //_blackboard = controller.MovementBrain.Blackboard;
        }

        private void Update()
        {
            stateText.text = _blackboard.DebugData.StateName;
            stateMessage.text = _blackboard.DebugData.Message;
        }

        private void LateUpdate()
        {
            transform.rotation = initialRotation;
        }
    }
}