using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IsometricMovement
{
    public class IsometricMove : MonoBehaviour
    {
        [SerializeField] private bool m_isPhysicsMovement;
        [SerializeField] float m_movementDelta = 4f;
        private Vector3 m_isometricForward, m_isometricRight;
        private float m_horizontalMovement , m_verticalMovement;
        private Rigidbody m_Rigidbody;

        #region Properties
        public bool isPhysicsMovement
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
        public Vector3 IsometricForward
        {
            get
            {
                return m_isometricForward;
            }

            private set
            {
                if (m_isometricForward == value)
                    return;

                m_isometricForward = value;
            }
        }
        public Vector3 IsometricRight
        {
            get
            {
                return m_isometricRight;
            }

            private set
            {
                if (m_isometricRight == value)
                    return;

                m_isometricRight = value;
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
        #endregion
        
        private void Awake()
        {
            m_isometricForward = Camera.main.transform.forward;
            m_isometricForward.y = 0;
            m_isometricForward = Vector3.Normalize(IsometricForward);

            m_isometricRight = Camera.main.transform.right;

            m_Rigidbody = GetComponent<Rigidbody>();
        }
        
        private void FixedUpdate()
        {
            HorizontalMovement = Input.GetAxis("HorizontalKey");
            VerticalMovement = Input.GetAxis("VerticalKey");
            
            Move(HorizontalMovement, VerticalMovement);
        }
        protected virtual void Move(float p_xAxis, float p_zAxis)
        {
            if (isPhysicsMovement)
            {
                Vector3 direction = new Vector3(p_xAxis * m_movementDelta * Time.fixedDeltaTime , 0, p_zAxis * m_movementDelta * Time.fixedDeltaTime);

                direction = Camera.main.transform.TransformDirection(direction);
                direction.y = 0;

                Rigidbody.MovePosition(Rigidbody.position + direction);
                if (direction != Vector3.zero)
                    Rigidbody.rotation = Quaternion.LookRotation(direction);
            }
            else
            {
                Vector3 direction = new Vector3(p_xAxis, 0, p_zAxis);
                Vector3 righMovement = IsometricRight * m_movementDelta * Time.deltaTime * direction.x;
                Vector3 upMovement = IsometricForward * m_movementDelta * Time.deltaTime * direction.z;

                Vector3 heading = Vector3.Normalize(righMovement + upMovement);
                if (heading != Vector3.zero)
                    transform.forward = heading;

                transform.position += righMovement;
                transform.position += upMovement;
            }
        }
    }
}
