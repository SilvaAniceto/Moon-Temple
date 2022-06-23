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

        #region Properties
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
        public Vector2 MovementDelta
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
        
        new void Awake()
        {
            base.Awake();

            if (m_moveInstance == null)
                m_moveInstance = this;

            m_Rigidbody = GetComponent<Rigidbody>();
        }
        
        private void FixedUpdate()
        {
            m_horizontalMovement = Input.GetAxis("Horizontal");
            m_verticalMovement = Input.GetAxis("Vertical");

            m_moveDelta = new Vector2(m_horizontalMovement, m_verticalMovement);
            
            if (m_moveDelta != Vector2.zero)
                Move(m_horizontalMovement, m_verticalMovement);
        }
        protected virtual void Move(float p_xAxis, float p_zAxis)
        {
            if (IsPhysicsMovement)
            {
                Vector3 direction = new Vector3(p_xAxis * m_movementDelta * Time.fixedDeltaTime , 0, p_zAxis * m_movementDelta * Time.fixedDeltaTime);

                direction = Camera.main.transform.TransformDirection(direction);
                direction.y = 0;

                m_Rigidbody.MovePosition(m_Rigidbody.position + direction);
                if (!IsometricRotation.m_rotationInstance.enabled)
                    if (direction != Vector3.zero)
                        m_Rigidbody.rotation = Quaternion.Slerp(m_Rigidbody.rotation, Quaternion.LookRotation(direction), 0.5f);
            }
            else
            {
                Vector3 direction = new Vector3(p_xAxis, 0, p_zAxis);
                Vector3 righMovement = IsometricRight * m_movementDelta * Time.deltaTime * direction.x;
                Vector3 upMovement = IsometricForward * m_movementDelta * Time.deltaTime * direction.z;

                if (!IsometricRotation.m_rotationInstance.enabled)
                {
                    Vector3 heading = Vector3.Normalize(righMovement + upMovement);
                    if (heading != Vector3.zero)
                        transform.forward = Vector3.Lerp(transform.forward, heading, 0.40f);
                }

                transform.position += righMovement;
                transform.position += upMovement;
            }
        }
    }
}
