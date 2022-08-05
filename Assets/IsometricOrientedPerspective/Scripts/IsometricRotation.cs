using System.Collections;
using System.IO;
using UnityEngine;

namespace IsometricOrientedPerspective
{
    public class IsometricRotation : IsometricOrientedPerspective
    {
        public static IsometricRotation m_rotationInstance;

        private bool m_isPhysicsRotation;
        private float m_rotationSensibility;
        private LayerMask m_layerMask;        
        private Rigidbody m_Rigidbody;
        [SerializeField]private Transform m_mouseCursor;

        #region Properties
        public bool IsPhysicsRotation
        {
            get
            {
                return m_isPhysicsRotation;
            }

            set
            {
                if (m_isPhysicsRotation == value)
                    return;

                m_isPhysicsRotation = value;
            }
        }
        public float RotationSensibility
        {
            get
            {
                return m_rotationSensibility;
            }

            set
            {
                if (m_rotationSensibility == value)
                    return;

                m_rotationSensibility = value;
            }
        }      
        public Rigidbody Rigidbody
        {
            get
            {
                return m_Rigidbody;
            }

            set
            {
                if (m_Rigidbody != null)
                    return;

                m_Rigidbody = value;
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
        public Transform MouseCursor
        {
            get
            {
                return m_mouseCursor;
            }
        }
        #endregion  

        private void OnEnable()
        {
            if (m_mouseCursor != null)
                m_mouseCursor.gameObject.SetActive(true);
        }

        private void OnDisable()
        {
            if (m_mouseCursor != null)
                m_mouseCursor.gameObject.SetActive(false);
        }

        public void Setup()
        {
            m_mouseCursor = Resources.Load<Transform>("Cursor");

            m_mouseCursor = Instantiate(m_mouseCursor);
        }

        public void Rotate(Ray p_raycast, Vector3 p_rotatePosition, LayerMask p_layerMask)
        {
            if (!IsometricCamera.m_instance.MovingCamera)
            {
                if (!Physics.Raycast(p_raycast, out RaycastHit raycastHit, float.MaxValue, p_layerMask))
                    return;
                else
                {
                    p_rotatePosition = raycastHit.point;
                    p_rotatePosition.y = transform.position.y;
                    m_mouseCursor.position = new Vector3(p_rotatePosition.x, raycastHit.point.y, p_rotatePosition.z);
                }

                if (m_isPhysicsRotation)
                {
                    Quaternion rotation = Quaternion.LookRotation(p_rotatePosition - transform.position);

                    if (Vector3.Distance(transform.position, m_mouseCursor.position) > 3 && !IsometricMove.m_moveInstance.OnMove)
                        m_Rigidbody.rotation = rotation;
                }
                else
                {
                    if (Vector3.Distance(transform.position, m_mouseCursor.position) > 3 && !IsometricMove.m_moveInstance.OnMove)
                        transform.LookAt(p_rotatePosition, Vector3.up);
                }
            }
        }
    }
}
