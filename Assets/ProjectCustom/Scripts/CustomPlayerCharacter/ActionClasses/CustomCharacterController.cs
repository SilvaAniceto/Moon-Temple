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
                OnSpeedUpAction?.Invoke(value, CurrentSpeed);
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

        public void UpdateThirdPersonMovePosition(Vector3 inputDirection, float movementSpeed)
        {
            if (CurrentInputState == PlayerInputState.FlightControll) return;

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

        #region CHARACTER FLIGHT
        public Vector3 FlightVelocity { get; set; }
        public Vector3 FlightForce { get; set; }
        public float FlightHorizontalRotation { get; set; }
        public float FlightVerticalRotation { get; set; }

        IEnumerator StartFlightDelay()
        {
            yield return new WaitUntil(() => transform.localPosition.y >= PreviousPosition.y + JumpSpeed);

            StartJump = false;
            OnGround = false;
            SetPlayerPhysicsSimulation(InFlightGravity);
            OnCharacterDirection.RemoveAllListeners();
            OnCharacterDirection.AddListener(UpdateFlightPosition);
        }
        public void EnteringFlightState()
        {
            float yVelocity = 0.0f;

            yVelocity = Mathf.Lerp(yVelocity, JumpSpeed, Time.deltaTime * 4.0f);

            CharacterController.Move(new Vector3(0.0f, yVelocity, 0.0f));

            FlightHorizontalRotation = 0.0f;
            FlightVerticalRotation = transform.eulerAngles.y;
        }
        public void UpdateFlightPosition(Vector3 inputDirection, float movementSpeed)
        {
            //if (CurrentInputState != PlayerInputState.FlightControll) return;

            //if (!SpeedUpAction)
            //{
            //    GravityVelocity = Vector3.zero;
            //    FlightVelocity = Vector3.zero;

            //    //UpdateFlightHeightPosition(VerticalAscendingInput, VerticalDescendingInput);

            //    Vector3 move = new Vector3();
            //    Vector3 right = inputDirection.x * Right;
            //    Vector3 forward = inputDirection.z * Forward;

            //    move = right + forward + Vector3.zero;

            //    CurrentyVelocity = Vector3.MoveTowards(CurrentyVelocity, move, CurrentAcceleration * Time.deltaTime);

            //    CharacterController.Move(CurrentyVelocity * Time.deltaTime * movementSpeed);

            //    CharacterController.Move(GravityVelocity * Time.deltaTime);

            //    FlightHorizontalRotation = 0.0f;
            //    FlightVerticalRotation = CustomCamera.Instance.transform.transform.eulerAngles.y;

            //    Vector3 flightDirection = new Vector3(FlightHorizontalRotation, FlightVerticalRotation, 0.0f);

            //    transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(flightDirection), CurrentAcceleration * Time.deltaTime);
            //}
            //else
            //{
            //    Vector3 move = new Vector3();
            //    Vector3 forward = 1.0f * transform.forward;
            //    Vector3 right = inputDirection.x * Vector3.right;
            //    Vector3 up = inputDirection.z * Vector3.up;

            //    move = right + up + Vector3.zero;

            //    FlightVelocity = Vector3.MoveTowards(FlightVelocity, transform.forward, CurrentAcceleration * Time.deltaTime);

            //    if (move != Vector3.zero)
            //    {
            //        FlightHorizontalRotation += up.y * 1.2f;
            //        FlightHorizontalRotation = Mathf.Clamp(FlightHorizontalRotation, -85f, 70f);

            //        FlightVerticalRotation += right.x * 1.8f;

            //        Vector3 flightDirection = new Vector3(FlightHorizontalRotation, FlightVerticalRotation, 0.0f);

            //        transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(flightDirection), 0.8f);
            //    }
            //    else
            //    {
            //        FlightVerticalRotation = transform.eulerAngles.y;
            //    }

            //    CharacterController.Move(FlightVelocity * Time.deltaTime * movementSpeed * 8);
            //}
        }
        public void UpdateFlightHeightPosition(bool ascendingFlight, bool descendingFlight)
        {
            //if (ascendingFlight)
            //{
            //    float y = GravityVelocity.y;
            //    y = Mathf.Lerp(y, JumpHeight * 9.81f * 3.0f, Time.deltaTime * 4.0f);
            //    GravityVelocity = new Vector3(0.0f, y, 0.0f);
            //}
            //else if (descendingFlight)
            //{
            //    float y = GravityVelocity.y;
            //    y = Mathf.Lerp(y, -JumpHeight * 9.81f * 6.0f, Time.deltaTime * 4.0f);
            //    GravityVelocity = new Vector3(0.0f, y, 0.0f);
            //}
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
            get;
            //{
            //    if (SpeedUpAction) return 3.6f * 2.5f;
            //    else return 3.6f;
            //}
        }
        public float BaseAcceleration { get => 2.0f; }
        public float UngroundSpeed { get => BaseSpeed * 0.8f; }
        public float UngroundAcceleration { get => BaseAcceleration * 0.8f; }
        public float InFlightSpeed
        {
            get;
            //{
            //    if (SpeedUpAction) return BaseSpeed * 1.8f;
            //    else return BaseSpeed;
            //}
        }
        public float InFlightAcceleration { get => BaseAcceleration * 2.5f; }
        public float CurrentSpeed
        {
            get => 0.415f * CharacterController.height * 3.2f;
            //{
            //    if (OnGround && CurrentInputState != PlayerInputState.FlightControll) return BaseSpeed;
            //    else if (!OnGround && CurrentInputState != PlayerInputState.FlightControll) return UngroundSpeed;
            //    else return InFlightSpeed;
            //}
        }
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

            SetPlayerPhysicsSimulation(ApplyGravity);
            OnVerticalAction.AddListener(Jump);

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
        }
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
                    return 0.0f;
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
            Debug.Log("Gravidade");
            GravityVelocity -= Gravity * GravityMultiplierFactor * Time.deltaTime;

            if (SlopeAngle() <= MaxSlopeAngle)
                CharacterController.Move(GravityVelocity * Time.deltaTime);
            else
                CharacterController.Move(GetSlopeMoveDirection(GravityVelocity + SlopeHit().transform.forward) * Time.deltaTime * BaseSpeed);
        }
        public void InFlightGravity()
        {
            Debug.Log("Gravidade em Voo");
            GravityVelocity -= Gravity * GravityMultiplierFactor * Time.deltaTime;
            CharacterController.Move(GravityVelocity * Time.deltaTime);
        }
        #endregion

        #region CHARACTER GROUND DETECTION
        public Transform GroundCheckOrigin { get; set; }
        public bool OnGround
        {
            get => onGround;
            set
            {
                if (value) CurrentInputState = PlayerInputState.GroundMove;

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

            CurrentyVelocity = CurrentyVelocity / Drag * 0.85f;

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
            else CharacterAnimator.SetFloat("MoveSpeed", /*Mathf.Clamp(SpeedUpAction ? CurrentyVelocity.magnitude * 2.0f : */CurrentyVelocity.magnitude/*, 0.0f, 2.0f)*/, 0.1f, Time.deltaTime);

            CharacterAnimator.SetBool("Jumping", VerticalState == VerticalState.Jumping);
            CharacterAnimator.SetBool("OnGround", onGround);
            CharacterAnimator.SetBool("Falling", VerticalState == VerticalState.Falling);
            CharacterAnimator.SetBool("InFlight", CurrentInputState == PlayerInputState.FlightControll);

            //if (CurrentInputState != PlayerInputState.FlightControll)
            //{
            //    CharacterAnimator.SetBool("SpeedFlight", false);
            //    return;
            //}

            //CharacterAnimator.SetBool("SpeedFlight", SpeedUpAction);
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
                    PreviousPosition = transform.position;
                    GravityVelocity = Vector3.zero;
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
            GUILayout.Label("   Action Time: " + ActionTime);
            GUILayout.Label("   Previous Input State:" + PreviousInputState);
            GUILayout.Label("   Input State:" + CurrentInputState);
            GUILayout.Label("   Vertical State: " + VerticalState);
            GUILayout.Label("   Vertical Action State: " + ChooseFlight);
            GUILayout.Label("   Gravity Multiplier: " + GravityMultiplierFactor);
            GUILayout.Label("   Last Position: " + PreviousPosition);
            GUILayout.Label("   Current Position: " + CurrentPosition);
            GUILayout.Label("   Gravity Force: " + Gravity); 
        }
    }
}
