using UnityEngine;

namespace IsometricOrientedPerspective
{
    public class IsometricMove : IsometricOrientedPerspective
    {
        public static IsometricMove m_moveInstance;

        private float m_horizontalMovement, m_verticalMovement;
        private bool m_leftClick;
        private bool m_isPhysicsMovement;
        private float m_moveDistance = 9f;
        private float m_movementDelta = 4f;
        private Rigidbody m_Rigidbody;
        private Vector2 m_moveDelta;
        private Vector3 m_startPosition;
        private float m_distanceTravelled;
        private bool m_onMove;

        #region Properties
        /// <summary>
        /// Define wheter on not the movement uses physics.
        /// </summary>
        public bool IsPhysicsMovement
        {
            get 
            { 
                return m_isPhysicsMovement;
            }

            set
            {
                if (m_isPhysicsMovement == value)
                    return;

                m_isPhysicsMovement = value;
            }
        }
        /// <summary>
        /// Define the limit distance that the movement can happen.
        /// </summary>
        public float MoveDistance
        {
            get
            {
                return m_moveDistance;
            }

            set
            {
                if (m_moveDistance == value)
                    return;

                m_moveDistance = value;
            }
        }
        /// <summary>
        /// Define the speed delta of the movement.
        /// </summary>
        public float MovementDelta
        {
            get
            {
                return m_movementDelta;
            }

            set
            {
                if (m_movementDelta == value)
                    return;

                m_movementDelta = value;
            }
        }
        /// <summary>
        /// Horizontal axi for movement in Isometric Perspective.
        /// </summary>
        public float HorizontalMovement
        {
            get
            {
                return m_horizontalMovement;
            }

            set
            {
                if (m_horizontalMovement == value)
                    return;

                m_horizontalMovement = value;
            }
        }
        /// <summary>
        /// Vertical axi for movement in Isometric Perspective.
        /// </summary>
        public float VerticalMovement
        {
            get
            {
                return m_verticalMovement;
            }
            set
            {
                if (m_verticalMovement == value)
                    return;

                m_verticalMovement = value;
            }
        }
        /// <summary>
        /// Input for left mouse button click.
        /// </summary>
        public bool LeftClick
        {
            get
            {
                return m_leftClick;
            }

            set
            {
                if (m_leftClick == value)
                    return;

                m_leftClick = value;
            }
        }
        /// <summary>
        /// Define wheter on not the movement is happening.
        /// </summary>
        public bool OnMove
        {
            get
            {
                return m_onMove;
            }

            set
            {
                if (m_onMove == value)
                    return;

                m_onMove = value;
            }
        }
        /// <summary>
        /// The rigidbody used to move with physics.
        /// </summary>
        public Rigidbody Rigidbody
        {
            get 
            { 
                return m_Rigidbody;
            }

            set
            {
                if (m_Rigidbody != null)
                    return;

                m_Rigidbody = value;
            }
        }
        /// <summary>
        /// Delta of the axis of movement input.
        /// </summary>
        public Vector2 MoveDelta
        {
            get
            {
                return m_moveDelta;
            }
            
            set
            {
                if (m_moveDelta == value)
                    return;

                m_moveDelta = value;
            }
        }
        /// <summary>
        /// The start position when the movement begins.
        /// </summary>
        public Vector3 StartPosition
        {
            get
            {
                return m_startPosition;
            }

            set
            {
                if (m_startPosition == value)
                    return;

                m_startPosition = value;
            }
        }                        
        #endregion

        public void Setup()
        {
            if (m_moveInstance == null)
                m_moveInstance = this;

            IsometricSetup();
        }

        /// <summary>
        /// Resolve the movement in Isometric Oriented Perspective.
        /// </summary>
        public void Move(float p_xAxis, float p_zAxis)
        {
            if (!IsometricCamera.m_instance.MovingCamera) // Prevents that the movement happens when the Camera is moving.
            { 
                if (IsPhysicsMovement) 
                {
                    if (IsometricRotation.m_rotationInstance.enabled)
                    {
                                                                                                                                                      //
                        float distance = Vector3.Distance(Direction.direction, transform.position);                                                   //     Resolve the point and click mouse 
                        m_onMove = distance > 0.2f ? true : false;                                                                                    // movement and rotation with physics.
                        if ((int)m_distanceTravelled < (int)m_moveDistance)                                                                           //
                            m_Rigidbody.MovePosition(Vector3.MoveTowards(transform.position, Direction.direction, m_movementDelta * Time.deltaTime)); //
                    }
                    else
                    {
                        Direction.direction = new Vector3(p_xAxis * m_movementDelta * Time.fixedDeltaTime, 0, p_zAxis * m_movementDelta * Time.fixedDeltaTime); //
                                                                                                                                                                //
                        Direction.direction = Camera.main.transform.TransformDirection(Direction.direction);                                                    //
                        Direction.direction.y = 0;                                                                                                              //   Resolve the axis movement using
                                                                                                                                                                // physics.
                        if ((int)m_distanceTravelled < (int)m_moveDistance)                                                                                     //
                            m_Rigidbody.MovePosition(m_Rigidbody.position + Direction.direction);                                                               //
                                                                                                                                                                //
                        m_onMove = true;                                                                                                                        //
                    }

                    if (!IsometricRotation.m_rotationInstance.enabled)                                                                         //
                        if (Direction.direction != Vector3.zero)                                                                               //    Resolves the rotation when using physics 
                            m_Rigidbody.rotation = Quaternion.Slerp(m_Rigidbody.rotation, Quaternion.LookRotation(Direction.direction), 0.5f); // axis movement.
                }
                else
                {
                    if (IsometricRotation.m_rotationInstance.enabled)
                    {
                        float distance = Vector3.Distance(Direction.direction, transform.position);                                              //
                        m_onMove = distance > 0.2f ? true : false;                                                                               //   Resolve the point and click mouse movement
                                                                                                                                                 // and rotation using transform movement.
                        if ((int)m_distanceTravelled < (int)m_moveDistance)                                                                      //
                            transform.position = Vector3.MoveTowards(transform.position, Direction.direction, m_movementDelta * Time.deltaTime); //
                    }
                    else
                    {
                        Direction.direction = new Vector3(p_xAxis, 0, p_zAxis);                                             //
                        Direction.righMovement = IsometricRight * m_movementDelta * Time.deltaTime * Direction.direction.x; //
                        Direction.upMovement = IsometricForward * m_movementDelta * Time.deltaTime * Direction.direction.z; //
                                                                                                                            //
                        if ((int)m_distanceTravelled < (int)m_moveDistance)                                                 //   Resolve the axis movement using transform movement.
                        {                                                                                                   //
                            transform.position += Direction.righMovement;                                                   //
                            transform.position += Direction.upMovement;                                                     //
                        }                                                                                                   //
                                                                                                                            //
                        m_onMove = true;                                                                                    //
                    }

                    if (!IsometricRotation.m_rotationInstance.enabled)
                    {
                        Direction.heading = Vector3.Normalize(Direction.righMovement + Direction.upMovement); //
                        if (Direction.heading != Vector3.zero)                                                //     Resolves the rotation when using transform axis movement.
                            transform.forward = Vector3.Lerp(transform.forward, Direction.heading, 0.40f);    //
                    }
                }
            }
        }

        /// <summary>
        /// Defines the input direction after changing the camera position.
        /// </summary>
        public void SetInputMoveDelta()
        {
            if (!m_isPhysicsMovement)
            {
                if (IsometricCamera.m_instance.CamPosition == IsometricCamera.CameraPosition.SOUTH)
                    m_moveDelta = new Vector2(HorizontalMovement, VerticalMovement);

                if (IsometricCamera.m_instance.CamPosition == IsometricCamera.CameraPosition.WEST)
                    m_moveDelta = new Vector2(VerticalMovement, -HorizontalMovement);

                if (IsometricCamera.m_instance.CamPosition == IsometricCamera.CameraPosition.NORTH)
                    m_moveDelta = new Vector2(-HorizontalMovement, -VerticalMovement);

                if (IsometricCamera.m_instance.CamPosition == IsometricCamera.CameraPosition.EAST)
                    m_moveDelta = new Vector2(-VerticalMovement, HorizontalMovement);
            }
            else
                m_moveDelta = new Vector2(HorizontalMovement, VerticalMovement);
        }

        /// <summary>
        /// Calculate the distance travelled when the movement begins.
        /// </summary>
        public void GetMoveDistance(Vector3 p_startPosition)
        {
            m_distanceTravelled = Vector3.Distance(p_startPosition, transform.position);

            if (m_distanceTravelled >= m_moveDistance)
                m_onMove = false;
        }
    }
}
