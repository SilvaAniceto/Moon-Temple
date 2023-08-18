using UnityEngine;
using UnityEngine.Events;

namespace IsometricGameController
{
    [RequireComponent(typeof(CapsuleCollider))]
    public class IsomectricCharacterController : MonoBehaviour, IIsometricController, IIsometricGravity
    {
        public static IsomectricCharacterController Instance { get; private set; }

        public float OnGroundSpeed { get => m_onGroundSpeed; set { if (m_onGroundSpeed == value) return; m_onGroundSpeed = value; } }
        public float OnGroundAcceleration { get => m_acceleration; set { if (m_acceleration == value) return; m_acceleration = value; } }
        public float OnAirSpeed { get => OnGroundSpeed * 0.8f; }
        public float OnAirAcceleration { get => OnGroundAcceleration * 0.8f; }
        public float MaxSlopeAngle { get => m_maxSlopeAngle; set { if (m_maxSlopeAngle == value) return; m_maxSlopeAngle = value; } }
        public Vector3 CurrentyVelocity { get; set; }
        public CapsuleCollider CapsuleCollider { get; set; }
        public CharacterController CharacterController { get; set; }
        public GameControllerState ControllerState { get => m_state; set { m_state = value; OnGameStateChanged?.Invoke(ControllerState); } }

        public float Gravity { get => 9.81f; }
        public Vector3 GravityVelocity { get; set; }
        public float JumpHeight { get => m_jumpHeight; set { if (value == m_jumpHeight) return; m_jumpHeight = value; } }
        public float JumpSpeed { get => Mathf.Sqrt(2.0f * JumpHeight * Gravity); }
        public float Drag { get => m_drag; set { if (value == m_drag) return; m_drag = value; } }
        public bool OnAir { get; set; }
        public LayerMask WhatIsGround { get => m_layerMask; set { if (value == m_layerMask) return; m_layerMask = value; } }

        [SerializeField] private LayerMask m_layerMask;
        [SerializeField] private float m_maxSlopeAngle = 45f;
        [SerializeField] private float m_onGroundSpeed = 8;

        [SerializeField][Range(1f, 5)] private float m_acceleration = 2.5f;
        [SerializeField][Range(1.2f, 10)] private float m_jumpHeight = 1.5f;
        [SerializeField][Range(0, 100)] private float m_drag = 0.5f;

        [SerializeField] private Transform m_cursor;

        public static UnityEvent<GameControllerState> OnGameStateChanged = new UnityEvent<GameControllerState>();

        private GameControllerState m_state;
        private Vector3 m_direction;
        private bool m_confirm;
        private bool m_accelerate;
        private bool m_jump;

        private void Awake()
        {
            if (Instance == null) Instance = this;

            if (CharacterController == null)
            {
                CharacterController = gameObject.AddComponent<CharacterController>();
                CharacterController.slopeLimit = 0;
            }

            OnGameStateChanged.AddListener(OnGameControllerStateChanged);
        }

        private void Start()
        {
            
        }

        void Update()
        {
            ApplyGravity();

            float speed = CheckGroundLevel() ? (m_accelerate ? OnGroundSpeed * 2f : OnGroundSpeed) : (m_accelerate ? OnAirSpeed * 2f : OnAirSpeed);
            float height = m_accelerate ? JumpHeight * 1.5f : JumpHeight;

            UpdateMovePosition(m_direction.normalized, speed);
            Jump(height, m_jump);
        }

        public void ApplyGravity()
        {
            if (!OnAir)
            {
                GravityVelocity -= new Vector3(0.0f, Gravity * 1.5f * Time.deltaTime, 0.0f);
            }
            if (!CheckGroundLevel() && !OnAir)
            {
                GravityVelocity -= new Vector3(0.0f, Gravity * 5f * Time.deltaTime, 0.0f);
            } 
            
            GravityVelocity -= new Vector3(0.0f, Gravity * Time.deltaTime, 0.0f);
            CharacterController.Move(GravityVelocity * Time.deltaTime);
        }

        public void UpdateMovePosition(Vector3 inputDirection, float movementSpeed)
        {
            Vector3 right = inputDirection.x * IsometricOrientedPerspective.IsometricRight;
            Vector3 forward = inputDirection.z * IsometricOrientedPerspective.IsometricForward;

            Vector3 move = right + forward + Vector3.zero;
            
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

            if (ControllerState == GameControllerState.Combat)
            {
                m_cursor.forward = IsometricOrientedPerspective.IsometricForward;
                m_cursor.Translate(inputDirection * Time.deltaTime * movementSpeed * OnGroundAcceleration);

                transform.LookAt(new Vector3(m_cursor.position.x, transform.position.y, m_cursor.position.z));
            }
        }

        public void Jump(float jumpHeight, bool jumpInput)
        {
            if (jumpInput && CheckGroundLevel())
            {
                OnAir = true;
                GravityVelocity = new Vector3(0.0f, JumpSpeed, 0.0f);
            }

            if (Mathf.RoundToInt(transform.position.y) >= Mathf.RoundToInt(Mathf.Sqrt(JumpSpeed)))
            {
                OnAir = false;
            }

            if (!jumpInput && OnAir)
            {
                OnAir = false;
                GravityVelocity = new Vector3(0.0f, transform.position.y, 0.0f);
            }
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

        public void SetInput(IsometricInputHandler inputs)
        {
            m_direction = inputs.IsometricMoveDirection;
            
            switch (m_state)
            {
                case GameControllerState.Exploring:
                    m_jump = inputs.JumpInput;
                    break;
                case GameControllerState.Combat:
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
                case GameControllerState.Combat:
                    m_cursor.gameObject.SetActive(true);
                    CharacterController.enabled = false;
                    break;
            }
        }

        public bool CheckGroundLevel()
        {
            bool ground;

            ground = Physics.CheckSphere(transform.position - new Vector3(0, 0.5f, 0), GetComponent<CapsuleCollider>().radius / 0.85f, WhatIsGround, QueryTriggerInteraction.Collide);

            if (ground && GravityVelocity.y < 0 && !OnSlope())
            {
                GravityVelocity = new Vector3(GravityVelocity.x, 0.0f, GravityVelocity.z);
                OnAir = false;
            }
            return ground;
        }

        public float ApplyDrag(float velocity, float drag)
        {
            return velocity * (1 / (1 + drag * Time.deltaTime));
        }
    }
}
