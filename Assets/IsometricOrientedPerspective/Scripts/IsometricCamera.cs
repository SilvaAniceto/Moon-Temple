using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IsometricOrientedPerspective
{
    public class IsometricCamera : IsometricOrientedPerspective
    {
        public static IsometricCamera m_instance;

        [SerializeField] Transform m_target;
        [SerializeField] Vector3 m_offset;
        [SerializeField] float m_sensibility;
        [SerializeField] Camera m_camera;
        [SerializeField] Vector3 m_currentOffset, m_leftOffset, m_rightOffset, m_currentPosition, m_nextPosition;
        [SerializeField] Vector3[] m_deltaPosition = { new Vector3(-30, 15, -30), new Vector3(30, 15, -30), new Vector3(30, 15, 30), new Vector3(-30, 15, 30)};
        [SerializeField] List<Vector3> m_auxDeltaPosition = new List<Vector3>();
        [SerializeField] float m_horizontalAxis;

        public enum CameraPosition { SOUTH, EAST, NORTH, WEST }
        public CameraPosition m_cameraPosition = CameraPosition.SOUTH;

        // Start is called before the first frame update
        void Start()
        {
            if (m_instance == null)
                m_instance = this;

            if (m_offset != m_currentOffset)
                m_currentOffset = m_offset;

            m_auxDeltaPosition.Add(m_deltaPosition[0]);
            m_auxDeltaPosition.Add(m_deltaPosition[1]);
            m_auxDeltaPosition.Add(m_deltaPosition[2]);
            m_auxDeltaPosition.Add(m_deltaPosition[3]);

            DefineCameraDirection();
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

            if (Input.GetKeyUp(KeyCode.Mouse1))
                SetCameraPosition();

            var targetRotation = Quaternion.LookRotation(m_target.position - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 50 * Time.deltaTime);
        }

        void GetLeftRightOffSet(Vector3 p_currentOffset, float p_horizontalAxis)
        {
            if (p_currentOffset != m_currentOffset || p_horizontalAxis == 0) return;

            if (p_horizontalAxis > 0.2f)
            {
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
                m_nextPosition = m_target.position + m_rightOffset;
            }
            if (p_horizontalAxis < -0.2f)
            {
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
                m_nextPosition = m_target.position + m_leftOffset;
            }
        }

        private void SetCameraPosition()
        {   
            if (m_horizontalAxis < 0)
                m_offset = m_leftOffset;

            if (m_horizontalAxis > 0)
                m_offset = m_rightOffset;

            LeanTween.move(this.gameObject, m_nextPosition, 0.5f).setOnComplete(() =>
            {
                m_currentOffset = m_offset;
                DefineCameraDirection();
            });
        }

        public void SetCameraFollow()
        {
            transform.position = m_target.position + m_offset;
        }

        public void DefineCameraDirection()
        {
            if (m_currentOffset == m_deltaPosition[0])
                m_cameraPosition = CameraPosition.SOUTH;

            if (m_currentOffset == m_deltaPosition[1])
                m_cameraPosition = CameraPosition.EAST;

            if (m_currentOffset == m_deltaPosition[2])
                m_cameraPosition = CameraPosition.NORTH;

            if (m_currentOffset == m_deltaPosition[3])
                m_cameraPosition = CameraPosition.WEST;
        }
    }
}
