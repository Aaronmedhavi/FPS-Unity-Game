using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseMovement : MonoBehaviour
{
    [Header("Mouse Settings")]
    public float mouseSensitivity = 500f;
    public float topClamp = -90f;
    public float bottomClamp = 90f;

    private float xRotation = 0f;
    private float yRotation = 0f;

    void Start()
    {
        // Lock the cursor in the middle and make it invisible
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        HandleMouseInput();
        ApplyRotation();
    }

    void HandleMouseInput()
    {
        // Get mouse inputs
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Rotate up & down
        xRotation -= mouseY;

        // Clamp the vertical rotation
        xRotation = Mathf.Clamp(xRotation, topClamp, bottomClamp);

        // Rotate left & right
        yRotation += mouseX;
    }

    void ApplyRotation()
    {
        // Apply rotation to the player
        transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
    }
}

