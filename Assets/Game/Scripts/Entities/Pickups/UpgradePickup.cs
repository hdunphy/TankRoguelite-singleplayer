using Assets.Game.Scripts.Controllers;
using Assets.Game.Scripts.Entities.Abilities;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Assets.Game.Scripts.Entities.Pickups
{
    public class UpgradePickup : CollectablePickup
    {
        [SerializeField, ValueDropdown(nameof(GetModifierProperties))] private string field;
        [SerializeField] private string value;
        [SerializeField] private Sprite icon;
        [SerializeField] private SpriteRenderer iconSpriteRenderer;

        private TankModifiers _modifier = new();

        public override void OnPickup(PlayerController controller)
        {
            controller.AddUpgrade(_modifier);
            Destroy(gameObject);
        }

        public IEnumerable GetModifierProperties()
        {
            return typeof(TankModifiers).GetProperties()
                .SelectMany(x => x.PropertyType == typeof(AbilityModifier) ?
                    x.PropertyType.GetProperties().Select(a => $"{x.Name}.{a.Name}") :
                    new List<string> { x.Name });
        }

        private void Awake()
        {
            SetModifier();

            iconSpriteRenderer.sprite = icon;
        }

        private void SetModifier()
        {
            if (field == null)
            {
                Debug.LogError($"Field is left null on {gameObject.name}");
                return;
            }

            var fields = field.Split('.');

            if (fields.Length == 1)
            {
                PropertyInfo propertyInfo = typeof(TankModifiers).GetProperty(field);
                propertyInfo.SetValue(_modifier, Convert.ChangeType(value, propertyInfo.PropertyType));
            }
            else if (fields.Length == 2)
            {
                PropertyInfo propertyInfo1 = typeof(TankModifiers).GetProperty(fields[0]);
                var obj = propertyInfo1.GetValue(_modifier);
                PropertyInfo propertyInfo2 = propertyInfo1.PropertyType.GetProperty(fields[1]);
                propertyInfo2.SetValue(obj, Convert.ChangeType(value, propertyInfo2.PropertyType));
            }
        }
    }
}