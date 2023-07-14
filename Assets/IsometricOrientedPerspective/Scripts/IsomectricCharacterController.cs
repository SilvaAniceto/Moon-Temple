using CharactersCreator;
using UnityEngine;

namespace IOP
{
    [RequireComponent(typeof(CapsuleCollider))]
    public class IsomectricCharacterController : MonoBehaviour
    {
        public static IsomectricCharacterController Instance { get; private set; }

        [SerializeField] private LayerMask m_layerMask;
        [SerializeField] private float m_maxSlopeAngle = 45f;
        [SerializeField] private float m_movementSpeed = 8;
        [SerializeField][Range(1f, 5)] private float m_acceleration = 2.5f;
        [SerializeField][Range(1f, 10)] private float m_rotation = 5f;
        [SerializeField][Range(1.2f, 3)] private float m_jumpHeight = 1.5f;
        [SerializeField][Range(-10f, -1.2f)] private float m_jumpForce = -3.0f;

        private CapsuleCollider m_capsuleCollider;
        private CharacterController m_characterController;
        private IsometricGravity Gravity = new IsometricGravity();
        private Vector3 m_currentVelocity;
        private void Awake()
        {
            m_capsuleCollider = GetComponent<CapsuleCollider>();
            Gravity.Collider = m_capsuleCollider;
            Gravity.LayerMask = m_layerMask;
        }
        private void Start()
        {
            if (m_characterController == null)
            {
                m_characterController = this.gameObject.AddComponent<CharacterController>();
                m_characterController.slopeLimit = 0;
            }
        }

        void Update()
        {
            Gravity.Position = transform.position;

            Vector3 direction = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            float speed = Input.GetKey(KeyCode.LeftShift) ? m_movementSpeed * 2f : m_movementSpeed;
            float height = Input.GetKey(KeyCode.LeftShift) ? m_jumpHeight * 1.5f : m_jumpHeight;

            Vector3 right = direction.x * IsometricOrientedPerspective.IsometricRight;
            Vector3 forward = direction.z * IsometricOrientedPerspective.IsometricForward;

            Vector3 move = right + forward + Vector3.zero;

            m_currentVelocity = Vector3.MoveTowards(m_currentVelocity, move, m_acceleration * Time.deltaTime);

            if (OnSlope() && IsometricGravity.OnGround()) m_characterController.Move(GetSlopeMoveDirection(m_currentVelocity) * Time.deltaTime * speed);
            else m_characterController.Move(m_currentVelocity * Time.deltaTime * speed);

            if (move != Vector3.zero)
            {
               transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(move), m_rotation * Time.deltaTime);
            }
            
            IsometricGravity.Jump(height, m_jumpForce, Input.GetButton("Jump") ? true : Input.GetButtonUp("Jump") ? false : false);

            m_characterController.Move(Gravity.Vector * Time.deltaTime);
        }

        private bool OnSlope()
        {
            if (Physics.SphereCast(transform.position - new Vector3(0, 0.5f, 0), m_capsuleCollider.radius, -transform.up, out RaycastHit hitInfo, Mathf.Infinity, m_layerMask, QueryTriggerInteraction.Collide))
            {
                float slopeAngle = Mathf.RoundToInt(Vector3.Angle(Vector3.up, hitInfo.normal));
                return slopeAngle <= m_maxSlopeAngle && slopeAngle != 0;
            }
            return false;
        }

        private float SlopeAngle()
        {
            if (Physics.SphereCast(transform.position - new Vector3(0, 0.5f, 0), m_capsuleCollider.radius, Vector3.down, out RaycastHit hitInfo, Mathf.Infinity, m_layerMask, QueryTriggerInteraction.Collide))
            {
                float slopeAngle = Mathf.RoundToInt(Vector3.Angle(Vector3.up, hitInfo.normal));
                return slopeAngle;
            }
            return 0;
        }

        private RaycastHit SlopeHit()
        {
            Physics.SphereCast(transform.position - new Vector3(0, 0.5f, 0), m_capsuleCollider.radius, Vector3.down, out RaycastHit hitInfo, Mathf.Infinity, m_layerMask, QueryTriggerInteraction.Collide);
            return hitInfo;
        }

        private Vector3 GetSlopeMoveDirection(Vector3 direction)
        {
            return Vector3.ProjectOnPlane(new Vector3(direction.x, (int)(direction.y * (int)SlopeAngle()), direction.z), SlopeHit().normal).normalized;
        }
    }
}
