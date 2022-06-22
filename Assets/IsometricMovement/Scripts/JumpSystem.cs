using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IsometricOrientedPerspective
{
    public class JumpSystem : MonoBehaviour
    {
        [SerializeField] float jumpTime, jumpForce;
        [SerializeField] bool isJumping, m_groundValue;
        [SerializeField] Rigidbody rb;
        [SerializeField] LayerMask m_layerMask;
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
                    m_groundValue = IsGround(m_sphereCollider, null, null);
                else if (m_boxCollider != null)
                    m_groundValue = IsGround(null, m_boxCollider, null);
                else if (m_capsuleCollider != null)
                    m_groundValue = IsGround(null, null, m_capsuleCollider);

                return m_groundValue;
            }
            
            private set
            {
                if (m_groundValue == value)
                    return;

                m_groundValue = value;
            }
        }

        #endregion

        private void Awake()
        {
            GetCollider();
        }

        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {            
            //onGround = Physics.Raycast(rb.transform.position, Vector3.down, 0.6f, layerMask);

            if (OnGroundLevel && Input.GetButtonDown("MouseLeftButton"))
            {
                isJumping = true;
                m_jumpDelayCounter = jumpTime;
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            }

            if (Input.GetButton("MouseLeftButton") && isJumping)
            {

                if (m_jumpDelayCounter > 0)
                {
                    rb.AddForce(Vector3.up * jumpForce, ForceMode.Force);
                    m_jumpDelayCounter -= Time.deltaTime;
                }
                else
                    isJumping = false;
            }

            if (Input.GetButtonUp("MouseLeftButton"))
            {
                isJumping = false;
            }
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
