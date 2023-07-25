using UnityEngine;
using UnityEngine.Events;

namespace IsometricGameController
{
    [RequireComponent(typeof(CapsuleCollider))]
    public class IsomectricCharacterController : IsometricGravity, IIsometricController
    {
        public static IsomectricCharacterController Instance { get; private set; }
        public Vector3 CurrentyVelocity { get; set; }
        public CapsuleCollider CapsuleCollider { get; set; }
        public CharacterController CharacterController { get; set; }
        public GameControllerState ControllerState 
        { 
            get => m_state; 
            set 
            {
                m_state = value;
                OnGameStateChanged?.Invoke(ControllerState); 
            } 
        }

        [SerializeField] private LayerMask m_layerMask;
        [SerializeField] private float m_maxSlopeAngle = 45f;
        [SerializeField] private float m_movementSpeed = 8;

        [SerializeField][Range(1f, 5)] private float m_acceleration = 2.5f;
        [SerializeField][Range(1f, 10)] private float m_rotation = 5f;
        [SerializeField][Range(1.2f, 3)] private float m_jumpHeight = 1.5f;

        public static UnityEvent<GameControllerState> OnGameStateChanged = new UnityEvent<GameControllerState>();

        [SerializeField] private Transform m_cursor;

        private GameControllerState m_state;
        private Vector3 m_direction;
        private bool m_confirm;
        private bool m_accelerate;
        private bool m_jump;

        private void Awake()
        {
            if (Instance == null) Instance = this;

            CapsuleCollider = GetComponent<CapsuleCollider>();
            Collider = CapsuleCollider;
            LayerMask = m_layerMask;

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
            float speed = m_accelerate ? m_movementSpeed * 2f : m_movementSpeed;
            float height = m_accelerate ? m_jumpHeight * 1.5f : m_jumpHeight;

            UpdateMovePosition(m_direction.normalized, speed);
            Jump(height, m_jump);
        }

        public void UpdateMovePosition(Vector3 inputDirection, float movementSpeed)
        {
            Position = transform.position;

            Vector3 right = inputDirection.x * IsometricOrientedPerspective.IsometricRight;
            Vector3 forward = inputDirection.z * IsometricOrientedPerspective.IsometricForward;

            Vector3 move = right + forward + Vector3.zero;

            CurrentyVelocity = Vector3.MoveTowards(CurrentyVelocity, move, m_acceleration * Time.deltaTime);

            if (OnSlope() && OnGround()) CharacterController.Move(GetSlopeMoveDirection(CurrentyVelocity) * Time.deltaTime * movementSpeed);
            else CharacterController.Move(CurrentyVelocity * Time.deltaTime * movementSpeed);

            if (move != Vector3.zero)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(move), m_rotation * Time.deltaTime);
            }

            CharacterController.Move(Vector * Time.deltaTime);

            if (ControllerState == GameControllerState.Combat)
            {
                m_cursor.forward = IsometricOrientedPerspective.IsometricForward;
                m_cursor.Translate(inputDirection * Time.deltaTime * movementSpeed * m_acceleration);

                transform.LookAt(new Vector3(m_cursor.position.x, transform.position.y, m_cursor.position.z));
            }
        }

        public bool OnSlope()
        {
            if (Physics.SphereCast(transform.position - new Vector3(0, 0.5f, 0), CapsuleCollider.radius, Vector3.down, out RaycastHit hitInfo, CapsuleCollider.radius / 2, m_layerMask, QueryTriggerInteraction.Collide))
            {
                float slopeAngle = Mathf.RoundToInt(Vector3.Angle(Vector3.up, hitInfo.normal));
                return slopeAngle <= m_maxSlopeAngle && slopeAngle != 0;
            }
            return false;
        }

        public float SlopeAngle()
        {
            if (Physics.SphereCast(transform.position - new Vector3(0, 0.5f, 0), CapsuleCollider.radius, Vector3.down, out RaycastHit hitInfo, CapsuleCollider.radius / 2, m_layerMask, QueryTriggerInteraction.Collide))
            {
                float slopeAngle = Mathf.RoundToInt(Vector3.Angle(Vector3.up, hitInfo.normal));
                return slopeAngle;
            }
            return 0;
        }

        public RaycastHit SlopeHit()
        {
            Physics.SphereCast(transform.position - new Vector3(0, 0.5f, 0), CapsuleCollider.radius, Vector3.down, out RaycastHit hitInfo, CapsuleCollider.radius / 2, m_layerMask, QueryTriggerInteraction.Collide);
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
    }
}
