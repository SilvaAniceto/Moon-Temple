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
            public bool leftClick;
            public bool jumpInput;
            public Vector3 rotatePosition;

            public void UpdateInputs()
            { 
                horizontalAxi  = Input.GetAxis("Horizontal");
                verticalAxi = Input.GetAxis("Vertical");
                leftClick = Input.GetMouseButton(0);
                rotatePosition = Input.mousePosition; 
                jumpInput = Input.GetButton("Jump") ? true : Input.GetButtonUp("Jump") ? false : false;
            }
        }

        #region CHARACTER SETTINGS
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
        #endregion

        #region Properties

        #endregion

        private CharacterInputs m_inputs = new CharacterInputs();
        private IsometricMove m_isoMove;
        private IsometricRotation m_isoRotation;
        private AreaMovement m_area;
        private JumpSystem m_jumpSystem;
        private Rigidbody m_rigidbody;
        private RaycastHit m_raycastHit;

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
            m_isoMove.Setup(controllerType);
            m_isoRotation.Setup(controllerType);

            m_area.SetupArea();
            m_jumpSystem.Setup();

            m_isoMove.OnMoveTypeChange.AddListener(MoveTypeChange);
            IsometricOrientedPerspective.OnControllTypeChange.AddListener(m_isoRotation.Setup);
            IsometricOrientedPerspective.OnControllTypeChange.AddListener(m_isoMove.Setup);
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

        }

        private void Update()
        {
            IsometricOrientedPerspective.Type = controllerType;

            m_isoMove.MoveContext = moveType;
            m_isoMove.SetInputMoveDelta(IsometricOrientedPerspective.Type, m_inputs.horizontalAxi, m_inputs.verticalAxi);
            m_inputs.UpdateInputs();

            if (m_isoMove.MoveDelta == Vector2.zero && m_isoMove.OnSlope())
                m_jumpSystem.OnSlope = m_isoMove.OnSlope();

            if (m_isoMove.MoveDelta != Vector2.zero && m_isoMove.OnSlope())
                m_jumpSystem.OnSlope = !m_isoMove.OnSlope();

            Ray ray = IsometricCamera.m_instance.GetRay(m_inputs.rotatePosition);

            if (Physics.Raycast(ray, out m_raycastHit, float.MaxValue, layerMask))
                m_isoMove.LeftClick = m_inputs.leftClick;

            switch (controllerType)
            {
                case IsometricOrientedPerspective.ControllType.PointAndClick:
                    m_isoRotation.Rotate(m_raycastHit.point, layerMask);
                    m_isoMove.Move(m_raycastHit.point, m_inputs.leftClick, m_rigidbody);
                    break;
                case IsometricOrientedPerspective.ControllType.KeyBoard:
                    m_isoMove.Move(m_isoMove.MoveDelta, m_rigidbody);
                    break;
            }
            
            m_jumpSystem.Jump(m_inputs.jumpInput);

            IsometricCamera.m_instance.MoveBase();

            m_area.DrawCircle(100, movementDistance, new Vector3(transform.position.x, transform.position.y - GetComponent<CapsuleCollider>().bounds.extents.y, transform.position.z));
        }
    }
}
