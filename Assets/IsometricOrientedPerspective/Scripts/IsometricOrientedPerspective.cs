using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IsometricOrientedPerspective
{
    public class IsometricOrientedPerspective : MonoBehaviour
    {
        private Vector3 m_isometricForward, m_isometricRight;

        #region Properties
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
        #endregion

        protected void Awake()
        {
            m_isometricForward = Camera.main.transform.forward;
            m_isometricForward.y = 0;
            m_isometricForward = Vector3.Normalize(IsometricForward);

            m_isometricRight = Camera.main.transform.right;
        }
    }
}
