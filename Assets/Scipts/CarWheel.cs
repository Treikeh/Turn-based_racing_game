using UnityEngine;

public class CarWheel : MonoBehaviour
{
    [Header("General")]
    public bool enableSteering = false;
    [SerializeField] private float brakeFactor = 0.25f;
    [SerializeField] private float wheelRadius = 0.25f;
    public Transform wheelMesh;


    [Header("Suspension")]
    [SerializeField] private bool enableSuspension = true;
    [SerializeField] private float springForce = 25f;
    [SerializeField] private float springDamping = 3f;


    [Header("Sideways friction")]
    [SerializeField] private bool enableSidewaysFriction = true;
    [SerializeField] private float gripFactor = 0.8f;
    private float tireMass = 0.2f;


    // Variables set by the car script
    [HideInInspector] public bool isBraking;
    [HideInInspector] public float verticalInput;
    [HideInInspector] public float restDist = 1f;
    [HideInInspector] public float topSpeed = 30f;
    [HideInInspector] public AnimationCurve powerCurve;
    [HideInInspector] public LayerMask groundLayer;
    [HideInInspector] public Rigidbody carRb;


    void FixedUpdate()
    {
        RaycastHit rayHit;
        bool rayDidHit = Physics.Raycast(transform.position, -transform.up, out rayHit, restDist + wheelRadius, groundLayer);

        // How much the car is moving at the position of this wheel
        Vector3 worldVel = carRb.GetPointVelocity(transform.position);

        // Set mesh position to rayHit position if the wheel is on the ground else set wheel to be at max suspension distance
        wheelMesh.position = rayDidHit? rayHit.point + (transform.up * wheelRadius) : transform.position + (-transform.up * (restDist - wheelRadius));


        // Suspension
        if (rayDidHit && enableSuspension)
        {
            Vector3 springDir = transform.up;

            float offset = restDist - rayHit.distance;

            float vel = Vector3.Dot(springDir, worldVel);

            float force = (offset * springForce) - (vel * springDamping);

            carRb.AddForceAtPosition(springDir * force * carRb.mass, transform.position);
        }

        // Sideways friction
        if (rayDidHit && enableSidewaysFriction)
        {
            Vector3 frictionDir = transform.right;

            float tireVel = Vector3.Dot(frictionDir, worldVel);

            float desiredVelChange = -tireVel * gripFactor;

            float desiredAccel = desiredVelChange / Time.fixedDeltaTime;

            carRb.AddForceAtPosition(frictionDir * tireMass * desiredAccel, transform.position);
        }

        // Acceleration
        if (rayDidHit)
        {
            if (isBraking)
            {
                Vector3 brakeDir = transform.forward;

                float tireVel = Vector3.Dot(brakeDir, worldVel);

                float desiredVelChange = -tireVel * brakeFactor;

                float desiredAccel = desiredVelChange / Time.fixedDeltaTime;

                carRb.AddForceAtPosition(brakeDir * tireMass * desiredAccel, transform.position);
            }
            else
            {
                Vector3 accelDir = transform.forward;
                
                float carSpeed = Vector3.Dot(carRb.gameObject.transform.forward, carRb.linearVelocity);

                float normalizedSpeed = Mathf.Clamp01(Mathf.Abs(carSpeed) / topSpeed);
                
                float availableTorque = powerCurve.Evaluate(normalizedSpeed) * verticalInput;

                carRb.AddForceAtPosition(accelDir * availableTorque * carRb.mass, transform.position);
            }
        }
    }
}
