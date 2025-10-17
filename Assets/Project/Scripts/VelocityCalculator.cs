using UnityEngine;

public class VelocityCalculator : MonoBehaviour
{
    private Vector3 _previousPos;
    public Vector3 Delta { get; private set; }   // platform displacement this frame

    void Start() => _previousPos = transform.position; //for the first frame

    void LateUpdate() // after all Updates moved it
    {
        Delta = transform.position - _previousPos;// calculate how far it moved
        _previousPos = transform.position;
    }
}
