using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IsometricOrientedPerspective;

namespace CharacterManager
{
    public class CharacterManager : MonoBehaviour
    {
        public class CharacterInputs
        {
            public bool m_inputStartMovement;
            public float m_horizontalAxi;
            public float m_verticalAxi;
            public bool m_movePoint;
            public Vector3 m_rotatePosition;

            public void UpdateInputs()
            { 
                m_horizontalAxi  = Input.GetAxis("Horizontal");
                m_verticalAxi = Input.GetAxis("Vertical");
                m_movePoint = Input.GetMouseButton(0);
                m_rotatePosition = Input.mousePosition; 
            }
        }

        [System.Serializable]
        public class CharacterSettings
        {
            [Header("Physics Settings")]
            public bool m_physicsMove;
            public bool m_physicsRotation;
            [Header("Movement Settings")]
            [Range(1,10)] public float m_movementSpeed;
            public float m_movementDistance;
            [Header("Rotation Settings")]
            [Range(0f, 100f)] public float m_rotationSpeed;
            public bool m_mouseRotation;
            [Header("Layer Settings")]
            public LayerMask m_layerMask;
        }

        #region Properties
        
        #endregion

        [SerializeField] private CharacterSettings m_settings = new CharacterSettings();
        private CharacterInputs m_inputs = new CharacterInputs();
        private IsometricMove m_isoMove;
        private IsometricRotation m_isoRotation;
        private AreaMovement m_area;
        
        private void Awake()
        {
            if (m_isoMove == null)
            {
                gameObject.AddComponent<IsometricMove>();
                m_isoMove = GetComponent<IsometricMove>();
            }

            if (m_isoRotation == null)
            {
                gameObject.AddComponent<IsometricRotation>();
                m_isoRotation = GetComponent<IsometricRotation>();
                m_isoRotation.enabled = false;
            }

            m_isoMove.Rigidbody = GetComponent<Rigidbody>();
            m_isoRotation.Rigidbody = GetComponent<Rigidbody>();

            Transform obj = Instantiate(Resources.Load<Transform>("Prefabs/MovementRadius"));

            m_area = obj.GetComponent<AreaMovement>();

            ApplySettings();
        }

        private void Start()
        {
            m_isoMove.Setup();
            m_isoRotation.Setup(m_settings.m_mouseRotation);
            m_area.SetupArea();
        }
        public void ApplySettings()
        {
            m_isoMove.IsPhysicsMovement = m_settings.m_physicsMove;
            m_isoMove.MoveDistance = m_settings.m_movementDistance;
            m_isoMove.MovementDelta = m_settings.m_movementSpeed;
            m_isoRotation.IsPhysicsRotation = m_settings.m_physicsRotation;
            m_isoRotation.RotationSensibility = m_settings.m_rotationSpeed;
            m_isoRotation.LayerMask = m_settings.m_layerMask;
            m_isoRotation.enabled = m_settings.m_mouseRotation;
        }

        private void Update()
        {
            m_isoMove.SetInputMoveDelta();
            m_inputs.UpdateInputs();

            m_isoMove.HorizontalMovement = m_inputs.m_horizontalAxi;
            m_isoMove.VerticalMovement = m_inputs.m_verticalAxi;
            m_isoMove.LeftClick = m_inputs.m_movePoint;
            m_isoRotation.RotatePosition = m_inputs.m_rotatePosition;
            m_isoRotation.RaycastHit = Camera.main.ScreenPointToRay(m_isoRotation.RotatePosition);

            if (m_isoRotation.enabled)
            {
                m_isoRotation.Rotate(m_isoRotation.RaycastHit, m_isoRotation.RotatePosition, m_isoRotation.LayerMask);

                if (Physics.Raycast(m_isoRotation.RaycastHit, out RaycastHit raycastHit, float.MaxValue, m_isoRotation.LayerMask))
                    if (m_isoMove.LeftClick && !m_isoMove.OnMove)
                    {
                        m_isoMove.Direction.direction = new Vector3(m_isoRotation.MouseCursor.position.x, transform.position.y, m_isoRotation.MouseCursor.position.z);

                        m_isoMove.OnMove = true;

                        m_isoMove.StartPosition = transform.position;
                    }

                if (m_isoMove.OnMove)
                    m_isoMove.Move(m_isoMove.Direction.direction.x, m_isoMove.Direction.direction.z);
            }           
            
            if (m_isoMove.IsPhysicsMovement) return;

            if (m_isoMove.OnMove)
            {
                m_isoMove.GetMoveDistance(m_isoMove.StartPosition);
                m_area.DrawCircle(100, m_isoMove.MoveDistance, new Vector3(m_isoMove.StartPosition.x, -0.85f, m_isoMove.StartPosition.z));
            }
            else
                m_area.DrawCircle(100, m_isoMove.MoveDistance, transform.position);

            IsometricCamera.m_instance.MoveBase();

            if (m_isoMove.MoveDelta != Vector2.zero && !m_isoRotation.enabled)
            {
                if (m_inputs.m_inputStartMovement)
                {
                    m_inputs.m_inputStartMovement = false;
                    m_isoMove.StartPosition = transform.position;
                }
                m_isoMove.Move(m_isoMove.MoveDelta.x, m_isoMove.MoveDelta.y);
            }
            else if (m_isoMove.MoveDelta == Vector2.zero && !m_isoRotation.enabled)
            {
                m_inputs.m_inputStartMovement = true;
                m_isoMove.OnMove = false;
            }
        }

        private void FixedUpdate()
        {
            if (!m_isoMove.IsPhysicsMovement) return;

            if (m_isoMove.OnMove)
            {
                m_isoMove.GetMoveDistance(m_isoMove.StartPosition);
                m_area.DrawCircle(100, m_isoMove.MoveDistance, new Vector3(m_isoMove.StartPosition.x, -0.85f, m_isoMove.StartPosition.z));
            }
            else
                m_area.DrawCircle(100, m_isoMove.MoveDistance, transform.position);

            IsometricCamera.m_instance.MoveBase();

            if (m_isoRotation.enabled)
            {
                if (m_isoMove.OnMove)
                    m_isoMove.Move(m_isoMove.Direction.direction.x, m_isoMove.Direction.direction.z);
            }
            else if (m_isoMove.MoveDelta != Vector2.zero && !m_isoRotation.enabled)
            {
                if (m_inputs.m_inputStartMovement)
                {
                    m_inputs.m_inputStartMovement = false;
                    m_isoMove.StartPosition = transform.position;
                }
                m_isoMove.Move(m_isoMove.MoveDelta.x, m_isoMove.MoveDelta.y);
            }
            else if (m_isoMove.MoveDelta == Vector2.zero && !m_isoRotation.enabled)
            {
                m_inputs.m_inputStartMovement = true;
                m_isoMove.OnMove = false;
            }
        }
    }
}
