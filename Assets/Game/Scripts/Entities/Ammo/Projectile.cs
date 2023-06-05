using Assets.Game.Scripts.Entities.Interfaces;
using Assets.Game.Scripts.Entities.ScriptableObjects;
using Assets.Game.Scripts.Utilities;
using System;
using UnityEngine;

namespace Assets.Game.Scripts.Entities
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Projectile : MonoBehaviour, IAmmo
    {
        [SerializeField] private ParticleSystem particleSmoke;
        [SerializeField] private LayerMask damageableLayerMask;
        [SerializeField] private LayerMask obstacleLayerMask; //Add a singleton of the layermasks

        private Rigidbody2D _rigidbody2d;
        private ProjectileData _data;
        private Vector2 _velocity;
        private int _currentBounces = 0;
        private bool _isDestroyed = false;

        public event Action OnDestroyed;
        public Vector2 Position => transform.position;

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

        public bool WillDamage(Vector3 position, GameObject targetedGameObject, LayerMask mask)
            => _data.CheckShot(transform.position, _velocity, _currentBounces, 100f, mask, "*", out CheckShotOutput shotOutput)
                && shotOutput.RaycastHit.collider.gameObject == targetedGameObject;


        readonly Vector2 orthoganalVector = new(-1, 1);
        public Vector2 GetSafeDirection(Vector2 currentPosition)
        {
            var direction = currentPosition.DirectionTo(Position);
            var safeDirection = direction * orthoganalVector;

            var hit = Physics2D.Raycast(currentPosition, safeDirection, 1f, obstacleLayerMask);
            // Check for obstacles in the way
            if (hit.collider != null)
            {
                // Reverse strafing direction if there's an obstacle
                safeDirection = -safeDirection;
            }

            var hit2 = Physics2D.Raycast(currentPosition, safeDirection, 1f, obstacleLayerMask);
            // Check for obstacles in the way
            if (hit2.collider != null)
            {
                safeDirection = direction;
            }

            return safeDirection;
        }
    }
}