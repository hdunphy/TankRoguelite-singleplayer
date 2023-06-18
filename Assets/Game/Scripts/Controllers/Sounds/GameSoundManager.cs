using UnityEngine;

namespace Assets.Game.Scripts.Controllers.Sounds
{
    public class GameSoundManager : SoundManagerBase
    {
        #region Singleton Code
        private static GameSoundManager _instance;

        public static GameSoundManager Instance => GetOrCreate();

        private static GameSoundManager GetOrCreate()
        {
            if (_instance == null)
            {
                var gameObject = new GameObject
                {
                    name = $"{typeof(GameSoundManager).Name} singleton",
                    hideFlags = HideFlags.DontSave
                };
                return gameObject.AddComponent<GameSoundManager>();
            }

            return _instance;
        }
        #endregion

        protected override GameObject SoundObject => gameObject;

        protected override void OnAwake()
        {
            if(_instance != null) {
                Destroy(this);
                return;
            }

            _instance = this;
        }

        protected override void OnStart()
        {
        }
    }
}