using UnityEngine;
using UnityEngine.Animations;

namespace CustomGameController
{
    public class Debugger : MonoBehaviour
    {
        //public bool enableDebugger = false;
        //public bool debugGroundDetection = false;
        //public bool debugSlopeDetection = false;
        //public bool debugDirection = false;
        //public CustomCharacterController controller;
        //public CustomCamera customCamera;
        //public Transform characterCompass;
        //private DebugInput DebugInput;

        //private void Awake()
        //{
        //    DebugInput = new DebugInput();
        //    DebugInput.Enable();
        //}

        //void Update()
        //{
        //    characterCompass.gameObject.SetActive(debugDirection);

        //    if (!enableDebugger) return;

        //    if (debugDirection)
        //    {
        //        characterCompass.SetParent(controller.transform);
        //        characterCompass.transform.localPosition = new Vector3(0.0f, controller.CharacterController.height - 0.25f, controller.CharacterController.radius);
        //        characterCompass.rotation = Quaternion.FromToRotation(characterCompass.up, controller.SlopeHit().normal) * characterCompass.rotation;
        //    }
        //    else
        //    {
        //        characterCompass.SetParent(transform);
        //    }
        //}
        //private void OnDrawGizmos()
        //{
        //    if (!enableDebugger) return;

        //    if (debugGroundDetection && controller.GroundCheckOrigin != null)
        //    {
        //        Gizmos.color = Color.red;
        //        Gizmos.DrawWireSphere(controller.GroundCheckOrigin.position, controller.CharacterController.radius);
        //    }

        //    if (debugSlopeDetection)
        //    {
        //        foreach (Transform t in controller.SlopeCheckList)
        //        {
        //            if (t != null)
        //            {
        //                Gizmos.color = Color.cyan;
        //                Gizmos.DrawLine(t.position, new Vector3(t.position.x, t.position.y - controller.CharacterController.height / 2.0f, t.position.z));
        //            }
        //        }
        //    }
        //}

        //private float TimeStamp = 0.66f;
        //private float ActionTimer = 0.33f;
        //private bool ActionTime
        //{
        //    get
        //    {
        //        if (TimeStamp >= 0)
        //        {
        //            TimeStamp -= Time.deltaTime;
        //            return false;
        //        }
        //        else if (TimeStamp < 0)
        //        {
        //            TimeStamp = 0.66f;

        //            ActionTimer -= Time.deltaTime;

        //            if (ActionTimer < 0)
        //            {
        //                TimeStamp = 0.66f;
        //                ActionTimer = 0.33f;
        //            }

        //            return true;
        //        }
        //        return false;
        //    }
        //}
    }
}
