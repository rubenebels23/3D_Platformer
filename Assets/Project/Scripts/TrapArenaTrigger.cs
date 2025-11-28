    using UnityEngine;

    public class ArenaTrigger : MonoBehaviour
    {
        public GameObject walls;
        private bool hasActivated = false;

        private void OnTriggerEnter(Collider other)
        {
            if (hasActivated) return;
            if (!other.CompareTag("Player")) return;

            walls.SetActive(true);
            hasActivated = true;
        }

        public void ResetTrigger()
        {
            Debug.Log("ArenaTrigger: Resetting trigger and walls.");
            hasActivated = false;
            walls.SetActive(false);      // <-- REQUIRED FIX
            gameObject.SetActive(true);  // ensure trigger reactivates
        }


        
    }
