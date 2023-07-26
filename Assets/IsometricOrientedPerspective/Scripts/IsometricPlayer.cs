using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IsometricGameController
{
    public class IsometricPlayer : MonoBehaviour
    {
        [SerializeField] private IsomectricCharacterController IsometricController;

        void Update()
        {
            ReadPlayerInput();
        }

        public void ReadPlayerInput()
        {
            IsometricInputHandler isometricInputHandler = new IsometricInputHandler();

            isometricInputHandler.HorizontalInput = Input.GetAxis("Horizontal");
            isometricInputHandler.VerticalInput = Input.GetAxis("Vertical");
            isometricInputHandler.IsometricMoveDirection = new Vector3(isometricInputHandler.HorizontalInput, 0, isometricInputHandler.VerticalInput);
            isometricInputHandler.JumpInput = Input.GetButton("Jump") ? true : Input.GetButtonUp("Jump") ? false : false;
            isometricInputHandler.AccelerateSpeed = Input.GetButtonDown("AccelerateSpeed");
            isometricInputHandler.PlayerConfirmEntry = Input.GetButtonDown("ConfirmEntry");

            IsometricController.SetInput(isometricInputHandler);
        }
    }
}
