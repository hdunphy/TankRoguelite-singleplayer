using Assets.Game.Scripts.Entities.ScriptableObjects;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Game.Scripts.Controllers.AI
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private EnemyController enemyPrefab;
        [SerializeField] private EnemySpawnPositions[] enemySpawns;

        private List<EnemyController> enemies = new();

        public void SpawnEnemies()
        {
            foreach(var enemy in enemySpawns)
            {
                var enemyGameObject = Instantiate(enemyPrefab, enemy.SpawnPosition, Quaternion.identity);
                enemyGameObject.SetEnemyData(enemy.EnemyData);
                enemyGameObject.enabled = false;
                enemies.Add(enemyGameObject);
            }
        }

        public void EnableEnemies(bool isEnabled) => enemies.ForEach(e => e.enabled = isEnabled);

        private void OnDrawGizmosSelected()
        {
            foreach(var enemy in enemySpawns)
            {
                Gizmos.DrawSphere(enemy.SpawnPosition, .25f);
            }
        }
    }

    [Serializable]
    public struct EnemySpawnPositions
    {
        public EnemyData EnemyData;
        public Vector3 SpawnPosition;
    }
}