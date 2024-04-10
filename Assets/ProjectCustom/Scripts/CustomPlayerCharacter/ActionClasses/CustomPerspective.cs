using UnityEngine;

namespace CustomGameController
{
    public static class CustomPerspective
    {
        public static Vector3 CustomForward
        {
            get
            {
                Vector3 forward = CustomPlayer.CharacterCamera.transform.forward;
                forward.y = 0;
                forward = Vector3.Normalize(forward);

                return forward;
            }
        }
        public static Vector3 CustomRight
        {
            get
            {
                return CustomPlayer.CharacterCamera.transform.right;
            }
        }
    }
}