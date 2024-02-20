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
        [SerializeField, Range(2.0f, 6.0f)] private int m_slopeCheckCount = 4;
        [SerializeField, Range(1.0f, 3.6f)] private float m_onGroundSpeed = 2.7f;
        [SerializeField, Range(1.2f, 5.0f)] private float m_acceleration = 2.0f;
        [SerializeField, Range(2.0f, 8.0f)] private float m_inFlightSpeed = 3.6f;
        [SerializeField, Range(1.2f, 5.0f)] private float m_inFlightAcceleration = 2.0f;
        [SerializeField, Range(1.2f, 10.0f)] private float m_jumpHeight = 1.8f;
        [SerializeField, Range(0.0f, 100.0f)] private float m_drag = 1.4f;

        [Header("Custom Camera Controller Settings")]
        [SerializeField] private CustomCamera CameraCustom;
        [SerializeField] private Transform m_CameraTarget;
        [SerializeField, Range(1f, 5)] private float m_CameraTargetHeight = 1.8f;
        [SerializeField, Range(0.5f, 1.5f)] private float m_cameraSensibility = 1.25f;
        [SerializeField] CameraPerspective m_defaultPerspective;
        [SerializeField] LayerMask m_thirdPersonCollisionFilter;
        [SerializeField] LayerMask m_isometricCollisionFilter;
        [SerializeField] Vector3 m_walkOfftset = Vector3.zero;
        [SerializeField] Vector3 m_sprintOfftset = Vector3.zero;
        [SerializeField] Vector3 m_hoverFlightOfftset = Vector3.zero;
        [SerializeField] Vector3 m_speedFlightOfftset = Vector3.zero;
        #endregion

        #region PRIVATE FIELDS
        private CustomInputActions InputActions;
        #endregion

        #region DEFAULTS METHODS
        private void Awake()
        {
            InputActions = new CustomInputActions();
            InputActions.Enable();
        }
        private void Start()
        {
            CameraCustom.ThirdPersonCollisionFilter = m_thirdPersonCollisionFilter;
            CameraCustom.IsometricCollisionFilter = m_isometricCollisionFilter;

            CustomController.GroundLayer = m_groundLayer;

            CustomController.MaxSlopeAngle = m_maxSlopeAngle;
            CustomController.SlopeCheckCount = m_slopeCheckCount;

            CustomController.OnGroundSpeed = m_onGroundSpeed;
            CustomController.OnGroundAcceleration = m_acceleration;

            CustomController.InFlightSpeed = m_inFlightSpeed;
            CustomController.InFlightAcceleration = m_inFlightAcceleration;

            CustomController.JumpHeight = m_jumpHeight;
            CustomController.Drag = m_drag;

            CameraCustom.CustomController = CustomController;
            CameraCustom.CameraTarget = m_CameraTarget;
            CameraCustom.CameraTargetHeight = m_CameraTargetHeight;
            CameraCustom.CameraSensibility = m_cameraSensibility;

            CustomController.SetupPlayer();
        }
        void Update()
        {
            ReadPlayerInput();
            CameraCustom.WalkOfftset = m_walkOfftset;
            CameraCustom.SprintOfftset = m_sprintOfftset;
            CameraCustom.HoverFlightOfftset = m_hoverFlightOfftset;
            CameraCustom.SpeedFlightOfftset = m_speedFlightOfftset;
        }
        #endregion

        #region INPUTS METHODS
        public void ReadPlayerInput()
        {
            CustomPlayerInputHandler inputHandler = new CustomPlayerInputHandler();

            Vector2 direction = InputActions.PlayerActions.Move.ReadValue<Vector2>();

            inputHandler.MoveDirectionInput = new Vector3(direction.x, 0.0f, direction.y);
            if (!InputActions.PlayerActions.FlightControl.IsPressed()) inputHandler.JumpInput = InputActions.PlayerActions.Jump.IsPressed();
            else inputHandler.JumpInput = InputActions.PlayerActions.Jump.WasPressedThisFrame();

            if (CustomController.InFlight)
            {
                inputHandler.VerticalAscendingInput = InputActions.PlayerActions.VerticalAscending.IsPressed();
                inputHandler.VerticalDescendingInput = InputActions.PlayerActions.VerticalDescending.IsPressed();
            }

            inputHandler.SprintInput = InputActions.PlayerActions.Sprint.IsPressed();

            Vector2 camAxis = InputActions.PlayerActions.CameraAxis.ReadValue<Vector2>();
            camAxis = new Vector2(camAxis.y, camAxis.x);

            inputHandler.CameraAxis = camAxis;
            inputHandler.CameraZoom = InputActions.PlayerActions.Zoom.ReadValue<float>();

            inputHandler.AirControlling = InputActions.PlayerActions.FlightControl.IsPressed();

            CustomController.SetInput(inputHandler);
            CameraCustom.SetInput(inputHandler);
        }
        #endregion
    }
}
