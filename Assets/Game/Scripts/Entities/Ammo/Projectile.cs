using Assets.Game.Scripts.Entities.Interfaces;
using Assets.Game.Scripts.Entities.ScriptableObjects;
using System;
using System.Collections;
using UnityEngine;

namespace Assets.Game.Scripts.Entities
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Projectile : MonoBehaviour, IAmmo
    {
        [SerializeField] private ParticleSystem particleSmoke;

        private Rigidbody2D _rigidbody2d;
        private ProjectileData _data;
        private Vector2 _velocity;
        private int _currentBounces = 0;
        private bool _isDestroyed = false;

        public event Action OnDestroyed;

        private void Awake()
        {
            _rigidbody2d = GetComponent<Rigidbody2D>();
        }

        public void OnDestroy()
        {
            OnDestroyed?.Invoke();
        }

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.TryGetComponent(out Damagable damagable))
            {
                var tookDamage = damagable.TakeDamage(_data.Damage, _data.DamageType);
                if (tookDamage)
                {
                    DespawnSelf();
                }
            }
            
            if (!_isDestroyed && collider.TryGetComponent(out Wall _) && ++_currentBounces > _data.NumberOfBounces)
            {
                DespawnSelf();
            }
        }

        private void SetVelocity()
        {
            _rigidbody2d.velocity = transform.right * _data.ProjectileSpeed;
            _velocity = _rigidbody2d.velocity;
        }

        // Update is called once per frame
        void Update()
        {
            if (_rigidbody2d.velocity != _velocity)
            {
                transform.right = _velocity = _rigidbody2d.velocity;
            }

            if (_rigidbody2d.velocity.sqrMagnitude != _data.ProjectileSpeed * _data.ProjectileSpeed)
            {
                SetVelocity();
            }
        }

        public void Initialize(AmmoData data)
        {
            _data = data as ProjectileData;
            SetVelocity();
        }

        private void DespawnSelf()
        {
            if (!_isDestroyed)
            {
                DetachParticle();
                _isDestroyed = true;
                Destroy(gameObject, 0.01f);
            }
        }

        public void DetachParticle()
        {
            //ParticleSystem.MainModule
            var main = particleSmoke.main;
            main.loop = false;
            var emissions = particleSmoke.emission;
            emissions.rateOverTime = 0;
            particleSmoke.transform.parent = null;
            Destroy(particleSmoke.gameObject, 2f);
        }
    }
}