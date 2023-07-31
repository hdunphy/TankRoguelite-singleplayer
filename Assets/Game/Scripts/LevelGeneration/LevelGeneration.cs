using Assets.Game.Scripts.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Game.Scripts.LevelGeneration
{
    public class LevelGeneration
    {
        private Queue<Room> _roomQueue;
        private int _totalRoomCount;
        private readonly LevelGenerationSettings _settings;
        private readonly Room _root;

        public Room Root => _root;

        public LevelGeneration(LevelGenerationSettings settings)
        {
            _roomQueue = new Queue<Room>();
            _totalRoomCount = 0;
            _settings = settings;

            _root = new Room(RoomType.Start);
            _roomQueue.Enqueue(_root);
        }

        public void Generate()
        {
            while (_roomQueue.Count > 0 && _totalRoomCount < _settings.MaxRooms)
            {
                var room = _roomQueue.Dequeue();

                var keys = room.Map.Keys.ToList();
                foreach (var doorSide in keys)
                {
                    if (room.Map[doorSide] is Room)
                    {
                        continue;
                    }

                    IRoom neighborRoom = GetRandomRoomType();

                    if(neighborRoom is Room neighbor)
                    {
                        neighbor.Position = room.Position + (doorSide.GetDirection() * _settings.RoomSize);
                        neighbor.Map[doorSide.Opposite()] = room;
                        _totalRoomCount++;
                        _roomQueue.Enqueue(neighbor);
                    }
                    room.Map[doorSide] = neighborRoom;
                }

            }
        }

        private IRoom GetRandomRoomType()
        {
            var chanceIndex = _settings.RoomChances.FindIndex(rc => rc.Chance < UnityEngine.Random.value);
            var roomType = chanceIndex == -1 ? RoomType.Empty : _settings.RoomChances[chanceIndex].Type;

            return roomType is RoomType.Empty ? new EmptyRoom() : new Room(roomType);
        }

    }

    public interface IRoom
    {
        public RoomType RoomType { get; }
        IDictionary<DoorSide, IRoom> Map { get; }
    }

    public enum RoomType { Empty, Normal, Start, End, Chest }

    [Serializable]
    public struct RoomChance
    {
        public RoomType Type;

        [Range(0, 1)]
        public float Chance;
    }

    public struct EmptyRoom : IRoom
    {
        public RoomType RoomType => RoomType.Empty;

        public IDictionary<DoorSide, IRoom> Map => new Dictionary<DoorSide, IRoom>();
    }

    public struct Room : IRoom
    {
        public IDictionary<DoorSide, IRoom> Map { get; }
        public RoomType RoomType { get; private set; }
        public Vector2 Position { get; set; }

        public Room(RoomType roomType)
        {
            Position = Vector2.zero;
            RoomType= roomType;
            Map = new Dictionary<DoorSide, IRoom>()
            {
                { DoorSide.Left, new EmptyRoom() },
                { DoorSide.Top, new EmptyRoom() },
                { DoorSide.Right, new EmptyRoom() },
                { DoorSide.Bottom, new EmptyRoom() },
            };
        }
    }
}