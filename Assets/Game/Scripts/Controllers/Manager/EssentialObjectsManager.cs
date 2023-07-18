using UnityEngine;

namespace Assets.Game.Scripts.Controllers.Manager
{
    public class EssentialObjectsManager : MonoBehaviour
    {
        private static EssentialObjectsManager Singleton { get; set; }

        private void Awake()
        {
            if (Singleton == null)
            {
                Singleton = this;
            }
            else
            {
                Debug.LogWarning($"Destroying object from scene: {gameObject.scene.name}");
                Destroy(gameObject);
            }
        }
    }
}