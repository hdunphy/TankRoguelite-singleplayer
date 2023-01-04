using Assets.Game.Scripts.Entities.Interfaces;
using Assets.Game.Scripts.Entities.ScriptableObjects;
using System;
using System.Collections;
using UnityEngine;

namespace Assets.Game.Scripts.Entities.Ammo
{
    [RequireComponent(typeof(Collider2D), typeof(Damagable))]
    public class Mine : MonoBehaviour, IAmmo
    {
        [SerializeField] private GameObject explosion;
        [SerializeField] private SpriteRenderer spriteRenderer;

        [SerializeField] private Color flashColor;
        [SerializeField] private float flashTime;

        private MineData _data;
        private float _detonationTime;
        private float _timerDelay;
        private bool _isDetonating = false;

        public event Action OnDestroyed;

        public void Initialize(AmmoData data)
        {
            _data = data as MineData;
            _detonationTime = _data.TimerDelay + Time.time;
            _timerDelay = _data.TimerDelay;

            StartCoroutine(Flash());
        }

        private void OnDestroy()
        {
            OnDestroyed?.Invoke();
        }

        private void Update()
        {
            if (Time.time >= _detonationTime)
            {
                StartCoroutine(Explode());
            }
        }

        private IEnumerator Flash()
        {
            float interval = (_timerDelay - (4 * flashTime)) / 4;
            Color startColor = spriteRenderer.color;

            for (int i = 0; i < 3; i++)
            {
                yield return new WaitForSeconds(interval);
                spriteRenderer.color = flashColor;
                yield return new WaitForSeconds(flashTime);
                spriteRenderer.color = startColor;
            }

            while (spriteRenderer.enabled)
            {
                yield return new WaitForSeconds(flashTime);
                spriteRenderer.color = flashColor;
                yield return new WaitForSeconds(flashTime);
                spriteRenderer.color = startColor;
            }
        }

        private IEnumerator Explode()
        {
            _isDetonating = true;
            ExplodeParticleEffect();

            DealDamage(_data.ExplosionInnerRadius);

            yield return new WaitForSeconds(_data.ExplosionOuterDelay);

            DealDamage(_data.ExplosionRadius);

            yield return new WaitForSeconds(_data.OnDestroyDelay);

            Destroy(gameObject);
        }

        private void DealDamage(float _radius)
        {
            var colliderInRadius = Physics2D.OverlapCircleAll(transform.position, _radius);

            foreach (Collider2D _collider in colliderInRadius)
            {
                if (_collider.TryGetComponent(out Damagable damageable) && _collider.gameObject != gameObject)
                {
                    damageable.TakeDamage(_data.Damage, _data.DamageType);
                }
            }
        }

        private void ExplodeParticleEffect()
        {
            explosion.SetActive(true);
            spriteRenderer.enabled = false;
        }

        public void InitiateExposion()
        {
            if (!_isDetonating)
            {
                StartCoroutine(Explode());
            }
        }
    }
}