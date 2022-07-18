using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IsometricOrientedPerspective
{
    public class IsometricOrientedPerspective : MonoBehaviour
    {
        private Vector3 m_isometricForward, m_isometricRight;
        private float m_horizontalMovement, m_verticalMovement;
        private Vector3 m_rotatePosition;
        private Ray m_raycastHit;
        private bool m_leftClick;

        #region Properties
        public Vector3 IsometricForward
        {
            get
            {
                return m_isometricForward;
            }
        }
        public Vector3 IsometricRight
        {
            get
            {
                return m_isometricRight;
            }  
        }
        public float HorizontalMovement
        {
            get
            {
                return m_horizontalMovement;
            }
        }
        public float VerticalMovement
        {
            get
            {
                return m_verticalMovement;
            } 
        }
        public Vector3 RotatePosition
        {
            get
            {
                return m_rotatePosition;
            }
        }
        public Ray RaycastHit
        {
            get
            {
                return m_raycastHit;
            }
        }
        public bool LeftClick
        {
            get
            {
                return m_leftClick;
            }

            set
            {
                if (m_leftClick == value)
                    return;

                m_leftClick = value;
            }
        }
        #endregion

        protected void Awake()
        {
            m_isometricForward = Camera.main.transform.forward;
            m_isometricForward.y = 0;
            m_isometricForward = Vector3.Normalize(m_isometricForward);

            m_isometricRight = Camera.main.transform.right;
        }

        protected void Update()
        {
            m_horizontalMovement = Input.GetAxis("Horizontal");
            m_verticalMovement = Input.GetAxis("Vertical");

            m_rotatePosition = Input.mousePosition;

            m_raycastHit = Camera.main.ScreenPointToRay(Input.mousePosition);

            m_leftClick = Input.GetMouseButtonDown(0);
        }
    }
}
