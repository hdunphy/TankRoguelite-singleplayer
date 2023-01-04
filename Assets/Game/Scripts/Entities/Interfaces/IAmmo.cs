using Assets.Game.Scripts.Entities.ScriptableObjects;
using System;

namespace Assets.Game.Scripts.Entities.Interfaces
{
    public interface IAmmo
    {
        public event Action OnDestroyed;

        public void Initialize(AmmoData data);
    }
}