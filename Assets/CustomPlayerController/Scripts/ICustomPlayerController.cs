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
        /// <summary>
        /// The field defines the target movement speed of the player's character.
        /// </summary>
        float OnGroundSpeed { get; set; }

        /// <summary>
        /// The field defines the accelaration time to reach the target speed of the player's character.
        /// </summary>
        float OnGroundAcceleration { get; set; }

        /// <summary>
        /// The field defines the target movement speed of the player's character when in the air.
        /// </summary>
        float OnAirSpeed { get; }

        /// <summary>
        /// The field defines the accelaration time to reach the target speed of the player's character when in air.
        /// </summary>
        float OnAirAcceleration { get; }

        /// <summary>
        /// The field defines the maximun angle of a slope that the player's character can move on.
        /// </summary>
        float MaxSlopeAngle { get; set; }

        /// <summary>
        /// The field defines the maximun jump height that player's character can reach.
        /// </summary>
        float JumpHeight { get; set; }

        /// <summary>
        /// The field defines the jump speed variation.
        /// </summary>
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
        #endregion

        #region PLAYER INPUT METHODS
        void SetInput(CustomPlayerInputHandler inputs);
        #endregion
    }
}
