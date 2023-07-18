using Assets.Game.Scripts.Utilities.MonoBehaviours;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Game.Scripts.Controllers.Manager
{
    public class GameManager : Singleton<GameManager>
    {
        [SerializeField] private int startingSceneBuildIndex;

        private int currentSceneBuildIndex;

        private void Awake()
        {
            currentSceneBuildIndex = startingSceneBuildIndex;
        }

        private void Start()
        {
            var player = FindObjectOfType<PlayerController>();
            player.enabled = false;
            StartCoroutine(EnterRoom(player, new() { LeavingDoorSide = Entities.DoorSide.Left }));
        }

        public void ExitRoom(PlayerController playerController, EnterRoomData enterRoomData)
        {
            StartCoroutine(SwitchScenes(playerController, enterRoomData));
        }

        private IEnumerator SwitchScenes(PlayerController playerController, EnterRoomData enterRoomData)
        {
            yield return SceneManager.UnloadSceneAsync(currentSceneBuildIndex);

            yield return EnterRoom(playerController, enterRoomData);
        }

        private IEnumerator EnterRoom(PlayerController playerController, EnterRoomData enterRoomData)
        {
            yield return SceneManager.LoadSceneAsync(GetNextSceneIndex());

            //yield return null;
            LevelManager.Instance.OnEnterRoom(playerController, enterRoomData);
        }

        private int GetNextSceneIndex() => currentSceneBuildIndex;
    }
}