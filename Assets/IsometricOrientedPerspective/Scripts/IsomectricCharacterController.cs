using CharacterManager;
using System.Collections;
using System.Collections.Generic;
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
        [SerializeField][Range (-20, 0)] private float m_gravity;
        [SerializeField][Range(0.1f, 1)] private float m_groundRadiusCheck = 0.85f;
        [SerializeField][Range(0.1f, 1)] private float m_slopeRadiusCheck = 0.85f;

        [SerializeField] private Vector3 m_gravityVector;
        private CapsuleCollider m_capsuleCollider;
        private CharacterController m_characterController;
        private void Awake()
        {
            m_capsuleCollider = GetComponent<CapsuleCollider>();
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
            if (OnGround() && m_gravityVector.y < 0)
            {
                m_gravityVector.y = 0f;
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

            //Changes the height position of the player..
            if (Input.GetButtonDown("Jump") && OnGround())
            {
                m_gravityVector.y += Mathf.Sqrt(m_jumpHeight * -3.0f * m_gravity);
            }

            m_gravityVector.y += m_gravity * Time.deltaTime;
            m_characterController.Move(m_gravityVector * Time.deltaTime);
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

        private bool OnGround()
        {
            bool ground;

            ground = Physics.CheckSphere(transform.position - new Vector3(0, 0.5f, 0), m_capsuleCollider.radius / m_groundRadiusCheck, m_layerMask, QueryTriggerInteraction.Collide);
           
            return ground;
        }

        //private void OnDrawGizmos()
        //{
        //    if (m_capsuleCollider == null) return;
        //    Gizmos.color = Color.red;
        //    Gizmos.DrawWireSphere(transform.position - new Vector3(0, 0.5f, 0), m_capsuleCollider.radius *  m_slopeRadiusCheck);
        //}
    }
}
