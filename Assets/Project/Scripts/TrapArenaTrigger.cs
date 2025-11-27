using UnityEngine;

public class ArenaTrigger : MonoBehaviour
{
    public GameObject walls;   // assign your walls parent here
    private bool hasActivated = false;

    private void OnTriggerEnter(Collider other)
    {
        if (hasActivated) return;                // already triggered once
        if (!other.CompareTag("Player")) return; // only react to player

        walls.SetActive(true);                   // show walls
        hasActivated = true;                     // lock trigger
    }

    // Call this from your player death code:
    public void RemoveWalls()
    {
        walls.SetActive(false);
    }
}
