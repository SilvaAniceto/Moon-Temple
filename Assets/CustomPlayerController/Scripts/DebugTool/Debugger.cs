using UnityEngine;

namespace CustomGameController
{
    public class Debugger : MonoBehaviour
    {
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
            //if (DebugInput.DebugActions.ToggleIsometric.WasPerformedThisFrame())
            //{
            //    customCamera.SetCameraPerspective(CameraPerspective.Isometric);
            //}

            //if (DebugInput.DebugActions.ToggleThirdPerson.WasPerformedThisFrame())
            //{
            //    customCamera.SetCameraPerspective(CameraPerspective.Third_Person);
            //}

            //if (DebugInput.DebugActions.ToggleFirstPerson.WasPerformedThisFrame())
            //{
            //    customCamera.SetCameraPerspective(CameraPerspective.First_Person);
            //}
        }
    }
}
