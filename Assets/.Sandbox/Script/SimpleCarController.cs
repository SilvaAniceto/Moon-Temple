using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SimpleCarController : MonoBehaviour
{
    public List<AxleInfo> axleInfos; // the information about each individual axle
    public float maxMotorTorque; // maximum torque the motor can apply to wheel
    public float maxSteeringAngle; // maximum steer angle the wheel can have
    public float maxBrakeTorque;
    public float gearRatio = 0.1f;
    public Rigidbody body;
    public float velocity;
    public float maxVelocity;
    //public float rpm;
    //public float motorTorque;
    public AnimationCurve torqueCurve;

    public float accelerator;
    public bool brake;
    public float steering;

    //private CustomInputActions InputActions;

    private void Awake()
    {
        //InputActions = new CustomInputActions();
        //InputActions.Enable();
    }

    private void Update()
    {
        ReadPlayerInput();

        velocity = Mathf.Round(Mathf.Abs(body.velocity.z));
        //rpm = Mathf.Round(axleInfos[0].leftWheel.rpm * 1000);
        //motorTorque = Mathf.Round(axleInfos[0].leftWheel.motorTorque);
    }

    public void FixedUpdate()
    {
        foreach (AxleInfo axleInfo in axleInfos)
        {
            if (axleInfo.steering)
            {
                axleInfo.leftWheel.steerAngle = steering * maxSteeringAngle;
                axleInfo.rightWheel.steerAngle = steering * maxSteeringAngle;
            }
            if (axleInfo.motor)
            {
                if (brake)
                {
                    body.velocity = Vector3.Lerp(body.velocity, Vector3.zero, Time.deltaTime);
                    axleInfo.leftWheel.motorTorque = 0;
                    axleInfo.rightWheel.motorTorque = 0;
                    axleInfo.leftWheel.brakeTorque = maxBrakeTorque;
                    axleInfo.rightWheel.brakeTorque = maxBrakeTorque;
                }
                else if (accelerator == 0 && !brake)
                {
                    body.velocity = Vector3.Lerp(body.velocity, Vector3.zero, Time.deltaTime / 1.5f);
                    axleInfo.leftWheel.motorTorque = 0;
                    axleInfo.rightWheel.motorTorque = 0;
                    axleInfo.leftWheel.brakeTorque = 0;
                    axleInfo.rightWheel.brakeTorque = 0;
                }
                else
                {
                    float appliedTorque = torqueCurve.Evaluate((velocity / maxVelocity) * maxMotorTorque * gearRatio) * accelerator;
                    
                    axleInfo.leftWheel.motorTorque = appliedTorque;
                    axleInfo.rightWheel.motorTorque = appliedTorque;
                    axleInfo.leftWheel.brakeTorque = 0;
                    axleInfo.rightWheel.brakeTorque = 0;
                }
            }
            ApplyLocalPositionToVisuals(axleInfo.leftWheel);
            ApplyLocalPositionToVisuals(axleInfo.rightWheel);
        }
    }

    public void ApplyLocalPositionToVisuals(WheelCollider collider)
    {
        if (collider.transform.childCount == 0)
        {
            return;
        }

        Transform visualWheel = collider.transform.GetChild(0);

        Vector3 position;
        Quaternion rotation;
        collider.GetWorldPose(out position, out rotation);

        visualWheel.transform.position = position;
        visualWheel.transform.rotation = rotation;
    }

    public void ReadPlayerInput()
    {
        Vector2 direction = Vector2.zero/*InputActions.PlayerActions.Move.ReadValue<Vector2>()*/;

        accelerator = Mathf.Clamp01(direction.y);
        brake = direction.y < 0.0f ? true : false;
        steering = direction.x;
    }
}

[System.Serializable]
public class AxleInfo
{
    public WheelCollider leftWheel;
    public WheelCollider rightWheel;
    public bool motor; // is this wheel attached to motor?
    public bool steering; // does this wheel apply steer angle?
}
