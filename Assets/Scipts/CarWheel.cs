using UnityEngine;

public class CarWheel : MonoBehaviour
{
    [Header("Suspension")]
    [SerializeField] private bool enableSuspension = true;
    [SerializeField] private float restDist = 1f;
    [SerializeField] private float springForce = 50f;
    [SerializeField] private float springDamping = 15f;

    [Header("Steering")]
    [SerializeField] private bool enableSteering = true;
    [SerializeField] private float gripFactor = 0.8f;
    private float tireMass = 0.1f;

    [Header("Acceleration")]
    [SerializeField] private bool enableAcceleration = true;
    [SerializeField] private float carTopSpeed = 10f;
    [SerializeField] private float brakeForce = 0.75f;
    [SerializeField] private AnimationCurve powerCurve;


    [Header("General")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Rigidbody carRB;


    // Update is called once per frame
    void FixedUpdate()
    {
        RaycastHit rayHit;
        bool rayDidHit = Physics.Raycast(transform.position, -transform.up, out rayHit, restDist * 1.25f, groundLayer);

        // How much the car is moving at the position of this wheel
        Vector3 worldVel = carRB.GetPointVelocity(transform.position);

        // Suspension
        if (rayDidHit && enableSuspension)
        {
            Vector3 springDir = transform.up;

            float offset = restDist - rayHit.distance;

            float vel = Vector3.Dot(springDir, worldVel);

            float force = (offset * springForce) - (vel * springDamping);

            carRB.AddForceAtPosition(springDir * force * carRB.mass, transform.position);
        }

        // Steering
        if (rayDidHit && enableSteering)
        {
            Vector3 steeringDir = transform.right;

            float steerVel = Vector3.Dot(steeringDir, worldVel);

            float desiredVelChange = -steerVel * gripFactor;

            float desiredAccel = desiredVelChange / Time.fixedDeltaTime;

            carRB.AddForceAtPosition(steeringDir * tireMass * desiredAccel, transform.position);
        }

        // Acceleration
        if (rayDidHit && enableAcceleration)
        {
            float verticalInput = Input.GetAxis("Vertical");
            bool isBraking = Input.GetKey(KeyCode.Space);

            if (isBraking)
            {
                Vector3 brakeDir = transform.forward;

                float tireVel = Vector3.Dot(brakeDir, worldVel);

                float desiredVelChange = -tireVel * brakeForce;

                float desiredAccel = desiredVelChange / Time.fixedDeltaTime;

                carRB.AddForceAtPosition(brakeDir * tireMass * desiredAccel, transform.position);
            }
            else
            {
                Vector3 accelDir = transform.forward;
                
                float carSpeed = Vector3.Dot(carRB.gameObject.transform.forward, carRB.linearVelocity);

                float normalizedSpeed = Mathf.Clamp01(Mathf.Abs(carSpeed) / carTopSpeed);
                
                float availableTorque = powerCurve.Evaluate(normalizedSpeed) * verticalInput;

                carRB.AddForceAtPosition(accelDir * availableTorque * carRB.mass, transform.position);
            }
        }
    }
}
