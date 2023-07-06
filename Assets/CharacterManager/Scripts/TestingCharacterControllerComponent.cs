//using CharacterManager;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using static IOP.IsometricOrientedPerspective;

//namespace IOP
//{
//    [RequireComponent(typeof(CharacterController))]
//    public class TestingCharacterControllerComponent : MonoBehaviour
//    {
//        [SerializeField] CharacterController controller;
//        [SerializeField] private Vector3 playerVelocity;
//        [SerializeField] private float playerSpeed = 2.0f;
//        [SerializeField] private float jumpHeight = 1.0f;
//        [SerializeField] private float gravityValue = -9.81f;

//        /[SerializeField] private IsometricMove m_isoMove;
//        [SerializeField] private JumpSystem m_jumpSystem;
//        // Start is called before the first frame update
//        void Start()
//        {
//            m_isoMove.Setup(ControllType.KeyBoard);
//            m_jumpSystem.Setup();
//            controller = GetComponent<CharacterController>();
//        }

//        // Update is called once per frame
//        void Update()
//        {
//            m_isoMove.SetInputMoveDelta(ControllType.KeyBoard, Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

//            //groundedPlayer = controller.isGrounded;
//            if (m_jumpSystem.OnGroundLevel && playerVelocity.y < 0)
//            {
//                playerVelocity.y = 0f;
//            }

//            Vector3 direction = new Vector3(m_isoMove.MoveDelta.x, 0, m_isoMove.MoveDelta.y);

//            Vector3 right = direction.x * IsometricMove.m_moveInstance.IsometricRight;
//            Vector3 forward = direction.z * IsometricMove.m_moveInstance.IsometricForward;

//            Vector3 move = right + forward + Vector3.zero;
//            controller.Move(move.normalized * Time.deltaTime * playerSpeed);

//            if (move != Vector3.zero)
//            {
//                gameObject.transform.forward = move;
//            }

//            // Changes the height position of the player..
//            if (Input.GetButtonDown("Jump") && m_jumpSystem.OnGroundLevel)
//            {
//                playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
//            }

//            playerVelocity.y += gravityValue * Time.deltaTime;
//            controller.Move(playerVelocity * Time.deltaTime);
//        }
//    }
//}
