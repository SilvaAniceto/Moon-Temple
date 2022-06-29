using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IsometricOrientedPerspective
{
    public class IsometricMove : IsometricOrientedPerspective
    {
        public static IsometricMove m_moveInstance;

        [SerializeField] private bool m_isPhysicsMovement;
        [SerializeField] private float m_movementDelta = 4f;
        private float m_horizontalMovement , m_verticalMovement;
        private Rigidbody m_Rigidbody;
        private Vector2 m_moveDelta;
        private MoveDirection m_moveDirection;

        #region Properties
        public float MovementDelta
        {
            get
            {
                return m_movementDelta;
            }

            private set
            {
                if (m_movementDelta == value)
                    return;

                m_movementDelta = value;
            }
        }
        public bool IsPhysicsMovement
        {
            get 
            { 
                return m_isPhysicsMovement;
            }

            private set
            {
                if (m_isPhysicsMovement == value)
                    return;

                m_isPhysicsMovement = value;
            }
        }
        public float HorizontalMovement
        {
            get 
            {
                return m_horizontalMovement; 
            }

            private set 
            {
                if (m_horizontalMovement == value)
                    return;

                m_horizontalMovement = value;
            }
        }
        public float VerticalMovement
        {
            get
            {
                return m_verticalMovement;
            }

            private set
            {
                if (m_verticalMovement == value)
                    return; 

                m_verticalMovement = value;
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
        public Vector2 MoveDelta
        {
            get
            {
                return m_moveDelta;
            }
            private set
            {
                if (m_moveDelta == value)
                    return;

                m_moveDelta = value;
            }
        }
        #endregion

        #region Classes
        private class MoveDirection
        {
           public Vector3 direction = new Vector3();
           public Vector3 righMovement = new Vector3();
           public Vector3 upMovement = new Vector3();
           public Vector3 heading = new Vector3();
        }
        #endregion

        new void Awake()
        {
            base.Awake();

            if (m_moveInstance == null)
                m_moveInstance = this;

            if (m_moveDirection == null)
                m_moveDirection = new MoveDirection();

            m_Rigidbody = GetComponent<Rigidbody>();
        }
        
        private void Update()
        {
            m_horizontalMovement = Input.GetAxis("Horizontal");
            m_verticalMovement = Input.GetAxis("Vertical");

            SetInputMoveDelta();

            if (IsPhysicsMovement) return;
            
            if (IsometricRotation.m_rotationInstance.enabled)
            {
                m_moveDelta = new Vector2(m_horizontalMovement, m_verticalMovement);
                if (m_moveDelta != Vector2.zero)
                    Move(m_moveDelta.x, m_moveDelta.y);
            }
            else
            {
                if (m_moveDelta != Vector2.zero)
                    Move(m_moveDelta.x, m_moveDelta.y);
            }
        }

        private void FixedUpdate()
        {
            if (!IsPhysicsMovement) return;

            m_moveDelta = new Vector2(m_horizontalMovement, m_verticalMovement);
            if (m_moveDelta != Vector2.zero)
                Move(m_moveDelta.x, m_moveDelta.y);
        }

        protected virtual void Move(float p_xAxis, float p_zAxis)
        {
            if (IsPhysicsMovement)
            {
                if (IsometricRotation.m_rotationInstance.enabled)
                {
                    float distance = Vector3.Distance(IsometricRotation.m_rotationInstance.MouseCursor.transform.position, transform.position);
                    
                    Vector3 zdirection = distance > 1.2f ? transform.forward * p_zAxis * m_movementDelta * Time.fixedDeltaTime : Vector3.zero;
                    Vector3 xdirection = transform.right * p_xAxis *m_movementDelta * Time.fixedDeltaTime;
                    m_moveDirection.direction = zdirection + xdirection;

                    m_Rigidbody.MovePosition(m_Rigidbody.position + m_moveDirection.direction);
                }
                else
                {
                    m_moveDirection.direction = new Vector3(p_xAxis * m_movementDelta * Time.fixedDeltaTime, 0, p_zAxis * m_movementDelta * Time.fixedDeltaTime);

                    m_moveDirection.direction = Camera.main.transform.TransformDirection(m_moveDirection.direction);
                    m_moveDirection.direction.y = 0;

                    m_Rigidbody.MovePosition(m_Rigidbody.position + m_moveDirection.direction);
                }

                if (!IsometricRotation.m_rotationInstance.enabled)
                    if (m_moveDirection.direction != Vector3.zero)
                        m_Rigidbody.rotation = Quaternion.Slerp(m_Rigidbody.rotation, Quaternion.LookRotation(m_moveDirection.direction), 0.5f);
            }
            else
            {
                if (IsometricRotation.m_rotationInstance.enabled)
                {
                    m_moveDirection.direction = new Vector3(p_xAxis, 0, p_zAxis);
                    m_moveDirection.righMovement = transform.right * m_movementDelta * Time.deltaTime * m_moveDirection.direction.x;
                    m_moveDirection.upMovement = transform.forward * m_movementDelta * Time.deltaTime * m_moveDirection.direction.z;
                }
                else
                {
                    m_moveDirection.direction = new Vector3(p_xAxis, 0, p_zAxis);
                    m_moveDirection.righMovement = IsometricRight * m_movementDelta * Time.deltaTime * m_moveDirection.direction.x;
                    m_moveDirection.upMovement = IsometricForward * m_movementDelta * Time.deltaTime * m_moveDirection.direction.z;
                }


                if (!IsometricRotation.m_rotationInstance.enabled)
                {
                    m_moveDirection.heading = Vector3.Normalize(m_moveDirection.righMovement + m_moveDirection.upMovement);
                    if (m_moveDirection.heading != Vector3.zero)
                        transform.forward = Vector3.Lerp(transform.forward, m_moveDirection.heading, 0.40f);
                }

                transform.position += m_moveDirection.righMovement;
                transform.position += m_moveDirection.upMovement;
            }
        }

        public void SetInputMoveDelta()
        {
            if (IsometricCamera.m_instance.m_cameraPosition == IsometricCamera.CameraPosition.SOUTH)
                m_moveDelta = new Vector2(m_horizontalMovement, m_verticalMovement);

            if (IsometricCamera.m_instance.m_cameraPosition == IsometricCamera.CameraPosition.WEST)
                m_moveDelta = new Vector2(m_verticalMovement, -m_horizontalMovement);

            if (IsometricCamera.m_instance.m_cameraPosition == IsometricCamera.CameraPosition.NORTH)
                m_moveDelta = new Vector2(-m_horizontalMovement, -m_verticalMovement);

            if (IsometricCamera.m_instance.m_cameraPosition == IsometricCamera.CameraPosition.EAST)
                m_moveDelta = new Vector2(-m_verticalMovement, m_horizontalMovement);
        }
    }
}
