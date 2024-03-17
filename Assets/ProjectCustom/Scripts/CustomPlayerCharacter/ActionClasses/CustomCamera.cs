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

            if (CustomController.CurrentInputState == PlayerInputState.FlightControll)
            {
                CameraOfftset = Vector3.Lerp(CameraOfftset, SpeedFlightOfftset, Time.deltaTime);

                //    float latitudinalOrientation = CustomController.transform.localEulerAngles.x > 180 ? CustomController.transform.localEulerAngles.x - 360 : CustomController.transform.localEulerAngles.x;
                //    float longitudinalOrientation = CustomController.transform.localEulerAngles.y > 180 ? CustomController.transform.localEulerAngles.y - 360 : CustomController.transform.localEulerAngles.y;

                //    if (latitudinalOrientation < -35.0f)
                //    {
                //        if (lookDirection != Vector2.zero)
                //        {
                //            if (lookDirection.x > 0.2f)
                //            {
                //                latitudinalThreshold = Mathf.Lerp(latitudinalThreshold, 125f, Time.deltaTime * 2.0f);
                //            }
                //            if (lookDirection.x < -0.2f)
                //            {
                //                latitudinalThreshold = Mathf.Lerp(latitudinalThreshold, 15.0f, Time.deltaTime * 2.0f);
                //            }
                //        }
                //        else
                //        {
                //            latitudinalThreshold = Mathf.Lerp(latitudinalThreshold, 40.0f, Time.deltaTime * 2.0f);
                //        }
                //        latitudinalOrientation += latitudinalThreshold;

                //    }
                //    else
                //    {
                //        if (lookDirection != Vector2.zero)
                //        {
                //            if (lookDirection.x > 0.2f)
                //            {
                //                latitudinalThreshold = Mathf.Lerp(latitudinalThreshold, -2.5f, Time.deltaTime * 2.0f);
                //            }
                //            if (lookDirection.x < -0.2f)
                //            {
                //                latitudinalThreshold = Mathf.Lerp(latitudinalThreshold, 20.0f, Time.deltaTime * 2.0f);
                //            }
                //        }
                //        else
                //        {
                //            latitudinalThreshold = Mathf.Lerp(latitudinalThreshold, 15.0f, Time.deltaTime * 2.0f);
                //        }
                //        latitudinalOrientation += latitudinalThreshold;
                //    }

                //    if (lookDirection != Vector2.zero)
                //    {
                //        if (lookDirection.y > 0.2f)
                //        {
                //            longitudinalThreshold = Mathf.Lerp(longitudinalThreshold, -20.0f, Time.deltaTime * 2.0f);
                //        }
                //        if (lookDirection.y < -0.2f)
                //        {
                //            longitudinalThreshold = Mathf.Lerp(longitudinalThreshold, Mathf.Clamp(longitudinalThreshold, 20.0f, 20.0f), Time.deltaTime * 2.0f);
                //        }
                //    }
                //    else
                //    {
                //        longitudinalThreshold = Mathf.Lerp(longitudinalThreshold, 0, Time.deltaTime * 2.0f);
                //    }

                //    longitudinalOrientation += longitudinalThreshold;
                //    transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(latitudinalOrientation, longitudinalOrientation, 0.0f), 4.5f * Time.deltaTime);

                //    m_xRot = 0.0f;
                //    m_yRot = CustomController.transform.localEulerAngles.y;
                //    return;
                xRot = CustomController.transform.localEulerAngles.x;

                xRot = xRot > 180 ? xRot - 360 : xRot;
                xRot = Mathf.Clamp(xRot, -5.0f, 70.0f);

                yRot = CustomController.transform.localEulerAngles.y;

                transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(xRot, yRot, 0.0f), 4.5f * Time.deltaTime);
                return;
            }

            CameraOfftset = Vector3.Lerp(CameraOfftset, DefaultOfftset, Time.deltaTime);

            xRot = Mathf.Clamp(xRot, -50.0f, 70.0f);

            transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(xRot, yRot, 0), 4.5f * Time.deltaTime);

            CustomController.Forward = CustomPerspective.CustomForward;
            CustomController.Right = CustomPerspective.CustomRight;
        }
        #endregion

    }
}