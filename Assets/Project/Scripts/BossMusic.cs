using UnityEngine;

public class BossMusic : MonoBehaviour
{
    public AudioClip BossMusicClip;

    private AudioSource source;
    private bool playerInside = false;   // tracks if the player is inside the trigger

    private void Start()
    {
        source = gameObject.AddComponent<AudioSource>();
        source.clip = BossMusicClip;
        source.loop = true;
        source.playOnAwake = false;
    }

    private void Update()
    {
        // If the player is not inside → make sure music is OFF
        if (!playerInside)
        {
            if (source.isPlaying)
                source.Stop();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.GetComponent<Player>()) return;

        playerInside = true;

        if (!source.isPlaying)
            source.Play();
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.GetComponent<Player>()) return;

        playerInside = false;
    }

    // Called from Player.Die()
    public void StopMusicImmediate()
    {
        playerInside = false;  // force "not inside"
        if (source.isPlaying)
            source.Stop();
    }
}
