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

        [Header("Custom Camera Controller Settings")]
        [SerializeField] private CustomCamera CameraCustom;
        [SerializeField] private Transform m_CameraTarget;
        [SerializeField, Range(1f, 5)] private float m_CameraTargetHeight = 1.8f;
        [SerializeField, Range(0.5f, 1.5f)] private float m_cameraSensibility = 1.25f;
        [SerializeField] LayerMask m_thirdPersonCollisionFilter;

        #endregion

        #region INPUTS SECTION
        public void ReadPlayerInput()
        {
            CustomPlayerInputHandler inputHandler = new CustomPlayerInputHandler();

            Vector2 direction = InputActions.PlayerActions.MoveDirection.ReadValue<Vector2>();

            inputHandler.MoveDirectionInput = new Vector3(direction.x, 0.0f, direction.y);

            inputHandler.ActionTypeInput = InputActions.PlayerActions.ActionType.IsPressed();

            if (inputHandler.ActionTypeInput)
            {
                if (InputActions.PlayerActions.VerticalAction.WasPressedThisFrame())
                {
                    inputHandler.ChooseFlightInput = !inputHandler.ChooseFlightInput;
                    inputHandler.ActionTypeInput = false;
                }
            }
            else inputHandler.VerticalActionInput = InputActions.PlayerActions.VerticalAction.IsPressed();

            inputHandler.SpeedUpInput = InputActions.PlayerActions.SpeedUp.IsPressed();

            Vector2 camAxis = InputActions.PlayerActions.CameraMove.ReadValue<Vector2>();
            camAxis = new Vector2(camAxis.y, camAxis.x);

            inputHandler.CameraAxis = camAxis;

            CustomController.SetInput(inputHandler);
            CameraCustom.SetInput(inputHandler);
        }
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
            CustomController.SetupCharacter(m_groundLayer);
            CameraCustom.SetupCamera(m_thirdPersonCollisionFilter, CustomController, m_CameraTarget, m_CameraTargetHeight, m_cameraSensibility);
        }
        void Update()
        {
            ReadPlayerInput();
        }
        #endregion
    }
}
