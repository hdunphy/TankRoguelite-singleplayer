using Assets.Game.Scripts.Controllers.Sounds;
using Assets.Game.Scripts.Entities.Interfaces;
using Assets.Game.Scripts.Entities.ScriptableObjects;
using Assets.Game.Scripts.Utilities;
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

        [SerializeField] private Sound explosionSound;
        [SerializeField] private Sound timerCountdownSound;

        [SerializeField] private Color flashColor;
        [SerializeField] private float flashTime;

        private MineData _data;
        private float _detonationTime;
        private float _timerDelay;
        private bool _isDetonating = false;
        private AudioSource _audioSourceExplosion;
        private AudioSource _audioSourceTimer;

        public event Action OnDestroyed;

        public Vector2 Position => transform.position;

        public void Initialize(AmmoData data)
        {
            _data = data as MineData;
            _detonationTime = _data.TimerDelay + Time.time;
            _timerDelay = _data.TimerDelay;

            _audioSourceExplosion.PlayOneShot(_data.InitializedSound.Clip, _data.InitializedSound.Volume);
            StartCoroutine(Flash());
        }

        private void Awake()
        {
            _audioSourceExplosion = gameObject.AddComponent<AudioSource>();
            _audioSourceTimer = gameObject.AddComponent<AudioSource>();

            explosionSound.AddToSource(_audioSourceExplosion);
            timerCountdownSound.AddToSource(_audioSourceTimer);
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
                _audioSourceTimer.Play();
                yield return new WaitForSeconds(flashTime);
                spriteRenderer.color = startColor;
            }

            while (spriteRenderer.enabled)
            {
                yield return new WaitForSeconds(flashTime);
                spriteRenderer.color = flashColor;
                _audioSourceTimer.Play();
                //pitch/vol increase?
                yield return new WaitForSeconds(flashTime);
                spriteRenderer.color = startColor;
            }
        }

        private IEnumerator Explode()
        {
            _isDetonating = true;
            ExplodeParticleEffect();
            _audioSourceExplosion.Play();

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

        public bool WillDamage(Vector3 position, GameObject targetedGameObject, LayerMask mask) => 
            Vector2.Distance(position, transform.position) <= _data.ExplosionRadius;

        public Vector2 GetSafeDirection(Vector2 currentPosition) => currentPosition.DirectionTo(Position);
    }
}