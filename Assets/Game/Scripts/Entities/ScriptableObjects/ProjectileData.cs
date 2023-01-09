using UnityEngine;

namespace Assets.Game.Scripts.Entities.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Data/Ammo/Projectile")]
    public class ProjectileData : AmmoData
    {
        public const string ANY_TAG = "*";
        [SerializeField] private float projectileSpeed;
        [SerializeField, Range(0, 4)] private int numberOfBounces;

        public float ProjectileSpeed => projectileSpeed;
        public int NumberOfBounces => numberOfBounces;

        public override bool CheckShot(Vector2 from, Vector2 direction, int currentNumberOfBounces, float distance, LayerMask layerMask, string targetTag, out CheckShotOutput checkShotOutput, bool debug = false, float travelDistance = 0)
        {
            bool hitTarget = false;

            //Follow Raycast in direction and calculate reflection angles of bullet
            var objectHit = Physics2D.Raycast(from, direction, distance, layerMask);
            checkShotOutput = new() { RaycastHit = objectHit, NumberOfBounces = currentNumberOfBounces };

            if (objectHit.collider != null)
            { //change to use TeamID instead of tags
                float currentDistance = objectHit.distance;
                checkShotOutput.TravelDistance = currentDistance + travelDistance;

                if (objectHit.collider.CompareTag("Wall"))
                {
                    if (currentNumberOfBounces < NumberOfBounces)
                    { //If the bullet can bounce check path of the bullet

                        float remDistance = distance - currentDistance;
                        var newFrom = objectHit.point;
                        var dir = objectHit.point - from;
                        var newDirection = Vector2.Reflect(dir, objectHit.normal);

                        //Use recurision to follow path of the bullet
                        hitTarget = CheckShot(newFrom, newDirection, ++currentNumberOfBounces, remDistance, layerMask, targetTag, out checkShotOutput, debug, checkShotOutput.TravelDistance);

                        if (debug && hitTarget)
                        {
                            Debug.DrawRay(from, dir, Color.yellow);
                        }
                    }
                }
                else if (targetTag == ANY_TAG || objectHit.collider.CompareTag(targetTag))
                {
                    hitTarget = true;

                    if (debug)
                    {
                        var dir = objectHit.point - from;
                        Debug.DrawRay(from, dir, Color.blue);
                    }
                }
            }

            return hitTarget;
        }
    }

    public class CheckShotOutput
    {
        public RaycastHit2D RaycastHit { get; set; }
        public int NumberOfBounces { get; set; }
        public float TravelDistance { get; set; }
    }
}