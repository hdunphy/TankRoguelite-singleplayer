using Assets.Game.Scripts.Controllers;
using Assets.Game.Scripts.Controllers.Manager;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Game.Scripts.Entities
{
    public class Door : MonoBehaviour
    {
        [SerializeField] private DoorSide doorSide;
        [SerializeField] private Transform enterLocation;
        [SerializeField] private UnityEvent<bool> onCloseDoor;

        public DoorSide DoorSide => doorSide;
        public Transform EnterLocation => enterLocation;

        public void TriggerCloseDoor(bool isClosed) => onCloseDoor?.Invoke(isClosed);

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.TryGetComponent(out PlayerController pc))
            {
                GameManager.Instance.ExitRoom(pc, new() { LeavingDoorSide = doorSide });
            }
        }
    }

    public static class DoorExtensions
    {
        public static DoorSide Opposite(this DoorSide side) => side switch
        {
            DoorSide.Right => DoorSide.Left,
            DoorSide.Left => DoorSide.Right,
            DoorSide.Top => DoorSide.Bottom,
            DoorSide.Bottom => DoorSide.Top,
            _ => side,
        };

        public static Vector2 GetDirection(this DoorSide doorSide) => doorSide switch
        {
            DoorSide.Right => Vector2.right,
            DoorSide.Left => Vector2.left,
            DoorSide.Top => Vector2.up,
            DoorSide.Bottom => Vector2.down,
            _ => Vector2.zero,
        };
    }

    public enum DoorSide { Left, Top, Right, Bottom };
}