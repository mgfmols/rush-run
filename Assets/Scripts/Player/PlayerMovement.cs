using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody playerBody;
    [SerializeField] private Transform feet;
    [SerializeField] private LayerMask floor;
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float sprintSpeed = 8f;
    [SerializeField] private float jumpForce = 5f;
    public bool canDoubleJump = false;
    public bool usedDoubleJump = false;

    private Vector3 playerMovementInput;

    void Update()
    {
        playerMovementInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));

        MovePlayer();
        Sprint();
        Jump();
    }

    private void MovePlayer()
    {
        Vector3 moveVector = transform.TransformDirection(playerMovementInput).normalized * walkSpeed;
        playerBody.velocity = new Vector3(moveVector.x, playerBody.velocity.y, moveVector.z);
    }

    private void Sprint()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            walkSpeed = sprintSpeed;
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            walkSpeed = 5f;
        }
    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (Physics.CheckSphere(feet.position, 0.1f, floor))
            {
                playerBody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            }
            else if (canDoubleJump && !Physics.CheckSphere(feet.position, 0.1f, floor))
            {
                playerBody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                canDoubleJump = false;
                usedDoubleJump = true;
            }
        }
    }
}