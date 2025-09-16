using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    [SerializeField] private float restDist = 0.75f;
    [SerializeField] private float maxTurnAngle = 30f;
    [SerializeField] private float topSpeed = 50;
    [SerializeField] private AnimationCurve powerCurve;
    [SerializeField] private LayerMask gorundLayer;
    [SerializeField] private List<CarWheel> wheels;

    private bool isBraking;
    private float horizonalInput;
    private float verticalInput;
    private Rigidbody rb;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        // Set wheel variables
        for (int i = 0; i < wheels.Count; i++)
        {
            CarWheel wheel = wheels[i];
            wheel.carRb = rb;
            wheel.topSpeed = topSpeed;
            wheel.powerCurve = powerCurve;
            wheel.restDist = restDist;
            wheel.groundLayer = gorundLayer;
        }
    }


    // Update is called once per frame
    void Update()
    {
        horizonalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        // Turn wheels
        for (int i = 0; i < wheels.Count; i++)
        {
            CarWheel wheel = wheels[i];
            wheel.isBraking = isBraking;
            wheel.verticalInput = verticalInput;

            if (wheel.enableSteering)
            {
                wheel.transform.localRotation = Quaternion.Euler(0f, maxTurnAngle * horizonalInput, 0f);
            }
        }
    }
}
