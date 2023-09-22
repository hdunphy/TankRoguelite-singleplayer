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

        public IRoom GetRandomRoomType()
        {
            var chanceIndex = RoomChances.FindIndex(rc => rc.Chance > Random.value);
            var roomType = chanceIndex == -1 ? RoomType.Empty : RoomChances[chanceIndex].Type;

            return roomType is RoomType.Empty ? new EmptyRoom() : new Room(roomType);
        }
    }
}