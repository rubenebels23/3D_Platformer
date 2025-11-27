using UnityEngine;

// Ensure this runs before your PlayerMovement LateUpdate
[DefaultExecutionOrder(-1000)]
public class VelocityCalculator : MonoBehaviour
{
    // Per-frame deltas (world space)
    public Vector3 FramePositionDelta { get; private set; }
    public Quaternion FrameRotationDelta { get; private set; }

    private Vector3 _prevPos;
    private Quaternion _prevRot;

    void Awake()
    {
        _prevPos = transform.position;
        _prevRot = transform.rotation;
        FramePositionDelta = Vector3.zero;
        FrameRotationDelta = Quaternion.identity;
    }

    // Compute how the platform actually moved BETWEEN frames
    void LateUpdate()
    {
        FramePositionDelta = transform.position - _prevPos;
        FrameRotationDelta = transform.rotation * Quaternion.Inverse(_prevRot);

        _prevPos = transform.position;
        _prevRot = transform.rotation;
    }

    // Displacement for a world point standing on the platform this frame.
    // Uses last frame's pivot (_prevPos) to compute pure rotational displacement.
    public Vector3 GetDisplacementAtPoint(Vector3 worldPoint)
    {
        Vector3 rPrev = worldPoint - _prevPos;                // point relative to last frame's pivot
        Vector3 rNow  = FrameRotationDelta * rPrev;           // rotate it by the actual frame rotation
        Vector3 rotDisp = rNow - rPrev;                       // rotational displacement
        return FramePositionDelta + rotDisp;                  // + linear displacement
    }
}
