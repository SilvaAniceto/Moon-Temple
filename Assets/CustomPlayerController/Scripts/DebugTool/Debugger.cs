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
        }
    }
}
