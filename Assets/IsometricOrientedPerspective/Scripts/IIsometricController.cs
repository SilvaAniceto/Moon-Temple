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
        public float HorizontalInput;
        public float VerticalInput;
        public Vector3 IsometricMoveDirection;
        public bool JumpInput;
        public bool AccelerateSpeed;
    }
    public interface IIsometricController
    {
        Vector3 CurrentyVelocity { get; set; }
        CapsuleCollider CapsuleCollider { get; set; }
        GameControllerState ControllerState { get; set; }
        CharacterController CharacterController { get; set; }

        bool OnSlope();
        float SlopeAngle();
        RaycastHit SlopeHit();
        void Jump(float jumpHeight, bool jumpInput);
        void SetInput(IsometricInputHandler inputs);
        Vector3 GetSlopeMoveDirection(Vector3 direction);
        void OnGameControllerStateChanged(GameControllerState state);
        void UpdateMovePosition(Vector3 inputDirection, float movementSpeed);
    }
}
