using System.Collections;
using System.IO;
using UnityEngine;

namespace IsometricOrientedPerspective
{
    public class IsometricRotation : IsometricOrientedPerspective
    {
        public static IsometricRotation m_rotationInstance;

        [SerializeField] private bool m_isPhysicsRotation;
        [Range(0f, 100f)][SerializeField] private float m_rotationSensibility;
        [SerializeField] private Transform m_mouseCursor;
        [SerializeField] private LayerMask m_layerMask;
        private float m_horizontalRotation, m_verticalRotation;
        private Rigidbody m_Rigidbody;

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
        public Transform MouseCursor
        {
            get
            {
                return m_mouseCursor;
            }
            private set
            {
                if (m_mouseCursor == value)
                    return;

                m_mouseCursor = value;
            }
        }
        public float HorizontalRotation
        {
            get
            {
                return m_horizontalRotation;
            }

            private set
            {
                if (m_horizontalRotation == value)
                    return;

                m_horizontalRotation = value;
            }
        }
        public float VerticalRotation
        {
            get
            {
                return m_verticalRotation;
            }

            private set
            {
                if (m_verticalRotation == value)
                    return;

                m_verticalRotation = value;
            }
        }
        public Rigidbody Rigidbody
        {
            get
            {
                return m_Rigidbody;
            }

            private set
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
        #endregion  

        private void OnEnable()
        {
            m_mouseCursor.gameObject.SetActive(true);
        }

        private void OnDisable()
        {
            if (m_mouseCursor != null)
                m_mouseCursor.gameObject.SetActive(false);
        }

        new void Awake()
        {
            base.Awake();

            if (m_rotationInstance == null)
                m_rotationInstance = this;

            m_Rigidbody = GetComponent<Rigidbody>();

            m_mouseCursor = Instantiate(m_mouseCursor);
        }

        new void Update()
        {
            base.Update();

            Rotate(RotatePosition, m_layerMask);
        }
        protected virtual void Rotate(Vector3 p_rotatePosition, LayerMask p_layerMask)
        {
            if (!IsometricCamera.m_instance.MovingCamera)
            {
                if (!Physics.Raycast(RaycastHit, out RaycastHit raycastHit, float.MaxValue, p_layerMask))
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
