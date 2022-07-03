using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IsometricOrientedPerspective
{
    public class IsometricCamera : IsometricOrientedPerspective
    {
        public static IsometricCamera m_instance;

        [SerializeField] private Camera m_camera;
        [Range(0f, 100f)][SerializeField] private float m_sensibility, m_zoomSensibility;
        [Range(2f, 10f)][SerializeField] private int m_zoomMultiplier;
        [Range(0f, 3f)][SerializeField] private float m_verticalOffset;
        [SerializeField] private Transform m_target, m_baseMotion;
        private Vector3 m_offset, m_currentOffset, m_leftOffset, m_rightOffset;
        private Vector3[] m_deltaPosition = { new Vector3(-30, 15, -30), new Vector3(30, 15, -30), new Vector3(30, 15, 30), new Vector3(-30, 15, 30) };
        private List<Vector3> m_auxDeltaPosition = new List<Vector3>();
        private float m_horizontalAxis;
        private float m_zoom;
        private bool m_movingCamera;
        [HideInInspector] public enum CameraPosition { SOUTH, EAST, NORTH, WEST }
        [HideInInspector] private CameraPosition m_cameraPosition = CameraPosition.SOUTH;

        #region Properties
        public CameraPosition CamPosition
        {
            get
            {
                return m_cameraPosition;
            }

            private set
            {
                if (m_cameraPosition == value)
                    return;

                m_cameraPosition = value;
            }
        }
        public Camera Camera
        {
            get
            {
                return m_camera;
            }

            private set
            {
                if (m_camera == value)
                    return;

                m_camera = value;
            }
        }
        public Transform Target
        {
            get
            {
                return m_target;
            }

            private set
            {
                if (m_target == value)
                    return;

                m_target = value;
            }
        }
        public Transform BaseMotion
        {
            get
            {
                return m_baseMotion;
            }

            private set
            {
                if (m_baseMotion == value)
                    return;

                m_baseMotion = value;
            }
        }
        public float Sensibility
        {
            get
            {
                return m_sensibility;
            }

            set
            {
                if (m_sensibility == value)
                    return;

                m_sensibility = value;
            }
        }
        public float ZoomSensibility
        {
            get 
            { 
                return m_zoomSensibility;
            }

            set
            {
                if (m_zoomSensibility == value)
                    return;

                m_zoomSensibility = value;
            }
        }
        public float VerticalOffset
        {
            get
            {
                return m_verticalOffset;
            }

            set
            {
                if (m_verticalOffset == value)
                    return;

                m_verticalOffset = value;
            }
        }
        public int ZoomMultiplier
        {
            get
            {
                return m_zoomMultiplier;
            }
            
            set
            {
                if (m_zoomMultiplier == value)
                    return;

                m_zoomMultiplier = value;
            }
        }
        public Vector3 OffSet
        {
            get
            {
                if (m_cameraPosition == CameraPosition.SOUTH)
                    m_offset = m_deltaPosition[0];

                if (m_cameraPosition == CameraPosition.EAST)
                    m_offset = m_deltaPosition[1];

                if (m_cameraPosition == CameraPosition.NORTH)
                    m_offset = m_deltaPosition[2];

                if (m_cameraPosition == CameraPosition.WEST)
                    m_offset = m_deltaPosition[3];

                return m_offset;
            }
        }
        public float HorizontalAxis
        {
            get
            {
                return m_horizontalAxis;
            }
        }
        #endregion

        new void Awake()
        {
            base.Awake();

            if (m_instance == null)
                m_instance = this;
            
            DefineCameraDirection();

            if (OffSet != m_currentOffset)
                m_currentOffset = m_offset;

            m_auxDeltaPosition.Add(m_deltaPosition[0]);
            m_auxDeltaPosition.Add(m_deltaPosition[1]);
            m_auxDeltaPosition.Add(m_deltaPosition[2]);
            m_auxDeltaPosition.Add(m_deltaPosition[3]);

            SetCameraPosition(m_horizontalAxis);
        }

        void Update()
        {
            if (Input.GetButton("CameraControll"))
            {
                m_movingCamera = true;

                Vector3 direction = new Vector3(Input.GetAxis("HorizontalCameraRotation"), 0, 0);
                Vector3 righMovement = transform.right * m_sensibility * Time.deltaTime * direction.x;

                if (!m_movingCamera)
                    transform.position += righMovement;

                GetLeftRightOffSet(m_currentOffset, Input.GetAxis("HorizontalCameraRotation"));
            }
            if (Input.GetButton("CameraControll2"))
            {
                GetLeftRightOffSet(m_currentOffset, Input.GetAxis("HorizontalCameraRotation"));

                if (HorizontalAxis != 0)
                    SetCameraPosition(HorizontalAxis);
            }

            if (Input.GetButtonUp("CameraControll"))
            {
                m_movingCamera = false;
                SetCameraPosition(HorizontalAxis);
            }

            m_zoom = Input.GetAxis("CameraZoom");

            m_camera.orthographicSize += -m_zoom * m_zoomMultiplier * m_zoomSensibility * Time.deltaTime;
            m_camera.orthographicSize = Mathf.Clamp(m_camera.orthographicSize, 5, 15);
            
            var targetRotation = Quaternion.LookRotation(m_target.position - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 100f * Time.deltaTime);
        }

        public void MoveBase()
        {
            m_baseMotion.position = m_target.position + new Vector3(0, m_verticalOffset, 0);
        }

        void GetLeftRightOffSet(Vector3 p_currentOffset, float p_horizontalAxis)
        {
            if (p_currentOffset != m_currentOffset || p_horizontalAxis == 0 ) return;

            if (p_horizontalAxis > 0.35f)
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
            }
            if (p_horizontalAxis < -0.35f)
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
            }
        }
        private void SetCameraPosition(float p_horizontalAxis)
        {
            if (p_horizontalAxis == 0) m_movingCamera = true;

            if (m_movingCamera) { m_movingCamera = false; return; }    
                
            m_movingCamera = true;

            if (p_horizontalAxis < 0) m_offset = m_leftOffset;

            if (p_horizontalAxis > 0) m_offset = m_rightOffset;

            LeanTween.move(this.gameObject, m_target.position + m_offset, 1.5f).setOnComplete(() =>
            {
                m_horizontalAxis = 0;
                m_currentOffset = m_offset;
                DefineCameraDirection();
                m_movingCamera = false;
            });
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
