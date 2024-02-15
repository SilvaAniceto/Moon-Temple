using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class SimpleCarController : MonoBehaviour
{
    public List<AxleInfo> axleInfos; // the information about each individual axle
    public float maxMotorTorque; // maximum torque the motor can apply to wheel
    public float maxBrakeTorque;
    public float maxSteeringAngle; // maximum steer angle the wheel can have
    public float steeringRangeAtMaxSpeed = 10;
    public float maxVelocity;
    public Rigidbody body;

    public float accelerator;
    public bool brake;
    public float steering;

    private CustomInputActions InputActions;

    float forwardSpeed;
    float speedFactor;
    float currentMotorTorque;
    float currentSteerRange;

    private void Awake()
    {
        InputActions = new CustomInputActions();
        InputActions.Enable();
    }

    private void Update()
    {
        ReadPlayerInput();

        forwardSpeed = Vector3.Dot(transform.forward, body.velocity);

        speedFactor = Mathf.InverseLerp(0, maxVelocity * 3.6f, forwardSpeed);

        currentMotorTorque = Mathf.Lerp(maxMotorTorque, 0, speedFactor);

        currentSteerRange = Mathf.Lerp(maxSteeringAngle, steeringRangeAtMaxSpeed, speedFactor);
    }

    public void FixedUpdate()
    {
        if (body.velocity.magnitude > maxVelocity)
        {
            body.velocity = Vector3.ClampMagnitude(body.velocity, maxVelocity * 3.6f);
        }

        foreach (AxleInfo axleInfo in axleInfos)
        {
            if (axleInfo.steering)
            {
                axleInfo.leftWheel.steerAngle = steering * currentSteerRange;
                axleInfo.rightWheel.steerAngle = steering * currentSteerRange;
            }
            if (brake)
            {
                axleInfo.leftWheel.motorTorque = 0;
                axleInfo.rightWheel.motorTorque = 0;
                axleInfo.leftWheel.brakeTorque = maxBrakeTorque;
                axleInfo.rightWheel.brakeTorque = maxBrakeTorque;
            }
            else
            {
                axleInfo.leftWheel.brakeTorque = 0;
                axleInfo.rightWheel.brakeTorque = 0;
            }
            if (axleInfo.motor)
            {
                if (accelerator != 0 && !brake)
                {
                    axleInfo.leftWheel.motorTorque = accelerator * currentMotorTorque;
                    axleInfo.rightWheel.motorTorque = accelerator * currentMotorTorque;
                    body.AddForce(transform.forward * maxMotorTorque, ForceMode.Force);
                    body.AddForceAtPosition(transform.forward * maxMotorTorque, axleInfo.leftWheel.transform.position, ForceMode.Force);
                    body.AddForceAtPosition(transform.forward * maxMotorTorque, axleInfo.rightWheel.transform.position, ForceMode.Force);
                }
            }

            ApplyLocalPositionToVisuals(axleInfo.leftWheel);
            ApplyLocalPositionToVisuals(axleInfo.rightWheel);
        }
    }
    private void LateUpdate()
    {
        //if (steering == 0) transform.localPosition = new Vector3(Mathf.Round(transform.localPosition.x) * 100 /100, transform.localPosition.y, transform.localPosition.z);
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
        accelerator = Mathf.Clamp01(InputActions.DriverActions.Accelerator.ReadValue<float>());
        brake = InputActions.DriverActions.Brake.IsPressed();
        Vector2 steer = InputActions.DriverActions.Steer.ReadValue<Vector2>();
        steering = steer.x;
    }

#if UNITY_EDITOR
    void OnGUI()
    {
        GUI.Box(new Rect(0, 0, 250, 250), "");
        GUILayout.Label("   Foward Speed: " + Mathf.Round(forwardSpeed) * 100 / 100);
        GUILayout.Label("   Speed Factor: " + Mathf.Round(speedFactor) * 100 / 100);
        GUILayout.Label("   Current Motor Torque: " + Mathf.Round(currentMotorTorque) * 100 / 100);
        GUILayout.Label("   Current Steer Range: " + Mathf.Round(currentSteerRange) * 100 / 100);
        GUILayout.Label("   Body Velocity: " + Mathf.Round(body.velocity.magnitude) * 100 / 100);
    }
#endif
}

[System.Serializable]
public class AxleInfo
{
    public WheelCollider leftWheel;
    public WheelCollider rightWheel;
    public bool motor; // is this wheel attached to motor?
    public bool steering; // does this wheel apply steer angle?
}
