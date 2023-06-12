using System.Collections.Generic;
using UnityEngine;

namespace Assets.Game.Scripts.Controllers.Tanks
{
    public class TankColorManipulator : MonoBehaviour
    {
        [SerializeField] private List<SpriteRenderer> sprites;

        public void ChangeSpriteColor(Color color) => sprites.ForEach(s => s.color = color);
    }
}