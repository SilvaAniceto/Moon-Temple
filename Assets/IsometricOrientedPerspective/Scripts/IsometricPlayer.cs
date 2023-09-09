using UnityEngine;

namespace IsometricGameController
{
    public class IsometricPlayer : MonoBehaviour
    {
        #region INSPECTOR FIELDS
        [SerializeField] private IsomectricCharacterController IsometricController;
        [SerializeField] private IsometricCamera IsometricCamera;
        [SerializeField] private LayerMask m_layerMask;
        [SerializeField] private float m_maxSlopeAngle = 45f;
        [SerializeField] private float m_onGroundSpeed = 8;

        [SerializeField][Range(1f, 5)] private float m_acceleration = 2.5f;
        [SerializeField][Range(1.2f, 10)] private float m_jumpHeight = 1.5f;
        [SerializeField][Range(0, 100)] private float m_drag = 0.5f;

        [SerializeField] private Transform m_cursor;
        #endregion
        private CustomInputActions InputActions;

        private void Awake()
        {
            InputActions = new CustomInputActions();
            InputActions.Enable();
        }

        private void Start()
        {
            IsometricController.Forward = IsometricOrientedPerspective.IsometricForward;
            IsometricController.Right = IsometricOrientedPerspective.IsometricRight;

            IsometricController.WhatIsGround = m_layerMask;
            IsometricController.MaxSlopeAngle = m_maxSlopeAngle;
            IsometricController.OnGroundSpeed = m_onGroundSpeed;

            IsometricController.OnGroundAcceleration = m_acceleration;
            IsometricController.JumpHeight = m_jumpHeight;
            IsometricController.Drag = m_drag;
        }
        void Update()
        {
            ReadPlayerInput();
        }

        public void ReadPlayerInput()
        {
            IsometricInputHandler isometricInputHandler = new IsometricInputHandler();

            Vector2 direction = InputActions.PlayerActions.Move.ReadValue<Vector2>();
            isometricInputHandler.IsometricMoveDirection = new Vector3(direction.x, 0, direction.y);

            isometricInputHandler.JumpInput = InputActions.PlayerActions.Jump.IsPressed();

            Vector2 cameraLook = InputActions.PlayerActions.CameraLook.ReadValue<Vector2>();
            isometricInputHandler.CameraLook = new Vector2(cameraLook.x, cameraLook.y);

            isometricInputHandler.CameraAimInput = InputActions.PlayerActions.CameraAim.IsPressed();

            IsometricController.SetInput(isometricInputHandler);
            IsometricCamera.SetInput(isometricInputHandler);
        }
    }
}
