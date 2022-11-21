using UnityEngine;

namespace IsometricOrientedPerspective
{
    public class IsometricRotation : IsometricOrientedPerspective
    {
        public static IsometricRotation m_rotationInstance;

        private bool m_enableRotation;
        private float m_rotationSensibility;
        private LayerMask m_layerMask; 
        private Transform m_mouseCursor;
        private Vector3 m_rotatePosition;

        #region Properties
        /// <summary>
        /// Define the rotation sensibility.
        /// </summary>
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
        /// <summary>
        /// Look at position when mouse rotation is enabled.
        /// </summary>
        public Vector3 RotatePosition
        {
            get
            {
                return m_rotatePosition;
            }

            set
            {
                if (m_rotatePosition == value)
                    return;

                m_rotatePosition = value;
            }
        }
        /// <summary>
        /// Target layer for the mouse cursor.
        /// </summary>
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
        /// <summary>
        /// Transform for the mouse cursor.
        /// </summary>
        public Transform MouseCursor
        {
            get
            {
                return m_mouseCursor;
            }
        }
        public bool EnableRotation
        {
            set
            {
                if (m_enableRotation == value)
                    return;

                m_enableRotation = value;

                Setup(m_enableRotation);
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

        public void Setup(bool p_cursorState)
        {
            if (m_rotationInstance == null)
                m_rotationInstance = this;

            if (m_mouseCursor == null)
            {
                m_mouseCursor = Resources.Load<Transform>("Prefabs/Cursor");

                m_mouseCursor = Instantiate(m_mouseCursor);
            } 

            m_mouseCursor.gameObject.SetActive(p_cursorState);
            this.enabled = p_cursorState;
        }

        /// <summary>
        /// Resolve the rotation in Isometric Oriented Perspective.
        /// </summary>
        public void Rotate(Ray p_raycast, Vector3 p_rotatePosition, LayerMask p_layerMask, bool p_isPhysics, Rigidbody p_rigidbody)
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

                if (p_isPhysics)
                {
                    Quaternion rotation = Quaternion.LookRotation(p_rotatePosition - transform.position);

                    if (Vector3.Distance(transform.position, m_mouseCursor.position) > 3 && !IsometricMove.m_moveInstance.OnMove)
                        p_rigidbody.rotation = rotation;
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
