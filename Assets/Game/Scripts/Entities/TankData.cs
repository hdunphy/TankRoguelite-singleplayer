using System.Collections;
using UnityEngine;

namespace Assets.Game.Scripts.Entities
{
    [CreateAssetMenu(menuName = "Data/Tank Data")]
    public class TankData : ScriptableObject
    {
        [field:SerializeField] public int Health { get; private set; }
        [field: SerializeField] public float GunRotationSpeed { get; private set; }
        [field: SerializeField] public MovementData MovementData { get; private set; }
    }
}