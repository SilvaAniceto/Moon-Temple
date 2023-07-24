using UnityEngine;

namespace IOP
{
    public interface IIsometricController
    {
        Vector3 CurrentyVelocity { get; set; }
        CapsuleCollider CapsuleCollider { get; set; }
        CharacterController CharacterController { get; set; }

        void UpdateMovePosition(Vector3 inputDirection, float movementSpeed);
        bool OnSlope();
        float SlopeAngle();
        RaycastHit SlopeHit();
        Vector3 GetSlopeMoveDirection(Vector3 direction);
    }
}
