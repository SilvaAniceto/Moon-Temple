using UnityEngine;

namespace CustomGameController
{
    [ExecuteAlways]
    public class CustomCamera : MonoBehaviour, ICustomCamera
    {
        //public static CustomCamera Instance;
        public Camera PlayerCamera { get; set; }
        public CameraPerspective CameraPerspective { get; set; }
        public Transform CameraTarget { get; set; }
        public Transform CameraPivot { get; set; }
        public float CameraSensibility { get; set; }
        public float CameraPan { get; set; }
        public float CameraTilt { get; set; }
        public bool ChangeCameraPerspective { get; set; }

        [SerializeField] private CustomCharacterController CustomController;
        private void Update()
        {
            UpdateCameraCustom();
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

        //float xRot = 0;
        //float yRot = 0;
        //private void Awake()
        //{
        //    if (Instance == null) Instance = this;

        //    PlayerCamera = GetComponentInChildren<Camera>();
        //}

        //void Update()
        //{
        //    if (CameraPerspective != CameraPerspective.First_Person)
        //    {
        //        var targetRotation = Quaternion.LookRotation(CameraTarget.position - transform.position);
        //        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 1);
        //        UpdateIsometricCamera(CameraTilt, CameraPan);
        //    }
        //    else if (CameraPerspective == CameraPerspective.First_Person)
        //    {
        //        UpdateFirstPersonCamera(CameraTilt, CameraPan);
        //    }
        //}
        //public void SetCameraPerspective(CameraPerspective perspective)
        //{
        //    CameraPerspective = perspective;
        //    switch (perspective)
        //    {
        //        case CameraPerspective.None:
        //            PlayerCamera.transform.localPosition = Vector3.zero;
        //            break;
        //        case CameraPerspective.Isometric:
        //            PlayerCamera.transform.localPosition = new Vector3(-25.0f, 15.0f, -25.0f);
        //            PlayerCamera.transform.localRotation = Quaternion.Euler( new Vector3(0.0f, 45.0f, 0.0f));

        //            PlayerCamera.orthographic = true;
        //            PlayerCamera.orthographicSize = 10;
        //            break;
        //        case CameraPerspective.Third_Person:
        //            PlayerCamera.transform.localPosition = new Vector3(0.0f, 4.5f, -10.0f);
        //            PlayerCamera.transform.localRotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, 0.0f));

        //            PlayerCamera.orthographic = false;
        //            break;
        //        case CameraPerspective.Over_Shoulder:
        //            break;
        //        case CameraPerspective.First_Person:
        //            PlayerCamera.transform.localPosition = new Vector3(0.0f, 1.5f, 0f);
        //            PlayerCamera.transform.localRotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, 0.0f));

        //            PlayerCamera.orthographic = false;
        //            break;
        //    }
        //}

        //public void UpdateFirstPersonCamera(float cameraTilt, float cameraPan)
        //{
        //    xRot += cameraTilt;
        //    yRot += cameraPan;
        //    PlayerCamera.transform.localRotation = Quaternion.Euler(xRot, yRot, 0.0f);
        //    CameraTarget.transform.localRotation = Quaternion.Euler(CameraTarget.transform.localRotation.x, yRot, CameraTarget.transform.localRotation.z);
        //}

        //public void UpdateIsometricCamera(float cameraTilt, float cameraPan)
        //{
        //    xRot += cameraTilt;
        //    yRot += cameraPan;
        //    CameraPivot.transform.localRotation = Quaternion.Euler(xRot, yRot, 0.0f);
        //}
    }
}