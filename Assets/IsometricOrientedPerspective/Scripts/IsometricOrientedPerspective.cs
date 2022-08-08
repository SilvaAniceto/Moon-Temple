using UnityEngine;

namespace IsometricOrientedPerspective
{
    public class IsometricOrientedPerspective : MonoBehaviour
    {
        private static Vector3 m_isometricForward, m_isometricRight;
        private static MoveDirection m_moveDirection;
        private float m_horizontalMovement, m_verticalMovement;
        private Vector3 m_rotatePosition;
        private Ray m_raycastHit;
        private bool m_leftClick;

        #region Properties
        /// <summary>
        /// New forward orientation for Isometric Perspective.
        /// </summary>
        public Vector3 IsometricForward
        {
            get
            {
                return m_isometricForward;
            }
        }
        /// <summary>
        /// New right orientation for Isometric Perspective.
        /// </summary>
        public Vector3 IsometricRight
        {
            get
            {
                return m_isometricRight;
            }  
        }
        /// <summary>
        /// Horizontal axi for movement in Isometric Perspective.
        /// </summary>
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
        /// <summary>
        /// Vertical axi for movement in Isometric Perspective.
        /// </summary>
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
        /// Physics ray to define mouse cursor position.
        /// </summary>
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
        /// <summary>
        /// Input for left mouse button click.
        /// </summary>
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
        /// <summary>
        /// Defines which direction the game object is facing, moving or rotating.
        /// </summary>
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

        /// <summary>
        /// Sets the isometric directions with the Camera informations.
        /// </summary>
        public void IsometricSetup()
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
