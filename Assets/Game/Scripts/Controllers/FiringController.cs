using Assets.Game.Scripts.Entities;
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

        public FiringType FiringType => firingType;
        public Vector2 FirePointDirection => firePoint.right;
        public Transform FirePoint => firePoint;

        private AbilityHolder _abilityHolder;

        private int _ammoInPlay;
        private TankData _tankData;
        private AbilityModifier abilityModifier => firingType is FiringType.Primary ? 
            _tankData.TankModifiers.PrimaryFireAbilityModifier : _tankData.TankModifiers.SecondaryFireAbilityModifier;
        private int totalAmmo => firingType is FiringType.Primary ? 
            _tankData.TotalPrimaryAmmo : _tankData.TotalSecondaryAmmo;

        private void Awake()
        {
            _abilityHolder = new AbilityHolder(ability, gameObject, FiringType);

            _ammoInPlay = 0;
        }

        private void Update()
        {
            _abilityHolder.Update(Time.deltaTime);
        }

        public void Fire(GameObject prefab, bool atFirePoint, AmmoData data)
        {
            var spawnTransform = atFirePoint ? firePoint : gameObject.transform;
            var instatiatedObject = Instantiate(prefab, spawnTransform.position, spawnTransform.rotation);

            if (instatiatedObject.TryGetComponent(out IAmmo ammo))
            {
                ammo.Initialize(data);
                ammo.OnDestroyed += Ammo_OnDestroyed;
                _abilityHolder.SetAbilityHasUse(++_ammoInPlay < totalAmmo);
            }
        }

        private void Ammo_OnDestroyed()
        {
            _abilityHolder.SetAbilityHasUse(--_ammoInPlay < totalAmmo);
        }

        public void SetAbility(Ability ability)
        {
            _abilityHolder.CancelAbility();
            _abilityHolder = new(ability, gameObject, FiringType);
            _abilityHolder.SetAbilityModifer(abilityModifier);

            _ammoInPlay = 0;
        }

        public void SetAbilityButtonPressed(bool isPressed)
        {
            _abilityHolder.SetAbilityButtonPressed(isPressed);
        }

        public void SetTankData(TankData data)
        {
            _tankData = data;
            _abilityHolder.SetAbilityModifer(abilityModifier);
        }
    }
}