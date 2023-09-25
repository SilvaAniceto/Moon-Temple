using UnityEngine;

namespace CustomGameController
{
    public interface ICustomGravity
    {
        #region SIMULATED PHYSICS PROPERTIES
        Vector3 LastGroundedPosition { get; set; }
        Vector3 CurrentUngroudedPosition { get; set; }
        bool OnGround { get; set; }
        bool Jumping { get; set; }
        bool Falling { get; set; }
        float Gravity { get; }
        float GravityMultiplierFactor { get; set; }
        float Drag { get; set; }
        LayerMask GroundLayer { get; set; }
        Vector3 GravityVelocity { get; set; }
        #endregion

        #region SIMULATED PHYSICS METHODS
        float ApplyDrag(float velocity, float drag);
        void ApplyGravity();
        bool CheckGroundLevel();
        #endregion
    }
}
