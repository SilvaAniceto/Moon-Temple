using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace CustomGameController
{
    [RequireComponent(typeof(CapsuleCollider))]
    public class CustomCharacterController : MonoBehaviour, ICustomPlayerController, ICustomGravity
    {
        public static CustomCharacterController Instance { get; private set; }

        public static UnityEvent<GameControllerState> OnGameStateChanged = new UnityEvent<GameControllerState>();
        IEnumerator CheckingUngroundedStates()
        {
            yield return new WaitForSeconds(0.125f);

            CurrentUngroudedPosition = transform.localPosition;

            if (LastGroundedPosition.y > CurrentUngroudedPosition.y)
            {
                GravityMultiplierFactor = 5f;
                Falling = true;
            }
            if (LastGroundedPosition.y < CurrentUngroudedPosition.y)
            {
                GravityMultiplierFactor = 1.5f;
                Jumping = true;
            }

            yield return new WaitUntil(() => onGround);

            GravityMultiplierFactor = 1.0f;
            Falling = false;
            Jumping = false;
            AllowJump = false;

            yield return new WaitForSeconds(0.2f);
            AllowJump = true;
        }

        #region SIMULATED PHYSICS PROPERTIES
        public Vector3 LastGroundedPosition { get; set; }
        public Vector3 CurrentUngroudedPosition { get; set; }
        public bool OnGround
        {
            get => onGround;
            set
            {
                if (onGround == value) return;
                onGround = value;
                if (!onGround)
                {
                    StopCoroutine(CheckingUngroundedStates());
                    LastGroundedPosition = transform.localPosition;
                    StartCoroutine(CheckingUngroundedStates());
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

        #region GAME CONTROLLER PROPERTIES
        public GameControllerState ControllerState { get => m_state; set { m_state = value; OnGameStateChanged?.Invoke(ControllerState); } }
        public Vector3 TurnBasedTargetPosition { get; set; }
        public Vector3 TurnBasedTargetDirection { get; set; }
        public float TurnBasedDistanceTravelled { get; set; }
        public bool TurnBasedMovementStarted { get; set; }
        #endregion

        #region PLAYER CONTROLLER PROPERTIES
        public CharacterController CharacterController { get; set; }
        public Vector3 Forward { get; set; }
        public Vector3 Right { get; set; }
        public Vector3 CurrentyVelocity { get; set; }
        public bool AllowJump { get; set; }
        #endregion

        #region PLAYER CONTROLLER SETTINGS
        public float OnGroundSpeed { get; set; }
        public float OnGroundAcceleration { get; set; }
        public float OnAirSpeed { get => OnGroundSpeed * 0.8f; }
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
            CharacterController.Move(GravityVelocity * Time.deltaTime);
        }
        public bool CheckGroundLevel()
        {
            bool ground;

            ground = Physics.CheckSphere(transform.position - new Vector3(0, 0.5f, 0), GetComponent<CapsuleCollider>().radius / 0.85f, GroundLayer, QueryTriggerInteraction.Collide);

            OnGround = ground;

            if (ground && GravityVelocity.y < 0 && !OnSlope()) GravityVelocity = new Vector3(GravityVelocity.x, 0.0f, GravityVelocity.z);

            return ground;
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
        #endregion

        #region PLAYER SLOPE METHODS
        public bool OnSlope()
        {
            if (Physics.SphereCast(transform.position - new Vector3(0, 0.5f, 0), GetComponent<CapsuleCollider>().radius, Vector3.down, out RaycastHit hitInfo, GetComponent<CapsuleCollider>().radius / 2, GroundLayer, QueryTriggerInteraction.Collide))
            {
                float slopeAngle = Mathf.RoundToInt(Vector3.Angle(Vector3.up, hitInfo.normal));
                return slopeAngle <= MaxSlopeAngle && slopeAngle != 0;
            }
            return false;
        }
        public float SlopeAngle()
        {
            if (Physics.SphereCast(transform.position - new Vector3(0, 0.5f, 0), GetComponent<CapsuleCollider>().radius, Vector3.down, out RaycastHit hitInfo, GetComponent<CapsuleCollider>().radius / 2, GroundLayer, QueryTriggerInteraction.Collide))
            {
                float slopeAngle = Mathf.RoundToInt(Vector3.Angle(Vector3.up, hitInfo.normal));
                return slopeAngle;
            }
            return 0;
        }
        public RaycastHit SlopeHit()
        {
            Physics.SphereCast(transform.position - new Vector3(0, 0.5f, 0), GetComponent<CapsuleCollider>().radius, Vector3.down, out RaycastHit hitInfo, GetComponent<CapsuleCollider>().radius / 2, GroundLayer, QueryTriggerInteraction.Collide);
            return hitInfo;
        }
        public Vector3 GetSlopeMoveDirection(Vector3 direction)
        {
            return Vector3.ProjectOnPlane(new Vector3(direction.x, (int)(direction.y * (int)SlopeAngle()), direction.z), SlopeHit().normal).normalized;
        }
        #endregion

        #region PLAYER JUMP & MOVEMENT METHODS
        public void Jump(bool jumpInput)
        {
            if (jumpInput)
            {
                if (AllowJump)
                {
                    float yVelocity = Mathf.Lerp(transform.position.y, JumpSpeed, 0.8f);

                    Vector3 jumpVector = new Vector3(0.0f, yVelocity, 0.0f);

                    jumpVector = Vector3.MoveTowards(jumpVector, jumpVector, Time.fixedDeltaTime);

                    CharacterController.Move(jumpVector * Time.deltaTime);
                }
            }
            if (!jumpInput && Jumping)
            {
                AllowJump = false;
            }
        }
        public void UpdateIsometricMovePosition(Vector3 inputDirection, float movementSpeed)
        {
            #region NOT IN USE
            //if (!TurnBasedMovementStarted)
            //{
            //    Vector3 right = inputDirection.x * Right;
            //    Vector3 forward = inputDirection.z * Forward;

            //    move = right + forward + Vector3.zero;
            //}
            //else
            //{
            //    move = new Vector3(inputDirection.x, 0, inputDirection.z);
            //}
            #endregion

            Vector3 move = new Vector3();
            Vector3 right = inputDirection.x * Right;
            Vector3 forward = inputDirection.z * Forward;

            move = right + forward + Vector3.zero;

            if (CheckGroundLevel())
            {
                CurrentyVelocity = Vector3.MoveTowards(CurrentyVelocity, move, OnGroundAcceleration * Time.deltaTime);

                if (OnSlope())
                    CharacterController.Move(GetSlopeMoveDirection(CurrentyVelocity) * Time.deltaTime * movementSpeed);
                else
                    CharacterController.Move(CurrentyVelocity * Time.deltaTime * movementSpeed);
            }
            else
            {
                CurrentyVelocity = Vector3.MoveTowards(CurrentyVelocity, move, OnAirAcceleration * Time.deltaTime);

                float x = ApplyDrag(CurrentyVelocity.x, Drag);
                float z = ApplyDrag(CurrentyVelocity.z, Drag);

                CurrentyVelocity = new Vector3(x, 0, z);

                CharacterController.Move(CurrentyVelocity * Time.deltaTime * movementSpeed);
            }

            if (move != Vector3.zero)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(move), 5 * Time.deltaTime);
            }
        }
        public void UpdateThirdPersonMovePosition(Vector3 inputDirection, float movementSpeed)
        {
            Vector3 move = new Vector3();
            Vector3 right = inputDirection.x * Right;
            Vector3 forward = inputDirection.z * Forward;

            move = right + forward + Vector3.zero;

            if (CheckGroundLevel())
            {
                CurrentyVelocity = Vector3.MoveTowards(CurrentyVelocity, move, OnGroundAcceleration * Time.deltaTime);

                if (OnSlope())
                    CharacterController.Move(GetSlopeMoveDirection(CurrentyVelocity) * Time.deltaTime * movementSpeed);
                else
                    CharacterController.Move(CurrentyVelocity * Time.deltaTime * movementSpeed);
            }
            else
            {
                CurrentyVelocity = Vector3.MoveTowards(CurrentyVelocity, move, OnAirAcceleration * Time.deltaTime);

                float x = ApplyDrag(CurrentyVelocity.x, Drag);
                float z = ApplyDrag(CurrentyVelocity.z, Drag);

                CurrentyVelocity = new Vector3(x, 0, z);

                CharacterController.Move(CurrentyVelocity * Time.deltaTime * movementSpeed);
            }

            if (move != Vector3.zero)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(move), 5 * Time.deltaTime);
            }
        }
        public void UpdateFirstPersonMovePosition(Vector3 inputDirection, float movementSpeed)
        {
            Vector3 move = new Vector3();
            Vector3 right = inputDirection.x * Right;
            Vector3 forward = inputDirection.z * Forward;

            move = right + forward + Vector3.zero;

            if (CheckGroundLevel())
            {
                CurrentyVelocity = Vector3.MoveTowards(CurrentyVelocity, move, OnGroundAcceleration * Time.deltaTime);

                if (OnSlope())
                    CharacterController.Move(GetSlopeMoveDirection(CurrentyVelocity) * Time.deltaTime * movementSpeed);
                else
                    CharacterController.Move(CurrentyVelocity * Time.deltaTime * movementSpeed);
            }
            else
            {
                CurrentyVelocity = Vector3.MoveTowards(CurrentyVelocity, move, OnAirAcceleration * Time.deltaTime);

                float x = ApplyDrag(CurrentyVelocity.x, Drag);
                float z = ApplyDrag(CurrentyVelocity.z, Drag);

                CurrentyVelocity = new Vector3(x, 0, z);

                CharacterController.Move(CurrentyVelocity * Time.deltaTime * movementSpeed);
            }

            //if (move != Vector3.zero)
            //{
            //    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(move), 5 * Time.deltaTime);
            //}
        }
        #endregion

        #region PLAYER INPUT METHODS
        public void SetInput(CustomPlayerInputHandler inputs)
        {
            m_direction = inputs.MoveDirectionInput;
            m_jump = inputs.JumpInput;
            m_sprint = inputs.SprintInput;

            #region NOT IN USE
            //switch (m_state)
            //{
            //    case GameControllerState.Exploring:
            //        m_jump = inputs.JumpInput;
            //        break;
            //    case GameControllerState.TurnBased:
            //        m_confirm = inputs.PlayerConfirmEntry;
            //        break;
            //}
            //if (inputs.SprintInput) m_accelerate = !m_accelerate;
            //if (m_direction == Vector3.zero) m_accelerate = false;
            #endregion
        }
        #endregion

        #region PRIVATE FIELDS
        private bool onGround;
        private GameControllerState m_state;

        private Vector3 m_direction;
        private bool m_sprint;
        private bool m_jump;
        #endregion

        #region DEFAULT METHODS
        private void Awake()
        {
            if (Instance == null) Instance = this;

            if (CharacterController == null)
            {
                CharacterController = gameObject.AddComponent<CharacterController>();
                CharacterController.slopeLimit = 0;
            }

            StartCoroutine(CheckingUngroundedStates());
            GravityMultiplierFactor = 1.0f;

            OnGameStateChanged.AddListener(OnGameControllerStateChanged);
        }
        private void Start()
        {

        }
        void Update()
        {
            ApplyGravity();

            float speed = CheckGroundLevel() ? (m_sprint ? OnGroundSpeed * 2f : OnGroundSpeed) : (m_sprint ? OnAirSpeed * 2f : OnAirSpeed);

            switch (CustomCamera.Instance.Perspective)
            {
                case CustomCamera.CameraPerspective.None:
                    break;
                case CustomCamera.CameraPerspective.Isometric:
                    UpdateIsometricMovePosition(m_direction.normalized, speed);
                    break;
                case CustomCamera.CameraPerspective.Third_Person:
                    UpdateThirdPersonMovePosition(m_direction.normalized, speed);
                    break;
                case CustomCamera.CameraPerspective.Over_Shoulder:
                    break;
                case CustomCamera.CameraPerspective.First_Person:
                    UpdateFirstPersonMovePosition(m_direction.normalized, speed);
                    break;
                default:
                    break;
            }
            Jump(m_jump);

            #region NOT IN USE
            //switch (ControllerState)
            //{
            //    case GameControllerState.Exploring:
            //    break;

            //    case GameControllerState.TurnBased:
            //        if (!TurnBasedMovementStarted)
            //        {
            //            TurnBasedTargetPosition = UpdateCursorPosition(m_direction.normalized, speed);

            //            if (m_confirm)
            //            {
            //                TurnBasedMovementStarted = true;
            //                TurnBasedTargetDirection = TurnBasedTargetPosition - transform.position;
            //            }
            //        }
            //        else
            //        {
            //            TurnBasedDistanceTravelled = Vector2.Distance(new Vector2(TurnBasedTargetPosition.x, TurnBasedTargetPosition.z), new Vector2(transform.position.x, transform.position.z));
            //            if (Mathf.RoundToInt(TurnBasedDistanceTravelled) != 0)
            //            {
            //                if (!CharacterController.enabled) CharacterController.enabled = true;
            //                UpdateIsometricMovePosition(TurnBasedTargetDirection.normalized, speed);
            //            }
            //            else
            //            {
            //                CurrentyVelocity = Vector3.zero;
            //                CharacterController.enabled = false;
            //                TurnBasedMovementStarted = false;
            //            }
            //        }
            //    break;
            //}
            #endregion
        }
        #endregion
    }
}
