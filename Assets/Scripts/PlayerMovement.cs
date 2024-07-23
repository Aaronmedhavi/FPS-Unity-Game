using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController controller;

    public float speed = 12f;
    public float gravity = -9.81f * 2;
    public float jumpHeight = 3f;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    Vector3 Velocity;

    bool isGrounded;
    bool isMoving;

    private Vector3 lastPosition = new Vector3(0f, 0f, 0f);
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }
    void Update()
    {
        // Ground check
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        // Reset default velocity
        if (isGrounded && Velocity.y < 0)
        {
            Velocity.y = -2f;
        }
        // Get inputs
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        // Create move vector
        Vector3 move = transform.right * x + transform.forward * z;
        // Move player
        controller.Move(move * speed * Time.deltaTime);
        // Check if player can jump
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            // Jump player
            Velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
        // Fall down
        Velocity.y += gravity * Time.deltaTime;
        // Execute jump
        controller.Move(Velocity * Time.deltaTime);
        if (lastPosition != gameObject.transform.position && isGrounded == true)
        {
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }
        lastPosition = gameObject.transform.position;
    }
}
