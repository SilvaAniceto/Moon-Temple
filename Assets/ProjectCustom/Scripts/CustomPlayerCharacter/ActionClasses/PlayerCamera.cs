using UnityEngine;

namespace CustomGameController
{
    [System.Serializable]
    public class PlayerCamera
    {
        [Range(0.1f, 1.0f)] public float m_cameraRotationSensibility;
        public LayerMask m_collisionFilter;

        private float m_cameraDistance;
        private float m_followPlayerDirectionDelayTime;

        private Transform m_cameraParent;
        private Transform m_cameraTarget;
        private Transform m_panAxis;
        private Transform m_tiltAxis;

        private Vector3 m_cameraOfftset;
        private Vector3 m_horizontalOffset = new Vector3(0.45f, 0.0f, -1.6f);
        private Vector3 m_verticalOffset;



        public void SetupCamera(Transform parentNestingReference, LayerMask thirdPersonCollisionFilter, float sensibility)
        {
            m_collisionFilter = thirdPersonCollisionFilter;

            m_cameraRotationSensibility = sensibility;

            m_cameraParent = new GameObject("CameraParent").transform;
            m_cameraParent.SetParent(parentNestingReference);
            m_cameraParent.localPosition = Vector3.zero;
            m_cameraParent.localRotation = Quaternion.Euler(0, 0, 0);

            m_panAxis = new GameObject("PanAxis").transform;
            m_panAxis.SetParent(m_cameraParent);
            m_panAxis.localPosition = Vector3.zero;
            m_panAxis.localRotation = Quaternion.Euler(0, 0, 0);

            m_tiltAxis = new GameObject("TiltAxis").transform;
            m_tiltAxis.SetParent(m_panAxis);
            m_tiltAxis.localPosition = Vector3.up * 1.7f;
            m_tiltAxis.localRotation = Quaternion.Euler(0, 0, 0);

            m_cameraTarget = PlayerCharacterController.CharacterCamera.transform;
            m_cameraTarget.SetParent(m_tiltAxis);
        }



        public void UpdateCameraLookDirection(Vector2 cameraPT, Vector3 characterDirection, bool speedingUpAction)
        {
            Vector2 lookDirection = new Vector3(cameraPT.x, cameraPT.y);

            lookDirection = lookDirection.normalized;

            if (lookDirection != Vector2.zero)
            {
                m_followPlayerDirectionDelayTime = 3.0f;

                if (Mathf.Abs(lookDirection.x * lookDirection.magnitude) > 0.65f)
                {
                    m_panAxis.localRotation = m_panAxis.localRotation * Quaternion.Euler(0, lookDirection.x * m_cameraRotationSensibility * Time.deltaTime * 100, 0);
                }

                if (Mathf.Abs(lookDirection.y * lookDirection.magnitude) > 0.65f)
                {
                    m_tiltAxis.localRotation = m_tiltAxis.localRotation * Quaternion.Euler(lookDirection.y * m_cameraRotationSensibility * Time.deltaTime * 100, 0, 0);
                    m_tiltAxis.localRotation = Quaternion.Euler(Mathf.Clamp(m_tiltAxis.localEulerAngles.x > 180 ? m_tiltAxis.localEulerAngles.x - 360 : m_tiltAxis.localEulerAngles.x, -50.0f, 70.0f), 0.0f, 0.0f);
                }

                return;
            }

            if (characterDirection != Vector3.zero && m_followPlayerDirectionDelayTime <= Time.fixedDeltaTime)
            {
                m_panAxis.localRotation = m_panAxis.localRotation * Quaternion.Euler(0, characterDirection.x * m_cameraRotationSensibility * Time.deltaTime * 100, 0);
                m_tiltAxis.localRotation = m_tiltAxis.localRotation;

                return;
            }

            m_followPlayerDirectionDelayTime = Mathf.Lerp(m_followPlayerDirectionDelayTime, 0.0f, Time.deltaTime);

            m_panAxis.rotation = m_panAxis.rotation;
            m_tiltAxis.localRotation = m_tiltAxis.localRotation;
        }

        

        public void UpdateCameraPositionAndOffset(Transform anchorReference, bool speedingUpAction, VerticalState verticalState)
        {
            m_cameraParent.position = anchorReference.position;
            m_cameraTarget.LookAt(m_panAxis.position + (m_panAxis.forward * 25.0f));
            m_cameraTarget.localEulerAngles = new Vector3(10.0f, m_cameraTarget.localEulerAngles.y, 0);

            m_cameraDistance = speedingUpAction ? Mathf.Lerp(m_cameraDistance, 1.0f, Time.deltaTime * 3.5f) : Mathf.Lerp(m_cameraDistance, 0.7f, Time.deltaTime * 3.5f);

            Physics.SphereCast(m_tiltAxis.position, 0.2f, m_cameraTarget.position - m_tiltAxis.position, out RaycastHit hitInfo, m_cameraDistance * 1.05f, m_collisionFilter);

            m_verticalOffset = verticalState == VerticalState.Jumping ? Vector3.Lerp(m_verticalOffset, Vector3.up * -0.8f + Vector3.forward * -1.0f, Time.deltaTime * 4.5f) : Vector3.Lerp(m_verticalOffset, Vector3.zero, Time.deltaTime * 4.5f);

            m_cameraOfftset = Vector3.Lerp(Vector3.zero, m_horizontalOffset + m_verticalOffset, hitInfo.collider == null ? m_cameraDistance : Mathf.Clamp(hitInfo.distance, 0.25f, 1));

            m_cameraTarget.localPosition = m_cameraOfftset;
        }
    }
}