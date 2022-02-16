using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarPhysics : MonoBehaviour
{
    //references
    public Rigidbody rb;

    //car stats
    public float acceleration = 1f;
    public float maxTurnRate = 1f;
    public float turnDampening = 0.9f;
    public float maxGrip = 0.02f;
    public float accelerationDropOff = 10f;
    public float gripFromAeroDyamicsFactor = 0.5f;
    public float accelerationForceFactor = 100f;

    //car data
    Vector3 velocity = Vector3.zero;
    float turnRate = 0f;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UIManager.SetSpeedometer(velocity.magnitude);
    }

    void FixedUpdate()
    {
        float deltaTime = Time.fixedDeltaTime;
        velocity = rb.velocity * deltaTime;
        float driftAngle = Vector3.Angle(velocity, transform.forward);
        if (driftAngle > 180f)
        {
            driftAngle = 180f - driftAngle;
        }
        float driftAngleSigned = driftAngle;
        if (Vector3.Cross(velocity, transform.forward).y < 0f)
        {
            driftAngleSigned = -driftAngleSigned;
        }
        Vector3 forceThisFrame = Vector3.zero;

        //control input, added keyboard just for debugging
        float turnRateFactor = maxTurnRate;
        float turnInput = InputManager.steerLeftPressed ? -1f : InputManager.steerRightPressed ? 1f : 0f;
        bool keyboardDebugControls = true;
        if (keyboardDebugControls)
        {
            turnInput += Input.GetKey(KeyCode.A) ? -1f : Input.GetKey(KeyCode.D) ? 1f : 0f;
        }
        bool isCountersteering = (driftAngleSigned > 5f && turnInput < 0f) || (driftAngleSigned < -5f && turnInput > 0f);
        if (isCountersteering)
        {
            turnRateFactor *= 0.5f;
        }else if (keyboardDebugControls)
        {
            //turnRateFactor = turnRateFactor * (Input.GetKey(KeyCode.Space) ? 3f : 1f); //todo: only allow for steering, not countersteering
        }
        if (Input.GetKey(KeyCode.Space))
        {
            turnRate *= 1.05f;
        }

        //turning
        turnRate = (turnRate + turnInput * turnRateFactor * deltaTime) * turnDampening;
        transform.Rotate(new Vector3(0f, turnRate, 0f));
        //rb.MoveRotation(Quaternion.Euler(Quaternion.Euler(new Vector3(0f, turnRate, 0f)) * transform.forward));

        //car physics
        float speedInForwardDirection = Mathf.Max(0f, Vector3.Dot(velocity, transform.forward));
        float accelerationFactor = Mathf.Max(0.05f, 1f - 0.5f * speedInForwardDirection / accelerationDropOff);
        //velocity += transform.forward * acceleration * accelerationFactor * deltaTime;
        forceThisFrame = transform.forward * acceleration * accelerationFactor * deltaTime;

        //real grip
        if(driftAngle > 0.1f)
        {
            //Vector3 lateralForceDirection = (Quaternion.Euler(0f, driftAngleSigned >= 0f ? 90f : -90f, 0f) * transform.forward).normalized;
            //velocity += lateralForceDirection * maxGrip;
        }

        //real grip test
        Vector3 lateralForceDirection = (Quaternion.Euler(0f, driftAngleSigned >= 0f ? 90f : -90f, 0f) * transform.forward).normalized;
        float lateralDrift = -Vector3.Dot(lateralForceDirection, velocity);
        float gripByLateralDrift = 0.75f - lateralDrift;
        float gripFromAeroDynamics = 1f + velocity.magnitude * gripFromAeroDyamicsFactor;
        //print(gripFromAeroDynamics);
        forceThisFrame += lateralForceDirection * maxGrip * gripByLateralDrift * gripFromAeroDynamics;
        //print(gripByLateralDrift);

        //rb.MovePosition(rb.position + velocity);
        rb.AddForce(forceThisFrame, ForceMode.Impulse);
        //transform.position += velocity;
        //rb.velocity = velocity / deltaTime;
        //print(velocity.magnitude + " " + driftAngleSigned + " " + driftAngle);
    }
}
