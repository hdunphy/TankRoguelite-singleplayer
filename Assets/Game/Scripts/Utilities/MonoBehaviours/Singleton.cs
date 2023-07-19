using UnityEngine;

namespace Assets.Game.Scripts.Utilities.MonoBehaviours
{
    public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T>
    {
        private static T _instance;

        protected Singleton()
        {
            if (_instance != null)
            {
                Debug.LogWarning($"{typeof(T).Name} singleton was already constructed");
            }
            else
            {
                _instance = (T)this;
            }
        }

        public static T Instance => GetOrCreate();

        private static T GetOrCreate()
        {
            if (_instance == null)
            {
                var gameObject = new GameObject
                {
                    name = $"{typeof(T).Name} singleton",
                    hideFlags = HideFlags.DontSave
                };
                return gameObject.AddComponent<T>();
            }

            return _instance;
        }
    }
}