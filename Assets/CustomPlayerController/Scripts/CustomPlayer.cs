using UnityEngine;

namespace CustomGameController
{
    public class CustomPlayer : MonoBehaviour
    {
        #region INSPECTOR FIELDS
        [Header("Custom Character Controller Settings")]
        [SerializeField] private CustomCharacterController CustomController;
        [SerializeField] private LayerMask m_groundLayer;
        [SerializeField] private float m_maxSlopeAngle = 45f;
        [SerializeField] private float m_onGroundSpeed = 8;
        [SerializeField, Range(1.2f, 5)] private float m_acceleration = 2.5f;
        [SerializeField, Range(1.2f, 10)] private float m_jumpHeight = 1.5f;
        [SerializeField, Range(0, 100)] private float m_drag = 0.5f;

        [Header("Custom Camera Controller Settings")]
        [SerializeField] private CustomCamera CameraCustom;
        [SerializeField] private Transform m_CameraTarget;
        [SerializeField, Range(1f, 5)] private float m_CameraTargetHeight = 0.0f;
        [SerializeField, Range(0.5f, 1.5f)] private float m_cameraSensibility = 1.0f;
        [SerializeField] CameraPerspective m_defaultPerspective;
        #endregion

        #region PRIVATE FIELDS
        private CustomInputActions InputActions;
        #endregion

        #region DEFAULTS METHODS
        private void Awake()
        {
            InputActions = new CustomInputActions();
            InputActions.Enable();
            CameraCustom.OnCameraPerspectiveChanged.AddListener(CustomController.SetCharacterMoveCallBacks);
        }
        private void Start()
        {
            CameraCustom.SetCameraPerspective(m_defaultPerspective);
            CustomController.SetCharacterMoveCallBacks(m_defaultPerspective);

            CustomController.GroundLayer = m_groundLayer;
            CustomController.MaxSlopeAngle = m_maxSlopeAngle;
            CustomController.OnGroundSpeed = m_onGroundSpeed;

            CustomController.OnGroundAcceleration = m_acceleration;
            CustomController.JumpHeight = m_jumpHeight;
            CustomController.Drag = m_drag;

            CameraCustom.CustomController = CustomController;
            CameraCustom.CameraTarget = m_CameraTarget;
            CameraCustom.CameraTargetHeight = m_CameraTargetHeight;
            CameraCustom.CameraSensibility = m_cameraSensibility;

            CustomController.SetupCharacter();
        }
        void Update()
        {
            ReadPlayerInput();
        }
        #endregion

        #region INPUTS METHODS
        public void ReadPlayerInput()
        {
            CustomPlayerInputHandler inputHandler = new CustomPlayerInputHandler();

            Vector2 direction = InputActions.PlayerActions.Move.ReadValue<Vector2>();

            inputHandler.MoveDirectionInput = new Vector3(direction.x, 0.0f, direction.y);
            inputHandler.JumpInput = InputActions.PlayerActions.Jump.IsPressed();
            inputHandler.SprintInput = InputActions.PlayerActions.Sprint.IsPressed();

            Vector2 camAxis = InputActions.PlayerActions.CameraAxis.ReadValue<Vector2>();
            camAxis = new Vector2(camAxis.y, camAxis.x);

            inputHandler.CameraAxis = camAxis;
            inputHandler.ChangeCameraPerspective = InputActions.PlayerActions.ChangeCameraPerspectiva.IsPressed();

            CustomController.SetInput(inputHandler);
            CameraCustom.SetInput(inputHandler);
        }
        #endregion
    }
}
