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
        Falling
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

            ActionType = inputs.ActionTypeInput;
            ChooseFlight = inputs.ChooseFlightInput;
        }
        #endregion

        #region CHARACTER MOVEMENT
        public Vector3 Forward { get; set; }
        public Vector3 Right { get; set; }
        public Vector3 CurrentyVelocity { get; set; }

        public void UpdateCharacterSpeed(bool speedAction, float speed)
        {
            if (speedAction)
                CurrentSpeed = speed * 3.6f;
            else
                CurrentSpeed = speed;
        }
        public void UpdateThirdPersonMovePosition(Vector3 inputDirection, float movementSpeed)
        {
            if (CurrentInputState == PlayerInputState.FlightControll) return;

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
                if (CurrentInputState == PlayerInputState.GroundMove)
                {
                    if (VerticalState == VerticalState.Jumping) return 1.2f;
                    if (VerticalState == VerticalState.Falling) return 2.2f;
                    return 1.0f;
                }
                if (CurrentInputState == PlayerInputState.FlightControll)
                {
                    return 1.0f;
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
        public float MaximunFlightHeight { get => Mathf.Round(Mathf.Pow(CharacterController.height, 10.015f)); }
        public float FlightHeightMultiplier { get => Mathf.Round(Mathf.InverseLerp(MaximunFlightHeight, JumpSpeed, GroundDistance) * 100) / 100; }
        public float FlightGravityDirection { get; set; }
        public float FlightForwardSpeedFactor { get; set; }
        public float InverseFlightSpeedFactor { get => Mathf.InverseLerp(BaseSpeed * 3.6f, BaseSpeed, CurrentSpeed); }
        public float FlightSpeedFactor { get => Mathf.InverseLerp(BaseSpeed, CurrentSpeed, CurrentSpeed); }
        //public float FlightAngleCorrection { get => Vector3.SignedAngle(transform.forward, ForwardFlightDirectionArchor.forward, Vector3.right); }


        IEnumerator StartFlightDelay()
        {
            yield return new WaitUntil(() => GroundDistance >= JumpSpeed);

            StartJump = false;
            OnGround = false;
            SetPlayerPhysicsSimulation(null);
            OnVerticalAction.RemoveAllListeners();
            OnVerticalAction.AddListener(UpdateFlightHeightPosition);
            OnCharacterDirection.RemoveAllListeners();
            OnCharacterDirection.AddListener(UpdateFlightPosition);
        }
        public void EnteringFlightState()
        {
            float yVelocity = 0.0f;

            yVelocity = Mathf.Lerp(yVelocity, JumpSpeed, Time.deltaTime * 4.0f);

            FlightVelocity = new Vector3(0.0f, yVelocity, 0.0f);

            CharacterController.Move(FlightVelocity);
        }
        public void UpdateFlightPosition(Vector3 inputDirection, float movementSpeed)
        {
            inputDirection = inputDirection.normalized;

            float xRot = transform.eulerAngles.x;
            float yRot = transform.eulerAngles.y;

            xRot += inputDirection.z * Mathf.Pow(BaseSpeed, 3.0f) * Mathf.Clamp(InverseFlightSpeedFactor, 0.6f, 0.7f);
            yRot += inputDirection.x * Mathf.Pow(BaseSpeed, 3.0f) * Mathf.Clamp(InverseFlightSpeedFactor, 0.6f, 0.7f);

            xRot = xRot > 180 ? xRot - 360 : xRot;
            xRot = Mathf.Clamp(xRot, -25.0f, 70.0f);

            Vector3 lookDirection = new Vector3(0.0f , yRot, 0.0f);
            
            transform.rotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(lookDirection), 4.5f * Time.deltaTime);

            Vector3 move = new Vector3();
            Vector3 forward = movementSpeed * ForwardFlightDirectionArchor.forward * FlightForwardSpeedFactor;

            move = forward + Vector3.zero;

            FlightVelocity = Vector3.MoveTowards(FlightVelocity, move, CurrentAcceleration * Time.deltaTime);

            CharacterController.Move(FlightVelocity * Time.deltaTime * movementSpeed);
        }
        public void UpdateFlightHeightPosition(bool verticalAction)
        {
            if (verticalAction)
            {
                FlightForwardSpeedFactor = Mathf.Lerp(FlightForwardSpeedFactor, 1.0f, Time.deltaTime);
                FlightGravityDirection = Mathf.Lerp(CurrentSpeed, -0.66f - (FlightSpeedFactor * 2.0f), Mathf.InverseLerp(0.0f, 1.0f, FlightForwardSpeedFactor));
            }
            else
            {
                FlightForwardSpeedFactor = 0.01f;
                FlightGravityDirection = Mathf.Lerp(FlightGravityDirection, -1.0f, CurrentAcceleration * Time.deltaTime);
            }

            //FlightForwardSpeedFactor = Mathf.Clamp01(FlightForwardSpeedFactor);

            //if (GroundDistance < JumpSpeed && !verticalAction)
            //{
            //    FlightGravityDirection = 0.0f;
            //}
            //else
            //{
            //    if (verticalAction)
            //    {
            //        FlightGravityDirection = Mathf.Lerp(FlightGravityDirection, 1.0f, CurrentAcceleration * Time.deltaTime);
            //    }
            //    else
            //    {
            //        FlightGravityDirection = Mathf.Lerp(FlightGravityDirection, -0.2f, CurrentAcceleration * Time.deltaTime);
            //    }
            //}

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

                    float yVelocity = Mathf.Lerp(0.0f, JumpSpeed, 0.8f);

                    GravityVelocity = new Vector3(0.0f, yVelocity, 0.0f);

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
                if (OnGround && CurrentInputState != PlayerInputState.FlightControll) return BaseAcceleration;
                else if (!OnGround && CurrentInputState != PlayerInputState.FlightControll) return UngroundAcceleration;
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
        public Transform GroundCheckOrigin { get; set; }
        public bool OnGround
        {
            get => onGround;
            set
            {
                if (value)
                {
                    CurrentInputState = PlayerInputState.GroundMove;
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
        private bool onGround;
        public LayerMask GroundLayer { get; set; }
        public float GroundDistance
        {
            get
            {
                Physics.Raycast(transform.position, Vector3.down, out RaycastHit hitInfo, MaximunFlightHeight, GroundLayer);

                return hitInfo.distance;
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
                PreviousPosition = transform.position;
            }
        }
        IEnumerator CheckingUngroundedStates()
        {
            AllowJump = false;
            StartJump = false;

            yield return new WaitUntil(() => OnGround);

            SetPlayerPhysicsSimulation(ApplyGravity);

            FlightGravityDirection = 0.0f;
            FlightForwardSpeedFactor = 0.0f;

            CurrentyVelocity = CurrentyVelocity / Drag * 0.85f;

            OnVerticalAction.RemoveAllListeners();
            OnVerticalAction.AddListener(Jump);
            OnCharacterDirection.RemoveAllListeners();
            OnCharacterDirection.AddListener(UpdateThirdPersonMovePosition);

            yield return new WaitForSeconds(0.45f);
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
            CharacterAnimator.SetBool("OnGround", onGround);
            CharacterAnimator.SetBool("Falling", VerticalState == VerticalState.Falling);
            CharacterAnimator.SetBool("InFlight", CurrentInputState == PlayerInputState.FlightControll);
            //CharacterAnimator.SetBool("SpeedFlight", FlightSpeedFactor == 0.0f && FlightForwardSpeedFactor > 0.0f ? true : false);
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
        private bool ActionType
        {
            set
            {
                if (value)
                {
                    if (ChooseFlight && CurrentInputState != PlayerInputState.FlightControll)
                    {
                        PreviousInputState = CurrentInputState;
                        CurrentInputState = PlayerInputState.FlightControll;
                        return;
                    }
                    else if (ChooseFlight && CurrentInputState != PlayerInputState.GroundMove)
                    {
                        PreviousInputState = CurrentInputState;
                        CurrentInputState = PlayerInputState.GroundMove;
                        return;
                    }
                }
                else if (!value)
                {
                    PreviousInputState = CurrentInputState;
                }
            }
        }

        private bool ChooseFlight { get; set; }

        private VerticalState VerticalState
        {
            get
            {
                if (CurrentInputState == PlayerInputState.GroundMove)
                {
                    if (CurrentPosition.y - PreviousPosition.y < -0.02f) return VerticalState.Falling;
                    else if (CurrentPosition.y - PreviousPosition.y > 0.02f) return VerticalState.Jumping;
                    else if (CurrentPosition.y - PreviousPosition.y == 0.0f) return VerticalState.Idle;
                }

                return VerticalState.Idle;
            }
        }

        private PlayerInputState PreviousInputState { get; set; }
        public PlayerInputState CurrentInputState
        {
            get => currentInputState;
            set
            {
                if (currentInputState == value) return;

                currentInputState = value;

                if (value == PlayerInputState.FlightControll)
                {
                    StopCoroutine("StartFlightDelay");
                    StartCoroutine("StartFlightDelay");
                    SetPlayerPhysicsSimulation(EnteringFlightState);
                }
                if (value == PlayerInputState.GroundMove)
                {
                    OnCharacterDirection.RemoveAllListeners();
                    OnCharacterDirection.AddListener(UpdateThirdPersonMovePosition);

                    PreviousPosition = transform.position;
                    GravityVelocity = Vector3.zero;
                    FlightVelocity = Vector3.zero;
                    SetPlayerPhysicsSimulation(ApplyGravity);
                }
            }
        }
        private PlayerInputState currentInputState;

        private Vector3 PreviousPosition;
        private Vector3 CurrentPosition { get => transform.position; }

        private void OnGUI()
        {
            GUI.Box(new Rect(0, 0, 300, 300), "");
            //GUILayout.Label("   Flight Forward Speed Factor: " + FlightForwardSpeedFactor); 
            //GUILayout.Label("   Action Time: " + ActionTime);
            //GUILayout.Label("   Previous Input State:" + PreviousInputState);
            //GUILayout.Label("   Input State:" + CurrentInputState);
            //GUILayout.Label("   Vertical State: " + VerticalState);
            //GUILayout.Label("   Vertical Action State: " + ChooseFlight);
            //GUILayout.Label("   Gravity Multiplier: " + GravityMultiplierFactor);
            //GUILayout.Label("   Last Position: " + PreviousPosition);
            //GUILayout.Label("   Current Position: " + CurrentPosition);
            //GUILayout.Label("   Gravity Force: " + Gravity);
            //GUILayout.Label("   Ground Distance: " + GroundDistance);
            //GUILayout.Label("   Current Speed: " + CurrentSpeed);
            //GUILayout.Label("   Current Velocity: " + CurrentyVelocity);
            //GUILayout.Label("   Current Velocity Magnitude: " + CurrentyVelocity.magnitude * Time.deltaTime);
            GUILayout.Label("   Current Velocity Factor: " + (CurrentSpeed / BaseSpeed));
            GUILayout.Label("   Inverse Flight Speed Percentage: " + InverseFlightSpeedFactor);
            GUILayout.Label("   Flight Speed Percentage: " + FlightSpeedFactor);
            GUILayout.Label("   Current Flight Factor: " + FlightGravityDirection);
            GUILayout.Label("   Current Flight Forward: " + Mathf.Round(FlightForwardSpeedFactor * 100) / 100);
            GUILayout.Label("   Current Flight Gravity Direction: " + Mathf.Round(FlightGravityDirection * 100) / 100);
            GUILayout.Label("   Flight Height Multiplier: " + FlightHeightMultiplier);            
            //GUILayout.Label("   Current Flight Velocity: " + FlightVelocity);
            //GUILayout.Label("   Current Flight Magnitude: " + FlightVelocity.magnitude);
            //GUILayout.Label("   Jump Speed: " + JumpSpeed);
            //GUILayout.Label("   Jump Height: " + JumpHeight);
        }
    }
}
