using UnityEngine;
using UnityEngine.Events;
using System.Collections;

namespace CustomGameController
{
    public struct CustomPlayerInputHandler
    {
        public Vector3 MoveDirectionInput;
        public bool VerticalActionInput;
        public bool SpeedUpInput;

        public Vector2 CameraAxis;

        public float LeftPropulsion;
        public float RightPropulsion;
    }

    public class CustomPlayer : MonoBehaviour
    {
        #region INSPECTOR FIELDS
        [Header("Custom Character Controller Settings")]
        [SerializeField] private LayerMask m_groundLayer;
        [SerializeField] private float m_maxSlopeAngle = 45f;

        [Header("Custom Camera Controller Settings")]
        [SerializeField, Range(0.1f, 1.0f)] private float m_cameraSensibility = 1.25f;
        [SerializeField] LayerMask m_thirdPersonCollisionFilter;
        #endregion

        #region PUBLIC PROPERTIES
        public static Camera CharacterCamera;
        #endregion

        #region PRIVATE PROPERTIES
        private CustomCharacterController CustomController { get => GetComponentInChildren<CustomCharacterController>(); }
        private CustomCamera CameraCustom { get => GetComponentInChildren<CustomCamera>(); }

        private CustomInputActions InputActions;
        private float DirectionVerticalDeltaRotation { get => Mathf.Clamp(Mathf.Round(CameraCustom.VerticalLocalEuler - CustomController.VerticalLocalEuler), -1.0f, 1.0f); }
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
            CameraCustom.SetupCamera(m_thirdPersonCollisionFilter, CustomController, m_cameraSensibility);
        }
        private void Update()
        {
            CameraCustom.CameraSensibility = m_cameraSensibility;
            PlayerPhysicsSimulation?.Invoke();

            CameraLookDirection?.Invoke(new Vector2(CameraPan, CameraTilt), CharacterDirection.normalized /*+ FlightDirection*/, CustomController, DirectionVerticalDeltaRotation);

            CharacterCheckSlopeAndGround?.Invoke();

            UpdateCharacterAnimation?.Invoke();

            ReadPlayerInput();
        }
        private void LateUpdate()
        {
            CameraPositionAndOffset?.Invoke(CustomController.transform, CustomController.SpeedingUpAction, CustomController.VerticalState);
        }
        #endregion

        #region PLAYER INPUTS SECTION
        [HideInInspector] public static UnityEvent<Vector3, float> OnCharacterDirection = new UnityEvent<Vector3, float>();

        [HideInInspector] public static UnityEvent<bool> OnVerticalAction = new UnityEvent<bool>();

        [HideInInspector] public static UnityEvent<bool, float> OnSpeedUpAction = new UnityEvent<bool, float>();

        [HideInInspector] public static UnityEvent<Vector3> OnFlightPropulsion = new UnityEvent<Vector3>();

        [HideInInspector] public static UnityEvent PlayerPhysicsSimulation = new UnityEvent();

        [HideInInspector] public static UnityEvent<Vector2, Vector3, CustomCharacterController, float> CameraLookDirection = new UnityEvent<Vector2, Vector3, CustomCharacterController, float>();

        [HideInInspector] public static UnityEvent<Transform, bool, VerticalState> CameraPositionAndOffset = new UnityEvent<Transform, bool, VerticalState>();

        [HideInInspector] public static UnityEvent CharacterCheckSlopeAndGround = new UnityEvent();

        [HideInInspector] public static UnityEvent UpdateCharacterAnimation = new UnityEvent();

        private Vector3 m_characterDirection;
        public Vector3 CharacterDirection
        {
            get
            {
                return  m_characterDirection;
            }
            set
            {
                if (value == Vector3.zero)
                {
                    if (CustomController.VerticalState != VerticalState.Flighting)
                    {
                        StartCoroutine(SmoothStop());
                        SpeedUpAction = false;
                    }
                }

                m_characterDirection = value;

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

        private bool m_speedUpAction;
        public bool SpeedUpAction
        {
            get
            {
                return m_speedUpAction;
            }
            set
            {
                m_speedUpAction = value;

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
        public float LeftPropulsion { get; set; }
        public float RightPropulsion { get; set; }

        private Vector3 m_flightDirection;
        public Vector3 FlightDirection 
        {
            get
            {
                return m_flightDirection;
            }
            set
            {
                if (value == Vector3.zero)
                {
                    if (CustomController.VerticalState == VerticalState.Flighting)
                    {
                        SpeedUpAction = false;
                    }
                }

                m_flightDirection = value;

                OnFlightPropulsion?.Invoke(value);
            }
        }

        public void ReadPlayerInput()
        {
            CustomPlayerInputHandler inputHandler = new CustomPlayerInputHandler();

            Vector2 direction = InputActions.PlayerActions.MoveDirection.ReadValue<Vector2>();

            inputHandler.MoveDirectionInput = new Vector3(direction.x, 0.0f, direction.y);

            inputHandler.VerticalActionInput = InputActions.PlayerActions.VerticalAction.WasPressedThisFrame();

            inputHandler.SpeedUpInput = InputActions.PlayerActions.SpeedAction.WasPressedThisFrame();

            Vector2 camAxis = InputActions.PlayerActions.CameraLook.ReadValue<Vector2>();
            camAxis = new Vector2(camAxis.x, camAxis.y);

            inputHandler.CameraAxis = camAxis;

            inputHandler.LeftPropulsion = InputActions.PlayerActions.LeftPropulsion.ReadValue<float>();
            inputHandler.RightPropulsion = InputActions.PlayerActions.RightPropulsion.ReadValue<float>();

            SetPlayerInput(inputHandler);
        }
        public void SetPlayerInput(CustomPlayerInputHandler inputs)
        {
            CharacterDirection = inputs.MoveDirectionInput;
            SpeedUpAction = inputs.SpeedUpInput ? !SpeedUpAction : SpeedUpAction;
            VerticalAction = inputs.VerticalActionInput;

            CameraPan = Mathf.Clamp(inputs.CameraAxis.x, -1, 1);
            CameraTilt = Mathf.Clamp(inputs.CameraAxis.y, -1, 1);

            //LeftPropulsion = Mathf.Round(inputs.LeftPropulsion * 100) / 100;
            //RightPropulsion = Mathf.Round(inputs.RightPropulsion * 100) / 100;

            //Vector3 up = Vector3.up * (LeftPropulsion + RightPropulsion) / 2.0f;
            //Vector3 right = Vector3.right * (LeftPropulsion - RightPropulsion) / 2.0f;
            //FlightDirection = up + right;
        }
        #endregion
    }
}
