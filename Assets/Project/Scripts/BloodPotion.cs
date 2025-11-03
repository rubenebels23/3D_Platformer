using UnityEngine;
using UnityEngine.Rendering;

public class BloodPotion : MonoBehaviour
{
    public int bloodRestoreAmount = 20;

    void Start()
    {

    }

    private void OnTriggerEnter(Collider collider)
    {
        var teleport = collider.GetComponent<TeleportController>();
        if (teleport == null) return;

        if (teleport.currentBlood < teleport.maxBlood)
        {
            teleport.RestoreBlood(bloodRestoreAmount);
            Destroy(gameObject);
        }
    }
}

