using Assets.Game.Scripts.Entities.Abilities;
using Assets.Game.Scripts.Entities.Interfaces;
using Assets.Game.Scripts.Entities.ScriptableObjects;
using UnityEngine;

namespace Assets.Game.Scripts.Controllers
{
    public enum FiringType { Primary, Secondary }
    public class FiringController : MonoBehaviour
    {
        [SerializeField] private Transform firePoint;
        [SerializeField, Tooltip("Starting Ability")] private Ability ability;
        [SerializeField] private FiringType firingType;
        [SerializeField] private int totalAmmo;

        public FiringType FiringType => firingType;
        public Vector2 FirePointDirection => firePoint.right;

        private AbilityHolder _abilityHolder;

        private int _currentAmmo;

        private void Awake()
        {
            _abilityHolder = new AbilityHolder(ability, gameObject, FiringType);

            _currentAmmo = totalAmmo;
        }

        private void Update()
        {
            _abilityHolder.Update(Time.deltaTime);
        }

        public void SetAbilityButtonPressed(bool isPressed)
        {
            _abilityHolder.SetAbilityButtonPressed(isPressed);
        }

        public void Fire(GameObject prefab, bool atFirePoint, AmmoData data)
        {
            var spawnTransform = atFirePoint ? firePoint : gameObject.transform;
            var instatiatedObject = Instantiate(prefab, spawnTransform.position, spawnTransform.rotation);

            if (instatiatedObject.TryGetComponent(out IAmmo ammo))
            {
                ammo.Initialize(data);
                ammo.OnDestroyed += Ammo_OnDestroyed;
                _abilityHolder.SetAbilityHasUse(--_currentAmmo > 0);
            }
        }

        private void Ammo_OnDestroyed()
        {
            _abilityHolder.SetAbilityHasUse(++_currentAmmo > 0);
        }

        public void SetAbility(Ability ability)
        {
            _abilityHolder.CancelAbility();
            _abilityHolder = new(ability, gameObject, FiringType);
        }
    }
}