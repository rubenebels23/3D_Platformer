using UnityEngine;

public class Player : MonoBehaviour
{
    public float maxStamina = 100f;
    public float currentStamina;
    public float staminaDrainPerSec = 5f;
    public StaminaBar StaminaBar;


    public MagicBar MagicBar;

    private PlayerMovement movement;

    void Start()
    {
        movement = GetComponent<PlayerMovement>();
        currentStamina = maxStamina;
        StaminaBar.SetMaxStamina(maxStamina);

        // currentMagic = maxMagic;
        // MagicBar.SetMaxMagic(maxMagic);
        
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
                TakeDamageStamina(staminaDrainPerSec * Time.deltaTime);
            }
            // regenerate stamina when not sprinting
            else if (currentStamina < maxStamina)
            {
                currentStamina = Mathf.Min(maxStamina, currentStamina + staminaDrainPerSec * Time.deltaTime);
                StaminaBar.SetStamina(currentStamina);
            }
        }


        if (currentStamina <= 0)
        {
            movement.isSprinting = false;
        }


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
    
    public void TakeDamageStamina(float amount)
    {

        currentStamina = Mathf.Max(0f, currentStamina - amount);
        StaminaBar.SetStamina(currentStamina);
        // Debug.Log("STAMINA ERAFFF!!");
    }

}
