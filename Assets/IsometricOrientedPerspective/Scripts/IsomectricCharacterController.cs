using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace IsometricGameController
{
    [RequireComponent(typeof(CapsuleCollider))]
    public class IsomectricCharacterController : MonoBehaviour, IIsometricController, IIsometricGravity
    {
        public static IsomectricCharacterController Instance { get; private set; }

        public static UnityEvent<GameControllerState> OnGameStateChanged = new UnityEvent<GameControllerState>();

        #region PROPERTIES
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
        public float OnGroundSpeed { get => m_onGroundSpeed; set { if (m_onGroundSpeed == value) return; m_onGroundSpeed = value; } }
        public float OnGroundAcceleration { get => m_acceleration; set { if (m_acceleration == value) return; m_acceleration = value; } }
        public bool AllowJump { get; set; }
        public float OnAirSpeed { get => OnGroundSpeed * 0.8f; }
        public float OnAirAcceleration { get => OnGroundAcceleration * 0.8f; }
        public float MaxSlopeAngle { get => m_maxSlopeAngle; set { if (m_maxSlopeAngle == value) return; m_maxSlopeAngle = value; } }
        public float TurnBasedDistanceTravelled { get; set; }
        public bool TurnBasedMovementStarted { get; set; }
        public Vector3 TurnBasedTargetPosition { get; set; }
        public Vector3 TurnBasedTargetDirection { get; set; }
        public Vector3 CurrentyVelocity { get; set; }
        public CapsuleCollider CapsuleCollider { get; set; }
        public CharacterController CharacterController { get; set; }
        public GameControllerState ControllerState { get => m_state; set { m_state = value; OnGameStateChanged?.Invoke(ControllerState); } }

        public Vector3 LastGroundedPosition { get; set; }
        public Vector3 CurrentUngroudedPosition { get; set; }
        public bool Jumping { get; set; }
        public bool Falling { get; set; }
        public float Gravity { get => 9.81f; }
        public float GravityMultiplierFactor { get; set; }
        public Vector3 GravityVelocity { get; set; }
        public float JumpHeight { get => m_accelerate? m_jumpHeight *1.5f : m_jumpHeight; set { if (value == m_jumpHeight) return; m_jumpHeight = value; } }
        public float JumpSpeed { get => Mathf.Sqrt(2.0f * JumpHeight * Gravity); }
        public float Drag { get => m_drag; set { if (value == m_drag) return; m_drag = value; } }
        public LayerMask WhatIsGround { get => m_layerMask; set { if (value == m_layerMask) return; m_layerMask = value; } }
        #endregion

        #region INSPECTOR FIELDS
        [SerializeField] private LayerMask m_layerMask;
        [SerializeField] private float m_maxSlopeAngle = 45f;
        [SerializeField] private float m_onGroundSpeed = 8;

        [SerializeField][Range(1f, 5)] private float m_acceleration = 2.5f;
        [SerializeField][Range(1.2f, 10)] private float m_jumpHeight = 1.5f;
        [SerializeField][Range(0, 100)] private float m_drag = 0.5f;

        [SerializeField] private Transform m_cursor;
        #endregion

        #region PRIVATE FIELDS
        private bool onGround;
        private GameControllerState m_state;
        private Vector3 m_direction;
        private bool m_confirm;
        private bool m_accelerate;
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

            float speed = CheckGroundLevel() ? (m_accelerate ? OnGroundSpeed * 2f : OnGroundSpeed) : (m_accelerate ? OnAirSpeed * 2f : OnAirSpeed);

            switch (ControllerState)
            {
                case GameControllerState.Exploring:
                    UpdateMovePosition(m_direction.normalized, speed);
                    Jump(m_jump);
                break;

                case GameControllerState.TurnBased:
                    if (!TurnBasedMovementStarted)
                    {
                        TurnBasedTargetPosition = UpdateCursorPosition(m_direction.normalized, speed);

                        if (m_confirm)
                        {
                            TurnBasedMovementStarted = true;
                            TurnBasedTargetDirection = TurnBasedTargetPosition - transform.position;
                        }
                    }
                    else
                    {
                        TurnBasedDistanceTravelled = Vector2.Distance(new Vector2(TurnBasedTargetPosition.x, TurnBasedTargetPosition.z), new Vector2(transform.position.x, transform.position.z));
                        if (Mathf.RoundToInt(TurnBasedDistanceTravelled) != 0)
                        {
                            if (!CharacterController.enabled) CharacterController.enabled = true;
                            UpdateMovePosition(TurnBasedTargetDirection.normalized, speed);
                        }
                        else
                        {
                            CurrentyVelocity = Vector3.zero;
                            CharacterController.enabled = false;
                            TurnBasedMovementStarted = false;
                        }
                    }
                break;
            }
        }
        #endregion

        #region ACTION METHODS
        public float ApplyDrag(float velocity, float drag)
        {
            return velocity * (1 / (1 + drag * Time.deltaTime));
        }
        public void ApplyGravity()
        {
            GravityVelocity -= new Vector3(0.0f, Gravity * GravityMultiplierFactor * Time.deltaTime, 0.0f);
            CharacterController.Move(GravityVelocity * Time.deltaTime);
        }
        public void UpdateMovePosition(Vector3 inputDirection, float movementSpeed)
        {
            Vector3 move = new Vector3();
            if (!TurnBasedMovementStarted)
            {
                Vector3 right = inputDirection.x * IsometricOrientedPerspective.IsometricRight;
                Vector3 forward = inputDirection.z * IsometricOrientedPerspective.IsometricForward;

                move = right + forward + Vector3.zero;
            }
            else
            {
                move = new Vector3(inputDirection.x, 0, inputDirection.z);
            }

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
        public Vector3 UpdateCursorPosition(Vector3 inputDirection, float movementSpeed)
        {
            m_cursor.forward = IsometricOrientedPerspective.IsometricForward;
            m_cursor.Translate(inputDirection * Time.deltaTime * movementSpeed * OnGroundAcceleration);

            transform.LookAt(new Vector3(m_cursor.position.x, transform.position.y, m_cursor.position.z));

            return m_cursor.position;
        }
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
        #endregion

        #region CHECKING METHODS
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
        public bool CheckGroundLevel()
        {
            bool ground;

            ground = Physics.CheckSphere(transform.position - new Vector3(0, 0.5f, 0), GetComponent<CapsuleCollider>().radius / 0.85f, WhatIsGround, QueryTriggerInteraction.Collide);

            OnGround = ground;

            if (ground && GravityVelocity.y < 0 && !OnSlope()) GravityVelocity = new Vector3(GravityVelocity.x, 0.0f, GravityVelocity.z);

            return ground;
        }
        public bool OnSlope()
        {
            if (Physics.SphereCast(transform.position - new Vector3(0, 0.5f, 0), GetComponent<CapsuleCollider>().radius, Vector3.down, out RaycastHit hitInfo, GetComponent<CapsuleCollider>().radius / 2, WhatIsGround, QueryTriggerInteraction.Collide))
            {
                float slopeAngle = Mathf.RoundToInt(Vector3.Angle(Vector3.up, hitInfo.normal));
                return slopeAngle <= MaxSlopeAngle && slopeAngle != 0;
            }
            return false;
        }
        public float SlopeAngle()
        {
            if (Physics.SphereCast(transform.position - new Vector3(0, 0.5f, 0), GetComponent<CapsuleCollider>().radius, Vector3.down, out RaycastHit hitInfo, GetComponent<CapsuleCollider>().radius / 2, WhatIsGround, QueryTriggerInteraction.Collide))
            {
                float slopeAngle = Mathf.RoundToInt(Vector3.Angle(Vector3.up, hitInfo.normal));
                return slopeAngle;
            }
            return 0;
        }
        public RaycastHit SlopeHit()
        {
            Physics.SphereCast(transform.position - new Vector3(0, 0.5f, 0), GetComponent<CapsuleCollider>().radius, Vector3.down, out RaycastHit hitInfo, GetComponent<CapsuleCollider>().radius / 2, WhatIsGround, QueryTriggerInteraction.Collide);
            return hitInfo;
        }
        public Vector3 GetSlopeMoveDirection(Vector3 direction)
        {
            return Vector3.ProjectOnPlane(new Vector3(direction.x, (int)(direction.y * (int)SlopeAngle()), direction.z), SlopeHit().normal).normalized;
        }
        #endregion

        #region MISC
        public void SetInput(IsometricInputHandler inputs)
        {
            m_direction = inputs.IsometricMoveDirection;
            
            switch (m_state)
            {
                case GameControllerState.Exploring:
                    m_jump = inputs.JumpInput;
                    break;
                case GameControllerState.TurnBased:
                    m_confirm = inputs.PlayerConfirmEntry;
                    break;
            }

            if (inputs.AccelerateSpeed) m_accelerate = !m_accelerate;
            if (m_direction == Vector3.zero) m_accelerate = false;
        }
        public void OnGameControllerStateChanged(GameControllerState state)
        {
            m_cursor.position = new Vector3(transform.position.x, transform.parent.position.y, transform.position.z);
            CharacterController.Move(Vector3.zero);
            switch (state)
            {
                case GameControllerState.Exploring:
                    m_cursor.gameObject.SetActive(false);
                    CharacterController.enabled = true;
                    break;
                case GameControllerState.TurnBased:
                    m_cursor.gameObject.SetActive(true);
                    CharacterController.enabled = false;
                    break;
            }
        }
        #endregion
    }
}
