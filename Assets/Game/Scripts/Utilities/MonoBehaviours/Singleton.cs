using UnityEngine;

namespace Assets.Game.Scripts.Utilities.MonoBehaviours
{
    public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T instance;

        // Property to get the Singleton instance
        public static T Instance
        {
            get
            {
                // If the instance is null, try to find an existing instance in the scene
                if (instance == null)
                {
                    instance = FindObjectOfType<T>();

                    // If still null, create a new GameObject with the script attached
                    if (instance == null)
                    {
                        Debug.LogWarning($"Could not locate singleton of type {typeof(T).Name}");
                        GameObject singletonObject = new(typeof(T).Name);
                        instance = singletonObject.AddComponent<T>();
                    }
                }

                return instance;
            }
        }

        // Called when the MonoBehaviour is initialized
        protected virtual void Awake()
        {
            // If an instance already exists and it's not this, destroy this GameObject
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
                return;
            }

            // Set the instance to this if it's not already set
            instance = this as T;
        }
    }

}