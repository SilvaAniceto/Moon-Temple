using UnityEngine;

namespace CustomGameController
{
    public class CustomCamera : MonoBehaviour
    {
        #region CAMERA PROPERTIES
        public Transform CameraTarget { get; set; }
        public Transform PanAxis { get; set; }
        public Transform TiltAxis { get; set; }
        public float CameraSensibility { get; set; }
        public LayerMask ThirdPersonCollisionFilter { get; set; }
        public CustomCharacterController CustomController { get; set; }
        public Vector3 CameraOfftset { get; set; }
        public Vector3 DefaultOfftset { get => Vector3.forward * -0.2f; }
        public Vector3 JumpOffset { get => (Vector3.up * -0.8f); }
        public Vector3 SprintOfftset { get => Vector3.forward * -0.8f; }
        public Vector3 FlightOfftset { get => new Vector3(-0.35f, -0.5f, -1.25f); }
        public Vector3 SpeedFlightOfftset { get => new Vector3(-0.35f, -0.5f, -2.5f); }
        public float VerticalLocalEuler { get => transform.localEulerAngles.y > 180 ? transform.localEulerAngles.y - 360 : transform.localEulerAngles.y; }
        #endregion

        #region CAMERA METHODS
        public void SetupCamera(LayerMask thirdPersonCollisionFilter, CustomCharacterController customController, float sensibility)
        {
            CustomPlayer.CameraLookDirection.AddListener(UpdateCameraLookDirection);
            CustomPlayer.CameraPositionAndOffset.AddListener(UpdateCameraPositionAndOffset);

            ThirdPersonCollisionFilter = thirdPersonCollisionFilter;

            CustomController = customController;
            CameraSensibility = sensibility;

            PanAxis = new GameObject("HorizontalAxis").transform;
            PanAxis.SetParent(transform);
            PanAxis.localPosition = Vector3.zero;
            PanAxis.localRotation = Quaternion.Euler(0, 0, 0);

            TiltAxis = new GameObject("VerticalAxis").transform;
            TiltAxis.SetParent(PanAxis);
            TiltAxis.localPosition = Vector3.up * 1.7f;
            TiltAxis.localRotation = Quaternion.Euler(0, 0, 0);

            CameraTarget = CustomPlayer.CharacterCamera.transform;
            CameraTarget.SetParent(TiltAxis);
            CameraTarget.localPosition = Vector3.forward * -1.2f + Vector3.right * 0.45f;
        }
        private void UpdateCameraLookDirection(Vector2 cameraPT, Vector3 characterDirection, CustomCharacterController customController, float directionVerticalDeltaRotation)
        {
            Vector2 lookDirection = new Vector3(cameraPT.x, cameraPT.y);

            lookDirection = lookDirection.normalized;

            float pan = PanAxis.localEulerAngles.y;
            float tilt = TiltAxis.localEulerAngles.x;

            pan += lookDirection.x * CameraSensibility;
            tilt -= lookDirection.y * CameraSensibility;

            tilt = tilt > 180 ? tilt - 360 : tilt;
            tilt = Mathf.Clamp(tilt, -50.0f, 70.0f);

            if (lookDirection != Vector2.zero)
            {
                if (Mathf.Abs(lookDirection.x * lookDirection.magnitude) > 0.45f)
                {
                    PanAxis.localEulerAngles = Vector3.up * pan;
                }

                if (Mathf.Abs(lookDirection.y * lookDirection.magnitude) > 0.125f)
                {
                    TiltAxis.localEulerAngles = Vector3.right * tilt;
                }

                return;
            }

            if (characterDirection != Vector3.zero)
            {
                pan -= characterDirection.x;

                PanAxis.rotation = Mathf.Abs(characterDirection.x) > 0.45f ? Quaternion.Slerp(PanAxis.rotation, Quaternion.Euler(0, pan, 0), 0.0009f) : PanAxis.rotation;
                TiltAxis.localRotation = TiltAxis.localRotation;

                return;
            }

            PanAxis.rotation = PanAxis.rotation;
            TiltAxis.localRotation = TiltAxis.localRotation;
        }
        private void UpdateCameraPositionAndOffset(Transform anchorReference, bool speedingUpAction, VerticalState verticalState)
        {
            //if (verticalState == VerticalState.Flighting)
            //{
            //    if (speedingUpAction)
            //    {
            //        CameraOfftset = Vector3.Lerp(CameraOfftset, SpeedFlightOfftset, Time.deltaTime * 4.5f);
            //    }
            //    else
            //    {
            //        CameraOfftset = Vector3.Lerp(CameraOfftset, FlightOfftset, Time.deltaTime * 4.5f);
            //    }
            //}
            //else if (verticalState == VerticalState.Jumping)
            //{
            //    if (speedingUpAction)
            //    {
            //        CameraOfftset = Vector3.Lerp(CameraOfftset, SprintOfftset + JumpOffset, Time.deltaTime * 4.5f);
            //    }
            //    else
            //    {
            //        CameraOfftset = Vector3.Lerp(CameraOfftset, DefaultOfftset + JumpOffset, Time.deltaTime * 4.5f);
            //    }
            //}
            //else
            //{
            //    if (speedingUpAction)
            //    {
            //        CameraOfftset = Vector3.Lerp(CameraOfftset, SprintOfftset, Time.deltaTime * 4.5f);
            //    }
            //    else
            //    {
            //        CameraOfftset = Vector3.Lerp(CameraOfftset, DefaultOfftset, Time.deltaTime * 4.5f);
            //    }
            //}

            transform.position = anchorReference.position;
            //CameraTarget.localPosition = Vector3.zero + CameraOfftset;
        }
        #endregion
    }
}