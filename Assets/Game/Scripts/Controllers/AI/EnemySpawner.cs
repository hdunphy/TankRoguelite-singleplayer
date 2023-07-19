using Assets.Game.Scripts.Controllers.Manager;
using Assets.Game.Scripts.Entities;
using Assets.Game.Scripts.Entities.ScriptableObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Game.Scripts.Controllers.AI
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private EnemyController enemyPrefab;
        [SerializeField] private EnemySpawnPositions[] enemySpawns;

        private readonly List<EnemyController> enemies = new();
        public IEnumerable<EnemyController> Enemies => enemies;

        public void SpawnEnemies()
        {
            foreach(var enemy in enemySpawns)
            {
                var enemyGameObject = Instantiate(enemyPrefab, enemy.SpawnPosition, Quaternion.identity);
                enemyGameObject.SetEnemyData(enemy.EnemyData);

                if(enemyGameObject.TryGetComponent(out Damagable damagable))
                {
                    damagable.OnDied.AddListener(CheckLevelComplete);
                }
                enemyGameObject.enabled = false;
                enemies.Add(enemyGameObject);
            }
        }

        public void EnableEnemies(bool isEnabled) => enemies.ForEach(e => e.enabled = isEnabled);

        public void CheckLevelComplete()
        {
            if(enemies.All(e => !e.isActiveAndEnabled))
            {
                LevelManager.Instance.EndLevel();
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