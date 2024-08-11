using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour
{
    public float normalMoveSpeed = 0.1f;       // Speed at which the GameObject moves
    public float additionalRunningMoveSpeed = 0.1f;       // Speed at which the GameObject moves
    public float normalMass = 0.2f;      // Desired height of the jump
    public float jumpHeight = 1f;      // Desired height of the jump
    public int massMultiplier = 3;      // Desired height of the jump

    public bool isMoving = false;
    public bool isRunning = false;

    float moveSpeed;       // Speed at which the GameObject moves

    public float rotationSpeed = 200f; // Speed at which the GameObject rotates
    public Rotting rotting;

    private Rigidbody rb;              // Rigidbody component for physics-based movement
    private bool isGrounded = true;    // Flag to check if the tomato is on the ground
    private float rottenPercentage;

    void Start()
    {
        // Get the Rigidbody component attached to the GameObject
        rb = GetComponent<Rigidbody>();

        if (rb == null)
        {
            Debug.LogError("Rigidbody component is missing from this GameObject.");
        }
    }

    void Update()
    {
        rottenPercentage = rotting.rottenPercentage;
        if (rottenPercentage > 0)
        {
            moveSpeed = normalMoveSpeed - (normalMoveSpeed * rottenPercentage / 100);
            rb.mass = normalMass + ((normalMass * rottenPercentage / 100)* massMultiplier);
        }

        else
        {
            moveSpeed = normalMoveSpeed;
            rb.mass = normalMass;
        }

        // Handle rotation and movement input
        Vector3 moveDirection = Vector3.zero;
        Vector3 rotationDirection = Vector3.zero;

        Jump();
        Move(moveDirection, rotationDirection);
        Run();
    }

    void Move(Vector3 moveDirection, Vector3 rotationDirection)
    {
        // Process the input for movement direction
        if (Input.GetKey(KeyCode.A))
        {
            // A - Increase X position, Decrease Z rotation
            moveDirection.x += 1;
            rotationDirection.z -= 1;
            isMoving = true;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            // D - Decrease X position, Increase Z rotation
            moveDirection.x -= 1;
            rotationDirection.z += 1;
            isMoving = true;
        }
        else if (Input.GetKey(KeyCode.W))
        {
            // W - Decrease Z position, Decrease X rotation
            moveDirection.z -= 1;
            rotationDirection.x -= 1;
            isMoving = true;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            // S - Increase Z position, Increase X rotation
            moveDirection.z += 1;
            rotationDirection.x += 1;
            isMoving = true;
        }

        else if (isMoving && isGrounded)
        {
            isMoving = false;
        }

        // Calculate the new velocity based on the move direction
        Vector3 newVelocity = Vector3.zero;
        if (isRunning)
            newVelocity = moveDirection.normalized * (moveSpeed + additionalRunningMoveSpeed);
        else
            newVelocity = moveDirection.normalized * moveSpeed;

        // Keep the current Y velocity (to not interfere with gravity or jumping)
        newVelocity.y = rb.velocity.y;

        // Apply the new velocity to the Rigidbody
        rb.velocity = newVelocity;

        // Rotate the GameObject using Rigidbody's MoveRotation
        Quaternion deltaRotation = Quaternion.Euler(rotationDirection * rotationSpeed * Time.deltaTime);
        rb.MoveRotation(rb.rotation * deltaRotation);
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            // Calculate the jump force based on the desired jump height and the object's mass
            float jumpForce = Mathf.Sqrt(2 * jumpHeight * rb.mass * Physics.gravity.magnitude);

            // Apply the jump force
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }

        if (!isGrounded)
            isMoving = true;
    }

    void Run()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            isRunning = true;
        }

        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            isRunning = false;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // Check if the tomato has landed on the ground
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            isMoving = false;
        }
    }

}
