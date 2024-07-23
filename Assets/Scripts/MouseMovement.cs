using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseMovement : MonoBehaviour
{
    public float mouseSensitivity = 500f;

    float xRotation = 0f;
    float yRotation = 0f;

    public float topClamp = -90f;
    public float bottomClamp = 90f;

    void Start()
    {
        // Lock the cursor in middle and make it invisible
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // Get mouse inputs
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Rotate up & down
        xRotation -= mouseY;

        // Limit rotation
        xRotation = Mathf.Clamp(xRotation, topClamp, bottomClamp);

        // Rotate left & right
        yRotation += mouseX;

        // Apply rotation to player
        transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);

    }
}
