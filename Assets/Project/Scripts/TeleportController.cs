using UnityEngine;

public class TeleportController : MonoBehaviour
{
    [Header("References")]
    public PlayerMovement playerMovement;    // drag your PlayerMovement here
    public Camera playerCamera;              // drag your player camera here

    [Header("Teleport Settings")]
    public float maxTeleportDistance = 20f;  // how far you can teleport
    public LayerMask groundMask;             // layers considered valid ground
    public KeyCode teleportKey = KeyCode.T;  // key to trigger teleport
    public float teleportCooldown = 5f;      // cooldown in seconds

    private float cooldownRemaining = 0f;

    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
    }

    void Update()
    {
        // decrease cooldown timer every frame
        if (cooldownRemaining > 0)
            cooldownRemaining -= Time.deltaTime;

        // check for key press
        if (Input.GetKeyDown(teleportKey))
        {
            TryTeleport();
        }
    }

    void TryTeleport()
    {
        // don’t teleport if missing references
        if (playerCamera == null || playerMovement == null)
        {
            Debug.LogWarning("TeleportController: Missing references!");
            return;
        }

        // stop if not grounded
        if (!playerMovement.groundedPlayer)
        {
            Debug.Log("Can't teleport while mid-air!");
            return;
        }

        // check cooldown
        if (cooldownRemaining > 0f)
        {
            Debug.Log($"Wait {cooldownRemaining:F1} seconds before teleporting again!");
            return;
        }

        // 1️⃣ Create a ray from camera forward
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);

        // 2️⃣ Shoot the ray
        if (Physics.Raycast(ray, out RaycastHit hitInfo, maxTeleportDistance, groundMask))
        {
            // 3️⃣ Use hit point as target position
            Vector3 targetPosition = hitInfo.point;

            // 4️⃣ Raise slightly to avoid clipping
            targetPosition.y += 1f;

            // 5️⃣ Teleport
            playerMovement.TeleportTo(targetPosition);
            cooldownRemaining = teleportCooldown; //  reset cooldown

            Debug.Log($"Teleported to {targetPosition}");
        }
        else
        {
            Debug.Log("No valid surface to teleport to!");
        }
    }
}
