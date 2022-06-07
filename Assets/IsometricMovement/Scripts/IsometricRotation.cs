using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IsometricMovement
{
    public class IsometricRotation : MonoBehaviour
    {
        [SerializeField] Transform m_mouseCursor;

        [SerializeField] Vector3 m_rotatePosition;
        
        Rigidbody m_Rigidbody;
        void Awake()
        {
            m_Rigidbody = GetComponent<Rigidbody>();
        }
        
        private void Update()
        {
            
            m_rotatePosition = Input.mousePosition;
            Ray ray = Camera.main.ScreenPointToRay(m_rotatePosition);

            if (Physics.Raycast(ray, out RaycastHit raycastHit))
            {
                m_rotatePosition = raycastHit.point;
                m_rotatePosition.y = 0f;
                m_mouseCursor.position = m_rotatePosition;

                //transform.LookAt(m_rotatePosition);
                Quaternion rotation = Quaternion.LookRotation(m_rotatePosition - transform.position);

                m_Rigidbody.rotation = rotation;
            }
            
        }
        protected virtual void Rotate(float p_xAxis, float p_zAxis)
        {

        }
    }
}
