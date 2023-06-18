﻿using Assets.Game.Scripts.Entities.Interfaces;
using Assets.Game.Scripts.Entities.ScriptableObjects;
using System;
using UnityEngine;

namespace Assets.Game.Scripts.Entities
{
    public class Wall : MonoBehaviour, IAmmo
    {
        private AudioSource _audioSource;
        public event Action OnDestroyed;
        public Vector2 Position => transform.position;

        public void Initialize(AmmoData data) {
            data.InitializedSound.AddToSource(_audioSource);
            _audioSource.Play();
        }

        public bool WillDamage(Vector3 position, GameObject targetedGameObject, LayerMask mask) => false;

        private void Awake()
        {
            _audioSource = gameObject.AddComponent<AudioSource>();
        }

        private void OnDestroy()
        {
            OnDestroyed?.Invoke();
        }

        //maybe return the inverse direction?
        public Vector2 GetSafeDirection(Vector2 currentPosition) => Vector2.zero;
    }
}