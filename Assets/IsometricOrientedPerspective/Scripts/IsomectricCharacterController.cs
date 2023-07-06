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
        [SerializeField][Range (-20, 0)] private float m_gravity;

        private Vector3 m_gravityVector;
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
                m_characterController.slopeLimit = m_maxSlopeAngle;
            }
        }

        void Update()
        {
            if (OnGround() && m_gravityVector.y < 0)
            {
                m_gravityVector.y = 0f;
            }

            Vector3 direction = AdjustInputMoveDirection(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

            Vector3 right = direction.x * IsometricOrientedPerspective.IsometricRight;
            Vector3 forward = direction.z * IsometricOrientedPerspective.IsometricForward;

            Vector3 move = right + forward + Vector3.zero;
            if (OnSlope()) m_characterController.Move(GetSlopeMoveDirection(move) * Time.deltaTime * m_movementSpeed);
            else m_characterController.Move(move * Time.deltaTime * m_movementSpeed);

            if (move != Vector3.zero)
            {
                gameObject.transform.forward = move;
            }

            // Changes the height position of the player..
            //if (Input.GetButtonDown("Jump") && m_jumpSystem.OnGroundLevel)
            //{
            //    playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
            //}

            m_gravityVector.y += m_gravity * Time.deltaTime;
            m_characterController.Move(m_gravityVector * Time.deltaTime);
        }

        private bool OnSlope()
        {
            if (Physics.SphereCast(transform.position - new Vector3(0, 0.5f, 0), m_capsuleCollider.radius / 0.85f, Vector3.down, out RaycastHit hitInfo, Mathf.Infinity, m_layerMask, QueryTriggerInteraction.Collide))
            {
                float slopeAngle = Vector3.Angle(Vector3.up, hitInfo.normal);
                return slopeAngle < m_maxSlopeAngle && slopeAngle != 0;
            }
            return false;
        }

        private float SlopeAngle()
        {
            if (Physics.SphereCast(transform.position - new Vector3(0, 0.5f, 0), m_capsuleCollider.radius / 0.85f, Vector3.down, out RaycastHit hitInfo, Mathf.Infinity, m_layerMask, QueryTriggerInteraction.Collide))
            {
                float slopeAngle = Vector3.Angle(Vector3.up, hitInfo.normal);
                return slopeAngle;
            }
            return 0;
        }

        private RaycastHit SlopeHit()
        {
            Physics.SphereCast(transform.position - new Vector3(0, 0.5f, 0), m_capsuleCollider.radius / 0.85f, Vector3.down, out RaycastHit hitInfo, Mathf.Infinity, m_layerMask, QueryTriggerInteraction.Collide);
            return hitInfo;
        }

        private Vector3 GetSlopeMoveDirection(Vector3 direction)
        {
            return Vector3.ProjectOnPlane(new Vector3(direction.x, (int)(direction.y * (int)SlopeAngle()), direction.z), SlopeHit().normal).normalized;
        }

        private bool OnGround()
        {
            bool ground = false;

            ground = Physics.CheckSphere(transform.position - new Vector3(0, 0.5f, 0), m_capsuleCollider.radius /0.85f, m_layerMask, QueryTriggerInteraction.Collide);
           
            return ground;
        }

        private Vector3 AdjustInputMoveDirection(float x, float y)
        {
            Vector3 vector3 = Vector3.zero;

            if (IsometricCamera.m_instance.CamPosition == IsometricCamera.CameraPosition.SOUTH)
                vector3 = new Vector3(x, 0, y);

            if (IsometricCamera.m_instance.CamPosition == IsometricCamera.CameraPosition.WEST)
                vector3 = new Vector3 (y, 0, -x);

            if (IsometricCamera.m_instance.CamPosition == IsometricCamera.CameraPosition.NORTH)
                vector3 = new Vector3(-x,  0, -y);

            if (IsometricCamera.m_instance.CamPosition == IsometricCamera.CameraPosition.EAST)
                vector3 = new Vector3(-y, 0, x);

            return vector3;
        }

        private void OnDrawGizmos()
        {
            if (m_capsuleCollider == null) return;
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position - new Vector3(0, 0.5f, 0), m_capsuleCollider.radius / 0.85f);
        }
    }
}
