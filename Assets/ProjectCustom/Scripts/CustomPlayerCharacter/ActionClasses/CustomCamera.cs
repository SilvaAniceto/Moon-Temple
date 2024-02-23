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
        public Vector3 CameraOfftset { get; set; }
        public Vector3 WalkOfftset { get; set; }
        public Vector3 SprintOfftset { get; set; }
        public Vector3 HoverFlightOfftset { get; set; }
        public Vector3 SpeedFlightOfftset { get; set; }
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

            lookDirection = lookDirection.normalized;

            m_xRot += lookDirection.x * CameraSensibility;
            m_yRot += lookDirection.y * CameraSensibility;

            angle = Vector3.SignedAngle(transform.forward, archorOrientation.forward, Vector3.right);

            if (CustomController.InFlight && CustomController.SprintInput)
            {
                CameraOfftset = Vector3.Lerp(CameraOfftset, SpeedFlightOfftset, Time.deltaTime);

                float latitudinalOrientation = CustomController.transform.localEulerAngles.x > 180 ? CustomController.transform.localEulerAngles.x - 360 : CustomController.transform.localEulerAngles.x;
                float longitudinalOrientation = CustomController.transform.localEulerAngles.y > 180 ? CustomController.transform.localEulerAngles.y - 360 : CustomController.transform.localEulerAngles.y;

                if (latitudinalOrientation < -35.0f)
                {
                    if (lookDirection != Vector3.zero)
                    {
                        if (lookDirection.x > 0.2f)
                        {
                            latitudinalThreshold = Mathf.Lerp(latitudinalThreshold, 125f, Time.deltaTime * 2.0f);
                        }
                        if (lookDirection.x < -0.2f)
                        {
                            latitudinalThreshold = Mathf.Lerp(latitudinalThreshold, 15.0f, Time.deltaTime * 2.0f);
                        }
                    }
                    else
                    {
                        latitudinalThreshold = Mathf.Lerp(latitudinalThreshold, 40.0f, Time.deltaTime * 2.0f);
                    }
                    latitudinalOrientation += latitudinalThreshold;

                }
                else
                {
                    if (lookDirection != Vector3.zero)
                    {
                        if (lookDirection.x > 0.2f)
                        {
                            latitudinalThreshold = Mathf.Lerp(latitudinalThreshold, -2.5f, Time.deltaTime * 2.0f);
                        }
                        if (lookDirection.x < -0.2f)
                        {
                            latitudinalThreshold = Mathf.Lerp(latitudinalThreshold, 20.0f, Time.deltaTime * 2.0f);
                        }
                    }
                    else
                    {
                        latitudinalThreshold = Mathf.Lerp(latitudinalThreshold, 15.0f, Time.deltaTime * 2.0f);
                    }
                    latitudinalOrientation += latitudinalThreshold;
                }

                if (lookDirection != Vector3.zero)
                {
                    if (lookDirection.y > 0.2f)
                    {
                        longitudinalThreshold = Mathf.Lerp(longitudinalThreshold, -20.0f, Time.deltaTime * 2.0f);
                    }
                    if (lookDirection.y < -0.2f)
                    {
                        longitudinalThreshold = Mathf.Lerp(longitudinalThreshold, Mathf.Clamp(longitudinalThreshold, 20.0f, 20.0f), Time.deltaTime * 2.0f);
                    }
                }
                else
                {
                    longitudinalThreshold = Mathf.Lerp(longitudinalThreshold, 0, Time.deltaTime * 2.0f);
                }

                longitudinalOrientation += longitudinalThreshold;
                transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(latitudinalOrientation, longitudinalOrientation, 0.0f), 4.5f * Time.deltaTime);

                m_xRot = 0.0f;
                m_yRot = CustomController.transform.localEulerAngles.y;
                return;
            }

            CameraOfftset = CustomController.SprintInput ? Vector3.Lerp(CameraOfftset, SprintOfftset, Time.deltaTime) : Vector3.Lerp(CameraOfftset, WalkOfftset, Time.deltaTime);

            m_xRot = Mathf.Clamp(m_xRot, -50.0f, 70.0f);

            transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(m_xRot, m_yRot, 0), 4.5f * Time.deltaTime);

            CustomController.Forward = CustomPerspective.CustomForward;
            CustomController.Right = CustomPerspective.CustomRight;
        }
        public void UpdateCameraFollow(float cameraDistance, Vector3 damping, Vector3 shoulderOffset)
        {
            //VirtualCameraFollow.ShoulderOffset = Vector3.MoveTowards(VirtualCameraFollow.ShoulderOffset, shoulderOffset, Time.deltaTime * 4.0f);
            //VirtualCameraFollow.CameraDistance = Mathf.Lerp(VirtualCameraFollow.CameraDistance, cameraDistance, Time.deltaTime * 4.0f);
            //VirtualCameraFollow.Damping = damping;
        }
        #endregion

        #region PRIVATE FIELDS
        private CinemachineVirtualCamera VirtualCamera;
        private Cinemachine3rdPersonFollow VirtualCameraFollow;

        private float m_xRot = 0;
        private float m_yRot = 0;

        private float angle;
        private float longitudinalThreshold;
        private float latitudinalThreshold;
        [SerializeField] private Transform archorOrientation;
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
            archorOrientation.position = new Vector3(CustomController.CharacterController.bounds.center.x, CustomController.CharacterController.bounds.center.y - CustomController.CharacterController.bounds.extents.y, CustomController.CharacterController.bounds.center.z);
            archorOrientation.localRotation = CustomController.transform.localRotation;
            transform.position = archorOrientation.position;
            CameraTarget.localPosition = Vector3.zero + CameraOfftset;
        }
        private void LateUpdate()
        {
        }
        #endregion
    }
}