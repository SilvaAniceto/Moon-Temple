using Cinemachine;
using UnityEngine;

namespace CustomGameController
{
    public class CustomCamera : MonoBehaviour, ICustomCamera
    {
        public static CustomCamera Instance;
        public Camera PlayerCamera { get; set; }
        public CameraPerspective CameraPerspective { get; set; }
        public Transform CameraTarget { get; set; }
        public Transform CameraPivot { get; set; }
        public float CameraTargetHeight { get; set; }
        public Vector3 CameraHeightOfftset { get => new Vector3(0.0f, CameraTargetHeight / 2, 0.0f); }
        public float CameraSensibility { get; set; }
        public float CameraPan { get; set; }
        public float CameraTilt { get; set; }
        public bool ChangeCameraPerspective { get; set; }

        [SerializeField] private CustomCharacterController CustomController;
        private Cinemachine3rdPersonFollow VirtualCamera;

        float xRot = 0;
        float yRot = 0;

        private void Awake()
        {
            if (Instance == null) Instance = this;

            PlayerCamera = Camera.main;
            VirtualCamera = GetComponentInChildren<Cinemachine3rdPersonFollow>();
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

        public void UpdateCameraCustom() 
        {
            CustomController.Forward = CustomPerspective.CustomForward;
            CustomController.Right = CustomPerspective.CustomRight;
        }

        void Update()
        {
            UpdateCameraCustom();

            if (CameraPerspective != CameraPerspective.First_Person)
            {
                UpdateIsometricCamera(CameraTilt, CameraPan);
            }
            else if (CameraPerspective == CameraPerspective.First_Person)
            {
                UpdateFirstPersonCamera(CameraTilt, CameraPan);
            }
        }
        public void SetCameraPerspective(CameraPerspective perspective)
        {
            CameraPerspective = perspective;
            switch (perspective)
            {
                case CameraPerspective.None:
                    break;
                case CameraPerspective.Isometric:
                    transform.rotation = Quaternion.Euler(new Vector3(0.0f, 45.0f, 0.0f));
                    PlayerCamera.orthographic = true;
                    VirtualCamera.CameraDistance = 45.0f;
                    break;
                case CameraPerspective.Third_Person:
                    transform.rotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, 0.0f));
                    PlayerCamera.orthographic = false;
                    VirtualCamera.CameraDistance = 5.0f;
                    break;
                case CameraPerspective.Over_Shoulder:
                    break;
                case CameraPerspective.First_Person:
                    transform.rotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, 0.0f));
                    PlayerCamera.orthographic = false;
                    VirtualCamera.CameraDistance = 0.0f;
                    break;
            }
        }

        public void UpdateFirstPersonCamera(float cameraTilt, float cameraPan)
        {
            xRot += cameraTilt;
            yRot += cameraPan;
            CameraTarget.transform.rotation = Quaternion.Euler(xRot, yRot, 0);
        }

        public void UpdateIsometricCamera(float cameraTilt, float cameraPan)
        {
            xRot += cameraTilt;
            yRot += cameraPan;

            xRot = Mathf.Clamp(xRot, 10.0f, 45.0f);
            yRot = Mathf.Clamp(yRot, -45.0f, 45.0f);

            CameraTarget.transform.localRotation = Quaternion.Euler(xRot, yRot, 0);
        }
    }
}