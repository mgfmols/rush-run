using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    private Vector2 playerMouseInput;
    private float xRotation;

    private bool menuOpen = false;
    public bool MenuOpen { get { return menuOpen; } set { menuOpen = value;  } }

    [SerializeField] private Transform playerCamera;
    [SerializeField] private float sensitivity = 2f;

    void OnEnable()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        playerMouseInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        if (!menuOpen)
        {
            PlayerMoveCamera();
        }
    }

    private void PlayerMoveCamera()
    {
        xRotation -= playerMouseInput.y * sensitivity;
        transform.Rotate(0f, playerMouseInput.x * sensitivity, 0f);
        playerCamera.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);
    }
}
