using UnityEngine;

public class MovingPlatforms : MonoBehaviour
{
    public Vector3 pointA;     // local offset start
    public Vector3 pointB;     // local offset end
    public float speed = 2f;

    private Vector3 worldA;
    private Vector3 worldB;
    private bool goingToB = true;

    void Start()
    {
        // Convert local offsets to world positions
        worldA = transform.position + pointA;
        worldB = transform.position + pointB;
    }

    void Update()
    {
        if (goingToB)
        {
            transform.position = Vector3.MoveTowards(transform.position, worldB, speed * Time.deltaTime);

            if (Vector3.Distance(transform.position, worldB) < 0.01f)
                goingToB = false;
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, worldA, speed * Time.deltaTime);

            if (Vector3.Distance(transform.position, worldA) < 0.01f)
                goingToB = true;
        }
    }
}
