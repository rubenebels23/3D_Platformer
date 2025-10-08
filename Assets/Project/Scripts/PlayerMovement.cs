// using System.Numerics;
using Unity.Mathematics;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 4f;
    public float sprintSpeed = 10f;
    public float jumpSpeed = 3f;
    public float gravity = -9.81f;
    public LayerMask groundMask;
    public Camera playerCamera;
    public float cameraDistance = 5f;
    private Vector3 playerVelocity;
    private bool groundedPlayer;

    private float coyoteTime = 0.2f;
    private float coyoteTimeCounter;

    private bool isSprinting = false;

    public Vector2 turn;
    public float sensitivity = 1f;

    private CharacterController controller;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // Walking
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        // get input direction in world space
        Vector3 move;
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        //combine input axes into one move vector
        move = (forward * z) + (right * x);

        // move the player
        controller.Move(move * moveSpeed * Time.deltaTime);


        //!sprint

        GetInput();
        SprintPlayer();

        void GetInput()
        {
            if (Input.GetKeyDown(KeyCode.LeftShift)) isSprinting = true;
            if (Input.GetKeyUp(KeyCode.LeftShift)) isSprinting = false;
            //GetKeyUp detects when a key is released.
        }

        void SprintPlayer()
        {
            if (isSprinting)
            {
                Debug.Log("Sprint Triggered");
                moveSpeed = sprintSpeed;
            }
            else
            {
                moveSpeed = 4f;
            }
        }

        // *Ground check
        groundedPlayer = Physics.CheckSphere(transform.position - new Vector3(0, controller.height / 2, 0), 0.25f, groundMask);

        // *Coyote time: reset only if grounded AND not moving up
        if (groundedPlayer && playerVelocity.y <= 0f)
        {
            coyoteTimeCounter = coyoteTime; // ready to jump
        }
        else
        {
            coyoteTimeCounter = Mathf.Max(0f, coyoteTimeCounter - Time.deltaTime); // ticking down
        }

        // Jump (uses coyote)
        if (Input.GetKeyDown(KeyCode.Space) && coyoteTimeCounter > 0f)
        {
            playerVelocity.y = jumpSpeed;
            coyoteTimeCounter = 0f; // no second jump in same window
        }

        // Gravity (with ground snap)
        if (groundedPlayer && playerVelocity.y < 0f)
        {
            playerVelocity.y = -2f; // stay stuck to ground
        }
        else
        {
            playerVelocity.y += gravity * Time.deltaTime; // normal fall
        }

        controller.Move(playerVelocity * Time.deltaTime);

        // Mouse look
        turn.x += Input.GetAxis("Mouse X") * sensitivity;
        turn.y += Input.GetAxis("Mouse Y") * sensitivity;
        turn.y = Mathf.Clamp(turn.y, -35f, 10f);

        transform.rotation = Quaternion.Euler(0, turn.x, 0);

        //! Calculate camera rotation for vertical look
        Quaternion cameraRotation = Quaternion.Euler(-turn.y, turn.x, 0);

        // Set camera position and rotation for third-person (offset)

        //!positioning player camera
        Vector3 offset = cameraRotation * new Vector3(0f, 0f, -cameraDistance);

        //!setting to player camera | Following the player at fixed position
        playerCamera.transform.position = transform.position + offset;

        //! makes the camera look to the players head
        playerCamera.transform.LookAt(transform.position + Vector3.up * 1.5f);
    }
}
