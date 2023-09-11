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

        [SerializeField][Range(1f, 5)] private float m_acceleration = 2.5f;
        [SerializeField][Range(1.2f, 10)] private float m_jumpHeight = 1.5f;
        [SerializeField][Range(0, 100)] private float m_drag = 0.5f;
        #endregion

        private CustomInputActions InputActions;

        private void Awake()
        {
            InputActions = new CustomInputActions();
            InputActions.Enable();
        }

        private void Start()
        {
            CameraCustom.Perspective = CustomCamera.CameraPerspective.Isometric;

            CustomController.GroundLayer = m_groundLayer;
            CustomController.MaxSlopeAngle = m_maxSlopeAngle;
            CustomController.OnGroundSpeed = m_onGroundSpeed;

            CustomController.OnGroundAcceleration = m_acceleration;
            CustomController.JumpHeight = m_jumpHeight;
            CustomController.Drag = m_drag;
        }
        void Update()
        {
            CustomController.Forward = CustomPerspective.CustomForward;
            CustomController.Right = CustomPerspective.CustomRight;

            ReadPlayerInput();
        }

        public void ReadPlayerInput()
        {
            CustomPlayerInputHandler inputHandler = new CustomPlayerInputHandler();

            Vector2 direction = InputActions.PlayerActions.Move.ReadValue<Vector2>();

            inputHandler.MoveDirectionInput = new Vector3(direction.x, 0.0f, direction.y);
            inputHandler.JumpInput = InputActions.PlayerActions.Jump.IsPressed();
            inputHandler.SprintInput = InputActions.PlayerActions.Sprint.IsPressed();

            CustomController.SetInput(inputHandler);
            CameraCustom.SetInput(inputHandler);
        }
    }
}
