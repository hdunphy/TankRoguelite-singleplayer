using Assets.Game.Scripts.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using Rand = UnityEngine.Random;

namespace Assets.Game.Scripts.LevelGeneration
{
    public interface ILevelGenerator
    {
        Room Root { get; }

        void Generate();
    }

    public class OnePathWithBranchesGenerator : ILevelGenerator
    {
        private readonly BranchesLevelGenerationSettings _settings;
        private readonly Room _root;

        public Room Root => _root;


        public OnePathWithBranchesGenerator(BranchesLevelGenerationSettings settings)
        {
            _settings = settings;
            _root = new Room(RoomType.Start);
        }

        public void Generate()
        {
            int initialPathLength = Mathf.RoundToInt(_settings.MainPathPercentage * _settings.MaxRooms);
            if (!CreateBranch(initialPathLength, Root))
            {
                Debug.LogError("Something went wrong");
            }
        }

        private bool CreateBranch(int length, Room start)
        {
            var currentRoom = start;
            TryGetRandomDirection(start, out var direction);
            bool isSuccessful = true;

            int currentLength = 1;
            while(currentLength < length)
            {
                if (Rand.value < _settings.ChangeDirectionChance || currentRoom.Map[direction] is not EmptyRoom)
                {
                    var allRooms = start.GetRooms(new());
                    if(TryGetRandomDirection(currentRoom, out var newDirection))
                    {
                        direction = newDirection;
                    }
                    else
                    {
                        currentRoom = currentRoom.Map.Values.First(x => x is Room) as Room;
                        currentLength--;

                        if (currentLength < 1) return false;
                    }
                }

                var neighborRoom = _settings.GetRandomRoomType();
                Room newRoom = neighborRoom is EmptyRoom ? new Room(RoomType.Normal) : neighborRoom as Room;
                newRoom.AddRoomToMap(_root, currentRoom, direction, _settings);
                //currentRoom.Map[direction] = newRoom;

                currentLength++;
            }

            return isSuccessful;
        }

        private static bool TryGetRandomDirection(Room room, out DoorSide direction)
        {
            var sides = room.Map.ToArray()
                .Where(kvp => kvp.Value is not EmptyRoom)
                .Select(kvp => kvp.Key).ToArray();

            bool hasDirection = sides.Length > 1;
            direction = hasDirection ? sides[Rand.Range(0, sides.Length)] : default;
            
            return hasDirection;
        }
    }

    public class DepthFirstRandomRoomGenerator : ILevelGenerator
    {
        private readonly Stack<Room> _roomStack;
        private int _totalRoomCount;
        private readonly LevelGenerationSettings _settings;
        private readonly Room _root;

        public Room Root => _root;

        public DepthFirstRandomRoomGenerator(LevelGenerationSettings settings)
        {
            _roomStack = new Stack<Room>();
            _totalRoomCount = 0;
            _settings = settings;

            _root = new Room(RoomType.Start);
            _roomStack.Push(_root);
        }

        public void Generate()
        {
            while (_roomStack.Count > 0 && _totalRoomCount < _settings.MaxRooms)
            {
                var room = _roomStack.Pop();

                var keys = room.Map.Keys.ToList();
                foreach (var doorSide in keys)
                {
                    if (room.Map[doorSide] is Room)
                    {
                        continue;
                    }

                    IRoom neighborRoom = _settings.GetRandomRoomType();

                    if(neighborRoom is Room neighbor)
                    {
                        neighbor.AddRoomToMap(_root, room, doorSide, _settings);

                        //neighbor.Map[doorSide.Opposite()] = room;
                        _totalRoomCount++;
                        _roomStack.Push(neighbor);
                    }
                    room.Map[doorSide] = neighborRoom;
                }

            }
        }
    }

    public interface IRoom
    {
        public RoomType RoomType { get; }
        IDictionary<DoorSide, IRoom> Map { get; }

        List<Room> GetRooms(List<Room> rooms);
    }

    public enum RoomType { Empty, Normal, Start, End, Chest }

    [Serializable]
    public struct RoomChance
    {
        public RoomType Type;

        [Range(0, 1)]
        public float Chance;
    }

    public class EmptyRoom : IRoom
    {
        public RoomType RoomType => RoomType.Empty;

        public IDictionary<DoorSide, IRoom> Map => new Dictionary<DoorSide, IRoom>();

        public List<Room> GetRooms(List<Room> rooms) => rooms;
    }

    public class Room : IRoom
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

        public List<Room> GetRooms(List<Room> rooms)
        {
            rooms.Add(this);

            foreach(var room in Map.Values)
            {
                if (!rooms.Contains(room))
                {
                    room.GetRooms(rooms);
                }
            }

            return rooms;
        }

        public void AddRoomToMap(Room root, Room fromRoom, DoorSide fromDirection, LevelGenerationSettings _settings)
        {
            Position = fromRoom.Position + (fromDirection.GetDirection() * _settings.RoomSize);
            var sides = Map.Keys.ToList();
            var currentMap = root.GetRooms(new());
            foreach (var potentialNeighbor in currentMap)
            {
                var matchingSide = sides.FindIndex(s => potentialNeighbor.Position == s.GetDirection() + Position);
                if (matchingSide != -1)
                {
                    Map[sides[matchingSide]] = potentialNeighbor;
                    potentialNeighbor.Map[sides[matchingSide].Opposite()] = this;
                }
            }
        }

        public bool IsPotentialNeighbor(Room other, Vector2 roomSize) =>
            Mathf.Abs(other.Position.x - Position.x) <= roomSize.x &&
                Mathf.Abs(other.Position.y - Position.y) <= roomSize.y;
    }
}