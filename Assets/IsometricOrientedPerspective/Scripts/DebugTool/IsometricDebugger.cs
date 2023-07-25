
using UnityEngine;

namespace IsometricGameController
{
    public class IsometricDebugger : MonoBehaviour
    {
        private IsomectricCharacterController controller;

        bool debugInput = false;

        void Start()
        {
            controller = IsomectricCharacterController.Instance;

            controller.ControllerState = GameControllerState.Exploring;
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                debugInput = !debugInput;

                if (debugInput) controller.ControllerState = GameControllerState.Combat;
                else controller.ControllerState = GameControllerState.Exploring;
            }
        }
    }
}
