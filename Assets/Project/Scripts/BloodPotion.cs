using UnityEngine;
using UnityEngine.Rendering;

public class BloodPotion : MonoBehaviour
{
    public int bloodRestoreAmount = 20;
    public GameObject pickupGlowPrefab; // Assign in Inspector
    public AudioClip pickupSound;
    public int staminaRestoreAmount = 100;

    void Start()
    {

    }

    private void OnTriggerEnter(Collider collider)
    {
        var player = collider.GetComponent<Player>();
        if (player == null) return;

        if (player.currentBlood < player.maxBlood)
        {
            player.RestoreBlood(bloodRestoreAmount);
            player.RestoreStamina(staminaRestoreAmount / 2); //! DELELTE LATER, GOOD FOR TESTING

            AudioSource.PlayClipAtPoint(pickupSound, transform.position);

            if (pickupGlowPrefab != null)
            {
                Instantiate(pickupGlowPrefab, transform.position, Quaternion.identity);
            }

            Destroy(gameObject);
        }
    }
}