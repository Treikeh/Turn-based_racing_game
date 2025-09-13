using UnityEngine;

public class Car : MonoBehaviour
{
    [SerializeField] private float maxTurnAngle = 30f;
    [SerializeField] private Transform frontLeftWheel;
    [SerializeField] private Transform frontRightWheel;

    private float horizonalInput;
    [HideInInspector] public Rigidbody rb;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }


    // Update is called once per frame
    void Update()
    {
        horizonalInput = Input.GetAxis("Horizontal");

        frontLeftWheel.localRotation = Quaternion.Euler(0f, maxTurnAngle * horizonalInput, 0f);
        frontRightWheel.localRotation = Quaternion.Euler(0f, maxTurnAngle * horizonalInput, 0f);
    }
}
