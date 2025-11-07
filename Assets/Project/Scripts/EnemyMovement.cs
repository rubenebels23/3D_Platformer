using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 4f;
    public float jumpHeight = 10f;
    public float gravity = -9.807f;
    public float airDrag = 2f;

    [Header("Ground Detection")]
    public LayerMask groundMask;
    private CharacterController controller;

    private Vector3 velocity;
    private Vector3 glideVelocity;
    private bool groundedEnemy;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        //Ground check
        groundedEnemy = Physics.CheckSphere(transform.position - new Vector3(0, controller.height / 2, 0), 0.25f, groundMask);

        //Apply gravity
        if (!groundedEnemy)
            velocity.y += gravity * Time.deltaTime;

        //Apply movement
        controller.Move((glideVelocity + velocity) * Time.deltaTime);
    }

    void LateUpdate()
    {
        // 1️⃣ Only do this if we're standing on something
        if (!groundedEnemy)
            return;

        // 2️⃣ Shoot ray straight down
        RaycastHit hitInfo;
        bool hitSomething = Physics.Raycast(
            transform.position,
            Vector3.down,
            out hitInfo,
            controller.height + 0.5f,
            groundMask
        );

        if (!hitSomething)
            return;

        // 3️⃣ Check if what we hit is a moving/rotating platform
        VelocityCalculator platform = hitInfo.collider.GetComponent<VelocityCalculator>();
        if (platform == null)
            return;

        // 4️⃣ Get how far the platform moved this frame
        Vector3 platformMovement = platform.GetVelocity(transform);
        if (platformMovement == Vector3.zero)
            return;

        // 5️⃣ Move enemy with platform
        controller.Move(platformMovement);
    }

}
