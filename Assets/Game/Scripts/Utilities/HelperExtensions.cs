using Unity.VisualScripting;
using UnityEngine;

namespace Assets.Game.Scripts.Utilities
{
    public static class HelperExtensions
    {
        public static Vector2 Rotate(this Vector2 value, float angle)
        {
            float sin = Mathf.Sin(angle * Mathf.Deg2Rad);
            float cos = Mathf.Cos(angle * Mathf.Deg2Rad);

            float tx = value.x;
            float ty = value.y;
            value.x = cos * tx - sin * ty;
            value.y = sin * tx + cos * ty;

            return value;
        }

        public static Vector2 DirectionTo(this Vector2 value, Vector2 target) => (target - value).normalized;
    }
}