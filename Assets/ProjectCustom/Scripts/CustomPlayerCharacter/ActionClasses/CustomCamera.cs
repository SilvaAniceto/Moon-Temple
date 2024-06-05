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
        public Transform CameraCloseAnchor { get; set; }
        public Vector3 CameraOfftset { get; set; }
        public Vector3 MaxOffset { get => new Vector3(0.45f, 0.0f, -1.6f); }
        public float CameraDistance { get; set; }
        #endregion

        #region CAMERA METHODS
        public void SetupCamera(LayerMask thirdPersonCollisionFilter, CustomCharacterController customController, float sensibility)
        {
            CustomPlayer.CameraLookDirection.AddListener(UpdateCameraLookDirection);
            CustomPlayer.CameraPositionAndOffset.AddListener(UpdateCameraPositionAndOffset);

            ThirdPersonCollisionFilter = thirdPersonCollisionFilter;

            CustomController = customController;
            CameraSensibility = sensibility;

            PanAxis = new GameObject("PanAxis").transform;
            PanAxis.SetParent(transform);
            PanAxis.localPosition = Vector3.zero;
            PanAxis.localRotation = Quaternion.Euler(0, 0, 0);

            TiltAxis = new GameObject("TiltAxis").transform;
            TiltAxis.SetParent(PanAxis);
            TiltAxis.localPosition = Vector3.up * 1.7f;
            TiltAxis.localRotation = Quaternion.Euler(0, 0, 0);

            CameraTarget = CustomPlayer.CharacterCamera.transform;
            CameraTarget.SetParent(TiltAxis);
        }
        private void UpdateCameraLookDirection(Vector2 cameraPT, Vector3 characterDirection, CustomCharacterController customController)
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
                pan += characterDirection.x;

                PanAxis.rotation = Mathf.Abs(characterDirection.x) > 0.45f ? Quaternion.Slerp(PanAxis.rotation, Quaternion.Euler(0, pan, 0), 0.2f) : PanAxis.rotation;
                TiltAxis.localRotation = TiltAxis.localRotation;

                return;
            }

            PanAxis.rotation = PanAxis.rotation;
            TiltAxis.localRotation = TiltAxis.localRotation;
        }
        private void UpdateCameraPositionAndOffset(Transform anchorReference, bool speedingUpAction, VerticalState verticalState)
        {
            transform.position = anchorReference.position;
            CameraTarget.LookAt(PanAxis.position + (PanAxis.forward * 25));
            CameraTarget.localEulerAngles = new Vector3(0, CameraTarget.localEulerAngles.y, 0);

            CameraDistance = speedingUpAction ? Mathf.Lerp(CameraDistance, 1.0f, Time.deltaTime * 3.5f) : Mathf.Lerp(CameraDistance, 0.7f, Time.deltaTime * 3.5f);

            Physics.SphereCast(TiltAxis.position, 0.2f, CameraTarget.position - TiltAxis.position, out RaycastHit hitInfo, CameraDistance * 1.05f, ThirdPersonCollisionFilter);

            CameraOfftset = Vector3.Lerp(Vector3.zero, MaxOffset, hitInfo.collider == null ? CameraDistance : Mathf.Clamp(hitInfo.distance, 0.25f, 1));

            CameraTarget.localPosition = CameraOfftset;
        }
        #endregion
    }
}