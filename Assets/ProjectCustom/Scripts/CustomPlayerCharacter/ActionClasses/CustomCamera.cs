using Cinemachine;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace CustomGameController
{
    public class CustomCamera : MonoBehaviour, ICustomCamera
    {
        public static CustomCamera Instance;

        #region CAMERA PROPERTIES
        public Camera PlayerCamera { get; set; }
        public LayerMask ThirdPersonCollisionFilter { get; set; }
        public LayerMask IsometricCollisionFilter { get; set; }
        public CustomCharacterController CustomController { get; set; }
        public Vector3 CameraHeightOfftset { get => new Vector3(0.0f, CameraTargetHeight / 2, 0.0f); }
        public int VerticalCameraDirection
        {
            get
            {
                if (CameraTarget.rotation.x < -0.02f) return 1;
                else if (CameraTarget.rotation.x > 0.02f) return -1;
                else return 0;
            }
        }
        #endregion

        #region CAMERA SETTINGS
        public Transform CameraTarget { get; set; }
        public float CameraTargetHeight { get; set; }
        public float CameraSensibility { get; set; }
        public float CurrentCameraDistance { get; set; }
        public Vector3 CurrentDamping { get; set; }
        public Vector3 CurrentShoulderOffset { get; set; }
        #endregion

        #region CAMERA INPUTS VALUES & METHODS
        public float CameraPan { get; set; }
        public float CameraTilt { get; set; }
        public float CameraZoom { get; set; }
        public void SetInput(CustomPlayerInputHandler inputs)
        {
            CameraPan = Mathf.Clamp(inputs.CameraAxis.y, -1, 1);
            CameraTilt = Mathf.Clamp(inputs.CameraAxis.x, -1, 1);
            CameraZoom = inputs.CameraZoom;
        }
        #endregion

        #region CAMERA METHODS
        public void UpdateCamera(float cameraTilt, float cameraPan, float cameraZoom)
        {
            Vector3 lookDirection = new Vector3(cameraTilt, cameraPan, cameraZoom);

            m_xRot += cameraTilt * CameraSensibility;
            m_yRot += cameraPan * CameraSensibility;

            if (CustomController.InFlight && CustomController.SprintInput)
            {
                if (lookDirection != Vector3.zero)
                {
                    m_xRot = Mathf.Clamp(m_xRot, -15.0f, 30.0f);

                    CameraTarget.transform.localRotation = Quaternion.Slerp(CameraTarget.transform.localRotation, Quaternion.Euler(m_xRot, m_yRot, 0), 4.5f * Time.deltaTime);
                }
                else
                {
                    CameraTarget.transform.localRotation = Quaternion.Slerp(CameraTarget.transform.localRotation, CustomController.transform.rotation, 4.5f * Time.deltaTime);

                    m_xRot = 0.0f;
                    m_yRot = CameraTarget.transform.localEulerAngles.y;
                }

                return;
            }

            m_xRot = Mathf.Clamp(m_xRot, -50.0f, 70.0f);

            CameraTarget.transform.localRotation = Quaternion.Slerp(CameraTarget.transform.localRotation, Quaternion.Euler(m_xRot, m_yRot, 0), 4.5f * Time.deltaTime);

            CustomController.Forward = CustomPerspective.CustomForward;
            CustomController.Right = CustomPerspective.CustomRight;

        }
        public void UpdateCameraFollow(float cameraDistance, Vector3 damping, Vector3 shoulderOffset)
        {
            VirtualCameraFollow.ShoulderOffset = Vector3.MoveTowards(VirtualCameraFollow.ShoulderOffset, shoulderOffset, Time.deltaTime * 4.0f);
            VirtualCameraFollow.CameraDistance = Mathf.Lerp(VirtualCameraFollow.CameraDistance, cameraDistance, Time.deltaTime * 4.0f);
            VirtualCameraFollow.Damping = damping;
        }
        #endregion

        #region PRIVATE FIELDS
        private CinemachineVirtualCamera VirtualCamera;
        private Cinemachine3rdPersonFollow VirtualCameraFollow;

        private float m_xRot = 0;
        private float m_yRot = 0;
        #endregion

        #region DEFAULT METHODS
        private void Awake()
        {
            if (Instance == null) Instance = this;

            PlayerCamera = Camera.main;
            VirtualCamera = GetComponentInChildren<CinemachineVirtualCamera>();
            VirtualCameraFollow = GetComponentInChildren<Cinemachine3rdPersonFollow>();
        }
        private void Start()
        {
            CurrentDamping = VirtualCameraFollow.Damping;
            CurrentShoulderOffset = VirtualCameraFollow.ShoulderOffset;

            CurrentCameraDistance = VirtualCameraFollow.CameraDistance;
        }
        void Update()
        {
            UpdateCamera(CameraTilt, CameraPan, CameraZoom);
        }
        private void LateUpdate()
        {
            CameraTarget.position = CustomController.transform.position + CameraHeightOfftset;
        }
        #endregion
    }
}