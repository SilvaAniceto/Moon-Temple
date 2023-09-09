using UnityEngine;
using UnityEngine.Events;

namespace IsometricGameController
{
    public enum GameControllerState
    {
        Exploring,
        TurnBased
    }
    public struct IsometricInputHandler
    {
        public bool PlayerConfirmEntry;
        public Vector3 IsometricMoveDirection;
        public bool JumpInput;
        public bool AccelerateSpeed;

        public float CameraZoomInput;
        public Vector2 CameraLook;
        public bool CameraAimInput;
    }
    public interface IIsometricController
    {
        bool OnGround { get; set; }
        float OnGroundSpeed { get; set; }
        float OnGroundAcceleration { get; set; }
        float OnAirSpeed { get; }
        float OnAirAcceleration { get; }
        float MaxSlopeAngle { get; set; }
        float TurnBasedDistanceTravelled { get; set; }
        bool AllowJump { get; set; }
        bool TurnBasedMovementStarted { get; set; }
        Vector3 Forward { get; set; }
        Vector3 Right { get; set; }
        Vector3 TurnBasedTargetPosition { get; set; }
        Vector3 TurnBasedTargetDirection { get; set; }
        Vector3 CurrentyVelocity { get; set; }
        CapsuleCollider CapsuleCollider { get; set; }
        CharacterController CharacterController { get; set; }
        GameControllerState ControllerState { get; set; }

        bool OnSlope();
        float SlopeAngle();
        void Jump(bool jumpInput);
        void UpdateMovePosition(Vector3 inputDirection, float movementSpeed);
        void SetInput(IsometricInputHandler inputs);
        void OnGameControllerStateChanged(GameControllerState state);
        Vector3 GetSlopeMoveDirection(Vector3 direction);
        Vector3 UpdateCursorPosition(Vector3 inputDirection, float movementSpeed);
        RaycastHit SlopeHit();
    }
}
