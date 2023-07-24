using TreeEditor;
using UnityEngine;

namespace IOP
{
    [RequireComponent(typeof(CapsuleCollider))]
    public class IsomectricCharacterController : IsometricGravity, IIsometricController
    {
        public static IsomectricCharacterController Instance { get; private set; }
        public Vector3 CurrentyVelocity { get; set; }
        public CapsuleCollider CapsuleCollider { get; set; }
        public CharacterController CharacterController { get; set; }
        
        [SerializeField] private LayerMask m_layerMask;
        [SerializeField] private float m_maxSlopeAngle = 45f;
        [SerializeField] private float m_movementSpeed = 8;
        [SerializeField][Range(1f, 5)] private float m_acceleration = 2.5f;
        [SerializeField][Range(1f, 10)] private float m_rotation = 5f;
        [SerializeField][Range(1.2f, 3)] private float m_jumpHeight = 1.5f;

        private void Awake()
        {
            CapsuleCollider = GetComponent<CapsuleCollider>();
            Collider = CapsuleCollider;
            LayerMask = m_layerMask;
        }

        private void Start()
        {
            if (CharacterController == null)
            {
                CharacterController = gameObject.AddComponent<CharacterController>();
                CharacterController.slopeLimit = 0;
            }
        }

        void Update()
        {
            Vector3 direction = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            bool jumpInput = Input.GetButton("Jump") ? true : Input.GetButtonUp("Jump") ? false : false;
            float speed = Input.GetKey(KeyCode.LeftShift) ? m_movementSpeed * 2f : m_movementSpeed;
            float height = Input.GetKey(KeyCode.LeftShift) ? m_jumpHeight * 1.5f : m_jumpHeight;

            Position = transform.position;

            UpdateMovePosition(direction.normalized, speed);
            Jump(height, jumpInput);
        }

        public void UpdateMovePosition(Vector3 inputDirection, float movementSpeed)
        {
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
    }
}
