using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CustomGameController
{
    public enum VerticalState
    {
        Idle,
        Jumping,
        Falling,
        Flighting
    }

    [System.Serializable]
    public class PlayerCharacter
    {
        public LayerMask m_groundLayer;
        public LayerMask m_wallLayer;

        [HideInInspector] public CharacterController m_characterController;
        [HideInInspector] public Animator m_characterAnimator;
        [HideInInspector] public bool m_onGround;
        
        private float m_currentSpeed;
        private bool m_speedingUpAction;
        private Vector3 m_currentyVelocity;
        private bool m_allowJump;
        private Vector3 m_gravity = Vector3.up * 9.81f;
        private Vector3 m_gravityVelocity;
        private float m_drag = 1.4f;
        private float m_maxSlopeAngle = 45;
        private int m_slopeCheckCount = 6;
        private List<Transform> m_slopeCheckList;
        private Transform m_wallCheckOrigin;
        private Transform m_groundCheckOrigin;
        private Transform m_archorReference;
        private Vector3 m_previousPosition;

        public VerticalState VerticalState
        {
            get
            {
                if (!CheckGroundLevel())
                {
                    if (m_characterController.transform.position.y - m_previousPosition.y < -0.02f) return VerticalState.Falling;
                    else if (m_characterController.transform.position.y - m_previousPosition.y > 0.02f) return VerticalState.Jumping;
                    else if (m_characterController.transform.position.y - m_previousPosition.y == 0.0f) return VerticalState.Idle;
                }

                return VerticalState.Idle;
            }
        }
        public bool SpeedingUpAction 
        {
            set
            {
                m_currentSpeed = value ? BaseSpeed * 3.6f : BaseSpeed;

                if (m_speedingUpAction == value) return;

                m_speedingUpAction = value;
            } 
        }
        private Vector3 CameraForward
        {
            get
            {
                Vector3 forward = PlayerCharacterController.CharacterCamera.transform.forward;
                forward.y = 0;
                forward = Vector3.Normalize(forward);

                return forward;
            }
        }
        private Vector3 CameraRight
        {
            get
            {
                return PlayerCharacterController.CharacterCamera.transform.right;
            }
        }
        private float BaseSpeed
        {
            get => 0.415f * m_characterController.height * 3.2f;
        }
        private Vector3 CharacterAnchorPosition { get => new Vector3(m_characterController.bounds.center.x, m_characterController.bounds.center.y - m_characterController.bounds.extents.y, m_characterController.bounds.center.z); }
        private float GravityMultiplierFactor
        {
            get
            {
                if (VerticalState == VerticalState.Jumping) return 0.9f;
                else if (VerticalState == VerticalState.Falling) return 2.2f;
                return 1.0f;
            }
        }
        private float JumpSpeed { get => Mathf.Sqrt(2.0f * m_characterController.height * 0.4f * m_gravity.y); }



        public void Jump(bool jumpInput)
        {
            if (jumpInput)
            {
                if (m_allowJump)
                {
                    m_gravityVelocity = new Vector3(0.0f, JumpSpeed, 0.0f);

                    m_allowJump = false;
                }
            }

            m_characterController.Move(m_gravityVelocity * Time.deltaTime);
        }



        public void SetupPlayerCharacter()
        {
            if (m_characterController != null)
            {
                m_characterController.slopeLimit = m_maxSlopeAngle;
            }

            GameObject t = new GameObject("~GroundCheckOrigin");
            m_groundCheckOrigin = t.transform;
            m_groundCheckOrigin.transform.SetParent(m_characterController.transform);

            t = new GameObject("~WallCheckOrigin");
            m_wallCheckOrigin = t.transform;
            m_wallCheckOrigin.transform.SetParent(m_characterController.transform);

            t = new GameObject("~ArchorReference");
            m_archorReference = t.transform;
            m_archorReference.transform.SetParent(m_characterController.transform);

            SetSlopeCheckTransform(m_slopeCheckCount, m_characterController.radius);
        }



        public void SetCheckersLocation()
        {
            m_groundCheckOrigin.position = CharacterAnchorPosition;

            m_archorReference.position = CharacterAnchorPosition;
            m_archorReference.localRotation = m_characterController.transform.localRotation;

            m_wallCheckOrigin.position = m_characterController.bounds.center;
        }



        private float ApplyDrag(float velocity, float drag)
        {
            return velocity * (1 / (1 + drag * Time.deltaTime));
        }



        public void ApplyGravity()
        {
            m_gravityVelocity -= m_gravity * GravityMultiplierFactor * Time.deltaTime;

            if (VerticalState == VerticalState.Flighting) m_gravityVelocity = new Vector3(0.0f, Mathf.Clamp(m_gravityVelocity.y, -m_gravity.y * 2.0f, 0.0f), 0.0f);

            if (SlopeAngle() <= m_characterController.slopeLimit)
                m_characterController.Move(m_gravityVelocity * Time.deltaTime);
            else
                m_characterController.Move(GetSlopeMoveDirection(m_gravityVelocity + SlopeHit().transform.forward) * Time.deltaTime * m_currentSpeed);
        }



        public void UpdateHorizontalPositionAndDirection(Vector3 inputDirection)
        {
            inputDirection = inputDirection.normalized;

            Vector3 right = inputDirection.x * CameraRight;
            Vector3 forward = inputDirection.z * CameraForward;

            Vector3 move = right + forward + Vector3.zero;

            if (move != Vector3.zero) m_characterController.transform.rotation = Quaternion.Slerp(m_characterController.transform.rotation, Quaternion.LookRotation(move), Time.deltaTime * 4.5f);

            if (CheckWallHit())
            {
                m_characterController.Move(Vector3.zero);
                return;
            }

            if (CheckGroundLevel())
            {
                m_currentyVelocity = Vector3.MoveTowards(m_currentyVelocity, move, 2.0f * Time.deltaTime);

                if (OnSlope())
                {
                    if (SlopeAngle() <= m_characterController.slopeLimit)
                    {
                        m_characterController.Move(GetSlopeMoveDirection(m_currentyVelocity) * Time.deltaTime * m_currentSpeed);
                    }
                    else
                    {
                        m_characterController.Move(Vector3.zero * Time.deltaTime * m_currentSpeed * 0.5f);
                    }
                }
                else
                    m_characterController.Move(m_currentyVelocity * Time.deltaTime * m_currentSpeed);
            }
            else
            {
                m_currentyVelocity = Vector3.MoveTowards(m_currentyVelocity, move, 2.0f * Time.deltaTime);

                float x = ApplyDrag(m_currentyVelocity.x, m_drag);
                float z = ApplyDrag(m_currentyVelocity.z, m_drag);

                m_currentyVelocity = new Vector3(x, 0, z);

                m_characterController.Move(m_currentyVelocity * Time.deltaTime * m_currentSpeed);
            }
        }



        public IEnumerator SmoothStop()
        {
            float delayedStopTime = Vector3.Distance(m_currentyVelocity, Vector3.zero);

            while (delayedStopTime > 0)
            {
                m_characterController.Move(Vector3.MoveTowards(m_currentyVelocity, Vector3.zero, 1.0f));
                delayedStopTime -= Time.deltaTime / m_currentSpeed;
            }

            m_currentyVelocity = Vector3.zero;

            yield return null;
        }



        public bool CheckGroundLevel()
        {
            bool ground;

            ground = Physics.CheckSphere(m_groundCheckOrigin.position, m_characterController.radius, m_groundLayer, QueryTriggerInteraction.Collide);

            if (ground && m_gravityVelocity.y < 0.0f)
            {
                m_gravityVelocity = Vector3.zero;
            }

            return ground;
        }



        public IEnumerator CheckingUngroundedStates()
        {
            m_previousPosition = m_characterController.transform.position;

            m_allowJump = false;

            yield return new WaitUntil(() => m_onGround);

            m_currentyVelocity = m_currentyVelocity / m_drag * 0.85f;

            yield return new WaitForSeconds(Time.fixedDeltaTime);
            m_allowJump = true;
        }



        private void SetSlopeCheckTransform(int checkCount, float radius)
        {
            m_slopeCheckList = new List<Transform>();

            for (int i = 0; i < checkCount; i++)
            {
                Transform t = new GameObject("~SlopeDetectionOrigin").transform;
                t.SetParent(m_characterController.transform);
                t.localPosition = Vector3.zero;

                m_slopeCheckList.Add(t);

                float circunferenceProgress = (float)i / checkCount;

                float currentRadian = circunferenceProgress * 2 * Mathf.PI;

                float xScaled = Mathf.Cos(currentRadian);
                float zScaled = Mathf.Sin(currentRadian);

                float x = xScaled * radius;
                float z = zScaled * radius;

                Vector3 currentPosition = new Vector3(t.localPosition.x + z, (m_characterController.height / 2) - 0.2f, t.localPosition.z + x);

                t.localPosition = currentPosition;
            }
        }



        private bool OnSlope()
        {
            if (!CheckGroundLevel()) return false;

            foreach (Transform t in m_slopeCheckList)
            {
                if (Physics.Raycast(t.position, Vector3.down, out RaycastHit hitInfo, m_characterController.height / 2 + 0.5f, m_groundLayer, QueryTriggerInteraction.Collide))
                {
                    return hitInfo.normal != Vector3.up;
                }
            }
            return false;
        }



        private float SlopeAngle()
        {
            if (!CheckGroundLevel()) return 0;

            foreach (Transform t in m_slopeCheckList)
            {
                if (Physics.Raycast(t.position, Vector3.down, out RaycastHit hitInfo, m_characterController.height / 2 + 0.5f, m_groundLayer, QueryTriggerInteraction.Collide))
                {
                    float slopeAngle = Mathf.RoundToInt(Vector3.Angle(Vector3.up, hitInfo.normal));
                    return slopeAngle;
                }
            }
            return 0;
        }



        public RaycastHit SlopeHit()
        {
            if (!CheckGroundLevel()) return new RaycastHit();

            foreach (Transform t in m_slopeCheckList)
            {
                Physics.Raycast(t.position, Vector3.down, out RaycastHit hitInfo, m_characterController.height / 2, m_groundLayer, QueryTriggerInteraction.Collide);

                if (hitInfo.collider != null) return hitInfo;
            }
            return new RaycastHit();
        }



        private Vector3 GetSlopeMoveDirection(Vector3 direction)
        {
            return Vector3.ProjectOnPlane(new Vector3(direction.x, (int)(direction.y * (int)SlopeAngle()), direction.z), SlopeHit().normal).normalized;
        }



        private bool CheckWallHit()
        {
            return Physics.SphereCast(m_wallCheckOrigin.position, m_characterController.radius * 0.2f,  m_characterController.transform.forward, out RaycastHit hitInfo,m_characterController.radius * 1.25f, m_wallLayer, QueryTriggerInteraction.Collide);
        }



        public void Animate()
        {
            if (CheckWallHit()) m_characterAnimator.SetFloat("MoveSpeed", 0.0f, 0.1f, Time.deltaTime);
            else m_characterAnimator.SetFloat("MoveSpeed", m_currentyVelocity.magnitude * (m_currentSpeed / BaseSpeed), 0.1f, Time.deltaTime);

            m_characterAnimator.SetBool("Jumping", VerticalState == VerticalState.Jumping);
            m_characterAnimator.SetBool("OnGround", VerticalState == VerticalState.Idle);
            m_characterAnimator.SetBool("Falling", VerticalState == VerticalState.Falling);
        }
    }
}
