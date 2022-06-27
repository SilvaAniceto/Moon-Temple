using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IsometricOrientedPerspective
{
    public class IsometricCamera : IsometricOrientedPerspective
    {
        [SerializeField] Transform m_target;
        [SerializeField] Vector3 m_offset;
        [SerializeField] float m_sensibility;
        [SerializeField] Camera m_camera;
        [SerializeField] Vector3 m_currentOffset, m_leftOffset, m_rightOffset;
        [SerializeField] Vector3[] m_deltaPosition = { new Vector3(-30, 0, -30), new Vector3(30, 0, -30), new Vector3(30, 0, 30), new Vector3(-30, 0, 30),};
        [SerializeField] List<Vector3> m_auxDeltaPosition = new List<Vector3>();
        [SerializeField] float m_horizontalAxis;
        // Start is called before the first frame update
        void Start()
        {
            if (m_offset != m_currentOffset)
                m_currentOffset = m_offset;

            m_auxDeltaPosition.Add(m_deltaPosition[0]);
            m_auxDeltaPosition.Add(m_deltaPosition[1]);
            m_auxDeltaPosition.Add(m_deltaPosition[2]);
            m_auxDeltaPosition.Add(m_deltaPosition[3]);
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKey(KeyCode.Mouse1))
            {
                Vector3 direction = new Vector3(Input.GetAxis("Mouse X"), 0, Input.GetAxis("Mouse Y"));
                Vector3 righMovement = transform.right * m_sensibility * Time.deltaTime * direction.x;
                //Vector3 upMovement = transform.up * m_sensibility * Time.deltaTime * direction.z;                

                GetLeftRightOffSet(m_currentOffset, Input.GetAxis("Mouse X"));

                transform.position += righMovement;
                //transform.position += upMovement;
            }
            else
            {
                transform.position = m_target.position + m_offset;
            }

            if (Input.GetKeyUp(KeyCode.Mouse1))
            {
                SetCameraPosition();
                transform.position = m_target.position + m_offset;
            }
                

            transform.LookAt(m_target);
        }

        void GetLeftRightOffSet(Vector3 p_currentOffset, float p_horizontalAxis)
        {
            if (p_currentOffset != m_currentOffset || p_horizontalAxis == 0) return;

            if (p_horizontalAxis > 0.2f)
            {
                Debug.Log("Right");

                m_horizontalAxis = 1;

                int index = m_auxDeltaPosition.IndexOf(p_currentOffset);

                if (index + 1 > m_auxDeltaPosition.Count - 1)
                {
                    m_leftOffset = m_auxDeltaPosition[index - 1];
                    m_rightOffset = m_auxDeltaPosition[0];
                }
                else if (index - 1 < 0)
                {
                    m_leftOffset = m_auxDeltaPosition[m_auxDeltaPosition.Count - 1];
                    m_rightOffset = m_auxDeltaPosition[index + 1];
                }
                else
                {
                    m_leftOffset = m_auxDeltaPosition[index - 1];
                    m_rightOffset = m_auxDeltaPosition[index + 1];
                }

            }
            if (p_horizontalAxis < -0.2f)
            {
                Debug.Log("Left");

                m_horizontalAxis = -1;

                int index = m_auxDeltaPosition.IndexOf(p_currentOffset);

                if (index - 1 < 0)
                {
                    m_leftOffset = m_auxDeltaPosition[m_auxDeltaPosition.Count - 1];
                    m_rightOffset = m_auxDeltaPosition[index + 1];
                }
                else if(index + 1 > m_auxDeltaPosition.Count - 1)
                {
                    m_leftOffset = m_auxDeltaPosition[index - 1];
                    m_rightOffset = m_auxDeltaPosition[0];
                }
                else
                {
                    m_leftOffset = m_auxDeltaPosition[index - 1];
                    m_rightOffset = m_auxDeltaPosition[index + 1];
                }
            }
        }

        private void SetCameraPosition()
        {   
            if (m_horizontalAxis < 0)
                m_offset = m_leftOffset;

            if (m_horizontalAxis > 0)
                m_offset = m_rightOffset;

            m_currentOffset = m_offset;
        }
    }
}
