using UnityEngine;

namespace Assets.Game.Scripts.Entities.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Data/Ammo/Wall")]
    public class WallData : AmmoData
    {
        public override bool CheckShot(Vector2 from, Vector2 direction, int currentNumberOfBounces, float distance, LayerMask layerMask, string targetTag, out CheckShotOutput checkShotOutput, bool debug = false, float travelDistance = 0)
        {
            checkShotOutput = new CheckShotOutput();
            return false;
        }
    }
}