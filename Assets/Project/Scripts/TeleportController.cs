using UnityEngine;

public class TeleportController : MonoBehaviour
{
    [Header("References")]
    public PlayerMovement playerMovement;    // drag your PlayerMovement here
    public Camera playerCamera;              // drag your player camera here

    [Header("Teleport Settings")]
    public float maxTeleportDistance = 20f;
    public LayerMask groundMask;             // layers considered valid ground

    public float teleportCooldown = 5f;      // cooldown in seconds

    private float cooldownRemaining = 0f;

    private Player player;

    public MagicBar magicBar;
    public float maxMagic = 100f;
    public float currentMagic;


    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        player = GetComponent<Player>();

        currentMagic = maxMagic;
        magicBar.SetMaxMagic(maxMagic);
    }

    void Update()
    {
        // decrease cooldown timer every frame
        if (cooldownRemaining > 0)
            cooldownRemaining -= Time.deltaTime;

        // check for key press
        if (Input.GetKeyDown(KeyCode.T))
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


            // 4b)  Get the CharacterController
            CharacterController cc = playerMovement.GetComponent<CharacterController>();

            // figure out where the capsule would be at the target
            Vector3 bottom = targetPosition + Vector3.up * cc.radius;
            Vector3 top = targetPosition + Vector3.up * (cc.height - cc.radius);

            // check if anything solid occupies that capsule
            bool blocked = Physics.CheckCapsule(bottom, top, cc.radius, ~0, QueryTriggerInteraction.Ignore);
            // use ~0 to check against everything, or use your own "solid" mask

            if (blocked)
            {
                Debug.Log("Teleport blocked: not enough headroom / space.");
                return;
            }

            // 5️⃣ Teleport
            playerMovement.TeleportTo(targetPosition);
            TakeDamageMagic(20f);
            cooldownRemaining = teleportCooldown; //  reset cooldown

            Debug.Log($"Teleported to {targetPosition}");
        }
        else
        {
            Debug.Log("No valid surface to teleport to!");
        }
    }

    public void TakeDamageMagic(float amount)
    {

        currentMagic = Mathf.Max(0f, currentMagic - amount);
        magicBar.SetMagic(currentMagic);
        Debug.Log("MAGIC ERAFFF!!");
    }
}
