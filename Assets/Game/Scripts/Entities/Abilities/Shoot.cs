using Assets.Game.Scripts.Controllers;
using Assets.Game.Scripts.Entities.ScriptableObjects;
using System.Linq;
using UnityEngine;

namespace Assets.Game.Scripts.Entities.Abilities
{
    [CreateAssetMenu(menuName = "Data/Abilities/Shoot")]
    public class Shoot : Ability
    {
        [SerializeField, Tooltip("Prefab to be instatiated when ability is triggerd")] private GameObject ammoPrefab;
        [SerializeField] private bool spawnAtFirePoint;
        [SerializeField] private AmmoData ammoData;

        public override void Activate(GameObject parent)
        {
            //TODO: need to determine primary vs secondary
            parent.GetComponents<FiringController>()
                .FirstOrDefault(x => x.FiringType == ((FiringType)Parameter) && x.enabled)
                ?.Fire(ammoPrefab, spawnAtFirePoint, ammoData);
        }

        public override void BeginCooldown(GameObject parent)
        {
        }

        public override void CancelAbility(GameObject parent)
        {
        }

        public bool CheckShot(Vector2 from, Vector2 direction, float distance, LayerMask layerMask, out CheckShotOutput checkShotOutput, string targetTag = "Player", bool debug = false)
            => ammoData.CheckShot(from, direction, 0, distance, layerMask, targetTag, out checkShotOutput, debug);
    }
}