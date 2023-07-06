using UnityEngine;

namespace IOP
{
    public static class IsometricOrientedPerspective
    {
        //private static Vector3 m_isometricForward, m_isometricRight;
        //private static MoveDirection m_moveDirection;
        //public enum ControllType
        //{
        //    PointAndClick,
        //    KeyBoard,
        //    Joystick
        //}
        //private static ControllType m_controllerType = ControllType.KeyBoard;
        //public static UnityEvent<ControllType> OnControllTypeChange = new UnityEvent<ControllType>();

        //#region Properties
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
        /// <summary>
        /// Defines which direction the game object is facing, moving or rotating.
        /// </summary>
        //public MoveDirection Direction
        //{
        //    get
        //    {
        //        return m_moveDirection;
        //    }
        //    set
        //    {
        //        m_moveDirection = value;
        //    }
        //}
        //public static ControllType Type
        //{
        //    get
        //    {
        //        return m_controllerType;
        //    }
        //    set
        //    {
        //        if (m_controllerType == value) return;
        //        m_controllerType = value;

        //        OnControllTypeChange?.Invoke(m_controllerType);
        //    }
        //}
        //#endregion

        //#region Classes
        //public class MoveDirection
        //{
        //    public Vector3 righMovement = new Vector3();
        //    public Vector3 upMovement = new Vector3();
        //    public Vector3 slopeMovement = new Vector3();
        //    public Vector3 heading = new Vector3();
        //}
        //#endregion

        ///// <summary>
        ///// Sets the isometric directions with the Camera informations.
        ///// </summary>
        //public void IsometricSetup()
        //{
        //    m_isometricForward = Camera.main.transform.forward;
        //    m_isometricForward.y = 0;
        //    m_isometricForward = Vector3.Normalize(m_isometricForward);

        //    m_isometricRight = Camera.main.transform.right;

        //    if (m_moveDirection == null)
        //        m_moveDirection = new MoveDirection();           
        //}
    }
}