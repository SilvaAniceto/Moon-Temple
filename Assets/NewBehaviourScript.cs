using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IsometricMovement
{
    public class NewBehaviourScript : MonoBehaviour
    {
        [SerializeField] float jumpCounter, jumpTime, jumpForce, value4, value5;
        [SerializeField] bool isJumping, onGround;
        [SerializeField] Rigidbody rb;
        [SerializeField] LayerMask layerMask;
        // Start is called before the first frame update
        void Start()
        {
            value4 = Physics.gravity.y;
        }

        // Update is called once per frame
        void Update()
        {            
            onGround = Physics.Raycast(rb.transform.position, Vector3.down, 0.6f, layerMask);

            if (onGround && Input.GetButtonDown("MouseLeftButton"))
            {
                isJumping = true;
                jumpCounter = jumpTime;
                rb.velocity = Vector3.up * jumpForce;
            }

            if (Input.GetButton("MouseLeftButton") && isJumping)
            {
                if (jumpCounter > 0)
                {
                    rb.velocity = Vector3.up * jumpForce;
                    jumpCounter -= Time.deltaTime;
                }
                else
                {
                    isJumping = false;
                }
            }

            if (Input.GetButtonUp("MouseLeftButton"))
            {
                isJumping = false;
            }
        }
    }
}
