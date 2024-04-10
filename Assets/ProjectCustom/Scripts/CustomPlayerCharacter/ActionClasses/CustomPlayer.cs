using UnityEngine;
using UnityEngine.Events;
using System.Collections;

namespace CustomGameController
{
    public class CustomPlayer : MonoBehaviour
    {
        #region INSPECTOR FIELDS
        [Header("Custom Character Controller Settings")]
        [SerializeField] private LayerMask m_groundLayer;
        [SerializeField] private float m_maxSlopeAngle = 45f;

        [Header("Custom Camera Controller Settings")]
        [SerializeField] private Transform m_CameraTarget;
        [SerializeField, Range(0.3f, 1.0f)] private float m_cameraSensibility = 1.25f;
        [SerializeField] LayerMask m_thirdPersonCollisionFilter;
        #endregion

        #region PUBLIC PROPERTIES
        public static Camera CharacterCamera;
        #endregion

        #region PRIVATE PROPERTIES
        private CustomCharacterController CustomController { get => GetComponentInChildren<CustomCharacterController>(); }
        private CustomCamera CameraCustom { get => GetComponentInChildren<CustomCamera>(); }

        private CustomInputActions InputActions;
        #endregion

        #region DEFAULTS METHODS
        private void Awake()
        {
            if (CharacterCamera == null) CharacterCamera = GetComponentInChildren<Camera>();

            InputActions = new CustomInputActions();
            InputActions.Enable();
        }
        private void Start()
        {
            CustomController.SetupCharacter(m_groundLayer);
            CameraCustom.SetupCamera(m_thirdPersonCollisionFilter, CustomController, m_CameraTarget, m_cameraSensibility);
        }
        private void Update()
        {
            PlayerPhysicsSimulation?.Invoke();

            CameraLookDirection?.Invoke(new Vector2(CameraTilt, CameraPan), CharacterDirection, CustomController.VerticalState, CustomController.transform.localEulerAngles.y);

            CharacterCheckSlopeAndGround?.Invoke();

            UpdateCharacterAnimation?.Invoke();

            ReadPlayerInput();
        }
        private void LateUpdate()
        {
            CameraPositionAndOffset?.Invoke(CustomController.ArchorReference);
        }
        #endregion

        #region PLAYER INPUTS SECTION
        [HideInInspector] public static UnityEvent<Vector3, float> OnCharacterDirection = new UnityEvent<Vector3, float>();

        [HideInInspector] public static UnityEvent<bool> OnVerticalAction = new UnityEvent<bool>();

        [HideInInspector] public static UnityEvent<bool, float> OnSpeedUpAction = new UnityEvent<bool, float>();

        [HideInInspector] public static UnityEvent PlayerPhysicsSimulation = new UnityEvent();

        [HideInInspector] public static UnityEvent<Vector2, Vector3, VerticalState, float> CameraLookDirection = new UnityEvent<Vector2, Vector3, VerticalState, float>();

        [HideInInspector] public static UnityEvent<Transform> CameraPositionAndOffset = new UnityEvent<Transform>();

        [HideInInspector] public static UnityEvent CharacterCheckSlopeAndGround = new UnityEvent();

        [HideInInspector] public static UnityEvent UpdateCharacterAnimation = new UnityEvent();

        private Vector3 characterDirection;
        public Vector3 CharacterDirection
        {
            get
            {
                return characterDirection;
            }
            set
            {
                if (value == Vector3.zero) StartCoroutine(SmoothStop());

                characterDirection = value;

                OnCharacterDirection?.Invoke(value.normalized, CustomController.CurrentSpeed);

                IEnumerator SmoothStop()
                {
                    float delayedStopTime = Vector3.Distance(CustomController.CurrentyVelocity, Vector3.zero);

                    while (delayedStopTime > 0)
                    {
                        CustomController.CharacterController.Move(Vector3.MoveTowards(CustomController.CurrentyVelocity, value, 1.0f));
                        delayedStopTime -= Time.deltaTime / CustomController.CurrentSpeed;
                    }

                    CustomController.CurrentyVelocity = Vector3.zero;

                    yield return null;
                }
            }
        }
        public bool SpeedUpAction
        {
            set
            {
                OnSpeedUpAction?.Invoke(value, CustomController.BaseSpeed);
            }
        }
        public bool VerticalAction
        {
            set
            {
                OnVerticalAction?.Invoke(value);
            }
        }
        public float CameraPan { get; set; }
        public float CameraTilt { get; set; }

        public void ReadPlayerInput()
        {
            CustomPlayerInputHandler inputHandler = new CustomPlayerInputHandler();

            Vector2 direction = InputActions.PlayerActions.MoveDirection.ReadValue<Vector2>();

            inputHandler.MoveDirectionInput = new Vector3(direction.x, 0.0f, direction.y);

            if (CustomController.VerticalState == VerticalState.InFlight)
            {
                inputHandler.VerticalActionInput = InputActions.PlayerActions.VerticalAction.IsPressed();
            }
            else
            {
                inputHandler.VerticalActionInput = InputActions.PlayerActions.VerticalAction.WasPressedThisFrame();
            }

            inputHandler.SpeedUpInput = InputActions.PlayerActions.SpeedAction.IsPressed();

            Vector2 camAxis = InputActions.PlayerActions.CameraLook.ReadValue<Vector2>();
            camAxis = new Vector2(camAxis.y, camAxis.x);

            inputHandler.CameraAxis = camAxis;

            SetPlayerInput(inputHandler);
        }
        public void SetPlayerInput(CustomPlayerInputHandler inputs)
        {
            CharacterDirection = inputs.MoveDirectionInput;
            SpeedUpAction = inputs.SpeedUpInput;
            VerticalAction = inputs.VerticalActionInput;

            CameraPan = Mathf.Clamp(inputs.CameraAxis.y, -1, 1);
            CameraTilt = Mathf.Clamp(inputs.CameraAxis.x, -1, 1);
        }
        #endregion
    }
}
