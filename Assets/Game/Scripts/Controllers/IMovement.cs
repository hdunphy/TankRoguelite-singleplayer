using Assets.Game.Scripts.Entities;
using UnityEngine;

namespace Assets.Game.Scripts.Controllers
{
    public interface IMovement
    {
        Vector2 Direction { get; }

        void SetTankData(TankData data);
        void SetMovementDirection(Vector2 movementDirection);
        void StopMoving();
    }
}
