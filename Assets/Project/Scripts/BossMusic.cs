using UnityEngine;

public class BossMusic : MonoBehaviour
{
    public AudioClip BossMusicClip;

    private AudioSource source;

    private void Start()
    {
        // Create AudioSource
        source = gameObject.AddComponent<AudioSource>();
        source.clip = BossMusicClip;
        source.loop = true;
        source.playOnAwake = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.GetComponent<Player>()) return;

        // Enter → play if not already
        if (!source.isPlaying)
            source.Play();
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.GetComponent<Player>()) return;

        // Exit → stop if playing
        if (source.isPlaying)
            source.Stop();
    }

    // Called from Player.Die()
    public void StopMusicImmediate()
    {
        if (source != null && source.isPlaying)
            source.Stop();
    }
}
