using System.Collections.Generic;
using UnityEngine;

namespace Assets.Game.Scripts.LevelGeneration
{
    [CreateAssetMenu(menuName = "Data/Level Generation/Settings")]
    public class LevelGenerationSettings : ScriptableObject
    {
        [SerializeField] private int maxRooms;
        [SerializeField] private List<RoomChance> roomChances;
        [SerializeField] private Vector2 roomSize;

        public int MaxRooms => maxRooms;
        public List<RoomChance> RoomChances => roomChances;
        public Vector2 RoomSize => roomSize;
    }
}