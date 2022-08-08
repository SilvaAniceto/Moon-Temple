using UnityEngine;

namespace CharacterManager
{
    public class JumpSystem : MonoBehaviour
    {
        public static JumpSystem m_jumpInstance;

        [Header("Time Between Jumps")]
        [SerializeField] float m_jumpDeltaTime;

        [Header("Max Jump Height")]
        [Range(50f, 100f)][SerializeField] float m_heightDelta;

        [Header("Ground LayerMask")]
        [SerializeField] LayerMask m_layerMask;

        private bool m_offGroundLevel, m_onGroundLevel, m_jumpInput;
        private float m_jumpDelayCounter;
        private Rigidbody m_rigidbody;
        private SphereCollider m_sphereCollider;
        private BoxCollider m_boxCollider;
        private CapsuleCollider m_capsuleCollider;

        #region Properties
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
        public bool JumpInput
        {
            get
            {
                return m_jumpInput;
            }

            private set
            {
                if (m_jumpInput == value)
                    return;

                m_jumpInput = value;
            }
        }
        #endregion

        private void Awake()
        {
            GetCollider();

            if (m_jumpInstance == null)
                m_jumpInstance = this;

            if (m_rigidbody == null)
                m_rigidbody = gameObject.GetComponent<Rigidbody>();
        }

        private void Update()
        {
            if (Input.GetButton("Jump"))
                m_jumpInput = true;
            else if (Input.GetButtonUp("Jump"))
                m_jumpInput = false;
        }

        private void FixedUpdate()
        { 
            if (OnGroundLevel && JumpInput)
            {
                m_offGroundLevel = true;
                m_jumpDelayCounter = m_jumpDeltaTime;
                m_rigidbody.AddForce(Vector3.up * m_heightDelta, ForceMode.Force);
            }

            if (JumpInput && m_offGroundLevel)
            {
                if (m_jumpDelayCounter > 0)
                {
                    m_rigidbody.AddForce(Vector3.up * m_heightDelta, ForceMode.Force);
                    m_jumpDelayCounter -= Time.deltaTime;
                }
                else
                    m_offGroundLevel = false;
            }

            if (JumpInput)
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
                ground =  Physics.CheckCapsule(p_capsuleCollider.bounds.center,
                    new Vector3(p_capsuleCollider.bounds.center.x, p_capsuleCollider.bounds.min.y, p_capsuleCollider.bounds.center.z),
                    p_capsuleCollider.radius * 0.9f, m_layerMask, QueryTriggerInteraction.Collide);

            if (p_boxCollider != null)
                ground = Physics.CheckBox(p_boxCollider.bounds.center, p_boxCollider.bounds.extents, Quaternion.identity , m_layerMask, QueryTriggerInteraction.Collide);

            if (p_sphereCollider != null)
                ground = Physics.CheckCapsule(p_sphereCollider.bounds.center,
                    new Vector3(p_sphereCollider.bounds.center.x, p_sphereCollider.bounds.min.y, p_sphereCollider.bounds.center.z),
                    p_sphereCollider.radius * 0.9f, m_layerMask, QueryTriggerInteraction.Collide);

            return ground;
        }
    }
}
