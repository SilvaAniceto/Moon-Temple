using UnityEngine;

namespace CustomGameController
{
    public class CustomCamera : MonoBehaviour
    {
        #region CAMERA PROPERTIES
        public Transform CameraTarget { get; set; }
        public float CameraSensibility { get; set; }
        public LayerMask ThirdPersonCollisionFilter { get; set; }
        public CustomCharacterController CustomController { get; set; }
        public Vector3 CameraOfftset { get; set; }
        public Vector3 DefaultOfftset { get => Vector3.forward * -0.2f; }
        public Vector3 SprintOfftset { get => Vector3.forward * -0.8f; }
        public Vector3 FlightOfftset { get => new Vector3(-0.35f, -0.5f, -1.25f); }
        public Vector3 SpeedFlightOfftset { get => new Vector3(-0.35f, -0.5f, -2.5f); }
        public float VerticalLocalEuler { get => transform.localEulerAngles.y > 180 ? transform.localEulerAngles.y - 360 : transform.localEulerAngles.y; }
        #endregion

        #region CAMERA METHODS
        public void SetupCamera(LayerMask thirdPersonCollisionFilter, CustomCharacterController customController, Transform cameraTarget, float sensibility)
        {
            CustomPlayer.CameraLookDirection.AddListener(UpdateCameraLookDirection);
            CustomPlayer.CameraPositionAndOffset.AddListener(UpdateCameraPositionAndOffset);

            ThirdPersonCollisionFilter = thirdPersonCollisionFilter;

            CustomController = customController;
            CameraTarget = cameraTarget;
            CameraSensibility = sensibility;
        }
        private void UpdateCameraLookDirection(Vector2 cameraPT, Vector3 characterDirection, CustomCharacterController customController, float directionVerticalDeltaRotation)
        {
            Vector2 lookDirection = new Vector3(cameraPT.x, cameraPT.y);

            lookDirection = lookDirection.normalized;

            float xRot = transform.localEulerAngles.x;
            float yRot = transform.localEulerAngles.y;

            yRot += lookDirection.y * (Mathf.Pow(5.0f, 2.0f) * CameraSensibility);
            xRot += lookDirection.x * (Mathf.Pow(5.0f, 2.0f) * CameraSensibility);

            if (customController.VerticalState == VerticalState.Flighting && customController.SpeedingUpAction)
            {
                xRot = xRot > 180 ? xRot - 360 : xRot;
                yRot = yRot > 180 ? yRot - 360 : yRot;

                xRot = Mathf.Clamp(xRot, -50.0f, 70.0f);
                yRot = Mathf.Clamp(yRot, -15.0f, 15.0f);

                transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(xRot, customController.transform.localEulerAngles.y, 0), 4.5f * Time.deltaTime);

                return;
            }
            else
            {
                xRot = xRot > 180 ? xRot - 360 : xRot;

                xRot = Mathf.Clamp(xRot, -50.0f, 70.0f);
            }

            if (lookDirection == Vector2.zero && Mathf.Abs(characterDirection.x) > Mathf.Abs(characterDirection.z))
            {
                transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(transform.localEulerAngles.x, customController.transform.localEulerAngles.y, 0), Time.deltaTime);
            }
            else
            {
                transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(xRot, yRot, 0), 4.5f * Time.deltaTime);
            }
        }
        private void UpdateCameraPositionAndOffset(Transform anchorReference, bool speedingUpAction, VerticalState verticalState)
        {
            if (verticalState == VerticalState.Flighting)
            {
                if (speedingUpAction)
                {
                    CameraOfftset = Vector3.Lerp(CameraOfftset, SpeedFlightOfftset, Time.deltaTime * 4.5f);
                }
                else
                {
                    CameraOfftset = Vector3.Lerp(CameraOfftset, FlightOfftset, Time.deltaTime * 4.5f);
                }
            }
            else
            {
                if (speedingUpAction)
                {
                    CameraOfftset = Vector3.Lerp(CameraOfftset, SprintOfftset, Time.deltaTime * 4.5f);
                }
                else
                {
                    CameraOfftset = Vector3.Lerp(CameraOfftset, DefaultOfftset, Time.deltaTime * 4.5f);
                }
            }

            transform.position = anchorReference.position;
            CameraTarget.localPosition = Vector3.zero + CameraOfftset;
        }
        #endregion
    }
}