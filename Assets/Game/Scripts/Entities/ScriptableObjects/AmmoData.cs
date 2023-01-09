using UnityEngine;

namespace Assets.Game.Scripts.Entities.ScriptableObjects
{
    public abstract class AmmoData : ScriptableObject
    {
        [SerializeField] private int damage;
        [SerializeField] private DamageType damageType;
        public int Damage => damage;
        public DamageType DamageType => damageType;

        public abstract bool CheckShot(Vector2 from, Vector2 direction, int currentNumberOfBounces, float distance, ref Vector2 targetPoint, LayerMask layerMask, string targetTag, bool debug = false);
    }
}