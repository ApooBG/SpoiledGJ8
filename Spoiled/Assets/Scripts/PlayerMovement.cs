using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] AudioClip roll;
    [SerializeField] AudioClip jump;
    [SerializeField] AudioSource audioSource;

    public float normalMoveSpeed = 0.1f;
    public float additionalRunningMoveSpeed = 0.1f;
    public float normalMass = 0.2f;
    public float jumpHeight = 1f;
    public int massMultiplier = 3;

    public bool isMoving = false;
    public bool isRunning = false;

    float moveSpeed;

    public float rotationSpeed = 200f;
    public Rotting rotting;

    private Rigidbody rb;
    private bool isGrounded = true;
    private float rottenPercentage;

    public Transform cameraTransform; // The parent of the camera, rotating around Y-axis

    private Vector3 initialCameraPosition;
    private Quaternion initialCameraRotation;
    private bool isRotating = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (rb == null)
        {
            Debug.LogError("Rigidbody component is missing from this GameObject.");
        }

        // Store the initial position and rotation of the camera
        initialCameraPosition = cameraTransform.localPosition;
        initialCameraRotation = cameraTransform.localRotation;
    }

    void Update()
    {
        if (isMoving)
        {
            audioSource.clip = roll;
        }

        else if (audioSource.clip == roll)
        {
            audioSource.clip = null;
        }

        if (!isGrounded)
        {
            audioSource.clip = jump;
        }

        else if (audioSource.clip == jump)
        {
            audioSource.clip = null;
        }

        rottenPercentage = rotting.rottenPercentage;
        if (rottenPercentage > 0)
        {
            moveSpeed = normalMoveSpeed - (normalMoveSpeed * rottenPercentage / 100);
            rb.mass = normalMass + ((normalMass * rottenPercentage / 100) * massMultiplier);
        }
        else
        {
            moveSpeed = normalMoveSpeed;
            rb.mass = normalMass;
        }

        Vector3 moveDirection = Vector3.zero;
        Vector3 rotationDirection = Vector3.zero;

        if (!isRotating)
        {
            Jump();
            Move(moveDirection, rotationDirection);
            Run();
        }
        
        // Rotate the camera horizontally when the right mouse button is held
        RotateCamera();
    }

    void Move(Vector3 moveDirection, Vector3 rotationDirection)
    {
        // Get camera's forward and right vectors
        Vector3 cameraForward = cameraTransform.forward;
        Vector3 cameraRight = cameraTransform.right;

        // Flatten the vectors so they don't affect vertical movement
        cameraForward.y = 0;
        cameraRight.y = 0;

        cameraForward.Normalize();
        cameraRight.Normalize();

        // Process the input for movement direction
        if (Input.GetKey(KeyCode.A))
        {
            moveDirection -= cameraRight; // Move left relative to the camera
            isMoving = true;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            moveDirection += cameraRight; // Move right relative to the camera
            isMoving = true;
        }
        else if (Input.GetKey(KeyCode.W))
        {
            moveDirection += cameraForward; // Move forward relative to the camera
            isMoving = true;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            moveDirection -= cameraForward; // Move backward relative to the camera
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

    void RotateCamera()
    {
        if (Input.GetMouseButton(1))
        {
            isRotating = true;
            float mouseX = Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * rotationSpeed * Time.deltaTime;

            // Rotate the camera horizontally (left/right)
            cameraTransform.Rotate(Vector3.up, mouseX, Space.World);

            // Rotate the camera vertically (up/down) with clamping
            float currentXRotation = cameraTransform.localEulerAngles.x;
            float desiredXRotation = currentXRotation - mouseY;

            // Clamp the vertical rotation to 90 degrees up and down from the start point
            if (desiredXRotation > 180f) desiredXRotation -= 360f; // Convert to -180 to 180 range
            desiredXRotation = Mathf.Clamp(desiredXRotation, -90f, 90f);

            // Apply the clamped rotation
            cameraTransform.localEulerAngles = new Vector3(desiredXRotation, cameraTransform.localEulerAngles.y, cameraTransform.localEulerAngles.z);
        }
        else if (isRotating)
        {
            // Smoothly return to initial position and rotation when the right mouse button is released
            cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, initialCameraPosition, Time.deltaTime * rotationSpeed);
            cameraTransform.localRotation = Quaternion.Lerp(cameraTransform.localRotation, initialCameraRotation, Time.deltaTime * rotationSpeed);

            if (Vector3.Distance(cameraTransform.localPosition, initialCameraPosition) < 0.01f && Quaternion.Angle(cameraTransform.localRotation, initialCameraRotation) < 1f)
            {
                cameraTransform.localPosition = initialCameraPosition;
                cameraTransform.localRotation = initialCameraRotation;
                isRotating = false;
            }
        }
    }
}
