using UnityEngine;

namespace CharactersCreator
{
    public class IsometricGravity
    {
        public static float UniversalGravity { get { return -9.81f; } }

        private static Vector3 m_gravityVector = Vector3.zero;
        private static Vector3 m_position = Vector3.zero;
        private static CapsuleCollider m_capsuleCollider;
        private static float m_groundRadiusCheck;
        private static LayerMask m_layerMask;

        public Vector3 Vector { get { return m_gravityVector; } set {if (value == m_gravityVector) return; m_gravityVector = value; } }
        public Vector3 Position { get { return m_position; } set { m_position = value; } }
        public CapsuleCollider Collider { get { return m_capsuleCollider; } set { m_capsuleCollider = value; } }
        public float GroundRadiusCheck { get { return m_groundRadiusCheck;} set { m_groundRadiusCheck = value;} }
        public LayerMask LayerMask { get { return m_layerMask; } set { m_layerMask = value; } }

        public static Vector3 GravityForce()
        {
            return new Vector3(0, UniversalGravity * Time.deltaTime, 0);
        }
        public static bool OnGround()
        {
            bool ground;

            ground = Physics.CheckSphere(m_position - new Vector3(0, 0.5f, 0), m_capsuleCollider.radius / m_groundRadiusCheck, m_layerMask, QueryTriggerInteraction.Collide);

            return ground;
        }
        public static Vector3 Jump(float jumpHeight, float jumpForce)
        {
            return new Vector3(0, Mathf.Sqrt(jumpHeight * jumpForce * UniversalGravity), 0);
        }
    }
}
