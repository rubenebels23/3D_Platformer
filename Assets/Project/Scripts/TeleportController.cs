using UnityEngine;

public class TeleportController : MonoBehaviour
{
    [Header("References")]
    public PlayerMovement playerMovement;
    public Camera playerCamera;
    public float maxTeleportDistance = 20f;
    public LayerMask groundMask;
    public float teleportCooldown = 5f;

    private float cooldownRemaining = 0f;
    private Player player;


    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        player = GetComponent<Player>();
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

        // check cooldown
        if (cooldownRemaining > 0f)
        {
            Debug.Log($"Wait {cooldownRemaining:F1} seconds before teleporting again!");
            return;
        }

        // 1️⃣ Create a ray from camera forward
        Ray ray = playerCamera.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0f));

        // 2️⃣ Shoot the ray
        if (Physics.Raycast(ray, out RaycastHit hitInfo, maxTeleportDistance, groundMask))
        {
            // 3️⃣ Use hit point as target position
            Vector3 targetPosition = hitInfo.point;

            // 4️⃣ Raise slightly to avoid clipping

            //raise it more if in air to avoid bug with staying in the air because of the animation
            if (!playerMovement.groundedPlayer)
                targetPosition.y += 3f;
            else
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
            player.TakeDamageBlood(20f);
            cooldownRemaining = teleportCooldown; //  reset cooldown

            Debug.Log($"Teleported to {targetPosition}");
        }
        else
        {
            Debug.Log("No valid surface to teleport to!");
        }
    }
}
