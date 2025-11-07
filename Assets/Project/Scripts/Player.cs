using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Stamina")]
    public float maxStamina = 100f;
    public float currentStamina;
    public float staminaDrainPerSec = 5f;
    public StaminaBar StaminaBar;

    [Header("Blood (Health)")]
    public float maxBlood = 100f;
    public float currentBlood;
    public BloodBar BloodBar;

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

    // -------- BLOOD (HEALTH) --------
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

    void Die()
    {
        Debug.Log("Player died!");
    }

    // -------- STAMINA --------
    public void TakeDamageStamina(float amount)
    {
        currentStamina = Mathf.Max(0f, currentStamina - amount);
        StaminaBar.SetStamina(currentStamina);
    }
}
