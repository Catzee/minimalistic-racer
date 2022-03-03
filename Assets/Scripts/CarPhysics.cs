using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarPhysics : MonoBehaviour
{
    //references
    public Rigidbody rb;
    public Transform[] wheels;

    //car stats
    public float acceleration = 1f;
    public float maxTurnRate = 1f;
    public float turnDampening = 0.9f;
    public float maxGrip = 0.02f;
    public float accelerationDropOff = 10f;
    public float gripFromAeroDyamicsFactor = 0.5f;
    public float accelerationForceFactor = 100f;
    public float wheelModelSteeringAngle = 30f;

    //internal car data
    Vector3 velocity = Vector3.zero;
    float turnRate = 0f;

    //data on track
    private int currentCheckpoint = 1;
    private float lapTime = -1f;



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
        if(!(GameManager.GetGameState() == GameManager.GAME_STATE_RACING))
        {
            return;
        }
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
        SetSteeringAngleForWheelModels(turnInput * wheelModelSteeringAngle);
        bool isCountersteering = (driftAngleSigned > 5f && turnInput < 0f) || (driftAngleSigned < -5f && turnInput > 0f);
        if (isCountersteering)
        {
            turnRateFactor *= 0.5f;
        }
        if (Input.GetKey(KeyCode.Space))
        {
            turnRate *= 1.05f;
        }

        //turning
        turnRate = (turnRate + turnInput * turnRateFactor * deltaTime) * turnDampening;
        transform.Rotate(new Vector3(0f, turnRate, 0f));

        //acceleration
        float speedInForwardDirection = Mathf.Max(0f, Vector3.Dot(velocity, transform.forward));
        float accelerationFactor = Mathf.Max(0.05f, 1f - 0.5f * speedInForwardDirection / accelerationDropOff);
        forceThisFrame = transform.forward * acceleration * accelerationFactor * deltaTime;


        //sideways grip
        Vector3 lateralForceDirection = (Quaternion.Euler(0f, driftAngleSigned >= 0f ? 90f : -90f, 0f) * transform.forward).normalized;
        float lateralDrift = -Vector3.Dot(lateralForceDirection, velocity);
        float gripByLateralDrift = 0.75f - lateralDrift;
        float gripFromAeroDynamics = 1f + velocity.magnitude * gripFromAeroDyamicsFactor;
        forceThisFrame += lateralForceDirection * maxGrip * gripByLateralDrift * gripFromAeroDynamics;

        //apply result
        rb.AddForce(forceThisFrame, ForceMode.Impulse);

        //time tracking
        lapTime += Time.fixedDeltaTime;
        UIManager.SetLaptimeValue(lapTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        int checkpointCheckResult = CheckpointManager.IsCheckpointNumber(other.transform);
        if(checkpointCheckResult == currentCheckpoint)
        {
            currentCheckpoint++;
            bool wasLastCheckpoint = currentCheckpoint >= CheckpointManager.trackCheckpoints.Length;
            if (wasLastCheckpoint)
            {
                currentCheckpoint = 0; //we hit all other checkpoints, now need to hit the finish line which is checkpoint 0
            }
            if(checkpointCheckResult == 0)
            {
                HighscoreManager.ReportLapTime(lapTime, GameManager.GetCurrentTrackIndex());
                lapTime = 0f;
            }
        }
    }

    private void SetSteeringAngleForWheelModels(float angle)
    {
        //smooth it a bit so it looks a bit nicer
        wheels[0].transform.localRotation = Quaternion.Slerp(wheels[0].transform.localRotation, Quaternion.Euler(0f, angle, 90f), Time.fixedDeltaTime * 10f);
        wheels[1].transform.localRotation = Quaternion.Slerp(wheels[1].transform.localRotation, Quaternion.Euler(0f, angle, 90f), Time.fixedDeltaTime * 10f);
    }

    public void ResetCar()
    {
        transform.position = GameManager.GetInstance().spawnPosition.position;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        velocity = Vector3.zero;
        turnRate = 0f;
        transform.forward = Vector3.forward;
        currentCheckpoint = 1;
        lapTime = 0f;
        UIManager.SetLaptimeValue(lapTime);
    }
}
