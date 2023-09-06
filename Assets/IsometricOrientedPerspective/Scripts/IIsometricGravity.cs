using UnityEngine;

namespace IsometricGameController
{
    public interface IIsometricGravity
    {
        Vector3 LastGroundedPosition { get; set; }
        Vector3 CurrentUngroudedPosition { get; set; }
        bool Jumping { get; set; }
        bool Falling { get; set; }
        float Gravity { get; }
        float GravityMultiplierFactor { get; set; }
        float Drag { get; set; }
        float JumpSpeed { get; }
        float JumpHeight { get; set; }
        LayerMask WhatIsGround { get; set; }
        Vector3 GravityVelocity { get; set; }

        float ApplyDrag(float velocity, float drag);
        void ApplyGravity();
        bool CheckGroundLevel();
    }
}
