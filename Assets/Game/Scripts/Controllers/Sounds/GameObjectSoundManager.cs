using UnityEngine;

namespace Assets.Game.Scripts.Controllers.Sounds
{
    public class GameObjectSoundManager : SoundManagerBase
    {
        private GameObject _soundChildObject;

        protected override GameObject SoundObject => _soundChildObject;

        protected override void OnAwake()
        {
            _soundChildObject = new GameObject("Sound Holder");
            _soundChildObject.transform.SetParent(transform);
            _soundChildObject.transform.SetPositionAndRotation(transform.position, transform.rotation);
        }

        public void SeperatePlaySoundAndDestroy(string soundName)
        {
            _soundChildObject.transform.SetParent(null);
            StartCoroutine(PlayAndWaitForSound(soundName, () => Destroy(_soundChildObject)));
        }

        protected override void OnStart()
        {
        }
    }
}