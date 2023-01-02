using Assets.Game.Scripts.Entities;
using UnityEngine;

namespace Assets.Game.Scripts.Controllers
{
    public interface IMovement
    {
        void SetMovementData(MovementData movementData);
        void SetMovementDirection(Vector2 movementDirection);
        void StopMoving();
    }
}
