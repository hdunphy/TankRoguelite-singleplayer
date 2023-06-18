﻿using Assets.Game.Scripts.Controllers.Sounds;
using UnityEngine;

namespace Assets.Game.Scripts.Entities.ScriptableObjects
{

    public abstract class AmmoData : ScriptableObject
    {
        [SerializeField] private int damage;
        [SerializeField] private DamageType damageType;
        [SerializeField] private Sound initializedSound;
        public int Damage => damage;
        public DamageType DamageType => damageType;

        public Sound InitializedSound => initializedSound;

        public abstract bool CheckShot(Vector2 from, Vector2 direction, int currentNumberOfBounces, float distance, LayerMask layerMask, string targetTag, out CheckShotOutput checkShotOutput, bool debug = false, float travelDistance = 0);
    }
}