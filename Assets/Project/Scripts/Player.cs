using UnityEngine;

public class Player : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;
    public float staminaDrainPerSec = 5f;
    public HealthBar healthBar;

    private PlayerMovement movement;

    void Start()
    {
        movement = GetComponent<PlayerMovement>();
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    void Update()
    {
        if (movement != null)
        {
            // check if player is pressing movement keys
            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");
            bool isMoving = Mathf.Abs(x) > 0.1f || Mathf.Abs(z) > 0.1f;

            // drain only when sprinting + moving
            if (movement.isSprinting && isMoving)
            {
                TakeDamage(staminaDrainPerSec * Time.deltaTime);
            }
            // regenerate stamina when not sprinting
            else if (currentHealth < maxHealth)
            {
                currentHealth = Mathf.Min(maxHealth, currentHealth + (staminaDrainPerSec / 2f) * Time.deltaTime);
                healthBar.SetHealth(currentHealth);
            }
        }


        if (currentHealth <= 0)
        {
            movement.isSprinting = false;
        }


        if (currentHealth <= 11)
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

    public void TakeDamage(float amount)
    {
        
        currentHealth = Mathf.Max(0f, currentHealth - amount);
        healthBar.SetHealth(currentHealth);
        // Debug.Log("STAMINA ERAFFF!!");
    }

}
