using UnityEngine;

namespace Assets.Game.Scripts.Entities.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Data/Ammo/Projectile")]
    public class ProjectileData : AmmoData
    {
        [SerializeField] private float projectileSpeed;
        [SerializeField, Range(0, 4)] private int numberOfBounces;

        public float ProjectileSpeed => projectileSpeed;
        public int NumberOfBounces => numberOfBounces;

        public override bool CheckShot(Vector2 from, Vector2 direction, int currentNumberOfBounces, float distance, ref Vector2 targetPoint, LayerMask layerMask, string targetTag, bool debug = false)
        {
            bool hitTarget = false;

            //Follow Raycast in direction and calculate reflection angles of bullet
            var objectHit = Physics2D.Raycast(from, direction, distance, layerMask);

            if (objectHit.collider != null)
            { //change to use TeamID instead of tags
                if (objectHit.collider.CompareTag(targetTag))
                {
                    hitTarget = true;

                    if (debug)
                    {
                        var dir = objectHit.point - from;
                        Debug.DrawRay(from, dir, Color.blue);
                    }
                }
                else if (objectHit.collider.CompareTag("Wall"))
                {
                    if (currentNumberOfBounces < NumberOfBounces)
                    { //If the bullet can bounce check path of the bullet
                        float remDistance = distance - Vector3.Distance(from, objectHit.point);
                        var newFrom = objectHit.point;
                        var dir = objectHit.point - from;
                        var newDirection = Vector2.Reflect(dir, objectHit.normal);

                        //Use recurision to follow path of the bullet
                        hitTarget = CheckShot(newFrom, newDirection, ++currentNumberOfBounces, remDistance, ref targetPoint, layerMask, targetTag, debug);

                        if (debug && hitTarget)
                        {
                            Debug.DrawRay(from, dir, Color.yellow);
                        }
                    }
                }
            }

            if (hitTarget)
            {
                targetPoint = objectHit.point;
            }
            return hitTarget;
        }
    }
}