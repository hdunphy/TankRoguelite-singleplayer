﻿using Assets.Game.Scripts.Controllers.AI;
using Assets.Game.Scripts.Entities;
using Assets.Game.Scripts.Utilities.MonoBehaviours;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Game.Scripts.Controllers.Manager
{
    public class LevelManager : Singleton<LevelManager>
    {
        [SerializeField] private float secondsToWait;
        [SerializeField] private float playerMovementSpeed;
        [SerializeField] private EnemySpawner spawner;
        [SerializeField] private List<Door> doors;
        [SerializeField] private UnityEvent onStartLevel;

        private PlayerController _playerController;
        private EnterRoomData _enterRoomData;

        public void OnEnterRoom(PlayerController playerController, EnterRoomData enterRoomData)
        {
            _playerController = playerController;
            _enterRoomData = enterRoomData;

            StartCoroutine(OnEnterRoomCoroutine());
        }

        private void EnablePlayerInteractions(bool isEnabled)
        {
            _playerController.enabled = isEnabled;
        }

        private IEnumerator OnEnterRoomCoroutine()
        {
            WaitForSeconds waitForSecond = new(secondsToWait);
            
            //disable player interactions & controls
            EnablePlayerInteractions(false);

            var targetDoor = doors.Single(d => d.DoorSide == _enterRoomData.LeavingDoorSide.Opposite());
            _playerController.transform.position = targetDoor.transform.position;

            var targetPosition = targetDoor.EnterLocation.position;
            while (Vector2.Distance(_playerController.transform.position, targetPosition) > 0.1f)
            {
                float blend = Mathf.Pow(0.5f, Time.deltaTime * playerMovementSpeed);
                Vector3.Lerp(targetPosition, _playerController.transform.position, blend);

                yield return null;
            }

            //close doors
            doors.ForEach(d => d.TriggerOpenDoor(false));

            yield return waitForSecond;

            //spawn enemies
            spawner.SpawnEnemies();

            yield return waitForSecond;

            //start round
            onStartLevel?.Invoke();
            EnablePlayerInteractions(true);
        }
    }

    public struct EnterRoomData
    {
        public DoorSide LeavingDoorSide { get; set; }
    }
}