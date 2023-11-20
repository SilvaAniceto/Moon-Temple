using UnityEngine;

namespace CustomGameController
{
    public static class CustomPerspective
    {
        public static Vector3 CustomForward
        {
            get
            {
                Vector3 isometricForward = Camera.main.transform.forward;
                isometricForward.y = 0;
                isometricForward = Vector3.Normalize(isometricForward);

                return isometricForward;
            }
        }
        public static Vector3 CustomRight
        {
            get
            {
                return Camera.main.transform.right;
            }
        }
    }
}