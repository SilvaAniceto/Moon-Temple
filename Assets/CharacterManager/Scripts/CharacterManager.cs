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
            public bool m_leftClick;
            public Vector3 m_rotatePosition;

            public void UpdateInputs()
            { 
                m_horizontalAxi  = Input.GetAxis("Horizontal");
                m_verticalAxi = Input.GetAxis("Vertical");
                m_leftClick = Input.GetMouseButtonDown(0);
                m_rotatePosition = Input.mousePosition; 
            }
        }

        [System.Serializable]
        public class CharacterSettings
        {
            public bool m_physicsMove;
            public float m_movementDistance;
            [Range(1,10)] public float m_movementSpeed;
        }
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

        private CharacterInputs m_inputs = new CharacterInputs();
        [SerializeField] private CharacterSettings m_settings = new CharacterSettings();

        private void Awake()
        {
            if (IsoMovement == null)
            {
                gameObject.AddComponent<IsometricMove>();
                IsoMovement = GetComponent<IsometricMove>();
            }

            IsoMovement.MovementSetup();
            IsoMovement.Rigidbody = GetComponent<Rigidbody>();
        }
        private void Start()
        {
            IsoMovement.IsPhysicsMovement = m_settings.m_physicsMove;
            IsoMovement.MoveDistance = m_settings.m_movementDistance;
            IsoMovement.MovementDelta = m_settings.m_movementSpeed;
        }

        private void Update()
        {
            IsoMovement.SetInputMoveDelta();
            m_inputs.UpdateInputs();

            IsoMovement.HorizontalMovement = m_inputs.m_horizontalAxi;
            IsoMovement.VerticalMovement = m_inputs.m_verticalAxi;
            IsoMovement.LeftClick = m_inputs.m_leftClick;
            IsoMovement.RotatePosition = m_inputs.m_rotatePosition;
            IsoMovement.RaycastHit = Camera.main.ScreenPointToRay(IsoMovement.RotatePosition);

            if (IsometricRotation.m_rotationInstance.enabled)
            {
                if (Physics.Raycast(IsoMovement.RaycastHit, out RaycastHit raycastHit, float.MaxValue, IsometricRotation.m_rotationInstance.LayerMask))
                    if (IsoMovement.LeftClick && IsoMovement.OnMove)
                    {
                        IsoMovement.Direction.direction = new Vector3(IsometricRotation.m_rotationInstance.MouseCursor.position.x, transform.position.y, IsometricRotation.m_rotationInstance.MouseCursor.position.z);

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
                AreaMovement.m_areaMovementInstance.DrawCircle(100, IsoMovement.MoveDistance, new Vector3(IsoMovement.StartPosition.x, -0.85f, IsoMovement.StartPosition.z));
            }
            else
                AreaMovement.m_areaMovementInstance.DrawCircle(100, IsoMovement.MoveDistance, transform.position);

            IsometricCamera.m_instance.MoveBase();

            if (IsoMovement.MoveDelta != Vector2.zero && !IsometricRotation.m_rotationInstance.enabled)
            {
                if (m_inputs.m_inputStartMovement)
                {
                    m_inputs.m_inputStartMovement = false;
                    IsoMovement.StartPosition = transform.position;
                }
                IsoMovement.Move(IsoMovement.MoveDelta.x, IsoMovement.MoveDelta.y);
            }
            else if (IsoMovement.MoveDelta == Vector2.zero && !IsometricRotation.m_rotationInstance.enabled)
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
                AreaMovement.m_areaMovementInstance.DrawCircle(100, IsoMovement.MoveDistance, new Vector3(IsoMovement.StartPosition.x, -0.85f, IsoMovement.StartPosition.z));
            }
            else
                AreaMovement.m_areaMovementInstance.DrawCircle(100, IsoMovement.MoveDistance, transform.position);

            IsometricCamera.m_instance.MoveBase();

            if (IsometricRotation.m_rotationInstance.enabled)
            {
                if (IsoMovement.OnMove)
                    IsoMovement.Move(IsoMovement.Direction.direction.x, IsoMovement.Direction.direction.z);
            }
            else if (IsoMovement.MoveDelta != Vector2.zero && !IsometricRotation.m_rotationInstance.enabled)
            {
                if (m_inputs.m_inputStartMovement)
                {
                    m_inputs.m_inputStartMovement = false;
                    IsoMovement.StartPosition = transform.position;
                }
                IsoMovement.Move(IsoMovement.MoveDelta.x, IsoMovement.MoveDelta.y);
            }
            else if (IsoMovement.MoveDelta == Vector2.zero && !IsometricRotation.m_rotationInstance.enabled)
            {
                m_inputs.m_inputStartMovement = true;
                IsoMovement.OnMove = false;
            }
        }
    }
}
