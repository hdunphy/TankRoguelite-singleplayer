using Sirenix.OdinInspector;
using UnityEngine;

namespace Assets.Game.Scripts.LevelGeneration
{
    public class LevelGenerationManager : MonoBehaviour
    {
        [SerializeField] private LevelGenerationSettings settings;
        [SerializeField] private GameObject roomPrefab;
        [SerializeField] private Vector2 startPosition;

        private LevelGeneration _levelGeneration;

        [Button]
        public void Generate()
        {
            _levelGeneration = new(settings);
            _levelGeneration.Generate();

            var root = _levelGeneration.Root;
            AddRoomsToWorld(root);
        }

        private void AddRoomsToWorld(IRoom room)
        {
            if(room is Room _room)
            {
                var roomObject = Instantiate(roomPrefab, _room.Position + startPosition, Quaternion.identity, transform);
            }

            foreach(var neighbor in room.Map.Values)
            {
                AddRoomsToWorld(neighbor);
            }
        }
    }
}