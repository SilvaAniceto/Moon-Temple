using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IsometricOrientedPerspective
{
    public class JumpSystem : MonoBehaviour
    {
        [SerializeField] float m_heightDeltaTime, m_heightDelta;
        private bool m_offGroundLevel, m_onGroundLevel, m_jumpInput;
        [SerializeField] LayerMask m_layerMask;
        private Rigidbody m_rigidbody;
        private float m_jumpDelayCounter;
        private SphereCollider m_sphereCollider;
        private BoxCollider m_boxCollider;
        private CapsuleCollider m_capsuleCollider;

        #region Properties
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

            if (m_rigidbody == null)
                m_rigidbody = gameObject.GetComponent<Rigidbody>();
        }

        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
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
                m_jumpDelayCounter = m_heightDeltaTime;
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
