// using System.Numerics;
using Unity.Mathematics;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    #region --- all variables ---

    #region --- Player Settings ---
    public float moveSpeed = 4f;
    public float sprintSpeed = 10f;
    public float jumpHeight = 10f;
    [HideInInspector] public float baseJumpHeight;
    public float gravity = -9.807f;
    public float airDrag = 2f;
    // how long after leaving ground you can still jump
    public float coyoteTime = 0.2f;
    public float jumpCost = 10f;
    private Player player;
    #endregion

    #region --- Components & References ---
    public LayerMask groundMask;
    public Camera playerCamera;
    private CharacterController controller;
    #endregion

    #region --- Camera Settings ---
    public float cameraDistance = 5f;
    public Vector2 turn;
    public float sensitivity = 1f;
    #endregion

    #region --- State ---
    private Vector3 playerVelocity;
    private Vector3 glideVelocity;         // horizontal momentum
    public bool groundedPlayer;
    public bool isSprinting = false;
    public bool airJumpAvailable = true;  // one extra jump in air

    private float coyoteTimeCounter;

    #endregion

    #endregion --- all variables ---

    void Awake()
    {
        baseJumpHeight = jumpHeight; // store the original value once
    }

    void Start()
    {
        controller = GetComponent<CharacterController>();
        player = GetComponent<Player>();           // get reference
        Cursor.lockState = CursorLockMode.Locked;
    }
    void Update()
    {
        #region --- Sprinting | coyotetimer | Input ---
        // Walking
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        if (Input.GetKeyDown(KeyCode.LeftShift)) isSprinting = true;
        if (Input.GetKeyUp(KeyCode.LeftShift)) isSprinting = false;

        //* Ground check
        groundedPlayer = Physics.CheckSphere(transform.position - new Vector3(0, controller.height / 2, 0), 0.25f, groundMask);

        // Reset air jumps on ground
        if (groundedPlayer)
            airJumpAvailable = true;

        // Coyote timer for jumping after leaving ground
        if (groundedPlayer && playerVelocity.y <= 0f)
            coyoteTimeCounter = coyoteTime;

        //counting down when in air to 0
        else
            coyoteTimeCounter = Mathf.Max(0f, coyoteTimeCounter - Time.deltaTime);


        // get Input direction in world space
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);
        //get combined direction 
        Vector3 moveDir = (forward * z + right * x).normalized;
        #endregion --- Sprinting | coyotetimer | Input ---

        #region --- Movement & Gliding ---
        //! targetspeed is the switch between moveSpeed and sprintSpeed so my normal moveSpeed won't get overwritten the whole time.
        float targetSpeed;
        if (isSprinting) targetSpeed = sprintSpeed;
        else targetSpeed = moveSpeed;


        // Checks if you’re pressing movement keys.
        // If yes:

        // figures out which way and how fast you should go.

        // Then glides your current glideVelocity toward that target (Lerp)
        if (x != 0f || z != 0f)
        {
            // Input: accelerate to target speed
            Vector3 targetVel = moveDir * targetSpeed;
            glideVelocity = Vector3.Lerp(glideVelocity, targetVel, 5f * Time.deltaTime);
        }
        else if (!groundedPlayer)
        {
            // if there is no Input in air: Slow down horizontally with Lerp
            glideVelocity = Vector3.Lerp(glideVelocity, Vector3.zero, airDrag * Time.deltaTime);
        }
        else
        {
            // no Input on ground = stop
            glideVelocity = Vector3.zero;
        }
        #endregion --- Movement & Gliding ---

        #region --- Jumping & Gravity ---
        // Ground or coyote jump (does NOT consume air jump)
        // --- Jumping & Gravity ---
        if (Input.GetKeyDown(KeyCode.Space) && coyoteTimeCounter > 0f)
        {
            playerVelocity.y = jumpHeight;
            coyoteTimeCounter = 0f;
            if (player != null)
                player.TakeDamage(jumpCost);

        }
        else if (Input.GetKeyDown(KeyCode.Space) && !groundedPlayer && airJumpAvailable)
        {
            playerVelocity.y = jumpHeight;
            airJumpAvailable = false;
            if (player != null)
                player.TakeDamage(jumpCost);

        }
        // Gravity
        if (!groundedPlayer)
        {
            playerVelocity.y += gravity * Time.deltaTime;
        }

        // move the player
        controller.Move((glideVelocity + playerVelocity) * Time.deltaTime);

        #endregion --- Jumping & Gravity ---        

        #region --- Camera Control ---
        // Mouse look
        turn.x += Input.GetAxis("Mouse X") * sensitivity;
        turn.y += Input.GetAxis("Mouse Y") * sensitivity;
        turn.y = Mathf.Clamp(turn.y, -35f, 10f);

        //! Rotate player horizontally (left/right)

        transform.rotation = Quaternion.Euler(0, turn.x, 0);

        //! Calculate camera rotation (vertical + horizontal look)
        Quaternion cameraRotation = Quaternion.Euler(-turn.y, turn.x, 0);

        // Set camera position and rotation for third-person (offset)

        //!positioning player camera
        Vector3 offset = cameraRotation * new Vector3(0f, 0f, -cameraDistance);

        //!setting to player camera | Following the player at fixed position
        playerCamera.transform.position = transform.position + offset;

        //! makes the camera look to the players head
        playerCamera.transform.LookAt(transform.position + Vector3.up * 1.5f);
        #endregion
    }

    
    // Runs after Update() every frame
    void LateUpdate()
    {
        
        // 1) Only do this if we're standing on something
        //
        if (groundedPlayer == false)
            return;

        //2) Shoot a ray straight down to see what’s below the player
        RaycastHit hitInfo;
        bool hitSomething = Physics.Raycast(
            transform.position,       // start at player position
            Vector3.down,             // cast straight down
            out hitInfo,              // store info about what we hit
            controller.height + 0.5f, // how far the ray goes
            groundMask                // only hit ground layers
        );
        if (hitSomething == false)
            return;

        //3) Check if what we hit is a moving platform
        VelocityCalculator platform = hitInfo.collider.GetComponent<VelocityCalculator>();
        if (platform == null)
            return;

        //4) Get how far the platform moved this frame
        Vector3 platformMovement = platform.GetVelocity(transform);
        if (platformMovement == Vector3.zero)
            return;

        //5) Move the player by the same amount so it stays in sync
        controller.Move(platformMovement);
    }

    //Reference to teleport the player
    public void TeleportTo(Vector3 newPos)
    {
        controller.enabled = false;     // disable CharacterController for safety
        transform.position = newPos;    // move the player
        controller.enabled = true;      // re-enable controller
    }

}