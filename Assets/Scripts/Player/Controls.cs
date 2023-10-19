using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Controls : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private Rigidbody playerBody;
    [SerializeField] private Transform feet;
    [SerializeField] private LayerMask floor;
    [SerializeField] private float jumpHeight = 50f;
    [Header("Player step")]
    [SerializeField] TerrainChecker terrainChecker;
    [SerializeField] FootstepSoundScript soundScript;
    [SerializeField] GameObject stepRayUpper;
    [SerializeField] GameObject stepRayLower;
    [SerializeField] float stepHeight = 0.3f;
    [SerializeField] float stepSmooth = 0.1f;

    [Header("UI")]
    [SerializeField] TMP_Text timerText;
    [SerializeField] PauseMenu pauseMenu;
    [SerializeField] Camera Camera;
    [SerializeField] ParticleSystem speedParticles;

    float terrainSpeed;
    private float movementSpeed = 10f;
    private bool isSprinting = false;
    private bool canDoubleJump = false;
    private bool usedDoubleJump = false;
    private bool pauseMenuEnabled = true;

    private bool resetSpeedOnGround = false;
    private bool giveSpeedOnGround = false;

    private Vector3 moveDirection;

    public Rigidbody RigidBody { get { return playerBody ; } }
    public float MovementSpeed { get { return movementSpeed; } set { movementSpeed = value; } }
    public bool IsSprinting { get { return isSprinting; } set { isSprinting = value; } }
    public bool CanDoubleJump { get { return canDoubleJump; } set { canDoubleJump = value; } }
    public bool UsedDoubleJump { get { return usedDoubleJump; } set { usedDoubleJump = value; } }
    public bool PauseMenuEnabled { get { return pauseMenuEnabled; } set { pauseMenuEnabled = value; } }

    private Vector3 playerMovementInput;

    void Start()
    {
        stepRayUpper.transform.position = new Vector3(stepRayLower.transform.position.x, stepRayLower.transform.position.y + stepHeight, stepRayLower.transform.position.z);
    }

    void Update()
    {
        playerMovementInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));
        terrainSpeed = terrainChecker.GetTerrainSpeed();
        MovePlayer();
        Sprint();
        Jump();
        AdjustView();

        if (resetSpeedOnGround)
        {
            ResetSprintOnGround();
        }
        if (giveSpeedOnGround)
        {
            GiveSprintOnGround();
        }
    }

    //Moves the player in a 2D space and normalizes diagonal movement.
    private void MovePlayer()
    {
        if (!Physics.Raycast(stepRayUpper.transform.position, moveDirection, out _, 1f))
        {
            moveDirection = transform.TransformDirection(playerMovementInput).normalized * movementSpeed * terrainSpeed;
            playerBody.velocity = new Vector3(moveDirection.x, playerBody.velocity.y, moveDirection.z);
        }

        /*if (Physics.Raycast(stepRayLower.transform.position, moveDirection, out _, 1f))
        {
            if (!Physics.Raycast(stepRayUpper.transform.position, moveDirection, out _, 1f))
            {
                playerBody.position -= new Vector3(0f, -stepSmooth, 0f);
                Debug.DrawRay(transform.position, moveDirection, Color.red, 1000);
            }
        }*/
        //StepClimb(moveDirection);
    }

    //Sets movement speed depending on LShift input.
    private void Sprint()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (!isSprinting)
            {
                if (!CheckIsJumping())
                {
                    movementSpeed *= 2;
                    isSprinting = true;
                    soundScript.SetNewSpeed(0.2f);
                }
                else
                {
                    if (resetSpeedOnGround)
                    {
                        resetSpeedOnGround = false;
                        isSprinting = true;
                    }
                    else
                    {
                        giveSpeedOnGround = true;
                    }
                }

            }
        }
        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (resetSpeedOnGround && !CheckIsJumping())
            {
                movementSpeed /= 2;
                isSprinting = true;
                soundScript.SetNewSpeed(0.2f);
            }
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            if (isSprinting)
            {
                if (!CheckIsJumping())
                {
                    movementSpeed /= 2;
                    isSprinting = false;
                    soundScript.SetNewSpeed(0.4f);
                }
                else
                {
                    // Reset speed on ground
                    isSprinting = false;
                    resetSpeedOnGround = true;
                    //giveSpeedOnGround = false;
                }
            }
            else
            {
                if (giveSpeedOnGround)
                {
                    giveSpeedOnGround = false;
                }
            }
        }
    }

    //Allows player to jump when grounded. Allows the player to double jump when carrying the double jump powerup while airborn.
    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (Physics.CheckSphere(feet.position, 0.2f, floor))
            {
                playerBody.AddForce(Vector3.up * jumpHeight, ForceMode.Impulse);
            }
            else if (canDoubleJump && CheckIsJumping())
            {
                playerBody.AddForce(Vector3.up * jumpHeight, ForceMode.Impulse);
                canDoubleJump = false;
                usedDoubleJump = true;
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape) && pauseMenuEnabled)
        {
            OpenPauseMenu();
        }
    }

    public bool CheckIsJumping()
    {
        return !Physics.CheckSphere(feet.position, 0.2f, floor);
    }

    public GameObject GetStandingOnObject()
    {
        Physics.Raycast(feet.position + new Vector3(0, 0.2f, 0), Vector3.down, out RaycastHit hit, 0.4f);
        return hit.collider.gameObject;
    }

    private void OpenPauseMenu()
    {
        pauseMenu.OpenMenu();
    }

    void StepClimb(Vector3 moveDirection)
    {
        if (Physics.Raycast(stepRayLower.transform.position, moveDirection, out _, 1f))
        {
            if (!Physics.Raycast(stepRayUpper.transform.position, moveDirection, out _, 1f))
            {
                playerBody.position -= new Vector3(0f, -stepSmooth, 0f);
                Debug.DrawRay(transform.position, moveDirection, Color.red, 1000);
            }
        }
    }

    void AdjustView()
    {
        // Speed particles
        if (playerBody.velocity.magnitude <= 18f && speedParticles.isPlaying)
        {
            speedParticles.Stop();
        }
        else if (playerBody.velocity.magnitude > 18f && speedParticles.isStopped)
        {
            speedParticles.Play();
        }

        // Camera FOV
        if (playerBody.velocity.magnitude < 0.1f)
        {
            Camera.fieldOfView = Mathf.Lerp(Camera.fieldOfView, 65f, 10f * Time.deltaTime);
        }
        else if (playerBody.velocity.magnitude < 5f)
        {
            Camera.fieldOfView = Mathf.Lerp(Camera.fieldOfView, 70f, 10f * Time.deltaTime);
        }
        else if (playerBody.velocity.magnitude < 10f)
        {
            Camera.fieldOfView = Mathf.Lerp(Camera.fieldOfView, 75f, 10f * Time.deltaTime);
        }
        else if (playerBody.velocity.magnitude < 15f)
        {
            Camera.fieldOfView = Mathf.Lerp(Camera.fieldOfView, 80f, 10f * Time.deltaTime);
        }
        else if (playerBody.velocity.magnitude < 20f)
        {
            Camera.fieldOfView = Mathf.Lerp(Camera.fieldOfView, 85f, 10f * Time.deltaTime);
        }
        else if (playerBody.velocity.magnitude < 25f)
        {
            Camera.fieldOfView = Mathf.Lerp(Camera.fieldOfView, 90f, 10f * Time.deltaTime);
        }
        else if (playerBody.velocity.magnitude < 30f)
        {
            Camera.fieldOfView = Mathf.Lerp(Camera.fieldOfView, 95f, 10f * Time.deltaTime);
        }
        else if (playerBody.velocity.magnitude < 35f)
        {
            Camera.fieldOfView = Mathf.Lerp(Camera.fieldOfView, 100f, 10f * Time.deltaTime);
        }
        else
        {
            Camera.fieldOfView = Mathf.Lerp(Camera.fieldOfView, 110f, 10f * Time.deltaTime);
        }
    }

    void ResetSprintOnGround()
    {
        if (!CheckIsJumping())
        {
            movementSpeed /= 2;
            soundScript.SetNewSpeed(0.4f);
            resetSpeedOnGround = false;
        }
    }

    void GiveSprintOnGround()
    {
        if (!CheckIsJumping())
        {
            movementSpeed *= 2;
            soundScript.SetNewSpeed(0.4f);
            isSprinting = true;
            giveSpeedOnGround = false;
        }
    }
}
