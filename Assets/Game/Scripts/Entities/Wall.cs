using Assets.Game.Scripts.Entities.Interfaces;
using Assets.Game.Scripts.Entities.ScriptableObjects;
using System;
using UnityEngine;

namespace Assets.Game.Scripts.Entities
{
    public class Wall : MonoBehaviour, IAmmo
    {
        public event Action OnDestroyed;

        public void Initialize(AmmoData data) { }

        public bool WillDamage(Vector3 position) => false;

        private void OnDestroy()
        {
            OnDestroyed?.Invoke();
        }
    }
}