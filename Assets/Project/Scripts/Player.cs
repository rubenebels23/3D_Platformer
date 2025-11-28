using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;


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

    public float startBlood = 80f;
    public float currentBlood;
    public BloodBar BloodBar;

    public Animator animator;

    private CharacterController controller;


    #endregion

    private PlayerMovement movement;


    public GameObject deathScreen;


    public AudioSource deathSound;   // <-- drag the AudioSource here

    private bool isDead = false;

    public GameObject coin;


    public ArenaTrigger arenaTrigger;



    void Start()
    {
        movement = GetComponent<PlayerMovement>();
        controller = GetComponent<CharacterController>();


        currentStamina = maxStamina;
        StaminaBar.SetMaxStamina(maxStamina);
        StaminaBar.SetStamina(currentStamina);

        currentBlood = startBlood;
        BloodBar.SetMaxBlood(maxBlood);
        BloodBar.SetBlood(currentBlood);
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
            // animator.SetBool("isJumping", true);

        }
        else
        {
            movement.jumpHeight = 6f;
            movement.jumpCost = 10f;
            // animator.SetBool("isJumping", false);
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
        if (isDead) return;  // <--- prevents double-death
        isDead = true;

        Debug.Log("Player died!");

        // Stop all boss music
        BossMusic[] allMusic = FindObjectsByType<BossMusic>(FindObjectsSortMode.None);
        foreach (var m in allMusic)
            m.StopMusicImmediate();

        // Show YOU DIED screen
        deathScreen.SetActive(true);

        // Play death sound ONCE
        if (deathSound != null)
            deathSound.Play();

        StartCoroutine(DeathSequence());
    }


    private IEnumerator DeathSequence()
    {
        // Screen visible for 5 seconds
        yield return new WaitForSeconds(5f);

        // Hide UI
        deathScreen.SetActive(false);

        // Respawn player
        Respawn();
    }



    public void Respawn()
    {
        isDead = false;

        currentBlood = maxBlood;
        BloodBar.SetBlood(currentBlood);

        currentStamina = maxStamina;
        StaminaBar.SetStamina(currentStamina);

        // TURN OFF WALLS ON RESPAWN
        GameObject arenaWalls = GameObject.Find("Arenawalls");
        if (arenaWalls != null)
            arenaWalls.SetActive(false);

        // RESET ALL BLOOD POTIONS
        BloodPotion[] bloodPotions = FindObjectsByType<BloodPotion>(FindObjectsSortMode.InstanceID);
        foreach (BloodPotion bp in bloodPotions)
        {
            bp.ResetPotion();
        }

        // Move player
        controller.enabled = false;
        transform.position = new Vector3(81.38f, 0.5f, 1071.89f);
        controller.enabled = true;
    }



    // STAMINA
    public void TakeDamageStamina(float amount)
    {
        currentStamina = Mathf.Max(0f, currentStamina - amount);
        StaminaBar.SetStamina(currentStamina);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.collider.CompareTag("Respawn"))
        {
            Die();
        }
    }

}