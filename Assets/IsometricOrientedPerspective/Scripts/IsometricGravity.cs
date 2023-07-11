using System.Collections;
using UnityEngine;

namespace CharactersCreator
{
    public class IsometricGravity : MonoBehaviour
    {
        private static Vector3 m_gravityVector = Vector3.zero;
        private static Vector3 m_position = Vector3.zero;
        private static CapsuleCollider m_capsuleCollider;
        private static float m_groundRadiusCheck = 0.85f;
        private static LayerMask m_layerMask;
        private static bool m_isJumping;

        public Vector3 Vector { get { return m_gravityVector; } set {if (value == m_gravityVector) return; m_gravityVector = value; } }
        public Vector3 Position { get { return m_position; } set { m_position = value; } }
        public CapsuleCollider Collider { get { return m_capsuleCollider; } set { m_capsuleCollider = value; } }
        public LayerMask LayerMask { get { return m_layerMask; } set { m_layerMask = value; } }

        public static Vector3 GravityForce()
        {
            return new Vector3(0, Physics.gravity.y * Time.deltaTime, 0);
        }
        public static bool OnGround()
        {
            bool ground;

            ground = Physics.CheckSphere(m_position - new Vector3(0, 0.5f, 0), m_capsuleCollider.radius / m_groundRadiusCheck, m_layerMask, QueryTriggerInteraction.Collide);

            if (ground && m_gravityVector.y < 0)
            {
                m_gravityVector = Vector3.zero;
                m_isJumping = false;
            }

            return ground;
        }
        public static void Jump(float jumpHeight, float jumpForce, bool jumpInput)
        {
            float verticalDisplacement = Mathf.Sqrt(jumpHeight * jumpForce * Physics.gravity.y);

            if (jumpInput && OnGround())
            {
                m_isJumping = true;
                m_gravityVector = new Vector3(0, Mathf.Sqrt(jumpHeight * jumpForce * Physics.gravity.y), 0);
            }

            if (m_position.y >= Mathf.Sqrt(verticalDisplacement))
            {
                m_isJumping = false;
            }

            if (!jumpInput && m_isJumping)
            {
                m_isJumping = false;
                m_gravityVector = new Vector3(0, m_position.y, 0);
            }

            m_gravityVector += GravityForce();
        }
    }
}
