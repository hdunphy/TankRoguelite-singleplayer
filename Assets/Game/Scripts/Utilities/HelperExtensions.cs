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

        // Method to move a vector towards another vector based on a percentage value
        public static Vector3 RotateTowards(this Vector3 originalVector, Vector3 targetVector, float percentage)
        {
            // Calculate the direction from the original vector to the target vector
            Vector3 direction = targetVector - originalVector;

            // Calculate the new vector by moving a percentage of the direction towards the target vector
            Vector3 newVector = originalVector + (direction * percentage);

            return newVector;
        }

        public static Vector2 RotateTowards(this Vector2 originalVector, Vector2 targetVector, float percentage) 
            => (Vector2)RotateTowards((Vector3)originalVector, (Vector3)targetVector, percentage);
    }
}