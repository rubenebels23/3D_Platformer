
// using System.Numerics;
using Unity.Mathematics;
using UnityEngine;
public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 4f;
    public float jumpSpeed = 3f;
    public float gravity = -9.81f;
    public LayerMask groundMask;
    public Camera playerCamera;
    public float cameraDistance = 5f;
    private Vector3 playerVelocity;
    private bool groundedPlayer;

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
        //Walking
        float x = Input.GetAxis("Horizontal"); // A,D, Left, Right
        float z = Input.GetAxis("Vertical"); // W,S, Up, Down

        Vector3 move;

        // Convert local directions (forward/horizontal) to world space based on the Player's rotation
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        // Combine inputs into a single direction

        move = (forward * z) + (right * x);
        controller.Move(move * moveSpeed * Time.deltaTime);


        // Putting a tiny sphere at the bottom of the player to check if it's on the ground
        groundedPlayer = Physics.CheckSphere(transform.position - new Vector3(0, controller.height / 2, 0), 0.25f, groundMask);


        // Jumping
        if (Input.GetKeyDown(KeyCode.Space) && groundedPlayer)
        {
            playerVelocity.y = jumpSpeed;

        }
        //apply gravity only when not grounded
        if (!groundedPlayer)
        {
            playerVelocity.y += gravity * Time.deltaTime;
        }
        controller.Move(playerVelocity * Time.deltaTime);



        turn.x += Input.GetAxis("Mouse X") * sensitivity;
        turn.y += Input.GetAxis("Mouse Y") * sensitivity;

        //how far you can look up and down
        turn.y = Mathf.Clamp(turn.y, -35f, 10f);

        // Rotate the player horizontally based on mouse input
        transform.rotation = Quaternion.Euler(0, turn.x, 0);

        //! Calculate camera rotation for vertical look
        Quaternion cameraRotation = Quaternion.Euler(-turn.y, turn.x, 0);


        // Set camera position and rotation for third-person

        //!positioning player camera
        Vector3 offset = cameraRotation * new Vector3(0f, 0f, -cameraDistance);

        //!setting to player camera | Following the player at fixed position
        playerCamera.transform.position = transform.position + offset;

        //! makes the camera look to the players head
        playerCamera.transform.LookAt(transform.position + Vector3.up * 1.5f);



    }
}