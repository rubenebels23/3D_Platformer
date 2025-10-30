    using UnityEngine;

public class RotatingPlatforms : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }
    public float RotateSpeedX = 0f;
    public float RotateSpeedY = 3f;
    public float RotateSpeedZ = 0f;
    public float MoveSpeed = 10f;


    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(RotateSpeedX * 3, RotateSpeedY * 3, RotateSpeedZ * 3) * Time.deltaTime);
        transform.position = new Vector3(transform.position.x, 1f + Mathf.PingPong(Time.time * MoveSpeed, 9f), transform.position.z);
    }

    //velocity calculator maken
}
    


    
