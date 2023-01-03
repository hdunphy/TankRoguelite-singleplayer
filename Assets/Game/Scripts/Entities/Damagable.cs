using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Game.Scripts.Entities
{
    public enum DamageType { Projectile, Explosion }

    public class Damagable : MonoBehaviour
    {
        [SerializeField] private int totalHealth;
        [SerializeField] private List<DamageType> affectedDamageTypes;
        public UnityEvent OnDied;
        public int CurrentHealth { get; private set; }

        private void Awake()
        {
            CurrentHealth = totalHealth;
        }

        public void TakeDamage(int damage, DamageType damageType)
        {
            if(affectedDamageTypes.Contains(damageType))
            {
                CurrentHealth -= damage;

                if(CurrentHealth <= 0)
                {
                    OnDied?.Invoke();
                }
            }
        }
    }
}