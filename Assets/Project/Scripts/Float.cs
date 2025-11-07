using UnityEngine;

public class FloatOnly : MonoBehaviour
{
    public float floatSpeed = 5f;
    public float floatHeight = 0.25f;
        
    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        
        float newY = Mathf.Sin(Time.time * floatSpeed) * floatHeight;
        transform.position = startPos + new Vector3(0f, newY, 0f);
    }

    
}
