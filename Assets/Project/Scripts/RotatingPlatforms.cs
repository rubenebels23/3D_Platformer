using UnityEngine;

public class RotatingPlatforms : MonoBehaviour
{
    public float MoveSpeed = 10f;
    public float StartHeight = 1f;      // starting Y height
    public float MoveRange = 9f;        // how far it moves up/down

    private float initialX;
    private float initialZ;

    void Start()
    {
        initialX = transform.position.x;
        initialZ = transform.position.z;
    }

    void Update()
    {
        float y = StartHeight + Mathf.PingPong(Time.time * MoveSpeed, MoveRange);
        transform.position = new Vector3(initialX, y, initialZ);
    }
}
