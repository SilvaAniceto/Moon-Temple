using UnityEngine;

namespace CustomGameController
{
    public enum GameControllerState
    {
        Exploring,
        TurnBased
    }
    public struct CustomPlayerInputHandler
    {
        public Vector3 MoveDirectionInput;
        public bool JumpInput;
        public bool SprintInput;

        public Vector2 CameraAxis;
        public bool ChangeCameraPerspective;
    }
    public interface ICustomPlayerController
    {
        #region GAME CONTROLLER PROPERTIES
        GameControllerState ControllerState { get; set; }
        Vector3 TurnBasedTargetPosition { get; set; }
        Vector3 TurnBasedTargetDirection { get; set; }
        float TurnBasedDistanceTravelled { get; set; }
        bool TurnBasedMovementStarted { get; set; }
        #endregion

        #region GAME CONTROLLER METHODS
        void OnGameControllerStateChanged(GameControllerState state);
        #endregion

        #region PLAYER CONTROLLER PROPERTIES
        CharacterController CharacterController { get; set; }
        Vector3 Forward { get; set; }
        Vector3 Right { get; set; }
        Vector3 CurrentyVelocity { get; set; }
        bool AllowJump { get; set; }
        #endregion

        #region PLAYER CONTROLLER SETTINGS
        float CurrentSpeed { get; }
        float CurrentAcceleration { get; }
        float OnGroundSpeed { get; set; }
        float OnGroundAcceleration { get; set; }
        float OnAirSpeed { get; }
        float OnAirAcceleration { get; }
        float MaxSlopeAngle { get; set; }
        float JumpHeight { get; set; }
        float JumpSpeed { get; }
        #endregion

        #region PLAYER SLOPE METHODS
        bool OnSlope();
        float SlopeAngle();
        RaycastHit SlopeHit();
        Vector3 GetSlopeMoveDirection(Vector3 direction);
        #endregion

        #region PLAYER JUMP & MOVEMENT METHODS
        void Jump(bool jumpInput);
        void UpdateIsometricMovePosition(Vector3 inputDirection, float movementSpeed);
        void UpdateThirdPersonMovePosition(Vector3 inputDirection, float movementSpeed);
        void UpdateFirstPersonMovePosition(Vector3 inputDirection, float movementSpeed);
        void UpdateOverShoulderMovePosition(Vector3 inputDirection, float movementSpeed);
        #endregion

        #region PLAYER INPUT VALUES & METHODS
        Vector3 PlayerDirection { get; set; }
        bool SprintInput { get; set; }
        bool JumpInput { get; set; }
        void SetInput(CustomPlayerInputHandler inputs);
        #endregion
    }
}
