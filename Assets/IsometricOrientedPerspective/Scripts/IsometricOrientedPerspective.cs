using UnityEngine;

namespace IsometricOrientedPerspective
{
    public class IsometricOrientedPerspective : MonoBehaviour
    {
        private static Vector3 m_isometricForward, m_isometricRight;
        private static MoveDirection m_moveDirection;
        private Ray m_raycastHit;
        

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
            public Vector3 slopeMovement = new Vector3();
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
