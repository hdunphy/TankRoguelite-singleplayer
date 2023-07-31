using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Rendering.Universal;

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
            foreach(Transform child in transform)
            {
                if (Application.isPlaying)
                    Destroy(child.gameObject);
                else if (Application.isEditor)
                    DestroyImmediate(child.gameObject);
            }

            _levelGeneration = new(settings);
            _levelGeneration.Generate();

            var rooms = _levelGeneration.Root.GetRooms(new());
            rooms.ForEach(r => Instantiate(roomPrefab, r.Position + startPosition, Quaternion.identity, transform));
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