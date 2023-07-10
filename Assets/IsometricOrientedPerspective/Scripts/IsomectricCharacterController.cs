using CharactersCreator;
using UnityEngine;

namespace IOP
{
    [RequireComponent(typeof(CapsuleCollider))]
    public class IsomectricCharacterController : MonoBehaviour
    {
        public static IsomectricCharacterController Instance { get; private set; }

        [SerializeField] private LayerMask m_layerMask;
        [SerializeField] private float m_maxSlopeAngle;
        [SerializeField] private float m_movementSpeed;
        [SerializeField][Range(1, 3)] private float m_jumpHeight;
        [SerializeField][Range(-10f, -1.2f)] private float m_jumpForce = -3.0f;

        [Range(0.1f, 1)] private float m_groundRadiusCheck = 0.85f;
        [Range(0.1f, 1)] private float m_slopeRadiusCheck = 1f;

        private CapsuleCollider m_capsuleCollider;
        private CharacterController m_characterController;
        private IsometricGravity Gravity = new IsometricGravity() { Vector = Vector3.zero};

        private void Awake()
        {
            m_capsuleCollider = GetComponent<CapsuleCollider>();
            Gravity.Collider = m_capsuleCollider;
            Gravity.GroundRadiusCheck = m_groundRadiusCheck;
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

            if (IsometricGravity.OnGround() && Gravity.Vector.y < 0)
            {
                Gravity.Vector = Vector3.zero;
            }
            Vector3 direction = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

            Vector3 right = direction.x * IsometricOrientedPerspective.IsometricRight;
            Vector3 forward = direction.z * IsometricOrientedPerspective.IsometricForward;

            Vector3 move = right + forward + Vector3.zero;

            if (OnSlope()) m_characterController.Move(GetSlopeMoveDirection(move) * Time.deltaTime * m_movementSpeed);
            else m_characterController.Move(move * Time.deltaTime * m_movementSpeed);

            if (move != Vector3.zero)
            {
                gameObject.transform.forward = move;
            }

            if (Input.GetButtonDown("Jump") && IsometricGravity.OnGround())
            {
                Gravity.Vector += IsometricGravity.Jump(m_jumpHeight, m_jumpForce);
            }

            Gravity.Vector += IsometricGravity.GravityForce();
            m_characterController.Move(Gravity.Vector * Time.deltaTime);
        }

        private bool OnSlope()
        {
            if (Physics.SphereCast(transform.position - new Vector3(0, 0.5f, 0), m_capsuleCollider.radius / m_slopeRadiusCheck, -transform.up, out RaycastHit hitInfo, Mathf.Infinity, m_layerMask, QueryTriggerInteraction.Collide))
            {
                float slopeAngle = Mathf.RoundToInt(Vector3.Angle(Vector3.up, hitInfo.normal));
                return slopeAngle <= m_maxSlopeAngle && slopeAngle != 0;
            }
            return false;
        }

        private float SlopeAngle()
        {
            if (Physics.SphereCast(transform.position - new Vector3(0, 0.5f, 0), m_capsuleCollider.radius / m_slopeRadiusCheck, Vector3.down, out RaycastHit hitInfo, Mathf.Infinity, m_layerMask, QueryTriggerInteraction.Collide))
            {
                float slopeAngle = Mathf.RoundToInt(Vector3.Angle(Vector3.up, hitInfo.normal));
                return slopeAngle;
            }
            return 0;
        }

        private RaycastHit SlopeHit()
        {
            Physics.SphereCast(transform.position - new Vector3(0, 0.5f, 0), m_capsuleCollider.radius / m_slopeRadiusCheck, Vector3.down, out RaycastHit hitInfo, Mathf.Infinity, m_layerMask, QueryTriggerInteraction.Collide);
            return hitInfo;
        }

        private Vector3 GetSlopeMoveDirection(Vector3 direction)
        {
            return Vector3.ProjectOnPlane(new Vector3(direction.x, (int)(direction.y * (int)SlopeAngle()), direction.z), SlopeHit().normal).normalized;
        }
    }
}
