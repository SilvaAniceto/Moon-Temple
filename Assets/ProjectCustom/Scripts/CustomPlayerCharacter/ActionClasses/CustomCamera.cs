using UnityEngine;

namespace CustomGameController
{
    public class CustomCamera : MonoBehaviour
    {
        public static CustomCamera Instance;

        #region PLAYER INPUTS SECTION
        public float CameraPan { get; set; }
        public float CameraTilt { get; set; }
        public void SetInput(CustomPlayerInputHandler inputs)
        {
            CameraPan = Mathf.Clamp(inputs.CameraAxis.y, -1, 1);
            CameraTilt = Mathf.Clamp(inputs.CameraAxis.x, -1, 1);
        }
        #endregion

        #region CAMERA PROPERTIES
        public Transform CameraTarget { get; set; }
        public float CameraTargetHeight { get; set; }
        public float CameraSensibility { get; set; }
        public LayerMask ThirdPersonCollisionFilter { get; set; }
        public CustomCharacterController CustomController { get; set; }
        public Vector3 CameraOfftset { get; set; }
        public Vector3 DefaultOfftset { get => Vector3.forward * -0.2f; }
        public Vector3 SprintOfftset { get => Vector3.forward * -0.8f; }
        public Vector3 SpeedFlightOfftset { get => new Vector3(-0.35f, 0.15f, -0.95f); }
        #endregion

        #region PRIVATE FIELDS
        private float m_xRot = 0;
        private float m_yRot = 0;

        private float angle;
        private float longitudinalThreshold;
        private float latitudinalThreshold;
        #endregion

        #region DEFAULT METHODS
        private void Awake()
        {
            if (Instance == null) Instance = this;
        }
        void Update()
        {
            UpdateCamera(CameraTilt, CameraPan);
        }
        private void LateUpdate()
        {
            transform.position = CustomController.ArchorReference.position;
            CameraTarget.localPosition = Vector3.zero + CameraOfftset;            
        }
        #endregion

        #region CAMERA METHODS
        public void SetupCamera(LayerMask thirdPersonCollisionFilter, CustomCharacterController customController, Transform cameraTarget, float targetHeight, float sensibility)
        {
            ThirdPersonCollisionFilter = thirdPersonCollisionFilter;

            CustomController = customController;
            CameraTarget = cameraTarget;
            CameraTargetHeight = targetHeight;
            CameraSensibility = sensibility;
        }
        public void UpdateCamera(float cameraTilt, float cameraPan)
        {
            Vector2 lookDirection = new Vector3(cameraTilt, cameraPan);

            lookDirection = lookDirection.normalized;

            float xRot = transform.localEulerAngles.x;
            float yRot = transform.localEulerAngles.y;

            yRot += lookDirection.y * (Mathf.Pow(5.0f, 2.0f) * CameraSensibility);
            xRot += lookDirection.x * (Mathf.Pow(5.0f, 2.0f) * CameraSensibility);

            xRot = xRot > 180 ? xRot - 360 : xRot;

            if (CustomController.VerticalState == VerticalState.InFlight)
            {
                CameraOfftset = Vector3.Lerp(CameraOfftset, SpeedFlightOfftset, Time.deltaTime);
            }
            else
            {
                CameraOfftset = Vector3.Lerp(CameraOfftset, DefaultOfftset, Time.deltaTime);
            }

            xRot = Mathf.Clamp(xRot, -50.0f, 70.0f);

            if (lookDirection == Vector2.zero && Mathf.Abs(CustomController.Input.x) > Mathf.Abs(CustomController.Input.z))
            {
                transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(transform.localEulerAngles.x, CustomController.transform.localEulerAngles.y, 0), Time.deltaTime);
            }
            else
            {
                transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(xRot, yRot, 0), 4.5f * Time.deltaTime);
            }

            CustomController.Forward = CustomPerspective.CustomForward;
            CustomController.Right = CustomPerspective.CustomRight;
        }
        #endregion

    }
}