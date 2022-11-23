using UnityEngine;

namespace IOP
{ 
    public class IsometricRotation : IsometricOrientedPerspective
    {
        public static IsometricRotation m_rotationInstance;

        private Transform m_mouseCursor;
        private RaycastHit m_raycastHit;

        #region Properties
        /// <summary>
        /// Transform for the mouse cursor.
        /// </summary>
        public Transform MouseCursor
        {
            get
            {
                return m_mouseCursor;
            }
        }
        #endregion  

        public void Setup(string p_value)
        {
            bool p_active = p_value == "PointAndClick" ? true : false;

            if (m_rotationInstance == null)
                m_rotationInstance = this;

            if (m_mouseCursor == null)
            {
                m_mouseCursor = Resources.Load<Transform>("Prefabs/Cursor");

                m_mouseCursor = Instantiate(m_mouseCursor);
            } 

            m_mouseCursor.gameObject.SetActive(p_active);
        }

        /// <summary>
        /// Resolve the rotation in Isometric Oriented Perspective.
        /// </summary>
        public void Rotate(Ray p_raycast, Vector3 p_rotatePosition, LayerMask p_layerMask)
        {
            if (IsometricCamera.m_instance.MovingCamera) return; // Prevents that the movement happens when the Camera is moving.            

            if (!Physics.Raycast(p_raycast, out m_raycastHit, float.MaxValue, p_layerMask))
                return;
            else
            {
                p_rotatePosition = m_raycastHit.point;
                p_rotatePosition.y = transform.position.y;
                m_mouseCursor.position = new Vector3(p_rotatePosition.x, m_raycastHit.point.y, p_rotatePosition.z);
            }

            if (Vector3.Distance(transform.position, m_mouseCursor.position) > 3 && !IsometricMove.m_moveInstance.OnMove)
                transform.LookAt(p_rotatePosition, Vector3.up);

            #region DEPRECATED
            //if (p_isPhysics)
            //{
            //    Quaternion rotation = Quaternion.LookRotation(p_rotatePosition - transform.position);

            //    if (Vector3.Distance(transform.position, m_mouseCursor.position) > 3 && !IsometricMove.m_moveInstance.OnMove)
            //        p_rigidbody.rotation = rotation;
            //}
            //else
            //{
            //    if (Vector3.Distance(transform.position, m_mouseCursor.position) > 3 && !IsometricMove.m_moveInstance.OnMove)
            //        transform.LookAt(p_rotatePosition, Vector3.up);
            //}
            #endregion
        }
    }
}
