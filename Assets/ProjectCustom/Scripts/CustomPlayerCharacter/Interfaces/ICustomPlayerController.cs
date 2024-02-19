using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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
        public bool VerticalAscendingInput;
        public bool VerticalDescendingInput;
        public bool SprintInput;

        public Vector2 CameraAxis;
        public float CameraZoom;
        //public bool ChangeCameraPerspective;

        public bool AirControlling;
        public bool StartFlight;
    }
    public interface ICustomPlayerController
    {
        #region GAME CONTROLLER METHODS
        void SetPlayerPhysicsSimulation(UnityAction actionSimulated);
        void SetupPlayer();
        bool CheckWallHit();
        void SetCheckersLocation();
        #endregion

        #region PLAYER CONTROLLER PROPERTIES
        CharacterController CharacterController { get; set; }
        Animator CharacterAnimator { get; }
        Transform WallCheckOrigin {  get; set; }
        Vector3 Forward { get; set; }
        Vector3 Right { get; set; }
        Vector3 CurrentyVelocity { get; set; }
        bool AllowJump { get; set; }
        bool StartJump { get; set; }
        float DelayedStopTime { get; set; }
        bool AllowFlight { get; set; }
        #endregion

        #region PLAYER CONTROLLER SETTINGS
        float CurrentSpeed { get; }
        float CurrentAcceleration { get; }
        float OnGroundSpeed { get; set; }
        float OnGroundAcceleration { get; set; }
        float UngroundSpeed { get; }
        float UngroundAcceleration { get; }
        float MaxSlopeAngle { get; set; }
        float JumpHeight { get; set; }
        float JumpSpeed { get; }
        #endregion

        #region PLAYER SLOPE PROPERTIES
        int SlopeCheckCount { get; set; }
        List<Transform> SlopeCheckList { get; set; }
        #endregion

        #region PLAYER SLOPE METHODS
        void SetSlopeCheckSystem(int checkCount, float radius);
        bool OnSlope();
        float SlopeAngle();
        RaycastHit SlopeHit();
        Vector3 GetSlopeMoveDirection(Vector3 direction);
        #endregion

        #region PLAYER JUMP, MOVEMENT & ANIMATION METHODS
        void Jump(bool jumpInput);
        void UpdateThirdPersonMovePosition(Vector3 inputDirection, float movementSpeed);
        void UpdateFirstPersonMovePosition(Vector3 inputDirection, float movementSpeed);
        void Animate();
        #endregion

        #region PLAYER INPUT VALUES & METHODS
        Vector3 PlayerDirection { get; set; }
        bool SprintInput { get; set; }
        bool VerticalAscendingInput { get; set; }
        bool VerticalDescendingInput { get; set; }
        bool FlightControlling { get; set; }
        void SetInput(CustomPlayerInputHandler inputs);
        #endregion
    }
}
