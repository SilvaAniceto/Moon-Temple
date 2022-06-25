using System.Collections;
using System.IO;
using UnityEngine;

namespace IsometricOrientedPerspective
{
    public class IsometricRotation : IsometricOrientedPerspective
    {
        public static IsometricRotation m_rotationInstance;

        private Vector3 m_rotatePosition;
        [SerializeField] private bool m_mouseCursorRotation, m_isPhysicsRotation;
        [Range(0f, 100f)][SerializeField] private float m_mouseSensibility;
        [SerializeField] private Transform m_mouseCursor;
        private float m_horizontalRotation, m_verticalRotation;
        private Rigidbody m_Rigidbody;
        [SerializeField] private LayerMask m_layerMask;

        #region Properties
        public Vector3 RotatePosition
        {
            get
            {
                return m_rotatePosition;
            }

            private set
            {
                if (m_rotatePosition == value)
                    return;

                m_rotatePosition = value;
            }
        }
        public bool MouseCursorRotation
        {
            get 
            {
                return m_mouseCursorRotation;
            }

            private set
            {
                if (m_mouseCursorRotation == value)
                    return;

                m_mouseCursorRotation = value;
            }
        }
        public bool IsPhysicsRotation
        {
            get
            {
                return m_isPhysicsRotation;
            }

            private set
            {
                if (m_isPhysicsRotation == value)
                    return;

                m_isPhysicsRotation = value;
            }
        }
        public float MouseSensibility
        {
            get
            {
                return m_mouseSensibility;
            }

            private set
            {
                if (m_mouseSensibility == value)
                    return;

                m_mouseSensibility = value;
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

        new void Awake()
        {
            base.Awake();

            if (m_rotationInstance == null)
                m_rotationInstance = this;

            m_Rigidbody = GetComponent<Rigidbody>();

            m_mouseCursor = Instantiate(m_mouseCursor);
        }

        private void Update()
        {
            if (m_mouseCursorRotation)
            {
                m_rotatePosition = Input.mousePosition;

                Rotate(m_rotatePosition, m_layerMask);
            }
            else 
            {
                m_horizontalRotation = Input.GetAxis("HorizontalRotation");
                m_verticalRotation = Input.GetAxis("VerticalRotation");

                Rotate(m_horizontalRotation, m_verticalRotation);
            }

            m_mouseCursor.gameObject.SetActive(m_mouseCursorRotation);
        }
        protected virtual void Rotate(float p_xAxis, float p_zAxis)
        {
            if (m_isPhysicsRotation)
            {
                Vector3 direction = new Vector3(p_xAxis * m_mouseSensibility * Time.fixedDeltaTime, 0, p_zAxis * m_mouseSensibility * Time.fixedDeltaTime);

                direction = Camera.main.transform.TransformDirection(direction);
                direction.y = 0;
                Debug.Log(direction);
                if (direction != Vector3.zero)
                    m_Rigidbody.rotation = Quaternion.LookRotation(direction);
            }
            else
            {
                Vector3 direction = new Vector3(p_xAxis, 0, p_zAxis);
                Vector3 righMovement = IsometricRight * m_mouseSensibility * Time.deltaTime * direction.x;
                Vector3 upMovement = IsometricForward * m_mouseSensibility * Time.deltaTime * direction.z;

                Vector3 heading = Vector3.Normalize(righMovement + upMovement);
                Debug.Log(heading);
                if (heading != Vector3.zero)
                    transform.forward =  heading; 
            }
        }
        protected virtual void Rotate(Vector3 p_rotatePosition, LayerMask p_layerMask)
        {
            Ray ray = Camera.main.ScreenPointToRay(p_rotatePosition);

            if (!Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, p_layerMask))
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

                if (Vector3.Distance(transform.position, m_mouseCursor.position) > 3)
                    m_Rigidbody.rotation = rotation;
            }
            else
            {
                if (Vector3.Distance(transform.position, m_mouseCursor.position) > 3)
                    transform.LookAt(p_rotatePosition, Vector3.up);
            }
        }
    }
}
