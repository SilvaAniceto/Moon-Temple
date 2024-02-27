using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CustomThirdPerson
{
    public class MovementTest : MonoBehaviour
    {
        [SerializeField] CharacterController characterController;

        [SerializeField] bool canMove;
        [SerializeField] float moveSpeed = 1.0f;
        [SerializeField] bool startStop;
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
            if (canMove)
            {
                characterController.Move(transform.forward * Time.deltaTime * moveSpeed);
                if (startStop)
                {
                    startStop = false;
                    StartCoroutine(StopWalk());
                }
            }


            IEnumerator StopWalk()
            {
                yield return new WaitForSeconds(1);

                canMove = false;
            }
        }
    }
}
