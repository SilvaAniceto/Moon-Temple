using UnityEngine.UI;
using UnityEngine;

namespace IsometricOrientedPerspective
{
    public class UICharacterController : UIPopUpController
    {
        [SerializeField] private Slider UIMoveSpeed, UIJumpHeight;
        [SerializeField] private Toggle UITogglePhysics, UIToggleMouseRotation;

        private bool m_physicsEnabled;

        new void Awake()
        {
            base.Awake();

            UIMoveSpeed.onValueChanged.AddListener((float p_movementDelta) =>
            {
                IsometricMove.m_moveInstance.MovementDelta = p_movementDelta;
            });

            UIJumpHeight.onValueChanged.AddListener((float p_jumpHeight) =>
            {
                JumpSystem.m_jumpInstance.HeightDelta = p_jumpHeight;
            });

            UITogglePhysics.onValueChanged.AddListener((bool p_physics) =>
            {
                m_physicsEnabled = p_physics;
                IsometricMove.m_moveInstance.IsPhysicsMovement = p_physics;
                IsometricRotation.m_rotationInstance.IsPhysicsRotation = p_physics;
            });

            UIToggleMouseRotation.onValueChanged.AddListener((bool p_physics) =>
            {
                IsometricRotation.m_rotationInstance.enabled = p_physics;
                IsometricRotation.m_rotationInstance.IsPhysicsRotation = m_physicsEnabled;
            });
        }

        void Start()
        {
            UIMoveSpeed.value = IsometricMove.m_moveInstance.MovementDelta;
            UIJumpHeight.value = JumpSystem.m_jumpInstance.HeightDelta;
            UITogglePhysics.isOn = IsometricMove.m_moveInstance.IsPhysicsMovement && IsometricRotation.m_rotationInstance.IsPhysicsRotation ? true : false;
            UIToggleMouseRotation.isOn = IsometricRotation.m_rotationInstance.enabled;

            if (isOpen)
                Hide();
        }

        void Update()
        {

        }
    }
}
