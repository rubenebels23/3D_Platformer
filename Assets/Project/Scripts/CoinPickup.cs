using UnityEngine;
using System.Collections;

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
        // Hide pickup text
        pickupText.SetActive(false);

        // Trigger win screen
        StartCoroutine(WinSequence());

        // Destroy the coin visual immediately
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<Collider>().enabled = false;
    }

    public GameObject winScreen;   // assign in Inspector
    public Player playerScript;    // assign player

    private IEnumerator WinSequence()
    {
        // Stop player movement
        playerScript.enabled = false;

        // Show win screen
        winScreen.SetActive(true);

        // Wait 5 seconds
        yield return new WaitForSeconds(5f);

        // Hide screen
        winScreen.SetActive(false);

        // Respawn player (normal respawn)
        playerScript.Respawn();

        // Re-enable movement
        playerScript.enabled = true;

        // Reset coin (reactivate visuals)
        ResetCoin();
    }


    public void ResetCoin()
    {
        // reactivate visuals and collider
        GetComponent<MeshRenderer>().enabled = true;
        GetComponent<Collider>().enabled = true;

        // hide UI text
        pickupText.SetActive(false);
    }



}
