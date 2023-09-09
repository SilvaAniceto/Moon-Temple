using System.Collections.Generic;
using UnityEngine;

namespace IsometricGameController
{
    public interface IIsometricCamera
    {
        Vector2 CameraLook { get; set; }
        bool CameraAimInput { get; set; }

        void SetInput(IsometricInputHandler inputs);
    }
    public class IsometricCamera : MonoBehaviour, IIsometricCamera
    {
        #region NOT IN USE
        //public static IsometricCamera m_instance;

        //[Header("Isometric Camera")]
        //[SerializeField] private Camera m_camera;

        //[Range(10f, 100f)][SerializeField] private float m_zoomSensibility;
        //[Range(2f, 10f)][SerializeField] private int m_zoomMultiplier;
        #endregion
        [Header("Camera Settings")]
        [Range(10f, 100f)][SerializeField] private float m_cameraSpeed;
        [Range(1f, 3f)][SerializeField] private float m_verticalOffset;

        [Header("Camera Target")]
        [SerializeField] private Transform m_target;

        [Header("Camera Base")]
        [SerializeField] private Transform m_baseMotion;
        #region NOT IN USE
        //private Vector3 m_offset, m_currentOffset, m_leftOffset, m_rightOffset;
        //private Vector3[] m_deltaPosition = { new Vector3(-30, 15, -30), new Vector3(30, 15, -30), new Vector3(30, 15, 30), new Vector3(-30, 15, 30) };
        //private List<Vector3> m_auxDeltaPosition = new List<Vector3>();

        //private float m_horizontalAxis;
        //private float m_zoomInput;
        //private bool m_movingCamera;
        //public enum CameraPosition { SOUTH, EAST, NORTH, WEST }
        //private CameraPosition m_cameraPosition = CameraPosition.SOUTH;

        //#region Properties
        ///// <summary>
        ///// Current camera position.
        ///// </summary>
        //public CameraPosition CamPosition
        //{
        //    get
        //    {
        //        return m_cameraPosition;
        //    }

        //    private set
        //    {
        //        if (m_cameraPosition == value)
        //            return;

        //        m_cameraPosition = value;
        //    }
        //}
        ///// <summary>
        ///// The Main Camera.
        ///// </summary>
        //public Camera Camera
        //{
        //    get
        //    {
        //        return m_camera;
        //    }

        //    private set
        //    {
        //        if (m_camera == value)
        //            return;

        //        m_camera = value;
        //    }
        //}
        ///// <summary>
        ///// Camera target for Look and Follow.
        ///// </summary>
        //public Transform Target
        //{
        //    get
        //    {
        //        return m_target;
        //    }

        //    private set
        //    {
        //        if (m_target == value)
        //            return;

        //        m_target = value;
        //    }
        //}
        ///// <summary>
        ///// Parent transform that moves with the target.
        ///// </summary>
        //public Transform BaseMotion
        //{
        //    get
        //    {
        //        return m_baseMotion;
        //    }

        //    private set
        //    {
        //        if (m_baseMotion == value)
        //            return;

        //        m_baseMotion = value;
        //    }
        //}
        ///// <summary>
        ///// Camera speed for changing position.
        ///// </summary>
        //public float CameraSpeed
        //{
        //    get
        //    {
        //        return m_cameraSpeed;
        //    }

        //    set
        //    {
        //        if (m_cameraSpeed == value)
        //            return;

        //        m_cameraSpeed = value;
        //    }
        //}
        ///// <summary>
        ///// Defines the Camera zoom sensibility.
        ///// </summary>
        //public float ZoomSensibility
        //{
        //    get 
        //    { 
        //        return m_zoomSensibility;
        //    }

        //    set
        //    {
        //        if (m_zoomSensibility == value)
        //            return;

        //        m_zoomSensibility = value;
        //    }
        //}
        ///// <summary>
        ///// Vertical offset relative to the target.
        ///// </summary>
        //public float VerticalOffset
        //{
        //    get
        //    {
        //        return m_verticalOffset;
        //    }

        //    set
        //    {
        //        if (m_verticalOffset == value)
        //            return;

        //        m_verticalOffset = value;
        //    }
        //}
        ///// <summary>
        ///// Multiplier that define the Camera zoom speed.
        ///// </summary>
        //public int ZoomMultiplier
        //{
        //    get
        //    {
        //        return m_zoomMultiplier;
        //    }

        //    set
        //    {
        //        if (m_zoomMultiplier == value)
        //            return;

        //        m_zoomMultiplier = value;
        //    }
        //}
        ///// <summary>
        ///// Offset for next position of the Camera.
        ///// </summary>
        //public Vector3 OffSet
        //{
        //    get
        //    {
        //        if (m_cameraPosition == CameraPosition.SOUTH)
        //            m_offset = m_deltaPosition[0];

        //        if (m_cameraPosition == CameraPosition.EAST)
        //            m_offset = m_deltaPosition[1];

        //        if (m_cameraPosition == CameraPosition.NORTH)
        //            m_offset = m_deltaPosition[2];

        //        if (m_cameraPosition == CameraPosition.WEST)
        //            m_offset = m_deltaPosition[3];

        //        return m_offset;
        //    }
        //}
        ///// <summary>
        ///// Horizontal input for next Camera position.
        ///// </summary>
        //public float HorizontalAxis
        //{
        //    get
        //    {
        //        return m_horizontalAxis;
        //    }
        //}
        ///// <summary>
        ///// Define wheter or not the Camera is moving.
        ///// </summary>
        //public bool MovingCamera
        //{
        //    get
        //    {
        //        return m_movingCamera;
        //    }
        //}
        //public float ZoomInput
        //{
        //    get
        //    {
        //        return m_zoomInput;
        //    }

        //    set
        //    {
        //        if (m_zoomInput == value) return;

        //        m_zoomInput = value;
        //    }
        //}
        //#endregion
        #endregion

        float yRot = 0;

        public Vector2 CameraLook { get; set; }
        public bool CameraAimInput { get; set; }

        private void Awake()
        {
            #region NOT IN USE
            //if (m_instance == null)
            //    m_instance = this;

            //if (m_target == null)
            //{
            //    Transform target = new GameObject("CameraTarget").transform;
            //    m_target = target;
            //    m_target.parent = m_baseMotion.transform;
            //}

            //DefineCameraDirection();

            //if (OffSet != m_currentOffset)
            //    m_currentOffset = m_offset;

            //m_auxDeltaPosition.Add(m_deltaPosition[0]);
            //m_auxDeltaPosition.Add(m_deltaPosition[1]);
            //m_auxDeltaPosition.Add(m_deltaPosition[2]);
            //m_auxDeltaPosition.Add(m_deltaPosition[3]);

            //SetCameraPosition(m_horizontalAxis);
            #endregion
        }

        void Update()
        {
            #region NOT IN USE
            //var targetRotation = Quaternion.LookRotation(m_target.position - transform.position);

            //if (m_movingCamera) transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 1);
            //else transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10 * Time.deltaTime);

            //m_zoomInput = Input.GetAxis("CameraZoom");

            //m_camera.orthographicSize += Mathf.Clamp(m_zoomInput * 10, -1, 1) * m_zoomMultiplier * m_zoomSensibility * Time.deltaTime;
            //m_camera.orthographicSize = Mathf.Clamp(m_camera.orthographicSize, 5, 25);

            //MoveBase();

            //if (m_movingCamera) return;

            //if (Input.GetButton("CameraControll"))
            //{
            //    Vector3 righMovement = transform.right * m_cameraSpeed * Time.deltaTime * Input.GetAxis("HorizontalCameraRotation");

            //    if (!m_movingCamera)
            //        transform.position += righMovement;

            //    GetLeftRightOffSet(m_currentOffset, Input.GetAxis("HorizontalCameraRotation"));
            //}

            //if (Input.GetButtonUp("CameraControll"))
            //{
            //    m_movingCamera = false;
            //    SetCameraPosition(HorizontalAxis);
            //}
            #endregion
            MoveBase();

            //if (CameraAimInput)
            //{
            //    yRot += CameraLook.y * Time.deltaTime * m_cameraSpeed;
            //    //yRot = Mathf.Clamp(yRot, -30.0f, 30.0f);
            //    m_baseMotion.localRotation = Quaternion.Euler(0, yRot, 0);
            //}
        }

        public void SetInput(IsometricInputHandler inputs)
        {
            CameraLook = new Vector2(inputs.CameraLook.y, inputs.CameraLook.x);
            CameraAimInput = inputs.CameraAimInput;
        }

        /// <summary>
        /// Move the Camera Base along the target.
        /// </summary>
        public void MoveBase()
        {
            m_baseMotion.position = m_target.position + new Vector3(0, m_verticalOffset, 0);
        }

        #region NOT IN USE
        ///// <summary>
        ///// Calculate and defines the next position for the Camera movement.
        ///// </summary>
        //void GetLeftRightOffSet(Vector3 p_currentOffset, float p_horizontalAxis)
        //{
        //    if (p_currentOffset != m_currentOffset || p_horizontalAxis == 0) return;

        //    if (p_horizontalAxis > 0.05f)
        //    {
        //        m_horizontalAxis = 1;

        //        int index = m_auxDeltaPosition.IndexOf(p_currentOffset);

        //        if (index + 1 > m_auxDeltaPosition.Count - 1)
        //        {
        //            m_leftOffset = m_auxDeltaPosition[index - 1];
        //            m_rightOffset = m_auxDeltaPosition[0];
        //        }
        //        else if (index - 1 < 0)
        //        {
        //            m_leftOffset = m_auxDeltaPosition[m_auxDeltaPosition.Count - 1];
        //            m_rightOffset = m_auxDeltaPosition[index + 1];
        //        }
        //        else
        //        {
        //            m_leftOffset = m_auxDeltaPosition[index - 1];
        //            m_rightOffset = m_auxDeltaPosition[index + 1];
        //        }
        //    }
        //    if (p_horizontalAxis < -0.05f)
        //    {
        //        m_horizontalAxis = -1;

        //        int index = m_auxDeltaPosition.IndexOf(p_currentOffset);

        //        if (index - 1 < 0)
        //        {
        //            m_leftOffset = m_auxDeltaPosition[m_auxDeltaPosition.Count - 1];
        //            m_rightOffset = m_auxDeltaPosition[index + 1];
        //        }
        //        else if(index + 1 > m_auxDeltaPosition.Count - 1)
        //        {
        //            m_leftOffset = m_auxDeltaPosition[index - 1];
        //            m_rightOffset = m_auxDeltaPosition[0];
        //        }
        //        else
        //        {
        //            m_leftOffset = m_auxDeltaPosition[index - 1];
        //            m_rightOffset = m_auxDeltaPosition[index + 1];
        //        }
        //    }
        //}

        ///// <summary>
        ///// Move and set the current position of the Camera.
        ///// </summary>
        //private void SetCameraPosition(float p_horizontalAxis)
        //{
        //    if (p_horizontalAxis == 0) m_movingCamera = true;

        //    if (m_movingCamera) { m_movingCamera = false; return; }    

        //    m_movingCamera = true;

        //    if (p_horizontalAxis < 0) m_offset = m_leftOffset;

        //    if (p_horizontalAxis > 0) m_offset = m_rightOffset;

        //    LeanTween.move(this.gameObject, m_target.position + m_offset, m_cameraSpeed * 0.05f).setOnComplete(() =>
        //    {
        //        m_horizontalAxis = 0;
        //        m_currentOffset = m_offset;
        //        DefineCameraDirection();
        //        m_movingCamera = false;
        //    });
        //}

        ///// <summary>
        ///// Define the Camera look direction based on the new position. 
        ///// </summary>
        //public void DefineCameraDirection()
        //{
        //    if (m_currentOffset == m_deltaPosition[0])
        //        m_cameraPosition = CameraPosition.SOUTH;

        //    if (m_currentOffset == m_deltaPosition[1])
        //        m_cameraPosition = CameraPosition.EAST;

        //    if (m_currentOffset == m_deltaPosition[2])
        //        m_cameraPosition = CameraPosition.NORTH;

        //    if (m_currentOffset == m_deltaPosition[3])
        //        m_cameraPosition = CameraPosition.WEST;
        //}

        //public Ray GetRay(Vector3 p_value)
        //{
        //    return m_camera.ScreenPointToRay(p_value);
        //}
        #endregion
    }
}