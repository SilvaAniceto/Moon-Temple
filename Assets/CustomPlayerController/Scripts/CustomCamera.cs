using Cinemachine;
using UnityEngine;

namespace CustomGameController
{
    public class CustomCamera : MonoBehaviour, ICustomCamera
    {
        public static CustomCamera Instance;
        public CustomCharacterController CustomController { get; set; }
        public Camera PlayerCamera { get; set; }
        public CameraPerspective CameraPerspective { get; set; }
        public CameraPerspectiveSettings FirstPerson { get => FirstPersonSettings; }
        public CameraPerspectiveSettings Isometric { get => IsometricSettings; }
        public CameraPerspectiveSettings ThirdPerson { get => ThirdPersonSettings; }
        public Transform CameraTarget { get; set; }
        public float CameraTargetHeight { get; set; }
        public Vector3 CameraHeightOfftset { get => new Vector3(0.0f, CameraTargetHeight / 2, 0.0f); }
        public float CameraSensibility { get; set; }
        public float CameraPan { get; set; }
        public float CameraTilt { get; set; }
        public bool ChangeCameraPerspective { get; set; }

        private Cinemachine3rdPersonFollow VirtualCamera;
        private CameraPerspectiveSettings CurrentSettings = new CameraPerspectiveSettings();

        private CameraPerspectiveSettings FirstPersonSettings = new CameraPerspectiveSettings(new Vector2(-70.0f, 70.0f), new Vector2(-360.0f, 360.0f), Quaternion.Euler(new Vector3(0.0f, 0.0f, 0.0f)), new Vector3(0.0f, 0.0f, 0.0f), new Vector3(0.0f, -0.75f, 0.0f), false, 0.0f);
        private CameraPerspectiveSettings IsometricSettings = new CameraPerspectiveSettings(new Vector2(10.0f, 45.0f), new Vector2(-45.0f, 45.0f), Quaternion.Euler(new Vector3(0.0f, 45.0f, 0.0f)), new Vector3(0.0f, 0.0f, 0.0f), new Vector3(0.0f, 0.0f, 0.0f), true, 45.0f);
        private CameraPerspectiveSettings ThirdPersonSettings = new CameraPerspectiveSettings(new Vector2(2.0f, 70.0f), new Vector2(-360.0f, 360.0f), Quaternion.Euler(new Vector3(0.0f, 0.0f, 0.0f)), new Vector3(0.0f, 0.5f, 0.3f), new Vector3(0.0f, 0.0f, 0.0f), false, 5.0f);

        float xRot = 0;
        float yRot = 0;

        private void Awake()
        {
            if (Instance == null) Instance = this;

            PlayerCamera = Camera.main;
            VirtualCamera = GetComponentInChildren<Cinemachine3rdPersonFollow>();
        }

        void Update()
        {
            UpdateCamera(CameraTilt, CameraPan);
        }

        private void LateUpdate()
        {
            CameraTarget.position = CustomController.transform.position + CameraHeightOfftset;
        }

        public void SetInput(CustomPlayerInputHandler inputs)
        {
            CameraPan = inputs.CameraAxis.y;
            CameraTilt = inputs.CameraAxis.x;
            ChangeCameraPerspective = inputs.ChangeCameraPerspective;
        }

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
                    break;
                case CameraPerspective.First_Person:
                    CurrentSettings = FirstPerson;
                    break;
            }

            transform.rotation = CurrentSettings.Rotation;
            PlayerCamera.orthographic = CurrentSettings.OrthographicPerspective;
            VirtualCamera.Damping = CurrentSettings.Damping;
            VirtualCamera.ShoulderOffset = CurrentSettings.ShoulderOffset;
            VirtualCamera.CameraDistance = CurrentSettings.CameraDistance;
        }

        public void UpdateCamera(float cameraTilt, float cameraPan)
        {
            xRot += cameraTilt;
            yRot += cameraPan;

            xRot = Mathf.Clamp(xRot, CurrentSettings.XRotationRange.x, CurrentSettings.XRotationRange.y);
            yRot = Mathf.Clamp(yRot, CurrentSettings.YRotationRange.x, CurrentSettings.YRotationRange.y);

            CameraTarget.transform.localRotation = Quaternion.Euler(xRot, yRot, 0);

            CustomController.Forward = CustomPerspective.CustomForward;
            CustomController.Right = CustomPerspective.CustomRight;
        }
    }
}