using UnityEngine;

public class SpinningPlatform : MonoBehaviour
{
    public float RotateSpeedX = 0f;
    public float RotateSpeedY = 3f;
    public float RotateSpeedZ = 0f;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(RotateSpeedX , RotateSpeedY , RotateSpeedZ ) * Time.deltaTime); 
    }
}
