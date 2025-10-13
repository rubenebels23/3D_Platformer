using UnityEngine;

public class Player : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;
    public float staminaDrainPerSec = 10f;   // tweak
    public HealthBar healthBar;

    private PlayerMovement movement;

    void Start() {
        movement = GetComponent<PlayerMovement>();
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    void Update() {
        if (movement != null && movement.isSprinting) 
        {
            TakeDamage(staminaDrainPerSec * Time.deltaTime);
        }

        else if (currentHealth < maxHealth) 
        {
            currentHealth = Mathf.Min(maxHealth, currentHealth + (staminaDrainPerSec / 2f) * Time.deltaTime);
            healthBar.SetHealth(currentHealth);
    }

    void TakeDamage(float amount) {
        currentHealth = Mathf.Max(0f, currentHealth - amount);
        healthBar.SetHealth(currentHealth);
        Debug.Log("STAMINA ERAFFF!!");
    }
}
}
