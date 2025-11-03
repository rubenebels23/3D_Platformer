using UnityEngine;

public class VelocityCalculator : MonoBehaviour
{
    private Vector3 _previousPos;
    public Vector3 Delta { get; private set; }   // platform displacement this frame

    public SpinningPlatform platform;

    void Start() => _previousPos = transform.position; //for the first frame

    void LateUpdate() // after all Updates moved it
    {
        Delta = transform.position - _previousPos;// calculate how far it moved
        _previousPos = transform.position;
    }

    // player script gets the platform's velocity from here
    public Vector3 GetVelocity(Transform playerPos)
    {
        Vector3 positionDifference = Delta * Time.deltaTime;
        if (platform != null)
        {
            Quaternion rotation = Quaternion.Euler(platform.RotateSpeedX * Time.deltaTime, platform.RotateSpeedY * Time.deltaTime, platform.RotateSpeedZ * Time.deltaTime);
            Vector3 position = rotation * (playerPos.position - platform.transform.position);

            positionDifference += position - (playerPos.position - platform.transform.position);
        }
        return positionDifference;
    }
}