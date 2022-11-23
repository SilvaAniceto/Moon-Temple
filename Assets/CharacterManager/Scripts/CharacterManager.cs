using UnityEngine;
using UnityEngine.Events;
using IOP;

namespace CharacterManager
{
    public class CharacterManager : MonoBehaviour
    {
        public class CharacterInputs
        {
            public bool inputStartMovement;
            public float horizontalAxi;
            public float verticalAxi;
            public bool movePoint;
            public bool jumpInput;
            public Vector3 rotatePosition;

            public void UpdateInputs()
            { 
                horizontalAxi  = Input.GetAxis("Horizontal");
                verticalAxi = Input.GetAxis("Vertical");
                movePoint = Input.GetMouseButton(0);
                rotatePosition = Input.mousePosition; 
                jumpInput = Input.GetButton("Jump") ? true : Input.GetButtonUp("Jump") ? false : false;
            }
        }
                
        [Header("Physics Settings")]
        [SerializeField] IsometricMove.MoveType moveType = IsometricMove.MoveType.FREE;
        [Header("Controller Type")]
        [SerializeField] IsometricOrientedPerspective.ControllType controllerType = IsometricOrientedPerspective.ControllType.KeyBoard;
        [Header("Movement Settings")]
        [Range(1,10)] public float movementSpeed;
        public float movementDistance;
        [Range(0, 45)] public float maxSlopeAngle;
        [Header("Layer Settings")]
        public LayerMask layerMask;
        [Header("Jump Settings")]
        [SerializeField] public float jumpDeltaTime;
        [Range(50f, 100f)] public  float heightDelta;

        #region Properties
        
        #endregion

        private CharacterInputs m_inputs = new CharacterInputs();
        private IsometricMove m_isoMove;
        private IsometricRotation m_isoRotation;
        private AreaMovement m_area;
        private JumpSystem m_jumpSystem;
        private Rigidbody m_rigidbody;
        private Ray m_ray;

        private void Awake()
        {
            if (m_isoMove == null)
            {
                gameObject.AddComponent<IsometricMove>();
                m_isoMove = GetComponent<IsometricMove>();
            }

            if (m_isoMove.OnMoveTypeChange == null)
                m_isoMove.OnMoveTypeChange = new UnityEvent<IsometricMove.MoveType>();

            if (m_isoRotation == null)
            {
                gameObject.AddComponent<IsometricRotation>();
                m_isoRotation = GetComponent<IsometricRotation>();
            }

            if (m_jumpSystem == null)
            {
                gameObject.AddComponent<JumpSystem>();
                m_jumpSystem = GetComponent<JumpSystem>();
            }

            GameObject obj = Instantiate(Resources.Load<GameObject>("Prefabs/MovementRadius"));

            m_area = obj.GetComponent<AreaMovement>();

            m_rigidbody = gameObject.GetComponent<Rigidbody>();

            ApplySettings();
        }

        private void Start()
        {
            m_isoMove.Setup();
            m_area.SetupArea();
            m_jumpSystem.Setup();

            m_isoMove.OnMoveTypeChange.AddListener(MoveTypeChange);
            IsometricOrientedPerspective.OnControllTypeChange.AddListener(m_isoRotation.Setup);
        }
        public void ApplySettings()
        {
            m_isoMove.MoveContext = moveType;

            m_isoMove.MovementDelta = movementSpeed;
            m_isoMove.MoveDistance = movementDistance;
            m_isoMove.MaxSlopeAngle = maxSlopeAngle;

            m_jumpSystem.LayerMask = layerMask;

            m_jumpSystem.JumpTime = jumpDeltaTime;
            m_jumpSystem.HeightDelta = heightDelta;

            MoveTypeChange(moveType);
        }

        void MoveTypeChange(IsometricMove.MoveType p_moveType)
        {
            bool value = p_moveType == IsometricMove.MoveType.COMBAT ? true : false;

            m_area.gameObject.SetActive(value);

            m_isoMove.MoveContext = moveType;
        }

        private void Update()
        {
            IsometricOrientedPerspective.Type = controllerType;

            m_isoMove.SetInputMoveDelta(IsometricOrientedPerspective.Type);
            m_inputs.UpdateInputs();

            m_isoMove.LeftClick = m_inputs.movePoint;
            m_isoMove.HorizontalMovement = m_inputs.horizontalAxi;
            m_isoMove.VerticalMovement = m_inputs.verticalAxi;

            m_ray = Camera.main.ScreenPointToRay(m_inputs.rotatePosition);

            m_jumpSystem.JumpInput = m_inputs.jumpInput;

            if (m_isoMove.MoveDelta == Vector2.zero && m_isoMove.OnSlope())
                m_jumpSystem.OnSlope = m_isoMove.OnSlope();

            if (m_isoMove.MoveDelta != Vector2.zero && m_isoMove.OnSlope())
                m_jumpSystem.OnSlope = !m_isoMove.OnSlope();

            #region DEPRECATED
            //if (m_isoRotation.enabled)
            //{
            //    m_isoRotation.Rotate(m_isoRotation.RaycastHit, m_isoRotation.RotatePosition, m_isoRotation.LayerMask, m_settings.physicsMove, m_rigidbody);

            //    if (Physics.Raycast(m_isoRotation.RaycastHit, out RaycastHit raycastHit, float.MaxValue, m_isoRotation.LayerMask))
            //        if (m_isoMove.LeftClick && !m_isoMove.OnMove)
            //        {
            //            m_isoMove.Direction.direction = new Vector3(m_isoRotation.MouseCursor.position.x, transform.position.y, m_isoRotation.MouseCursor.position.z);

            //            m_isoMove.OnMove = true;

            //            m_isoMove.StartPosition = transform.position;
            //        }

            //    if (m_isoMove.OnMove)
            //        m_isoMove.Move(m_isoMove.Direction.direction.x, m_isoMove.Direction.direction.z);
            //}           

            //if (m_settings.physicsMove) return;

            //if (m_isoMove.OnMove)
            //{
            //    m_isoMove.GetMoveDistance(m_isoMove.StartPosition);
            //    m_area.DrawCircle(100, m_isoMove.MoveDistance, new Vector3(m_isoMove.StartPosition.x, -0.85f, m_isoMove.StartPosition.z));
            //}
            //else
            //    m_area.DrawCircle(100, m_isoMove.MoveDistance, transform.position);

            //IsometricCamera.m_instance.MoveBase();

            //if (m_isoMove.MoveDelta != Vector2.zero && !m_isoRotation.enabled)
            //{
            //    if (m_inputs.inputStartMovement)
            //    {
            //        m_inputs.inputStartMovement = false;
            //        m_isoMove.StartPosition = transform.position;
            //    }
            //    m_isoMove.Move(m_isoMove.MoveDelta.x, m_isoMove.MoveDelta.y, m_rigidbody);
            //}
            //else if (m_isoMove.MoveDelta == Vector2.zero && !m_isoRotation.enabled)
            //{
            //    m_inputs.inputStartMovement = true;
            //    m_isoMove.OnMove = false;
            //}
            #endregion

            switch (controllerType)
            {
                case IsometricOrientedPerspective.ControllType.PointAndClick:
                    m_isoRotation.Rotate(m_ray, m_inputs.rotatePosition, layerMask);
                    break;
                case IsometricOrientedPerspective.ControllType.KeyBoard:
                    m_isoMove.Move(m_isoMove.MoveDelta.x, m_isoMove.MoveDelta.y, m_rigidbody);
                    break;
            }

            IsometricCamera.m_instance.MoveBase();
        }

        private void FixedUpdate()
        {
            m_jumpSystem.Jump();

            #region DEPRECATED
            //if (!m_settings.physicsMove) return;

            //if (m_isoMove.OnMove)
            //{
            //    m_isoMove.GetMoveDistance(m_isoMove.StartPosition);
            //    m_area.DrawCircle(100, m_isoMove.MoveDistance, new Vector3(m_isoMove.StartPosition.x, -0.85f, m_isoMove.StartPosition.z));
            //}
            //else
            //    m_area.DrawCircle(100, m_isoMove.MoveDistance, transform.position);

            

            //if (m_isoRotation.enabled)
            //{
            //    if (m_isoMove.OnMove)
            //        m_isoMove.Move(m_isoMove.Direction.direction.x, m_isoMove.Direction.direction.z, m_rigidbody);
            //}
            //else if (m_isoMove.MoveDelta != Vector2.zero && !m_isoRotation.enabled)
            //{
            //    if (m_inputs.inputStartMovement)
            //    {
            //        m_inputs.inputStartMovement = false;
            //        m_isoMove.StartPosition = transform.position;
            //    }
            //    m_isoMove.Move(m_isoMove.MoveDelta.x, m_isoMove.MoveDelta.y, m_rigidbody);
            //}
            //else if (m_isoMove.MoveDelta == Vector2.zero && !m_isoRotation.enabled)
            //{
            //    m_inputs.inputStartMovement = true;
            //    m_isoMove.OnMove = false;
            //}
            #endregion
        }
    }
}
