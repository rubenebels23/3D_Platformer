using UnityEngine;

public class Player : MonoBehaviour
{
    #region Stamina
    public float maxStamina = 100f;
    public float currentStamina;
    public float staminaDrainPerSec = 5f;
    public StaminaBar StaminaBar;
    #endregion

    #region Blood (Health)
    public float maxBlood = 100f;
    public float currentBlood;
    public BloodBar BloodBar;
    #endregion

    private PlayerMovement movement;

    void Start()
    {
        movement = GetComponent<PlayerMovement>();

        currentStamina = maxStamina;
        StaminaBar.SetMaxStamina(maxStamina);

        currentBlood = maxBlood;
        BloodBar.SetMaxBlood(maxBlood);
    }

    void Update()
    {
        HandleStamina();
    }

    void HandleStamina()
    {
        if (movement == null) return;

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        bool isMoving = Mathf.Abs(x) > 0.1f || Mathf.Abs(z) > 0.1f;

        if (movement.isSprinting && isMoving)
        {
            TakeDamageStamina(staminaDrainPerSec * Time.deltaTime);
        }
        else if (currentStamina < maxStamina)
        {
            currentStamina = Mathf.Min(maxStamina, currentStamina + staminaDrainPerSec * Time.deltaTime);
            StaminaBar.SetStamina(currentStamina);
        }

        if (currentStamina <= 0)
            movement.isSprinting = false;

        if (currentStamina <= 11)
        {
            movement.jumpHeight = 0f;
            movement.jumpCost = 0f;
        }
        else
        {
            movement.jumpHeight = 6f;
            movement.jumpCost = 10f;
        }
    }

    // BLOOD (HEALTH) 
    public void TakeDamageBlood(float amount)
    {
        currentBlood = Mathf.Max(0f, currentBlood - amount);
        BloodBar.SetBlood(currentBlood);
        Debug.Log($"Player took {amount} blood damage! HP: {currentBlood}");

        if (currentBlood <= 0)
            Die();
    }

    public void RestoreBlood(float amount)
    {
        currentBlood = Mathf.Min(maxBlood, currentBlood + amount);
        BloodBar.SetBlood(currentBlood);
        Debug.Log($"Blood restored! HP: {currentBlood}");
    }

    //! DELETE LATER, GOOD FOR TESTING
    public void RestoreStamina(float amount)
    {
        currentStamina = Mathf.Min(maxStamina, currentStamina + amount);
        StaminaBar.SetStamina(currentStamina);
        Debug.Log($"Stamina restored! Stamina: {currentStamina}");
    }

    public void Die()
    {
        Debug.Log("Player died!");

        // Destroy(gameObject);
        Respawn();

    }

    public void Respawn()
    {
        // Debug.Log("AAAAA");

        currentBlood = maxBlood;
        BloodBar.SetBlood(currentBlood);

        currentStamina = maxStamina;
        StaminaBar.SetStamina(currentStamina);

        CharacterController controller = GetComponent<CharacterController>();
        if (controller != null)
            controller.enabled = false;

        // Move player
        transform.position = new Vector3(-57.24f, 1f, 8.7f);

        // Re-enable controller
        if (controller != null)
            controller.enabled = true;

        Debug.Log("Player respawned!");
    }
    // STAMINA
    public void TakeDamageStamina(float amount)
    {
        currentStamina = Mathf.Max(0f, currentStamina - amount);
        StaminaBar.SetStamina(currentStamina);
    }
}
