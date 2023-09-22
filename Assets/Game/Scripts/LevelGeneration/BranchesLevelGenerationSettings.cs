using System;
using UnityEngine;

namespace Assets.Game.Scripts.LevelGeneration
{
    [CreateAssetMenu(menuName = "Data/Level Generation/Branching Settings")]
    public class BranchesLevelGenerationSettings : LevelGenerationSettings
    {
        [SerializeField, Range(0.1f, 1)] private float mainPathPercentage;
        [SerializeField, Range(0, 1)] private float changeDirectionChance;

        public float MainPathPercentage { get => mainPathPercentage; set => mainPathPercentage = value; }
        public float ChangeDirectionChance { get => changeDirectionChance; set => changeDirectionChance = value; }
    }
}