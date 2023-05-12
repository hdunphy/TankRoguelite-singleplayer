using Assets.Game.Scripts.Entities.ScriptableObjects;
using System;
using UnityEngine;

namespace Assets.Game.Scripts.Entities.Interfaces
{
    public interface IAmmo
    {
        public event Action OnDestroyed;

        public void Initialize(AmmoData data);
        bool WillDamage(Vector3 position);
        Vector2 Position { get; }
    }
}