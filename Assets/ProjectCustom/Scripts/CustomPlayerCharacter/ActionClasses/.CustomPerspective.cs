using UnityEngine;

namespace CustomGameController
{
    public static class CustomPerspective
    {
        public static Vector3 CustomForward
        {
            get
            {
                Vector3 forward = PlayerCharacterController.CharacterCamera.transform.forward;
                forward.y = 0;
                forward = Vector3.Normalize(forward);

                return forward;
            }
        }
        public static Vector3 CustomRight
        {
            get
            {
                return PlayerCharacterController.CharacterCamera.transform.right;
            }
        }
    }
}