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

        public bool TakeDamage(int damage, DamageType damageType)
        {
            bool tookDamage = false;
            if(affectedDamageTypes.Contains(damageType))
            {
                CurrentHealth -= damage;
                tookDamage = true;

                if(CurrentHealth <= 0)
                {
                    OnDied?.Invoke();
                }
            }

            return tookDamage;
        }
    }
}