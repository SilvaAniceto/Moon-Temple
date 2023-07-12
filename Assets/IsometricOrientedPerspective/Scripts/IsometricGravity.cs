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
        private static float m_verticalDisplacement;
        public Vector3 Vector { get { return m_gravityVector; } set {if (value == m_gravityVector) return; m_gravityVector = value; } }
        public Vector3 Position { get { return m_position; } set { m_position = value; } }
        public CapsuleCollider Collider { get { return m_capsuleCollider; } set { m_capsuleCollider = value; } }
        public LayerMask LayerMask { get { return m_layerMask; } set { m_layerMask = value; } }
        public bool Jumping { get { return m_isJumping;} }
        public float VerticalDisplacement { get { return Mathf.Sqrt(m_verticalDisplacement); } }
        public float Y { get { return m_position.y; } }
        public static Vector3 GravityForce()
        {
            if (!OnGround() && !m_isJumping) return new Vector3(0, (Physics.gravity.y * (Mathf.Sqrt(m_verticalDisplacement) / 2)) * Time.deltaTime, 0);
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
            m_verticalDisplacement = Mathf.Sqrt(jumpHeight * jumpForce * Physics.gravity.y);

            if (jumpInput && OnGround())
            {
                m_isJumping = true;
                m_gravityVector = new Vector3(0, m_verticalDisplacement, 0);
            }

            if (Mathf.RoundToInt(m_position.y) >= Mathf.RoundToInt(Mathf.Sqrt(m_verticalDisplacement)))
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
