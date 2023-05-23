using Assets.Game.Scripts.Entities.ScriptableObjects;
using System;
using UnityEngine;

namespace Assets.Game.Scripts.Entities.Interfaces
{
    public interface IAmmo
    {
        Vector2 Position { get; }

        public event Action OnDestroyed;

        Vector2 GetSafeDirection(Vector2 currentPosition);
        public void Initialize(AmmoData data);
        bool WillDamage(Vector3 position, GameObject targetedGameObject);
    }
}