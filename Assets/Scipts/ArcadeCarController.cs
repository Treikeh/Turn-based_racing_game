using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ArcadeCarController : MonoBehaviour
{
    [Header("Car Settings")]
    public float acceleration = 50f;
    public float maxSpeed = 30f;
    public float steering = 50f;
    public float driftFactor = 0.95f; // lower = more drift
    public float traction = 5f;       // snap-back force to forward

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = new Vector3(0, -0.5f, 0); // makes car more stable
    }

    void FixedUpdate()
    {
        float moveInput = Input.GetAxis("Vertical");   // W/S or Up/Down
        float turnInput = Input.GetAxis("Horizontal"); // A/D or Left/Right

        // Forward acceleration
        if (moveInput != 0)
            rb.AddForce(transform.forward * moveInput * acceleration, ForceMode.Acceleration);

        // Limit max speed
        Vector3 flatVel = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
        if (flatVel.magnitude > maxSpeed)
            rb.linearVelocity = flatVel.normalized * maxSpeed + Vector3.up * rb.linearVelocity.y;

        // Steering (scaled by speed so you can�t spin at 0 speed)
        float steerAmount = turnInput * steering * (rb.linearVelocity.magnitude / maxSpeed);
        rb.MoveRotation(rb.rotation * Quaternion.Euler(0, steerAmount * Time.fixedDeltaTime, 0));

        // --- DRIFT / TRACTION ---
        Vector3 localVel = transform.InverseTransformDirection(rb.linearVelocity);
        localVel.x *= driftFactor; // reduce sideways velocity (fake grip)
        rb.linearVelocity = transform.TransformDirection(localVel);

        // Extra stability to avoid fishtailing
        rb.linearVelocity = Vector3.Lerp(rb.linearVelocity, transform.forward * localVel.z, Time.fixedDeltaTime * traction);
    }

    void OnCollisionStay(Collision col)
    {
        // Dampen velocity when in contact with walls
        rb.linearVelocity *= 0.98f; // tune down as needed
    }


}
