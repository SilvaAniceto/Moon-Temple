using CustomGameController;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SimpleCarController : MonoBehaviour
{
    public List<AxleInfo> axleInfos; // the information about each individual axle
    public float maxMotorTorque; // maximum torque the motor can apply to wheel
    public float maxSteeringAngle; // maximum steer angle the wheel can have
    public Rigidbody body;
    public float velocity;
    public float rpm;

    bool accelerator;
    bool brake;
    float steering;

    private CustomInputActions InputActions;

    private void Awake()
    {
        InputActions = new CustomInputActions();
        InputActions.Enable();
    }

    private void Update()
    {
        ReadPlayerInput();

        velocity = Mathf.Round(Mathf.Abs(body.velocity.z));
        rpm = axleInfos[0].leftWheel.rpm;
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
                if (accelerator)
                {
                    axleInfo.leftWheel.motorTorque = Mathf.Pow(maxMotorTorque, 4);
                    axleInfo.rightWheel.motorTorque = Mathf.Pow(maxMotorTorque, 4);
                    axleInfo.leftWheel.brakeTorque = 0;
                    axleInfo.rightWheel.brakeTorque = 0;
                }
                if (brake)
                {
                    body.velocity = Vector3.Lerp(body.velocity, Vector3.zero, Time.deltaTime );
                    axleInfo.leftWheel.motorTorque = 0;
                    axleInfo.rightWheel.motorTorque = 0;
                    axleInfo.leftWheel.brakeTorque = Mathf.Pow(maxMotorTorque, 6);
                    axleInfo.rightWheel.brakeTorque = Mathf.Pow(maxMotorTorque, 6);
                }
                if (!accelerator && !brake)
                {
                    body.velocity = Vector3.Lerp(body.velocity, Vector3.zero, Time.deltaTime / 1.5f);
                    axleInfo.leftWheel.motorTorque = 0;
                    axleInfo.rightWheel.motorTorque = 0;
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
        Vector2 direction = InputActions.PlayerActions.Move.ReadValue<Vector2>();

        accelerator = InputActions.PlayerActions.Sprint.IsPressed();
        brake = InputActions.PlayerActions.Jump.IsPressed();
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
