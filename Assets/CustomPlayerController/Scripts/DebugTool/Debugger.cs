using UnityEngine;

namespace CustomGameController
{
    public class Debugger : MonoBehaviour
    {
        public bool enableDebugger = false;
        public CustomCharacterController controller;
        public CustomCamera customCamera;

        private DebugInput DebugInput;

        private void Awake()
        {
            DebugInput = new DebugInput();
            DebugInput.Enable();
        }

        void Update()
        {
            if (!enableDebugger) return;

            if (DebugInput.DebugActions.ToggleIsometric.WasPerformedThisFrame())
            {
                customCamera.SetCameraPerspective(CameraPerspective.Isometric);
            }

            if (DebugInput.DebugActions.ToggleThirdPerson.WasPerformedThisFrame())
            {
                customCamera.SetCameraPerspective(CameraPerspective.Third_Person);
            }

            if (DebugInput.DebugActions.ToggleFirstPerson.WasPerformedThisFrame())
            {
                customCamera.SetCameraPerspective(CameraPerspective.First_Person);
            }

            if (DebugInput.DebugActions.ToggleOverShoulder.WasPerformedThisFrame())
            {
                customCamera.SetCameraPerspective(CameraPerspective.Over_Shoulder);
            }
        }
    }
}
