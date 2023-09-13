using UnityEngine;

namespace CustomGameController
{
    public class CustomPlayer : MonoBehaviour
    {
        #region INSPECTOR FIELDS
        [SerializeField] private CustomCharacterController CustomController;
        [SerializeField] private CustomCamera CameraCustom;
        [SerializeField] private LayerMask m_groundLayer;
        [SerializeField] private float m_maxSlopeAngle = 45f;
        [SerializeField] private float m_onGroundSpeed = 8;

        [SerializeField, Range(1f, 5)] private float m_acceleration = 2.5f;
        [SerializeField, Range(1.2f, 10)] private float m_jumpHeight = 1.5f;
        [SerializeField, Range(0, 100)] private float m_drag = 0.5f;

        [SerializeField] private Transform m_CameraTarget;
        [SerializeField] private Transform m_CameraPivot;
        [SerializeField, Range(0.1f, 1.0f)] private float m_cameraSensibility = 1.0f;
        #endregion

        private CustomInputActions InputActions;

        private void Awake()
        {
            InputActions = new CustomInputActions();
            InputActions.Enable();
        }

        private void Start()
        {
            CameraCustom.CameraPerspective = CameraPerspective.Third_Person;

            CustomController.GroundLayer = m_groundLayer;
            CustomController.MaxSlopeAngle = m_maxSlopeAngle;
            CustomController.OnGroundSpeed = m_onGroundSpeed;

            CustomController.OnGroundAcceleration = m_acceleration;
            CustomController.JumpHeight = m_jumpHeight;
            CustomController.Drag = m_drag;

            CameraCustom.CameraTarget = m_CameraTarget;
            CameraCustom.CameraPivot = m_CameraPivot;
            CameraCustom.CameraSensibility = m_cameraSensibility;
        }
        void Update()
        {
            ReadPlayerInput();
        }

        public void ReadPlayerInput()
        {
            CustomPlayerInputHandler inputHandler = new CustomPlayerInputHandler();

            Vector2 direction = InputActions.PlayerActions.Move.ReadValue<Vector2>();

            inputHandler.MoveDirectionInput = new Vector3(direction.x, 0.0f, direction.y);
            inputHandler.JumpInput = InputActions.PlayerActions.Jump.IsPressed();
            inputHandler.SprintInput = InputActions.PlayerActions.Sprint.IsPressed();

            Vector2 camAxis = InputActions.PlayerActions.CameraAxis.ReadValue<Vector2>();
            camAxis = new Vector2(camAxis.y, camAxis.x);

            inputHandler.CameraAxis = camAxis.normalized;

            CustomController.SetInput(inputHandler);
            CameraCustom.SetInput(inputHandler);
        }
    }
}
