using UnityEngine;

namespace IOP
{
    public static class IsometricOrientedPerspective
    {
        /// <summary>
        /// New forward orientation for Isometric Perspective.
        /// </summary>
        public static Vector3 IsometricForward
        {
            get
            {
                Vector3 isometricForward = Camera.main.transform.forward;
                isometricForward.y = 0;
                isometricForward = Vector3.Normalize(isometricForward);

                return isometricForward;
            }
        }
        /// <summary>
        /// New right orientation for Isometric Perspective.
        /// </summary>
        public static Vector3 IsometricRight
        {
            get
            {
                return Camera.main.transform.right;
            }
        }
    }
}