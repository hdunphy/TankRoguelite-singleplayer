using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;

namespace Assets.Game.Scripts.Controllers.Sounds
{
    [Serializable]
    public class Sound
    {
        public string name;

        public AudioClip Clip;

        [Range(0f, 1f)]
        public float Volume;

        [Range(0.1f, 3f)]
        public float Pitch;

        public bool Loop;

        public bool PlayOnStart;

        public AudioMixerGroup AudioMixerGroup;

        [HideInInspector]
        public AudioSource AudioSource;
    }

    public abstract class SoundManagerBase : MonoBehaviour
    {
        [SerializeField, Tooltip("List of sounds available to play")] protected List<Sound> Sounds;
        protected abstract GameObject SoundObject { get; }

        private void Awake()
        {
            OnAwake();

            Sounds.ForEach((s) =>
            {
                s.AudioSource = SoundObject.AddComponent<AudioSource>();
                s.AudioSource.volume = s.Volume;
                s.AudioSource.pitch = s.Pitch;
                s.AudioSource.loop = s.Loop;
                s.AudioSource.clip = s.Clip;
                s.AudioSource.outputAudioMixerGroup = s.AudioMixerGroup; //Not sure if this is accurate
            });
        }

        private void Start()
        {
            OnStart();

            Sounds.ForEach((s) =>
            {
                if (s.PlayOnStart)
                    s.AudioSource.Play();
            });
        }

        public void PlaySound(string name)
        {
            Sound _sound = Sounds.FirstOrDefault(s => s.name == name);

            if (_sound != null)
            {
                _sound.AudioSource.Play();
            }
            else
            {
                Debug.LogWarning($"Couldn't find sound: {name}");
            }
        }

        public void PlaySoundAfterDelay(string name, float delay)
        {
            StartCoroutine(PlaySoundAfterDelayCoroutine(name, delay));
        }

        private IEnumerator PlaySoundAfterDelayCoroutine(string name, float delay)
        {
            yield return new WaitForSeconds(delay);

            PlaySound(name);
        }

        public IEnumerator PlayAndWaitForSound(string name, Action playAfterSound)
        {
            Sound _sound = Sounds.First(s => s.name == name);
            if (_sound != null)
            {
                _sound.AudioSource.Play();

                while (_sound.AudioSource.isPlaying)
                {
                    yield return null;
                }
            }

            playAfterSound?.Invoke();
        }

        public void ToggleSound(string name, bool play)
        {
            Sound _sound = Sounds.FirstOrDefault(s => s.name == name);

            if (_sound == null)
            {
                return;
            }

            if (play && !_sound.AudioSource.isPlaying)
            {
                _sound.AudioSource.Play();
            }
            else
            {
                _sound.AudioSource.Stop();
            }
        }

        protected abstract void OnAwake();

        protected abstract void OnStart();
    }
}