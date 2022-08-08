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
        public IsometricMove IsoMovement
        {
            get
            {
                return IsometricMove.m_moveInstance;
            }

            set
            {
                if (IsometricMove.m_moveInstance == value)
                    return;

                IsometricMove.m_moveInstance = value;
            }
        }
        public IsometricRotation IsoRotation
        {
            get
            {
                return IsometricRotation.m_rotationInstance;
            }

            set
            {
                if (IsometricRotation.m_rotationInstance == value)
                    return;

                IsometricRotation.m_rotationInstance = value;
            }
        }
        #endregion

        [SerializeField] private CharacterSettings m_settings = new CharacterSettings();
        private CharacterInputs m_inputs = new CharacterInputs();
        private AreaMovement m_area;
        
        private void Awake()
        {
            if (IsoMovement == null)
            {
                gameObject.AddComponent<IsometricMove>();
                IsoMovement = GetComponent<IsometricMove>();
            }

            if (IsoRotation == null)
            {
                gameObject.AddComponent<IsometricRotation>();
                IsoRotation = GetComponent<IsometricRotation>();
                IsoRotation.enabled = false;
            }

            IsoMovement.Rigidbody = GetComponent<Rigidbody>();
            IsoRotation.Rigidbody = GetComponent<Rigidbody>();

            Transform obj = Instantiate(Resources.Load<Transform>("Prefabs/MovementRadius"));

            m_area = obj.GetComponent<AreaMovement>();

            ApplySettings();
        }

        private void Start()
        {
            IsoMovement.Setup();
            IsoRotation.Setup(m_settings.m_mouseRotation);            
        }
        public void ApplySettings()
        {
            IsoMovement.IsPhysicsMovement = m_settings.m_physicsMove;
            IsoMovement.MoveDistance = m_settings.m_movementDistance;
            IsoMovement.MovementDelta = m_settings.m_movementSpeed;
            IsoRotation.IsPhysicsRotation = m_settings.m_physicsRotation;
            IsoRotation.RotationSensibility = m_settings.m_rotationSpeed;
            IsoRotation.LayerMask = m_settings.m_layerMask;
            IsoRotation.enabled = m_settings.m_mouseRotation;
        }

        private void Update()
        {
            IsoMovement.SetInputMoveDelta();
            m_inputs.UpdateInputs();

            IsoMovement.HorizontalMovement = m_inputs.m_horizontalAxi;
            IsoMovement.VerticalMovement = m_inputs.m_verticalAxi;
            IsoRotation.LeftClick = m_inputs.m_movePoint;
            IsoRotation.RotatePosition = m_inputs.m_rotatePosition;
            IsoRotation.RaycastHit = Camera.main.ScreenPointToRay(IsoRotation.RotatePosition);

            if (IsoRotation.enabled)
            {
                IsoRotation.Rotate(IsoRotation.RaycastHit, IsoRotation.RotatePosition, IsoRotation.LayerMask);

                if (Physics.Raycast(IsoRotation.RaycastHit, out RaycastHit raycastHit, float.MaxValue, IsoRotation.LayerMask))
                    if (IsoRotation.LeftClick && !IsoMovement.OnMove)
                    {
                        IsoMovement.Direction.direction = new Vector3(IsoRotation.MouseCursor.position.x, transform.position.y, IsoRotation.MouseCursor.position.z);

                        IsoMovement.OnMove = true;

                        IsoMovement.StartPosition = transform.position;
                    }

                if (IsoMovement.OnMove)
                    IsoMovement.Move(IsoMovement.Direction.direction.x, IsoMovement.Direction.direction.z);
            }           
            
            if (IsoMovement.IsPhysicsMovement) return;

            if (IsoMovement.OnMove)
            {
                IsoMovement.GetMoveDistance(IsoMovement.StartPosition);
                m_area.DrawCircle(100, IsoMovement.MoveDistance, new Vector3(IsoMovement.StartPosition.x, -0.85f, IsoMovement.StartPosition.z));
            }
            else
                m_area.DrawCircle(100, IsoMovement.MoveDistance, transform.position);

            IsometricCamera.m_instance.MoveBase();

            if (IsoMovement.MoveDelta != Vector2.zero && !IsoRotation.enabled)
            {
                if (m_inputs.m_inputStartMovement)
                {
                    m_inputs.m_inputStartMovement = false;
                    IsoMovement.StartPosition = transform.position;
                }
                IsoMovement.Move(IsoMovement.MoveDelta.x, IsoMovement.MoveDelta.y);
            }
            else if (IsoMovement.MoveDelta == Vector2.zero && !IsoRotation.enabled)
            {
                m_inputs.m_inputStartMovement = true;
                IsoMovement.OnMove = false;
            }
        }

        private void FixedUpdate()
        {
            if (!IsoMovement.IsPhysicsMovement) return;

            if (IsoMovement.OnMove)
            {
                IsoMovement.GetMoveDistance(IsoMovement.StartPosition);
                m_area.DrawCircle(100, IsoMovement.MoveDistance, new Vector3(IsoMovement.StartPosition.x, -0.85f, IsoMovement.StartPosition.z));
            }
            else
                m_area.DrawCircle(100, IsoMovement.MoveDistance, transform.position);

            IsometricCamera.m_instance.MoveBase();

            if (IsoRotation.enabled)
            {
                if (IsoMovement.OnMove)
                    IsoMovement.Move(IsoMovement.Direction.direction.x, IsoMovement.Direction.direction.z);
            }
            else if (IsoMovement.MoveDelta != Vector2.zero && !IsoRotation.enabled)
            {
                if (m_inputs.m_inputStartMovement)
                {
                    m_inputs.m_inputStartMovement = false;
                    IsoMovement.StartPosition = transform.position;
                }
                IsoMovement.Move(IsoMovement.MoveDelta.x, IsoMovement.MoveDelta.y);
            }
            else if (IsoMovement.MoveDelta == Vector2.zero && !IsoRotation.enabled)
            {
                m_inputs.m_inputStartMovement = true;
                IsoMovement.OnMove = false;
            }
        }
    }
}
