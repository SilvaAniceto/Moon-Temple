using UnityEngine;

namespace CharacterManager
{
    public class JumpSystem : MonoBehaviour
    {
        public static JumpSystem m_jumpInstance;

        [SerializeField] private bool m_offGroundLevel, m_onGroundLevel, m_onSlope, m_jumpInput;
        private float m_jumpDelayCounter, m_heightDelta, m_jumpDeltaTime;
        private Rigidbody m_rigidbody;
        private SphereCollider m_sphereCollider;
        private BoxCollider m_boxCollider;
        [SerializeField] private CapsuleCollider m_capsuleCollider;
        [SerializeField] private LayerMask m_layerMask;
        [SerializeField][Range(0, 1)] float radius;

        #region Properties
        public float JumpTime
        {
            get
            {
                return m_jumpDeltaTime;
            }
            set
            {
                if (m_jumpDeltaTime == value)
                    return;

                m_jumpDeltaTime = value;
            }
        }
        public float HeightDelta
        {
            get
            {
                return m_heightDelta;
            }

            set
            {
                if (m_heightDelta == value)
                    return;

                m_heightDelta = value;
            }
        }        
        public bool OnGroundLevel
        {
            get
            { 
                if (m_sphereCollider != null)
                    m_onGroundLevel = IsGround(m_sphereCollider, null, null);
                else if (m_boxCollider != null)
                    m_onGroundLevel = IsGround(null, m_boxCollider, null);
                else if (m_capsuleCollider != null)
                    m_onGroundLevel = IsGround(null, null, m_capsuleCollider);

                return m_onGroundLevel;
            }
            
            private set
            {
                if (m_onGroundLevel == value)
                    return;

                m_onGroundLevel = value;
            }
        }
        public LayerMask LayerMask
        {
            get
            {
                return m_layerMask;
            }

            set
            {
                if (m_layerMask == value)
                    return;

                m_layerMask = value;
            }
        }
        public bool OnSlope
        {
            get
            {
                return m_onSlope;
            }
            set
            {
                if (value == m_onSlope)
                    return;

                m_onSlope = value;

                if (m_onSlope && !m_offGroundLevel) m_rigidbody.MovePosition(m_rigidbody.position);
            }
        }
        public bool JumpInput
        {
            set
            {
                if (m_jumpInput == value)
                    return;

                m_jumpInput = value;
            }
        }
        #endregion

        public void Setup()
        {
            GetCollider();

            if (m_jumpInstance == null)
                m_jumpInstance = this;

            if (m_rigidbody == null)
                m_rigidbody = gameObject.GetComponent<Rigidbody>();
        }
        
        public void Jump()
        { 
            if (OnGroundLevel && m_jumpInput)
            {
                m_offGroundLevel = true;
                m_jumpDelayCounter = m_jumpDeltaTime;
                m_rigidbody.AddForce(Vector3.up * m_heightDelta, ForceMode.Force);
            }

            if (m_jumpInput && m_offGroundLevel)
            {
                if (m_jumpDelayCounter > 0)
                {
                    m_rigidbody.AddForce(Vector3.up * m_heightDelta, ForceMode.Force);
                    m_jumpDelayCounter -= Time.deltaTime;
                }
                else
                    m_offGroundLevel = false;
            }

            if (m_jumpInput)
                m_offGroundLevel = false;
        }

        private void GetCollider()
        {
            if(gameObject.GetComponent<SphereCollider>())
                if (m_sphereCollider == null)
                    m_sphereCollider = gameObject.GetComponent<SphereCollider>();
            if (gameObject.GetComponent<BoxCollider>())
                if (m_boxCollider == null)
                    m_boxCollider = gameObject.GetComponent<BoxCollider>();
            if (gameObject.GetComponent<CapsuleCollider>())
                if (m_capsuleCollider == null)
                    m_capsuleCollider = gameObject.GetComponent<CapsuleCollider>();
        }

        private bool IsGround(SphereCollider p_sphereCollider = null,BoxCollider p_boxCollider = null,CapsuleCollider p_capsuleCollider = null)
        {
            bool ground = false;
           
            if (p_capsuleCollider != null)
                //ground =  Physics.CheckCapsule(p_capsuleCollider.bounds.center,
                //    new Vector3(p_capsuleCollider.bounds.center.x, p_capsuleCollider.bounds.min.y, p_capsuleCollider.bounds.center.z),
                //    p_capsuleCollider.radius/* * 0.9f*/, m_layerMask, QueryTriggerInteraction.Collide);
                ground = Physics.CheckSphere(transform.position - new Vector3(0, 0.5f, 0), p_capsuleCollider.radius / radius, m_layerMask, QueryTriggerInteraction.Collide);

            if (p_boxCollider != null)
                ground = Physics.CheckBox(p_boxCollider.bounds.center, p_boxCollider.bounds.extents, Quaternion.identity , m_layerMask, QueryTriggerInteraction.Collide);

            if (p_sphereCollider != null)
                //ground = Physics.CheckCapsule(p_sphereCollider.bounds.center,
                //    new Vector3(p_sphereCollider.bounds.center.x, p_sphereCollider.bounds.min.y, p_sphereCollider.bounds.center.z),
                //    p_sphereCollider.radius/* * 0.9f*/, m_layerMask, QueryTriggerInteraction.Collide);
                ground = Physics.CheckSphere(p_capsuleCollider.bounds.center, p_capsuleCollider.radius, m_layerMask, QueryTriggerInteraction.Collide);

            return ground;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(transform.position - new Vector3(0, 0.5f, 0), m_capsuleCollider.radius / radius);
        }
    }
}
