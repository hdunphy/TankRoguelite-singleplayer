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

        private DepthFirstRandomRoomGenerator _levelGeneration;
        private int generationNumber = 0;
        private GameObject levelGenerationGO;

        [Button]
        public void Generate()
        {
            if (levelGenerationGO != null)
            {
                if (Application.isPlaying)
                {
                    Destroy(levelGenerationGO);
                }
                else if (Application.isEditor)
                {
                    DestroyImmediate(levelGenerationGO);
                }
            }

            levelGenerationGO = new GameObject($"Gereration {generationNumber++}");
            levelGenerationGO.transform.SetParent(transform);

            _levelGeneration = new(settings);
            _levelGeneration.Generate();

            var rooms = _levelGeneration.Root.GetRooms(new());
            int i = 0;
            rooms.ForEach(r =>
            {
                var room = Instantiate(roomPrefab, r.Position + startPosition, Quaternion.identity, levelGenerationGO.transform);
                room.name = $"Room {i++}";
            });
        }

        private void AddRoomsToWorld(IRoom room)
        {
            if (room is Room _room)
            {
                var roomObject = Instantiate(roomPrefab, _room.Position + startPosition, Quaternion.identity, transform);
            }

            foreach (var neighbor in room.Map.Values)
            {
                AddRoomsToWorld(neighbor);
            }
        }
    }
}