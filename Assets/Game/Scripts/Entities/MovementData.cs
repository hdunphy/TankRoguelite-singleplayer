using System;
using UnityEngine;

namespace Assets.Game.Scripts.Entities
{
    [Serializable]
    public class MovementData
    {
        [field: SerializeField] public float MoveSpeed { get; private set; }
        [field: SerializeField] public float TankRotationSpeed { get; private set; }
    }
}