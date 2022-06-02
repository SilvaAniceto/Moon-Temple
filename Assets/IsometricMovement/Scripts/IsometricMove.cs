using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IsometricMovement
{
    public class IsometricMove : MonoBehaviour
    {
        [SerializeField] bool m_isPhysicsMovement;
        [SerializeField] float speed = 4f;
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
        // Start is called before the first frame update
        private void Awake()
        {
            m_isometricForward = Camera.main.transform.forward;
            m_isometricForward.y = 0;
            m_isometricForward = Vector3.Normalize(m_isometricForward);

            m_isometricRight = Camera.main.transform.right;

            m_Rigidbody = GetComponent<Rigidbody>();
        }


        // Update is called once per frame
        private void Update()
        {
            HorizontalMovement = Input.GetAxis("HorizontalKey");
            VerticalMovement = Input.GetAxis("VerticalKey");
            
            Move(HorizontalMovement, VerticalMovement);
        }
        
        void Move(float p_xAxis, float p_zAxis)
        {
            if (isPhysicsMovement)
            {
                Vector3 direction = new Vector3(p_xAxis * speed * Time.deltaTime , 0, p_zAxis * speed * Time.deltaTime);

                direction = Camera.main.transform.TransformDirection(direction);
                direction.y = 0;

                Rigidbody.MovePosition(Rigidbody.position + direction);
            }
            else
            {
                Vector3 direction = new Vector3(p_xAxis, 0, p_zAxis);
                Vector3 righMovement = IsometricRight * speed * Time.deltaTime * direction.x;
                Vector3 upMovement = IsometricForward * speed * Time.deltaTime * direction.z;

                //Vector3 heading = Vector3.Normalize(righMovement + upMovement);
                //transform.forward = heading;

                transform.position += righMovement;
                transform.position += upMovement;
            }
        }
    }
}
