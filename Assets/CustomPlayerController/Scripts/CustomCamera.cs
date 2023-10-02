using Cinemachine;
using UnityEngine;
using UnityEngine.Events;

namespace CustomGameController
{
    public class CustomCamera : MonoBehaviour, ICustomCamera
    {
        public static CustomCamera Instance;

        [HideInInspector] public UnityEvent<CameraPerspective> OnCameraPerspectiveChanged = new UnityEvent<CameraPerspective>();

        #region CAMERA PROPERTIES
        public Camera PlayerCamera { get; set; }
        public CustomCharacterController CustomController { get; set; }
        public CameraPerspectiveSettings FirstPerson { get => FirstPersonSettings; }
        public CameraPerspectiveSettings Isometric { get => IsometricSettings; }
        public CameraPerspectiveSettings ThirdPerson { get => ThirdPersonSettings; }
        public CameraPerspectiveSettings OverShoulder { get => OverShoulderSettings; }
        public Vector3 CameraHeightOfftset { get => new Vector3(0.0f, CameraTargetHeight / 2, 0.0f); }
        #endregion

        #region CAMERA SETTINGS
        public Transform CameraTarget { get; set; }
        public float CameraTargetHeight { get; set; }
        public float CameraSensibility { get; set; }
        public CameraPerspective CameraPerspective { get; set; }
        #endregion

        #region CAMERA INPUTS VALUES & METHODS
        public float CameraPan { get; set; }
        public float CameraTilt { get; set; }
        public bool ChangeCameraPerspective
        {
            set
            {
                if (m_changePerspective == value) return;

                m_changePerspective = value;

                if (m_changePerspective)
                {
                    int index = (int)CameraPerspective;

                    index++;

                    if (index > 3) index = 0;
                    if (index < 0) index = 3;

                    OnCameraPerspectiveChanged?.Invoke((CameraPerspective)index);
                }
            }
        }
        public void SetInput(CustomPlayerInputHandler inputs)
        {
            CameraPan = Mathf.Clamp(inputs.CameraAxis.y, -1, 1);
            CameraTilt = Mathf.Clamp(inputs.CameraAxis.x, -1, 1);
            ChangeCameraPerspective = inputs.ChangeCameraPerspective;
        }
        #endregion

        #region CAMERA METHODS
        public void SetCameraPerspective(CameraPerspective perspective)
        {
            CameraPerspective = perspective;
            switch (CameraPerspective)
            {
                case CameraPerspective.Isometric:
                    CurrentSettings = Isometric;
                    break;
                case CameraPerspective.Third_Person:
                    CurrentSettings = ThirdPerson;
                    break;
                case CameraPerspective.Over_Shoulder:
                    CurrentSettings = OverShoulder;
                    break;
                case CameraPerspective.First_Person:
                    CurrentSettings = FirstPerson;
                    break;
            }

            transform.rotation = CurrentSettings.Rotation;
            PlayerCamera.orthographic = CurrentSettings.OrthographicPerspective;
            VirtualCamera.m_Lens.OrthographicSize = CurrentSettings.ViewSize;
            VirtualCamera.m_Lens.FieldOfView = CurrentSettings.ViewSize;
            VirtualCameraFollow.Damping = CurrentSettings.Damping;
            VirtualCameraFollow.ShoulderOffset = CurrentSettings.ShoulderOffset;
            VirtualCameraFollow.CameraDistance = CurrentSettings.CameraDistance;
        }
        public void UpdateCamera(float cameraTilt, float cameraPan)
        {
            m_xRot += cameraTilt * CameraSensibility;
            m_yRot += cameraPan * CameraSensibility;

            m_xRot = CurrentSettings.ClampXRotation ? Mathf.Clamp(m_xRot, CurrentSettings.XRotationRange.x, CurrentSettings.XRotationRange.y) : m_xRot;
            m_yRot = CurrentSettings.ClampYRotation ? Mathf.Clamp(m_yRot, CurrentSettings.YRotationRange.x, CurrentSettings.YRotationRange.y) : m_yRot;

            CameraTarget.transform.localRotation = Quaternion.Euler(m_xRot, m_yRot, 0);

            CustomController.Forward = CustomPerspective.CustomForward;
            CustomController.Right = CustomPerspective.CustomRight;
        }
        public void SetPerspectiveSettings()
        {
            FirstPersonSettings = new CameraPerspectiveSettings(new Vector2(-70.0f, 70.0f), true, new Vector2(0.0f, 0.0f), false, Quaternion.Euler(new Vector3(0.0f, 0.0f, 0.0f)), new Vector3(0.0f, 0.0f, 0.0f), new Vector3(0.0f, -0.75f, 0.0f), false, 60.0f, 0.0f);
            IsometricSettings = new CameraPerspectiveSettings(new Vector2(25.0f, 25.0f), true, new Vector2(15.0f, 15.0f), true, Quaternion.Euler(new Vector3(0.0f, 30.0f, 0.0f)), new Vector3(0.0f, 0.0f, 0.0f), new Vector3(0.0f, 0.0f, 0.0f), true, 6.0f, 45.0f);
            ThirdPersonSettings = new CameraPerspectiveSettings(new Vector2(-50.0f, 70.0f), true, new Vector2(0.0f, 0.0f), false, Quaternion.Euler(new Vector3(0.0f, 0.0f, 0.0f)), new Vector3(0.0f, 0.5f, 0.3f), new Vector3(0.0f, 0.5f, 0.0f), false, 60.0f, 6.0f);
            OverShoulderSettings = new CameraPerspectiveSettings(new Vector2(-50.0f, 70.0f), true, new Vector2(0.0f, 0.0f), false, Quaternion.Euler(new Vector3(0.0f, 0.0f, 0.0f)), new Vector3(0.0f, 0.5f, 0.3f), new Vector3(0.6f, -0.8f, 0.0f), false, 70.0f, 2.8f);
        }
        #endregion

        #region PRIVATE FIELDS
        private CinemachineVirtualCamera VirtualCamera;
        private Cinemachine3rdPersonFollow VirtualCameraFollow;
        private CameraPerspectiveSettings CurrentSettings = new CameraPerspectiveSettings();

        private CameraPerspectiveSettings FirstPersonSettings;
        private CameraPerspectiveSettings IsometricSettings;
        private CameraPerspectiveSettings ThirdPersonSettings;
        private CameraPerspectiveSettings OverShoulderSettings;

        private float m_xRot = 0;
        private float m_yRot = 0;
        private bool m_changePerspective = false;
        #endregion

        #region DEFAULT METHODS
        private void Awake()
        {
            if (Instance == null) Instance = this;

            SetPerspectiveSettings();

            PlayerCamera = Camera.main;
            VirtualCamera = GetComponentInChildren<CinemachineVirtualCamera>();
            VirtualCameraFollow = GetComponentInChildren<Cinemachine3rdPersonFollow>();

            OnCameraPerspectiveChanged.AddListener(SetCameraPerspective);
        }
        void Update()
        {
            UpdateCamera(CameraTilt, CameraPan);
        }
        private void LateUpdate()
        {
            CameraTarget.position = CustomController.transform.position + CameraHeightOfftset;
        }
        #endregion
    }
}