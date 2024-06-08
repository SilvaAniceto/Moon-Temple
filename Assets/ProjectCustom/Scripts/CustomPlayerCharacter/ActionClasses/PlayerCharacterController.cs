using UnityEngine;

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

    public class PlayerCharacterController : MonoBehaviour
    {
        public static Camera CharacterCamera;

        #region INSPECTOR FIELDS
        [Header("Custom Character Controller Settings")]
        [SerializeField] private PlayerCharacter m_playerCharacter = new PlayerCharacter();

        [Header("Custom Camera Controller Settings")]
        [SerializeField] private PlayerCamera m_playerCamera = new PlayerCamera();
        #endregion

        #region PRIVATE PROPERTIES
        private CustomInputActions InputActions;
        private bool OnGround
        {
            get => m_playerCharacter.m_onGround;
            set
            {
                if (value)
                {
                    m_playerCharacter.m_characterController.transform.localRotation = Quaternion.Euler(0.0f, m_playerCharacter.m_characterController.transform.localEulerAngles.y, 0.0f);
                }

                if (m_playerCharacter.m_onGround == value) return;

                m_playerCharacter.m_onGround = value;

                if (!m_playerCharacter.m_onGround && m_playerCharacter.SlopeHit().collider == null)
                {
                    StopCoroutine(m_playerCharacter.CheckingUngroundedStates());
                    StartCoroutine(m_playerCharacter.CheckingUngroundedStates());
                }
            }
        }
        #endregion

        #region DEFAULTS METHODS
        private void Awake()
        {
            if (CharacterCamera == null) CharacterCamera = GetComponentInChildren<Camera>();

            if (m_playerCharacter.m_characterController == null)
            {
                m_playerCharacter.m_characterController = gameObject.GetComponentInChildren<CharacterController>();
            }

            if (m_playerCharacter.m_characterAnimator == null)
            {
                m_playerCharacter.m_characterAnimator = gameObject.GetComponentInChildren<Animator>();
            }

            InputActions = new CustomInputActions();
            InputActions.Enable();
        }
        private void Start()
        {
            m_playerCharacter.SetupPlayerCharacter();
            m_playerCamera.SetupCamera(transform, m_playerCamera.m_collisionFilter, m_playerCamera.m_cameraRotationSensibility);

            StartCoroutine(m_playerCharacter.CheckingUngroundedStates());
        }
        private void Update()
        {
            m_playerCharacter.ApplyGravity();
            OnGround = m_playerCharacter.CheckGroundLevel();
            m_playerCharacter.SetCheckersLocation();
            m_playerCharacter.Animate();

            m_playerCamera.UpdateCameraLookDirection(new Vector2(CameraPan, CameraTilt), CharacterDirection.normalized , SpeedUpAction);

            ReadPlayerInput();
        }
        private void LateUpdate()
        {
            m_playerCamera.UpdateCameraPositionAndOffset(m_playerCharacter.m_characterController.transform, SpeedUpAction, m_playerCharacter.VerticalState);
        }
        #endregion

        #region PLAYER INPUTS SECTION
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
                    StartCoroutine(m_playerCharacter.SmoothStop());
                    SpeedUpAction = false;
                }

                m_characterDirection = value;

                m_playerCharacter.UpdateHorizontalPositionAndDirection(value.normalized);
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

                m_playerCharacter.SpeedingUpAction = value;
            }
        }
        public bool VerticalAction
        {
            set
            {
                m_playerCharacter.Jump(value);
            }
        }
        public float CameraPan { get; set; }
        public float CameraTilt { get; set; }

        //public float LeftPropulsion { get; set; }
        //public float RightPropulsion { get; set; }
        //private Vector3 m_flightDirection;
        //public Vector3 FlightDirection 
        //{
        //    get
        //    {
        //        return m_flightDirection;
        //    }
        //    set
        //    {
        //        if (value == Vector3.zero)
        //        {
        //            if (CustomController.VerticalState == VerticalState.Flighting)
        //            {
        //                SpeedUpAction = false;
        //            }
        //        }

        //        m_flightDirection = value;

        //        OnFlightPropulsion?.Invoke(value);
        //    }
        //}

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
