using UnityEngine;
using UnityEngine.Events;

namespace IsometricGameController
{
    public enum GameControllerState
    {
        Exploring,
        Combat
    }
    public struct IsometricInputHandler
    {
        public bool PlayerConfirmEntry;
        public float HorizontalInput;
        public float VerticalInput;
        public Vector3 IsometricMoveDirection;
        public bool JumpInput;
        public bool AccelerateSpeed;
    }
    public interface IIsometricController
    {
        GameControllerState ControllerState { get; set; }
        Vector3 CurrentyVelocity { get; set; }
        CapsuleCollider CapsuleCollider { get; set; }
        CharacterController CharacterController { get; set; }

        void OnGameControllerStateChanged(GameControllerState state);
        void UpdateMovePosition(Vector3 inputDirection, float movementSpeed);
        bool OnSlope();
        float SlopeAngle();
        RaycastHit SlopeHit();
        Vector3 GetSlopeMoveDirection(Vector3 direction);
        void SetInput(IsometricInputHandler inputs);
    }
}
