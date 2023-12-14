using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.XR;
using UnityEngine.ProBuilder;

namespace CustomGameController
{
    [RequireComponent(typeof(CharacterController))]

    public class CustomCharacterController : MonoBehaviour, ICustomPlayerController, ICustomGravity, ICustomAirController
    {
        public static CustomCharacterController Instance { get; private set; }

        [HideInInspector] public UnityEvent<GameControllerState> OnGameStateChanged = new UnityEvent<GameControllerState>();

        [HideInInspector] public UnityEvent<Vector3, float> OnCharacterMove = new UnityEvent<Vector3, float>();

        [HideInInspector] public UnityEvent<bool> OnVerticalControl = new UnityEvent<bool>();

        [HideInInspector] public UnityEvent CharacterPhysicsSimulation = new UnityEvent();
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

            SetCharacterPhysicsSimulation(ApplyGravity);

            CurrentyVelocity = CurrentyVelocity / Drag * 0.85f;
            GravityMultiplierFactor = 1.0f;
            Falling = false;
            Jumping = false;
            AllowJump = false;
            StartJump = false;
            FlightControlling = false;
            InFlight = false;

            SetCharacterMoveCallBacks(CustomCamera.Instance.CameraPerspective);

            yield return new WaitForSeconds(0.35f);
            AllowJump = true;
        }

        #region SIMULATED PHYSICS PROPERTIES
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
        public float Gravity { get => 9.81f; }
        public float GravityMultiplierFactor { get; set; }
        public float Drag { get; set; }
        public LayerMask GroundLayer { get; set; }
        public Vector3 GravityVelocity { get; set; }
        #endregion

        #region AIR SIMULATION PROPERTIES
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
        #endregion

        #region GAME CONTROLLER PROPERTIES
        public GameControllerState ControllerState { get => m_state; set { m_state = value; OnGameStateChanged?.Invoke(ControllerState); } }
        public Vector3 TurnBasedTargetPosition { get; set; }
        public Vector3 TurnBasedTargetDirection { get; set; }
        public float TurnBasedDistanceTravelled { get; set; }
        public bool TurnBasedMovementStarted { get; set; }
        #endregion

        #region PLAYER CONTROLLER PROPERTIES
        public CharacterController CharacterController { get; set; }
        public Animator CharacterAnimator { get => GetComponent<Animator>(); }
        public Transform WallCheckOrigin { get; set; }
        public Vector3 Forward { get; set; }
        public Vector3 Right { get; set; }
        public Vector3 CurrentyVelocity { get; set; }
        public bool AllowJump { get; set; }
        public bool StartJump { get; set; }
        public float DelayedStopTime { get; set; }
        #endregion

        #region PLAYER CONTROLLER SETTINGS
        public float CurrentSpeed
        {
            get
            {
                if (OnGround && !InFlight) return OnGroundSpeed;
                else if (!OnGround && !InFlight) return OnAirSpeed;
                else return InFlightSpeed;
            }
        }
        public float CurrentAcceleration
        {
            get
            {
                if (OnGround && !InFlight) return OnGroundAcceleration;
                else if (!OnGround && !InFlight) return OnAirAcceleration;
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
        public float OnAirSpeed
        {
            get
            {
                return (OnGroundSpeed * 0.8f);
            }
        }
        public float OnAirAcceleration { get => OnGroundAcceleration * 0.8f; }
        public float MaxSlopeAngle { get; set; }
        public float JumpHeight { get; set; }
        public float JumpSpeed { get => Mathf.Sqrt(2.0f * JumpHeight * Gravity); }
        #endregion

        #region SIMULATED PHYSICS METHODS
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
        public void CheckGroundLevel()
        {
            bool ground;

            ground = Physics.CheckSphere(GroundCheckOrigin.position, CharacterController.radius, GroundLayer, QueryTriggerInteraction.Collide);

            OnGround = ground;

            if (ground && GravityVelocity.y < -CharacterController.radius)
            {
                GravityVelocity = new Vector3(GravityVelocity.x, 0.0f, GravityVelocity.z);
            }
            
            if (ground)LastGroundedPosition = transform.localPosition;
        }
        #endregion

        #region GAME CONTROLLER METHODS
        public void OnGameControllerStateChanged(GameControllerState state)
        {
            //m_cursor.position = new Vector3(transform.position.x, transform.parent.position.y, transform.position.z);
            //CharacterController.Move(Vector3.zero);
            //switch (state)
            //{
            //    case GameControllerState.Exploring:
            //        m_cursor.gameObject.SetActive(false);
            //        CharacterController.enabled = true;
            //        break;
            //    case GameControllerState.TurnBased:
            //        m_cursor.gameObject.SetActive(true);
            //        CharacterController.enabled = false;
            //        break;
            //}
        }
        public void SetCharacterPhysicsSimulation(UnityAction actionSimulated)
        {
            CharacterPhysicsSimulation.RemoveAllListeners();
            CharacterPhysicsSimulation.AddListener(actionSimulated);
        }
        public void SetupCharacter()
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

            SetCharacterPhysicsSimulation(ApplyGravity);
            OnVerticalControl.AddListener(Jump);

            StartCoroutine("CheckingUngroundedStates");
            GravityMultiplierFactor = 1.0f;

            OnGameStateChanged.AddListener(OnGameControllerStateChanged);

            SetSlopeCheckSystem(SlopeCheckCount, CharacterController.radius);
        }
        public bool CheckWallHit()
        {
            return Physics.Raycast(WallCheckOrigin.position, transform.forward, CharacterController.radius + 0.1f, GroundLayer, QueryTriggerInteraction.Collide);
        }
        #endregion

        #region PLAYER SLOPE PROPERTIES
        public int SlopeCheckCount { get; set; }
        public List<Transform> SlopeCheckList { get; set; }
        #endregion

        #region PLAYER SLOPE METHODS
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

        #region AIR SIMULATION METHODS
        public void EnteringAirState()
        {
            float yVelocity = 0.0f;

            yVelocity = Mathf.Lerp(yVelocity, JumpHeight / 2.0f, Time.deltaTime * 4.0f);

            GravityVelocity = new Vector3(0.0f, yVelocity, 0.0f);
            CharacterController.Move(GravityVelocity);
        }
        public void IdleAirState()
        {
            //float y = GravityVelocity.y;
            //y = 0.25f * Mathf.Sin(Time.time * 9.81f);
            //GravityVelocity = new Vector3(0.0f, y, 0.0f);
            //CharacterController.Move(GravityVelocity * Time.deltaTime);
        }
        public void UpdateFlightPosition(Vector3 inputDirection, float movementSpeed)
        {
            Vector3 move = new Vector3();
            Vector3 right = inputDirection.x * Right;
            Vector3 forward = inputDirection.z * Forward;

            move = right + forward + Vector3.zero;

            CurrentyVelocity = Vector3.MoveTowards(CurrentyVelocity, move, CurrentAcceleration * Time.deltaTime * CurrentSpeed);

            CharacterController.Move(CurrentyVelocity * Time.deltaTime * movementSpeed);

            Quaternion Rot = CustomCamera.Instance.CameraTarget.transform.rotation;

            Rot.x = 0.0f;
            Rot.z = 0.0f;

            transform.rotation = Rot;

            //if (move != Vector3.zero && SprintInput) transform.rotation = Quaternion.FromToRotation(transform.up, forward) * transform.rotation;

            //UpdateAirHeightPosition();
        }

        public void UpdateAirHeightPosition()
        {

        }
        #endregion

        #region PLAYER JUMP, MOVEMENT & ANIMATION METHODS
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
        public void UpdateThirdPersonMovePosition(Vector3 inputDirection, float movementSpeed)
        {
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
        public void Animate()
        {
            if (CheckWallHit()) CharacterAnimator.SetFloat("MoveSpeed", 0.0f, 0.1f, Time.deltaTime);
            else CharacterAnimator.SetFloat("MoveSpeed", Mathf.Clamp(SprintInput ? CurrentyVelocity.magnitude * 2.0f : CurrentyVelocity.magnitude, 0.0f, 2.0f), 0.1f, Time.deltaTime);

            CharacterAnimator.SetBool("Jumping", StartJump);
            CharacterAnimator.SetBool("OnGround", onGround);
            CharacterAnimator.SetBool("Falling", Falling);
        }
        #endregion

        #region PLAYER INPUT VALUES & METHODS
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

                OnCharacterMove?.Invoke(PlayerDirection, CurrentSpeed);

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
        public bool SprintInput { get; set; }
        public bool VerticalAscendingInput
        {
            get
            {
                return m_VerticalAscendingInput;
            }
            set
            {
                if (FlightControlling) return;

                m_VerticalAscendingInput = value;

                OnVerticalControl?.Invoke(m_VerticalAscendingInput);
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
                        GravityVelocity = Vector3.zero;
                        SetCharacterPhysicsSimulation(ApplyGravity);
                    }
                    else
                    {
                        SetCharacterPhysicsSimulation(IdleAirState);
                    }
                }
                else
                {
                    if (m_flightControlling)
                    {
                        StopCoroutine(StartFlightDelay());
                        StartCoroutine(StartFlightDelay());
                        SetCharacterPhysicsSimulation(EnteringAirState);
                    }
                }

                IEnumerator StartFlightDelay()
                {
                    yield return new WaitUntil(() => transform.localPosition.y >= LastGroundedPosition.y + JumpHeight / 2.0f);

                    InFlight = true;
                    SetCharacterPhysicsSimulation(IdleAirState);
                    SetCharacterMoveCallBacks(CustomCamera.Instance.CameraPerspective);
                }
            }
        }
        public void SetInput(CustomPlayerInputHandler inputs)
        {
            PlayerDirection = inputs.MoveDirectionInput;
            PlayerDirection.Normalize();

            SprintInput = inputs.SprintInput;
            VerticalAscendingInput = inputs.VerticalAscendingInput;
            VerticalDescendingInput = inputs.VerticalDescendingInput;

            if (inputs.AirControlling) FlightControlling = !FlightControlling;
        }
        #endregion

        #region PRIVATE FIELDS
        private bool onGround;
        private GameControllerState m_state;

        private float m_OnGroundSpeed;
        private float m_InFlightSpeed;

        private Vector3 m_PlayerDirection;
        private bool m_VerticalAscendingInput;
        private bool m_flightControlling;
        #endregion

        #region DEFAULT METHODS
        public float yDir;
        void Update()
        {
            CharacterPhysicsSimulation?.Invoke();
            yDir = CustomCamera.Instance.VerticalCameraDirection;
            Animate();
        }
        void FixedUpdate()
        {
            CheckGroundLevel();
        }
        #endregion

        #region CALLBACKS
        public void SetCharacterMoveCallBacks(CameraPerspective cameraPerspective)
        {
            OnCharacterMove.RemoveAllListeners();

            if (InFlight)
            {
                OnCharacterMove.AddListener(UpdateFlightPosition);
            }
            else
            {
                if (cameraPerspective == CameraPerspective.First_Person) OnCharacterMove.AddListener(UpdateFirstPersonMovePosition);
                else OnCharacterMove.AddListener(UpdateThirdPersonMovePosition);
            }
        }
        #endregion
    }
}
