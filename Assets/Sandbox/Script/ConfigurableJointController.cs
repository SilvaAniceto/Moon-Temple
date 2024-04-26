using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CustomThirdPerson
{
    public class ConfigurableJointController : MonoBehaviour
    {
        [SerializeField] ConfigurableJoint joint;
        [SerializeField, Range(0.0f, 1.0f)] float value;
        [SerializeField] float otherValue;
        void FixedUpdate()
        {
            otherValue = Mathf.Lerp(-2.0f, 2.0f, new Vector3(value, 0.0f, 0.0f).magnitude);

            joint.targetPosition = new Vector3(0.0f, otherValue, 0.0f);
            joint.targetVelocity = Vector3.MoveTowards(joint.targetVelocity, new Vector3(0.0f, otherValue / 0.2f, 0.0f), Time.fixedDeltaTime);
        }
    }
}
