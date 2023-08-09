using UnityEngine;

namespace IsometricGameController
{
    public interface IIsometricGravity
    {
        bool OnAir { get; set; }
        float Gravity { get; }
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
