using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace CustomGameController
{
    public struct CustomPlayerInputHandler
    {
        public Vector3 MoveDirectionInput;
        public bool VerticalActionInput;
        public bool SpeedUpInput;

        public Vector2 CameraAxis;

        public bool ActionTypeInput;

        public bool ChooseFlightInput;
    }

    public enum PlayerInputState
    {
        ActionType,
        GroundMove,
        FlightControll
    }

    public enum VerticalState
    {
        Idle,
        Jumping,
        Falling,
        InFlight
    }

    [RequireComponent(typeof(CharacterController))]

    public class CustomCharacterController : MonoBehaviour
    {
        [HideInInspector] public UnityEvent<Vector3, float> OnCharacterDirection = new UnityEvent<Vector3, float>();

        [HideInInspector] public UnityEvent<bool> OnVerticalAction = new UnityEvent<bool>();

        [HideInInspector] public UnityEvent<bool, float> OnSpeedUpAction = new UnityEvent<bool, float>();

        [HideInInspector] public UnityEvent PlayerPhysicsSimulation = new UnityEvent();

        #region PLAYER INPUTS SECTION
        public Vector3 CharacterDirection
        {
            set
            {
                if (value == Vector3.zero ) StartCoroutine(SmoothStop());

                OnCharacterDirection?.Invoke(value.normalized, CurrentSpeed);

                IEnumerator SmoothStop()
                {
                    float delayedStopTime = Vector3.Distance(CurrentyVelocity, Vector3.zero);

                    while (delayedStopTime > 0)
                    {
                        CharacterController.Move(Vector3.MoveTowards(CurrentyVelocity, value, 1.0f));
                        delayedStopTime -= Time.deltaTime / CurrentSpeed;
                    }

                    CurrentyVelocity = Vector3.zero;

                    yield return null;
                }
            }
        }
        public bool SpeedUpAction
        {
            set
            {
                OnSpeedUpAction?.Invoke(value, BaseSpeed);
            }
        }
        public bool VerticalAction
        {
            set
            {
                OnVerticalAction?.Invoke(value);
            }
        }

        public void SetInput(CustomPlayerInputHandler inputs)
        {
            CharacterDirection = inputs.MoveDirectionInput;
            SpeedUpAction = inputs.SpeedUpInput;
            VerticalAction = inputs.VerticalActionInput;
        }
        #endregion

        #region CHARACTER MOVEMENT
        public Vector3 Forward { get; set; }
        public Vector3 Right { get; set; }
        public Vector3 CurrentyVelocity { get; set; }

        public void UpdateCharacterSpeed(bool speedAction, float speed)
        {
            if (speedAction)
            {
                if (OnGround || !InFlight)
                {
                    SpeedFlight = false;
                    CurrentSpeed = speed * 3.6f;
                    return;
                }
                if (Gliding)
                {
                    CurrentSpeed = speed * 4.8f;
                    SpeedFlight = true;
                    return;
                }
            }
            else
            {
                if (OnGround || !InFlight)
                {
                    SpeedFlight = false;
                    CurrentSpeed = speed;
                    return;
                }
                if (InFlight)
                {
                    SpeedFlight = false;
                    CurrentSpeed = speed * 3.6f;
                    return;
                }
            }
        }
        public void UpdateThirdPersonMovePosition(Vector3 inputDirection, float movementSpeed)
        {
            if (VerticalState == VerticalState.InFlight) return;

            inputDirection = inputDirection.normalized;

            Vector3 move = new Vector3();
            Vector3 right = inputDirection.x * Right;
            Vector3 forward = inputDirection.z * Forward;

            move = right + forward + Vector3.zero;

            if (move != Vector3.zero) transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(move), 4.5f * Time.deltaTime);

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
        //public void UpdateFirstPersonMovePosition(Vector3 inputDirection, float movementSpeed)
        //{
        //    inputDirection = inputDirection.normalized;

        //    Vector3 move = new Vector3();
        //    Vector3 right = inputDirection.x * Right;
        //    Vector3 forward = inputDirection.z * Forward;

        //    move = right + forward + Vector3.zero;

        //    if (OnGround)
        //    {
        //        CurrentyVelocity = Vector3.MoveTowards(CurrentyVelocity, move, CurrentAcceleration * Time.deltaTime);

        //        if (OnSlope())
        //        {
        //            if (SlopeAngle() <= MaxSlopeAngle)
        //            {
        //                CharacterController.Move(GetSlopeMoveDirection(CurrentyVelocity) * Time.deltaTime * movementSpeed);
        //            }
        //            else
        //            {
        //                CharacterController.Move(Vector3.zero * Time.deltaTime * movementSpeed * 0.5f);
        //            }
        //        }
        //        else
        //            CharacterController.Move(CurrentyVelocity * Time.deltaTime * movementSpeed);
        //    }
        //    else
        //    {
        //        CurrentyVelocity = Vector3.MoveTowards(CurrentyVelocity, move, CurrentAcceleration * Time.deltaTime);

        //        float x = ApplyDrag(CurrentyVelocity.x, Drag);
        //        float z = ApplyDrag(CurrentyVelocity.z, Drag);

        //        CurrentyVelocity = new Vector3(x, 0, z);

        //        CharacterController.Move(CurrentyVelocity * Time.deltaTime * movementSpeed);
        //    }

        //    Quaternion Rot = CustomCamera.Instance.CameraTarget.transform.rotation;

        //    Rot.x = 0.0f;
        //    Rot.z = 0.0f;

        //    transform.rotation = Rot;
        //}
        #endregion

        #region CHARACTER GRAVITY
        public Vector3 Gravity { get => Vector3.up * 9.81f; }
        public float GravityMultiplierFactor 
        { 
            get
            {
                if (!OnGround && !InFlight)
                {
                    if (VerticalState == VerticalState.Jumping) return 1.2f;
                    if (VerticalState == VerticalState.Falling) return 2.2f;
                    return 1.0f;
                }
                if (!OnGround && InFlight)
                {
                    if (Gliding) return 0.1f;

                    return 1.2f;
                }

                return 0.0f;
            }
        }
        public float Drag { get => 1.4f; }
        public Vector3 GravityVelocity { get; set; }
                
        public float ApplyDrag(float velocity, float drag)
        {
            return velocity * (1 / (1 + drag * Time.deltaTime));
        }
        public void ApplyGravity()
        {
            GravityVelocity -= Gravity * GravityMultiplierFactor * Time.deltaTime;

            if (SlopeAngle() <= MaxSlopeAngle)
                CharacterController.Move(GravityVelocity * Time.deltaTime);
            else
                CharacterController.Move(GetSlopeMoveDirection(GravityVelocity + SlopeHit().transform.forward) * Time.deltaTime * CurrentSpeed);
        }
        #endregion

        #region CHARACTER FLIGHT
        public Vector3 FlightVelocity { get; set; }
        public Transform ForwardFlightDirectionArchor { get; set; }
        public bool InFlight { get; set; }
        public float MaximunFlightHeight { get => JumpSpeed * 1.4f; }

        private float flightHeight;
        public float FlightHeight
        {
            get
            {
                return flightHeight;
            }
            set
            {
                if (OnGround) flightHeight = value;

                if (flightHeight < 0.8f && value < 1.0f)
                {
                    flightHeight = value;
                    Gliding = false;
                }
                else if (flightHeight >= 0.8f && !Gliding)
                {
                    Gliding = true;
                }
                else if (value <= 0.05f && Gliding)
                {
                    flightHeight = value;
                    Gliding = false;
                }
            }
        }
        public float FlightGravityDirection { get; set; }
        public float FlightForwardSpeedFactor { get; set; }
        public bool Gliding { get; set; }
        public bool SpeedFlight { get; set; }

        public void CheckEnterFlightState(bool value)
        {
            if (value)
            {
                InFlight = true;
                OnCharacterDirection.RemoveAllListeners();
                OnCharacterDirection.AddListener(UpdateFlightPosition);
                OnVerticalAction.RemoveAllListeners();
                OnVerticalAction.AddListener(UpdateFlightHeightPosition);
            }
        }
        public void UpdateFlightPosition(Vector3 inputDirection, float movementSpeed)
        {
            inputDirection = inputDirection.normalized;

            //float xRot = transform.eulerAngles.x;
            //float yRot = transform.eulerAngles.y;

            //xRot += inputDirection.z * Mathf.Pow(BaseSpeed, 3.0f) * Mathf.Clamp(InverseFlightSpeedFactor, 0.6f, 0.7f);
            //yRot += inputDirection.x * Mathf.Pow(BaseSpeed, 3.0f) /** Mathf.Clamp(InverseFlightSpeedFactor, 0.6f, 0.7f)*/;

            //xRot = xRot > 180 ? xRot - 360 : xRot;
            //xRot = Mathf.Clamp(xRot, -25.0f, 70.0f);

            //Vector3 lookDirection = new Vector3(0.0f , yRot, 0.0f);

            //transform.rotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(lookDirection), UngroundAcceleration * Time.deltaTime);
            Vector3 move = new Vector3();
            Vector3 right = inputDirection.x * transform.right;
            Vector3 forward = inputDirection.z * transform.forward;

            move = right + forward + Vector3.zero;

            if (move != Vector3.zero) transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(move), Time.deltaTime / CurrentAcceleration);

            CurrentyVelocity = Vector3.MoveTowards(CurrentyVelocity, move, CurrentAcceleration * Time.deltaTime);

            CharacterController.Move(CurrentyVelocity * Time.deltaTime * movementSpeed);
        }
        public void UpdateFlightHeightPosition(bool verticalAction)
        {
            if (!Gliding)
            {
                if (verticalAction)
                {
                    InFlight = true;
                    float flightHeight = Mathf.Clamp(Mathf.Round(Mathf.InverseLerp(PreviousPosition.y, PreviousPosition.y + MaximunFlightHeight, CurrentPosition.y) * 100.0f) / 100, 0.0f, 1.0f);
                    FlightHeight = flightHeight;
                    FlightGravityDirection = Mathf.Lerp(Gravity.y, 0.0f, flightHeight);
                }
                else
                {
                    InFlight = false;
                    FlightGravityDirection = 0.0f;
                }
            }
            else
            {
                float flightHeight = Mathf.Clamp(Mathf.Round(Mathf.InverseLerp(PreviousPosition.y + MaximunFlightHeight, CurrentPosition.y, CurrentPosition.y - PreviousPosition.y) * 100.0f) / 100, 0.0f, 1.0f);
                FlightHeight = flightHeight;

                if (verticalAction)
                {
                    InFlight = true;
                }
                else
                {
                    InFlight = false;
                }
            }

            Vector3 move = new Vector3();
            Vector3 up = FlightGravityDirection * Vector3.up;

            move = up + new Vector3(FlightVelocity.x, 0.0f, FlightVelocity.z);

            FlightVelocity = Vector3.MoveTowards(FlightVelocity, move, Time.deltaTime * Gravity.y);

            CharacterController.Move(FlightVelocity * Gravity.y * Time.deltaTime);
        }
        #endregion

        #region CHARACTER JUMP
        public float JumpHeight { get => CharacterController.height * 0.8f; }
        public float JumpSpeed { get => Mathf.Sqrt(2.0f * JumpHeight * Gravity.y); }

        public bool AllowJump { get; set; }
        public bool StartJump { get; set; }

        public void Jump(bool jumpInput)
        {
            if (jumpInput)
            {
                if (AllowJump)
                {
                    StartJump = true;

                    GravityVelocity = new Vector3(0.0f, JumpSpeed, 0.0f);

                    AllowJump = false;
                }
            }

            CharacterController.Move(GravityVelocity * Time.deltaTime);
        }
        #endregion

        #region PRIVATE CHARACTER PROPERTIES
        private CharacterController CharacterController { get => GetComponent<CharacterController>(); }
        private Vector3 CharacterAnchorPosition { get => new Vector3(CharacterController.bounds.center.x, CharacterController.bounds.center.y - CharacterController.bounds.extents.y, CharacterController.bounds.center.z); }
        private Vector3 CharacterCenterPosition {  get =>  CharacterController.bounds.center; }
        public Transform ArchorReference { get; private set; }
        private Animator CharacterAnimator { get => GetComponent<Animator>(); }
        #endregion

        #region CHARACTER MOVEMENT PROPERTIES
        public float BaseSpeed
        {
            get => 0.415f * CharacterController.height * 3.2f;
        }
        public float BaseAcceleration { get => 2.0f; }
        public float UngroundAcceleration { get => BaseAcceleration * 0.8f; }
        public float InFlightAcceleration { get => BaseAcceleration * 2.5f; }
        public float CurrentSpeed { get; set; }
        public float CurrentAcceleration
        {
            get
            {
                if (OnGround && !InFlight) return BaseAcceleration;
                else if (!OnGround && !InFlight) return UngroundAcceleration;
                else return InFlightAcceleration;
            }
        }
        #endregion

        #region DEFAULT METHODS
        void Update()
        {
            PlayerPhysicsSimulation?.Invoke();

            CheckGroundLevel();

            SetCheckersLocation();

            Animate();
        }
        #endregion

        #region CHARACTER SETUPS
        public void SetupCharacter(LayerMask groundLayer)
        {
            if (CharacterController == null)
            { 
                CharacterController.slopeLimit = MaxSlopeAngle;
            }

            GroundLayer = groundLayer;

            GameObject t = new GameObject("~GroundCheckOrigin");
            GroundCheckOrigin = t.transform;
            GroundCheckOrigin.transform.SetParent(transform);

            t = new GameObject("~WallCheckOrigin");
            WallCheckOrigin = t.transform;
            WallCheckOrigin.transform.SetParent(transform);

            t = new GameObject("~ArchorReference");
            ArchorReference = t.transform;
            ArchorReference.transform.SetParent(transform);

            t = new GameObject("~ForwardFlightDirectionArchor");
            ForwardFlightDirectionArchor = t.transform;
            ForwardFlightDirectionArchor.transform.SetParent(transform.parent);

            SetPlayerPhysicsSimulation(ApplyGravity);

            OnSpeedUpAction.AddListener(UpdateCharacterSpeed);

            StartCoroutine("CheckingUngroundedStates");

            SetSlopeCheckSystem(SlopeCheckCount, CharacterController.radius);
        }
        public void SetPlayerPhysicsSimulation(UnityAction actionSimulated)
        {
            PlayerPhysicsSimulation.RemoveAllListeners();
            if (actionSimulated != null) PlayerPhysicsSimulation.AddListener(actionSimulated);
        }
        public void SetCheckersLocation()
        {
            GroundCheckOrigin.position = CharacterAnchorPosition;

            ArchorReference.position = CharacterAnchorPosition;
            ArchorReference.localRotation = transform.localRotation;

            WallCheckOrigin.position = CharacterCenterPosition;

            ForwardFlightDirectionArchor.position = CharacterAnchorPosition;
            ForwardFlightDirectionArchor.localRotation = Quaternion.Euler(new Vector3(0.0f, transform.localEulerAngles.y, 0.0f));
        }
        #endregion

        #region CHARACTER GROUND DETECTION
        private bool onGround;
        public bool OnGround
        {
            get => onGround;
            set
            {
                if (value)
                {
                    InFlight = false;
                    Gliding = false;
                    FlightHeight = 0.0f;
                    FlightVelocity = Vector3.zero;
                }

                if (onGround == value) return;

                onGround = value;

                if (!onGround && SlopeHit().collider == null)
                {
                    StopCoroutine("CheckingUngroundedStates");
                    StartCoroutine("CheckingUngroundedStates");
                }
            }
        }
        public Transform GroundCheckOrigin { get; set; }
        public LayerMask GroundLayer { get; set; }
        public float GroundDistance
        {
            get
            {
                Physics.Raycast(transform.localPosition, Vector3.down, out RaycastHit hitInfo, 600, GroundLayer);

                return Mathf.Round(hitInfo.distance * 100.0f) / 100.0f;
            }
        }

        public void CheckGroundLevel()
        {
            bool ground;

            ground = Physics.CheckSphere(GroundCheckOrigin.position, CharacterController.radius, GroundLayer, QueryTriggerInteraction.Collide);

            OnGround = ground;

            if (ground && GravityVelocity.y < 0.0f)
            {
                GravityVelocity = Vector3.zero;
            }
        }
        IEnumerator CheckingUngroundedStates()
        {
            PreviousPosition = transform.position;

            AllowJump = false;
            StartJump = false;

            OnVerticalAction.AddListener(CheckEnterFlightState);            

            yield return new WaitUntil(() => OnGround);

            FlightGravityDirection = 0.0f;
            FlightForwardSpeedFactor = 0.0f;

            CurrentyVelocity = CurrentyVelocity / Drag * 0.85f;

            OnVerticalAction.RemoveAllListeners();
            OnVerticalAction.AddListener(Jump);
            OnCharacterDirection.RemoveAllListeners();
            OnCharacterDirection.AddListener(UpdateThirdPersonMovePosition);

            yield return new WaitForSeconds(Time.fixedDeltaTime);
            AllowJump = true;
        }
        #endregion

        #region CHARACTER SLOPE DETECTION
        public float MaxSlopeAngle { get => 45.0f; }
        public int SlopeCheckCount { get => 6; }
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

        #region CHARACTER WALL DETECTION
        public Transform WallCheckOrigin { get; set; }

        public bool CheckWallHit()
        {
            return Physics.Raycast(WallCheckOrigin.position, transform.forward, CharacterController.radius + 0.1f, GroundLayer, QueryTriggerInteraction.Collide);
        }
        #endregion

        #region PLAYER ANIMATION
        public void Animate()
        {
            if (CheckWallHit()) CharacterAnimator.SetFloat("MoveSpeed", 0.0f, 0.1f, Time.deltaTime);
            else CharacterAnimator.SetFloat("MoveSpeed", CurrentyVelocity.magnitude * (CurrentSpeed / BaseSpeed), 0.1f, Time.deltaTime);

            CharacterAnimator.SetBool("Jumping", VerticalState == VerticalState.Jumping);
            CharacterAnimator.SetBool("OnGround", VerticalState == VerticalState.Idle);
            CharacterAnimator.SetBool("Falling", VerticalState == VerticalState.Falling);
            CharacterAnimator.SetBool("InFlight", VerticalState == VerticalState.InFlight);
            CharacterAnimator.SetBool("SpeedFlight", SpeedFlight);
        }
        #endregion 

        //------------------------------------------------------------------------------------------------------------------------
        private float TimeStamp = 0.66f;
        private float ActionTimer = 0.33f;
        private bool ActionTime
        {
            get
            {
                if (TimeStamp >= 0)
                {
                    TimeStamp -= Time.deltaTime;
                    return false;
                }
                else if (TimeStamp < 0)
                {
                    TimeStamp = 0.66f;

                    ActionTimer -= Time.deltaTime;

                    if (ActionTimer < 0)
                    {
                        TimeStamp = 0.66f;
                        ActionTimer = 0.33f;
                    }

                    return true;
                }
                return false;
            }
        }

        public VerticalState VerticalState
        {
            get
            {
                if (!OnGround && !InFlight)
                {
                    if (CurrentPosition.y - PreviousPosition.y < -0.02f) return VerticalState.Falling;
                    else if (CurrentPosition.y - PreviousPosition.y > 0.02f) return VerticalState.Jumping;
                    else if (CurrentPosition.y - PreviousPosition.y == 0.0f) return VerticalState.Idle;
                }
                if (!OnGround && InFlight)
                {
                    return VerticalState.InFlight;
                }

                return VerticalState.Idle;
            }
        }

        private Vector3 PreviousPosition;
        private Vector3 CurrentPosition { get => transform.position; }

        private void OnGUI()
        {
            //GUI.Box(new Rect(0, 0, 350, 200), "");
            //GUILayout.Label("   Flight Speed Factor: " + FlightSpeedFactor);
        }
    }
}
