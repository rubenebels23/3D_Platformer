using UnityEngine;

public class SpinningPlatform : MonoBehaviour
{
    public float RotateSpeedX = 0f;
    public float RotateSpeedY = 3f;
    public float RotateSpeedZ = 0f;

    public float PauseSeconds = 0f;   // how long to pause
    public float SpinSeconds = 1f;    // how long to spin before pausing again

    private float timer = 0f;
    private bool isSpinning = true;

    void Update()
    {
        timer += Time.deltaTime;

        if (isSpinning)
        {
            // spinning logic
            transform.Rotate(new Vector3(RotateSpeedX, RotateSpeedY, RotateSpeedZ) * Time.deltaTime);

            // after SpinSeconds → switch to pause mode
            if (timer >= SpinSeconds)
            {
                timer = 0f;
                isSpinning = false;
            }
        }
        else
        {
            // paused — do nothing
            if (timer >= PauseSeconds)
            {
                timer = 0f;
                isSpinning = true; // start spinning again
            }
        }
    }
}
