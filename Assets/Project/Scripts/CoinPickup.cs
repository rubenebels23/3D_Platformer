using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    public Transform player;
    public GameObject pickupText;
    public float showDistance = 4f;

    void Update()
    {
        if (player == null) return;

        float dist = Vector3.Distance(player.position, transform.position);

        // Too far → hide text
        if (dist > showDistance)
        {
            pickupText.SetActive(false);
            return;
        }

        // Close enough → show text
        pickupText.SetActive(true);

        // --- PERFECT BILLBOARD ROTATION (no flip, no mirror) ---
        Vector3 lookDir = Camera.main.transform.position - pickupText.transform.position;
        lookDir.y = 0; // keep text upright
        pickupText.transform.rotation = Quaternion.LookRotation(-lookDir);
        // --------------------------------------------------------

        // Press E to collect coin
        if (Input.GetKeyDown(KeyCode.E))
        {
            Collect();
        }
    }

    void Collect()
    {
        pickupText.SetActive(false);
        Destroy(gameObject);
        Debug.Log("Coin collected!");
    }
}
