using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IsometricOrientedPerspective
{
    public class IsometricPerspective : MonoBehaviour
    {
        private static Vector3 m_isometricForward, m_isometricRight;
        private static MoveDirection m_moveDirection;
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

            set
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
            set
            {
                if (m_verticalMovement == value)
                    return;

                m_verticalMovement = value;
            }
        }
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
        public Ray RaycastHit
        {
            get
            {
                return m_raycastHit;
            }

            set
            {
                m_raycastHit = value;
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
        public MoveDirection Direction
        {
            get
            {
                return m_moveDirection;
            }
            set
            {
                m_moveDirection = value;
            }
        }
        #endregion

        #region Classes
        public class MoveDirection
        {
            public Vector3 direction = new Vector3();
            public Vector3 righMovement = new Vector3();
            public Vector3 upMovement = new Vector3();
            public Vector3 heading = new Vector3();
        }
        #endregion

        public static void Setup()
        {
            m_isometricForward = Camera.main.transform.forward;
            m_isometricForward.y = 0;
            m_isometricForward = Vector3.Normalize(m_isometricForward);

            m_isometricRight = Camera.main.transform.right;

            if (m_moveDirection == null)
                m_moveDirection = new MoveDirection();
        }
    }
}
