using Assets.Game.Scripts.Entities.ScriptableObjects;
using System;
using UnityEngine;

namespace Assets.Game.Scripts.Controllers.AI
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private EnemyController enemyPrefab;
        [SerializeField] private EnemySpawnPositions[] enemySpawns;



        private void Start()
        {
            SpawnEnemies();
        }

        public void SpawnEnemies()
        {
            foreach(var enemy in enemySpawns)
            {
                var enemyGameObject = Instantiate(enemyPrefab, enemy.SpawnPosition, Quaternion.identity);
                enemyGameObject.SetEnemyData(enemy.EnemyData);
            }
        }

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