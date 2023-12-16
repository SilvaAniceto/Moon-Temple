using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace CustomGameController
{
    [RequireComponent(typeof(CharacterController))]

    public class CustomCharacterController : MonoBehaviour, ICustomPlayerController, ICustomGravity, ICustomAirController
    {
        public static CustomCharacterController Instance { get; private set; }

        [HideInInspector] public UnityEvent<Vector3, float> OnPlayerDirection = new UnityEvent<Vector3, float>();

        [HideInInspector] public UnityEvent<bool> OnPlayerJump = new UnityEvent<bool>();

        [HideInInspector] public UnityEvent PlayerPhysicsSimulation = new UnityEvent();
        IEnumerator CheckingUngroundedStates()
        {
            yield return new WaitForEndOfFrame();

            CurrentUngroundedPosition = transform.localPosition;

            if (Mathf.Round(LastGroundedPosition.y * 100.0f) / 100.0f > Mathf.Round(CurrentUngroundedPosition.y * 100.0f) / 100.0f)
            {
                GravityMultiplierFactor = 2.2f;
                Falling = true;
            }

            if (!InFlight && !FlightControlling)
            {
                if (Mathf.Round(LastGroundedPosition.y * 100.0f) / 100.0f < Mathf.Round(CurrentUngroundedPosition.y * 100.0f) / 100.0f)
                {
                    GravityMultiplierFactor = 1.2f;
                    Jumping = true;
                }
            }

            yield return new WaitUntil(() => onGround);

            SetPlayerPhysicsSimulation(ApplyGravity);

            CurrentyVelocity = CurrentyVelocity / Drag * 0.85f;
            GravityMultiplierFactor = 1.0f;
            Falling = false;
            Jumping = false;
            AllowJump = false;
            StartJump = false;
            FlightControlling = false;
            InFlight = false;

            SetPlayerMovevement(CustomCamera.Instance.CameraPerspective);

            yield return new WaitForSeconds(0.35f);
            AllowJump = true;
        }

        #region PRIVATE FIELDS
        private bool onGround;

        private float m_OnGroundSpeed;
        private float m_InFlightSpeed;

        private Vector3 m_PlayerDirection;
        private bool m_JumpInput;
        private bool m_VerticalAscendingInput;
        private bool m_flightControlling;
        #endregion

        #region DEFAULT METHODS
        void Update()
        {
            PlayerPhysicsSimulation?.Invoke();

            Animate();

            UpdateFlightPosition(PlayerDirection, CurrentSpeed);
        }
        void FixedUpdate()
        {
            CheckGroundLevel();
        }
        #endregion

        #region PLAYER PROPERTIES
        public CharacterController CharacterController { get; set; }
        public Animator CharacterAnimator { get => GetComponent<Animator>(); }
        #endregion

        #region PLAYER SETUPS
        public void SetupPlayer()
        {
            if (Instance == null) Instance = this;

            if (CharacterController == null)
            {
                CharacterController = gameObject.GetComponent<CharacterController>();
                CharacterController.slopeLimit = MaxSlopeAngle;
            }

            GameObject t = new GameObject("~GroundCheckOrigin");
            GroundCheckOrigin = t.transform;
            GroundCheckOrigin.transform.SetParent(transform);
            GroundCheckOrigin.transform.localPosition = Vector3.zero;

            t = new GameObject("~WallCheckOrigin");
            WallCheckOrigin = t.transform;
            WallCheckOrigin.transform.SetParent(transform);
            WallCheckOrigin.transform.localPosition = new Vector3(0.0f, CharacterController.height / 2.0f, 0.0f);

            SetPlayerPhysicsSimulation(ApplyGravity);
            OnPlayerJump.AddListener(Jump);

            StartCoroutine("CheckingUngroundedStates");
            GravityMultiplierFactor = 1.0f;

            SetSlopeCheckSystem(SlopeCheckCount, CharacterController.radius);
        }
        public void SetPlayerPhysicsSimulation(UnityAction actionSimulated)
        {
            PlayerPhysicsSimulation.RemoveAllListeners();
            if (actionSimulated != null) PlayerPhysicsSimulation.AddListener(actionSimulated);
        }
        public void SetPlayerMovevement(CameraPerspective cameraPerspective)
        {
            OnPlayerDirection.RemoveAllListeners();

            if (cameraPerspective == CameraPerspective.First_Person) OnPlayerDirection.AddListener(UpdateFirstPersonMovePosition);
            else OnPlayerDirection.AddListener(UpdateThirdPersonMovePosition);
        }
        #endregion

        #region PLAYER GRAVITY
        public float Gravity { get => 9.81f; }
        public float GravityMultiplierFactor { get; set; }
        public float Drag { get; set; }
        public Vector3 GravityVelocity { get; set; }

        public float ApplyDrag(float velocity, float drag)
        {
            return velocity * (1 / (1 + drag * Time.deltaTime));
        }
        public void ApplyGravity()
        {
            GravityVelocity -= new Vector3(0.0f, Gravity * GravityMultiplierFactor * Time.deltaTime, 0.0f);

            if (SlopeAngle() <= MaxSlopeAngle)
                CharacterController.Move(GravityVelocity * Time.deltaTime);
            else
                CharacterController.Move(GetSlopeMoveDirection(GravityVelocity + SlopeHit().transform.forward) * Time.deltaTime * OnGroundSpeed);
        }
        #endregion

        #region PLAYER GROUND DETECTION
        public Vector3 LastGroundedPosition { get; set; }
        public Vector3 CurrentUngroundedPosition { get; set; }
        public Transform GroundCheckOrigin { get; set; }
        public bool OnGround
        {
            get => onGround;
            set
            {
                if (onGround == value) return;

                onGround = value;

                if (!onGround && SlopeHit().collider == null)
                {
                    StopCoroutine("CheckingUngroundedStates");
                    StartCoroutine("CheckingUngroundedStates");
                }
            }
        }
        public bool Jumping { get; set; }
        public bool Falling { get; set; }
        public LayerMask GroundLayer { get; set; }

        public void CheckGroundLevel()
        {
            bool ground;

            ground = Physics.CheckSphere(GroundCheckOrigin.position, CharacterController.radius, GroundLayer, QueryTriggerInteraction.Collide);

            OnGround = ground;

            if (ground && GravityVelocity.y < -CharacterController.radius)
            {
                GravityVelocity = new Vector3(GravityVelocity.x, 0.0f, GravityVelocity.z);
            }

            if (ground) LastGroundedPosition = transform.localPosition;
        }
        #endregion

        #region PLAYER SLOPE DETECTION
        public float MaxSlopeAngle { get; set; }
        public int SlopeCheckCount { get; set; }
        public List<Transform> SlopeCheckList { get; set; }

        public void SetSlopeCheckSystem(int checkCount, float radius)
        {
            SlopeCheckList = new List<Transform>();

            for (int i = 0; i < checkCount; i++)
            {
                GameObject obj = new GameObject("~SlopeOrigin");
                Transform t = obj.transform;
                t.SetParent(transform);
                t.localPosition = Vector3.zero;

                SlopeCheckList.Add(t);

                float circunferenceProgress = (float)i / checkCount;

                float currentRadian = circunferenceProgress * 2 * Mathf.PI;

                float xScaled = Mathf.Cos(currentRadian);
                float zScaled = Mathf.Sin(currentRadian);

                float x = xScaled * radius;
                float z = zScaled * radius;

                Vector3 currentPosition = new Vector3(t.localPosition.x + z, (CharacterController.height / 2) - 0.2f, t.localPosition.z + x);

                t.localPosition = currentPosition;
            }
        }
        public bool OnSlope()
        {
            if (!OnGround) return false;

            foreach (Transform t in SlopeCheckList)
            {
                if (Physics.Raycast(t.position, Vector3.down, out RaycastHit hitInfo, CharacterController.height / 2 + 0.5f, GroundLayer, QueryTriggerInteraction.Collide))
                {
                    return hitInfo.normal != Vector3.up;
                }
            }
            return false;
        }
        public float SlopeAngle()
        {
            if (!OnGround) return 0;

            foreach (Transform t in SlopeCheckList)
            {
                if (Physics.Raycast(t.position, Vector3.down, out RaycastHit hitInfo, CharacterController.height / 2 + 0.5f, GroundLayer, QueryTriggerInteraction.Collide))
                {
                    float slopeAngle = Mathf.RoundToInt(Vector3.Angle(Vector3.up, hitInfo.normal));
                    return slopeAngle;
                }
            }
            return 0;
        }
        public RaycastHit SlopeHit()
        {
            if (!OnGround) return new RaycastHit();

            foreach (Transform t in SlopeCheckList)
            {
                Physics.Raycast(t.position, Vector3.down, out RaycastHit hitInfo, CharacterController.height / 2, GroundLayer, QueryTriggerInteraction.Collide);

                if (hitInfo.collider != null) return hitInfo;
            }
            return new RaycastHit();
        }
        public Vector3 GetSlopeMoveDirection(Vector3 direction)
        {
            return Vector3.ProjectOnPlane(new Vector3(direction.x, (int)(direction.y * (int)SlopeAngle()), direction.z), SlopeHit().normal).normalized;
        }
        #endregion

        #region PLAYER INPUT HANDLER
        public void SetInput(CustomPlayerInputHandler inputs)
        {
            PlayerDirection = inputs.MoveDirectionInput;
            PlayerDirection.Normalize();

            SprintInput = inputs.SprintInput;
            JumpInput = inputs.JumpInput;
            VerticalAscendingInput = inputs.VerticalAscendingInput;
            VerticalDescendingInput = inputs.VerticalDescendingInput;

            if (inputs.AirControlling) FlightControlling = !FlightControlling;
        }
        #endregion

        #region PLAYER MOVEMENT
        public Transform WallCheckOrigin { get; set; }
        public Vector3 PlayerDirection
        {
            get
            {
                return m_PlayerDirection;
            }
            set
            {
                m_PlayerDirection = value;

                if (m_PlayerDirection == Vector3.zero) StartCoroutine(SmoothStop());

                if (InFlight) return;

                OnPlayerDirection?.Invoke(PlayerDirection, CurrentSpeed);

                IEnumerator SmoothStop()
                {
                    SprintInput = false;

                    DelayedStopTime = Vector3.Distance(CurrentyVelocity, Vector3.zero);

                    while (DelayedStopTime > 0)
                    {
                        CharacterController.Move(Vector3.MoveTowards(CurrentyVelocity, m_PlayerDirection, 1.0f));
                        DelayedStopTime -= Time.deltaTime / CurrentSpeed;
                    }

                    CurrentyVelocity = Vector3.zero;

                    yield return null;
                }
            }
        }
        public Vector3 Forward { get; set; }
        public Vector3 Right { get; set; }
        public Vector3 CurrentyVelocity { get; set; }
        public float DelayedStopTime { get; set; }
        public float CurrentSpeed
        {
            get
            {
                if (OnGround && !InFlight) return OnGroundSpeed;
                else if (!OnGround && !InFlight) return UngroundSpeed;
                else return InFlightSpeed;
            }
        }
        public float CurrentAcceleration
        {
            get
            {
                if (OnGround && !InFlight) return OnGroundAcceleration;
                else if (!OnGround && !InFlight) return UngroundAcceleration;
                else return InFlightAcceleration;
            }
        }
        public float OnGroundSpeed
        {
            get
            {
                if (SprintInput) return m_OnGroundSpeed * 2.8f;
                else return m_OnGroundSpeed;
            }
            set
            {
                if (m_OnGroundSpeed == value) return;

                m_OnGroundSpeed = value;
            }
        }
        public float OnGroundAcceleration { get; set; }
        public float UngroundSpeed
        {
            get
            {
                return (OnGroundSpeed * 0.8f);
            }
        }
        public float UngroundAcceleration { get => OnGroundAcceleration * 0.8f; }
        public bool SprintInput { get; set; }

        public bool CheckWallHit()
        {
            return Physics.Raycast(WallCheckOrigin.position, transform.forward, CharacterController.radius + 0.1f, GroundLayer, QueryTriggerInteraction.Collide);
        }
        public void UpdateThirdPersonMovePosition(Vector3 inputDirection, float movementSpeed)
        {
            if (InFlight) return;

            inputDirection = inputDirection.normalized;

            Vector3 move = new Vector3();
            Vector3 right = inputDirection.x * Right;
            Vector3 forward = inputDirection.z * Forward;

            move = right + forward + Vector3.zero;

            if (move != Vector3.zero)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(move), 4.5f * Time.deltaTime);
            }

            if (CheckWallHit())
            {
                CharacterController.Move(Vector3.zero);
                return;
            }

            if (OnGround)
            {
                CurrentyVelocity = Vector3.MoveTowards(CurrentyVelocity, move, CurrentAcceleration * Time.deltaTime);

                if (OnSlope())
                {
                    if (SlopeAngle() <= MaxSlopeAngle)
                    {
                        CharacterController.Move(GetSlopeMoveDirection(CurrentyVelocity) * Time.deltaTime * movementSpeed);
                    }
                    else
                    {
                        CharacterController.Move(Vector3.zero * Time.deltaTime * movementSpeed * 0.5f);
                    }
                }
                else
                    CharacterController.Move(CurrentyVelocity * Time.deltaTime * movementSpeed);
            }
            else
            {
                CurrentyVelocity = Vector3.MoveTowards(CurrentyVelocity, move, CurrentAcceleration * Time.deltaTime);

                float x = ApplyDrag(CurrentyVelocity.x, Drag);
                float z = ApplyDrag(CurrentyVelocity.z, Drag);

                CurrentyVelocity = new Vector3(x, 0, z);

                CharacterController.Move(CurrentyVelocity * Time.deltaTime * movementSpeed);
            }
        }
        public void UpdateFirstPersonMovePosition(Vector3 inputDirection, float movementSpeed)
        {
            inputDirection = inputDirection.normalized;

            Vector3 move = new Vector3();
            Vector3 right = inputDirection.x * Right;
            Vector3 forward = inputDirection.z * Forward;

            move = right + forward + Vector3.zero;

            if (OnGround)
            {
                CurrentyVelocity = Vector3.MoveTowards(CurrentyVelocity, move, CurrentAcceleration * Time.deltaTime);

                if (OnSlope())
                {
                    if (SlopeAngle() <= MaxSlopeAngle)
                    {
                        CharacterController.Move(GetSlopeMoveDirection(CurrentyVelocity) * Time.deltaTime * movementSpeed);
                    }
                    else
                    {
                        CharacterController.Move(Vector3.zero * Time.deltaTime * movementSpeed * 0.5f);
                    }
                }
                else
                    CharacterController.Move(CurrentyVelocity * Time.deltaTime * movementSpeed);
            }
            else
            {
                CurrentyVelocity = Vector3.MoveTowards(CurrentyVelocity, move, CurrentAcceleration * Time.deltaTime);

                float x = ApplyDrag(CurrentyVelocity.x, Drag);
                float z = ApplyDrag(CurrentyVelocity.z, Drag);

                CurrentyVelocity = new Vector3(x, 0, z);

                CharacterController.Move(CurrentyVelocity * Time.deltaTime * movementSpeed);
            }

            Quaternion Rot = CustomCamera.Instance.CameraTarget.transform.rotation;

            Rot.x = 0.0f;
            Rot.z = 0.0f;

            transform.rotation = Rot;
        }
        #endregion

        #region PLAYER JUMP
        public float JumpHeight { get; set; }
        public float JumpSpeed { get => Mathf.Sqrt(2.0f * JumpHeight * Gravity); }
        public bool JumpInput
        {
            get
            {
                return m_JumpInput;
            }
            set
            {
                if (InFlight) return;

                m_JumpInput = value;

                OnPlayerJump?.Invoke(m_JumpInput);
            }
        }
        public bool AllowJump { get; set; }
        public bool StartJump { get; set; }

        public void Jump(bool jumpInput)
        {
            if (jumpInput)
            {
                if (AllowJump)
                {
                    StartJump = true;

                    float yVelocity = Mathf.Lerp(transform.position.y, JumpSpeed, 0.8f);

                    GravityVelocity = new Vector3(0.0f, yVelocity, 0.0f);

                    AllowJump = false;
                }
            }

            CharacterController.Move(GravityVelocity * Time.deltaTime);
        }
        #endregion

        #region PLAYER FLIGHT
        public bool InFlight { get; set; }
        public float InFlightSpeed
        {
            get
            {
                if (SprintInput) return m_InFlightSpeed * 4.0f;
                else return m_InFlightSpeed;
            }
            set
            {
                if (m_InFlightSpeed == value) return;

                m_InFlightSpeed = value;
            }
        }
        public float InFlightAcceleration { get; set; }
        public Vector3 FlightVelocity { get; set; }
        public bool VerticalAscendingInput
        {
            get
            {
                return m_VerticalAscendingInput;
            }
            set
            {
                m_VerticalAscendingInput = value;
            }
        }
        public bool VerticalDescendingInput { get; set; }
        public bool FlightControlling
        {
            get
            {
                return m_flightControlling;
            }
            set
            {
                if (m_flightControlling == value) return;

                m_flightControlling = value;

                if (InFlight)
                {
                    if (!m_flightControlling)
                    {
                        InFlight = false;
                        GravityVelocity = Vector3.zero;
                        SetPlayerPhysicsSimulation(ApplyGravity);
                    }
                }
                else
                {
                    if (m_flightControlling)
                    {
                        StopCoroutine(StartFlightDelay());
                        StartCoroutine(StartFlightDelay());
                        SetPlayerPhysicsSimulation(EnteringFlightState);
                    }
                }

                IEnumerator StartFlightDelay()
                {
                    yield return new WaitUntil(() => transform.localPosition.y >= LastGroundedPosition.y + JumpHeight / 2.0f);

                    InFlight = true;
                    StartJump = false;
                    Jumping = false;
                    Falling = false;
                    CurrentUngroundedPosition = Vector3.zero;
                    LastGroundedPosition = Vector3.zero;
                    SetPlayerMovevement(CustomCamera.Instance.CameraPerspective);
                    SetPlayerPhysicsSimulation(null);
                }
            }
        }

        public void EnteringFlightState()
        {
            float yVelocity = 0.0f;

            yVelocity = Mathf.Lerp(yVelocity, JumpHeight / 2.0f, Time.deltaTime * 4.0f);

            GravityVelocity = new Vector3(0.0f, yVelocity, 0.0f);
            CharacterController.Move(GravityVelocity);
        }
        public void UpdateFlightPosition(Vector3 inputDirection, float movementSpeed)
        {
            if (!InFlight) return;

            if (!SprintInput)
            {
                GravityVelocity = Vector3.zero;

                UpdateFlightHeightPosition(VerticalAscendingInput, VerticalDescendingInput);

                Vector3 move = new Vector3();
                Vector3 right = inputDirection.x * Right;
                Vector3 forward = inputDirection.z * Forward;

                move = right + forward + Vector3.zero;

                CurrentyVelocity = Vector3.MoveTowards(CurrentyVelocity, move, CurrentAcceleration * Time.deltaTime);

                CharacterController.Move(CurrentyVelocity * Time.deltaTime * movementSpeed);

                CharacterController.Move(GravityVelocity * Time.deltaTime);

            }
            else
            {
                Vector3 move = new Vector3();
                Vector3 forward = 1.0f * transform.forward;
                Vector3 right = inputDirection.x * transform.right;
                Vector3 up = -inputDirection.z * Vector3.up;

                move = right + up + Vector3.zero;

                FlightVelocity = Vector3.MoveTowards(FlightVelocity, forward, CurrentAcceleration * Time.deltaTime);

                if (move != Vector3.zero)
                {
                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(move), Time.deltaTime);
                }

                CharacterController.Move(FlightVelocity * Time.deltaTime * movementSpeed /** 8*/);

            }
            Quaternion Rot = CustomCamera.Instance.CameraTarget.transform.rotation;

            Rot.x = transform.rotation.x;
            Rot.z = 0.0f;

            transform.rotation = Rot;
        }
        public void UpdateFlightHeightPosition(bool ascendingFlight, bool descendingFlight)
        {
            if (ascendingFlight)
            {
                float y = GravityVelocity.y;
                y = Mathf.Lerp(y, JumpHeight * 9.81f, Time.deltaTime * 4.0f);
                GravityVelocity = new Vector3(0.0f, y, 0.0f);
            }
            else if (descendingFlight)
            {
                float y = GravityVelocity.y;
                y = Mathf.Lerp(y, -JumpHeight * 9.81f, Time.deltaTime * 4.0f);
                GravityVelocity = new Vector3(0.0f, y, 0.0f);
            }
        }
        #endregion

        #region PLAYER ANIMATION
        public void Animate()
        {
            if (CheckWallHit()) CharacterAnimator.SetFloat("MoveSpeed", 0.0f, 0.1f, Time.deltaTime);
            else CharacterAnimator.SetFloat("MoveSpeed", Mathf.Clamp(SprintInput ? CurrentyVelocity.magnitude * 2.0f : CurrentyVelocity.magnitude, 0.0f, 2.0f), 0.1f, Time.deltaTime);

            CharacterAnimator.SetBool("Jumping", StartJump);
            CharacterAnimator.SetBool("OnGround", onGround);
            CharacterAnimator.SetBool("Falling", Falling);
        }
        #endregion 
    }
}
