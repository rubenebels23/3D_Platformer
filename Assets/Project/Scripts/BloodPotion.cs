using UnityEngine;

public class BloodPotion : MonoBehaviour
{
    public int bloodRestoreAmount = 20;
    public GameObject pickupGlowPrefab;
    public AudioClip pickupSound;
    public int staminaRestoreAmount = 100;

    private bool isCollected = false;

    private void OnTriggerEnter(Collider collider)
    {
        if (isCollected) return;

        Player player = collider.GetComponent<Player>();
        if (player == null) return;

        if (player.currentBlood < player.maxBlood)
        {
            player.RestoreBlood(bloodRestoreAmount);
            player.RestoreStamina(staminaRestoreAmount / 2); // temp

            if (pickupSound != null)
                AudioSource.PlayClipAtPoint(pickupSound, transform.position);

            if (pickupGlowPrefab != null)
                Instantiate(pickupGlowPrefab, transform.position, Quaternion.identity);

            HidePotion();
        }
    }

    void HidePotion()
    {
        isCollected = true;

        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<Collider>().enabled = false;
    }

    public void ResetPotion()
    {
        isCollected = false;

        GetComponent<MeshRenderer>().enabled = true;
        GetComponent<Collider>().enabled = true;
    }
}
