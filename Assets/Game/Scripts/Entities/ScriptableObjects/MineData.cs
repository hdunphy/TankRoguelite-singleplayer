using UnityEngine;

namespace Assets.Game.Scripts.Entities.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Data/Ammo/Mine")]
    public class MineData : AmmoData
    {
        [SerializeField] private float timerDelay = 1;
        [SerializeField] private float explosionRadius;
        [SerializeField] private float explosionInnerRadius;
        [SerializeField] private float explosionOuterDelay = .25f;
        [SerializeField] private float onDestroyDelay = .5f;
        [SerializeField] private int health = 1;

        public float TimerDelay => timerDelay;
        public float ExplosionRadius => explosionRadius;
        public float ExplosionInnerRadius => explosionInnerRadius;
        public float ExplosionOuterDelay => explosionOuterDelay;
        public float OnDestroyDelay => onDestroyDelay;
        public int Health => health;

        public override bool CheckShot(Vector2 from, Vector2 direction, int currentNumberOfBounces, float distance, LayerMask layerMask, string targetTag, out CheckShotOutput checkShotOutput, bool debug = false, float travelDistance = 0)
        {
            checkShotOutput = new();
            return true;
        }
    }
}