using Assets.Game.Scripts.Entities;
using UnityEngine;

namespace Assets.Game.Scripts.Controllers
{
    public interface IMovement
    {
        void SetTankData(TankData data);
        void SetMovementDirection(Vector2 movementDirection);
        void StopMoving();
    }
}
