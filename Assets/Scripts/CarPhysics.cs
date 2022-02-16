using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarPhysics : MonoBehaviour
{
    //car stats
    public float acceleration = 1f;
    public float maxTurnRate = 1f;
    public float turnDampening = 0.9f;
    public float maxGrip = 0.02f;
    public float accelerationDropOff = 10f;

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

        //control input, added keyboard just for debugging
        float turnInput = InputManager.steerLeftPressed ? -1f : InputManager.steerRightPressed ? 1f : 0f;
        bool keyboardDebugControls = true;
        if (keyboardDebugControls)
        {
            turnInput += Input.GetKey(KeyCode.A) ? -1f : Input.GetKey(KeyCode.D) ? 1f : 0f;
        }

        //turning
        turnRate = (turnRate + turnInput * maxTurnRate * deltaTime) * turnDampening;
        transform.Rotate(new Vector3(0f, turnRate, 0f));

        //car physics
        float speedInForwardDirection = Mathf.Max(0f, Vector3.Dot(velocity, transform.forward));
        float accelerationFactor = Mathf.Max(0.05f, 1f - 0.5f * speedInForwardDirection / accelerationDropOff);
        velocity += transform.forward * acceleration * accelerationFactor * deltaTime;
        
        //grip
        float driftAngle = Vector3.Angle(velocity, transform.forward);
        if(driftAngle > 180f)
        {
            driftAngle = 180f - driftAngle;
        }
        float driftAngleSigned = driftAngle;
        if (Vector3.Cross(velocity, transform.forward).y < 0f)
        {
            driftAngleSigned = -driftAngleSigned;
        }
        float gripBraking = maxGrip * driftAngle / 90f;
        //velocity = velocity * (1f - gripBraking);

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
        velocity += lateralForceDirection * maxGrip * gripByLateralDrift;
        //print(gripByLateralDrift);

        transform.position += velocity;
        //print(velocity.magnitude + " " + driftAngleSigned + " " + driftAngle);
    }
}
