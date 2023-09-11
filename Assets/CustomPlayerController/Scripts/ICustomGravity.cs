using UnityEngine;

namespace CustomGameController
{
    public interface ICustomGravity
    {
        #region SIMULATED PHYSICS PROPERTIES
        /// <summary>
        /// The field is used by Custom Character Controller to define the last position of the player's character before go ungrounded.
        /// </summary>
        Vector3 LastGroundedPosition { get; set; }

        /// <summary>
        /// The field is used by Custom Character Controller to define the delayd position of the player's character after go ungrounded.
        /// </summary>
        Vector3 CurrentUngroudedPosition { get; set; }

        /// <summary>
        /// The field is used by Custom Character Controller to specify whether the player's character is on groud level or not.
        /// </summary>
        bool OnGround { get; set; }

        /// <summary>
        /// The field is used by Custom Character Controller as an ungrounded state. It's specify wheter the player's character is jumping or not.
        /// </summary>
        bool Jumping { get; set; }

        /// <summary>
        /// The field is used by Custom Character Controller as an ungrounded state. It's specify wheter the player's character is falling or not.
        /// </summary>
        bool Falling { get; set; }

        /// <summary>
        /// The field defines the value of the simulated gravity.
        /// </summary>
        float Gravity { get; }

        /// <summary>
        /// The field is used along with ungrounded states to better emulate vertical axis physics.
        /// </summary>
        float GravityMultiplierFactor { get; set; }

        /// <summary>
        /// The field is used along with ungrounded states to better emulate horizontal axis physics.
        /// </summary>
        float Drag { get; set; }

        /// <summary>
        /// Defines which Unity layer is the ground level.
        /// </summary>
        LayerMask GroundLayer { get; set; }

        /// <summary>
        /// The field is used by Custom Character Controller to calculate de gravity variation to applied to player's character.
        /// </summary>
        Vector3 GravityVelocity { get; set; }
        #endregion

        #region SIMULATED PHYSICS METHODS
        /// <summary>
        /// This method applys drag to the player's character.
        /// </summary>
        float ApplyDrag(float velocity, float drag);

        /// <summary>
        /// This method applys gravity to the player's character.
        /// </summary>
        void ApplyGravity();

        /// <summary>
        /// This method checks if player's character collider is on ground level.
        /// </summary>
        bool CheckGroundLevel();
        #endregion
    }
}
