using UnityEngine;
using UnityEngine.Events;

namespace IOP
{
    public class IsometricMove : IsometricOrientedPerspective
    {
        public static IsometricMove m_moveInstance;

        private float m_horizontalMovement, m_verticalMovement;
        private bool m_leftClick;
        private float m_moveDistance = 9f;
        private float m_movementDelta = 4f;
        private Vector2 m_moveDelta;
        private Vector3 m_startPosition;
        private float m_distanceTravelled;
        private float m_maxSlopeAngle, m_slopeAngle;
        private bool m_onMove;
        private RaycastHit m_slopeHit;
        public enum MoveType { FREE, COMBAT}
        private MoveType m_moveType = MoveType.FREE;

        public UnityEvent<MoveType> OnMoveTypeChange;

        #region Properties
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
        public MoveType MoveContext
        {
            get
            {
                return m_moveType;
            }

            set
            {
                if (m_moveType == value)
                    return;

                m_moveType = value;
                OnMoveTypeChange?.Invoke(m_moveType);
            } 
        }
        public RaycastHit SlopeHit
        {
            get
            {
                return m_slopeHit;
            }
        }
        public float MaxSlopeAngle
        {
            get
            {
                return m_maxSlopeAngle;
            }
            set
            {
                if (m_maxSlopeAngle == value)
                    return;

                m_maxSlopeAngle = value;
            }
        }
        public float SlopeAngle
        {
            get
            {
                return m_slopeAngle;
            }
            set
            {
                if (m_slopeAngle == value)
                    return;

                m_slopeAngle = value;
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
        /// Resolve the movement in Isometric Oriented Perspective with no Physics.
        /// </summary>
        public void Move(float p_xAxis, float p_zAxis)
        {
            if (IsometricCamera.m_instance.MovingCamera) return; // Prevents that the movement happens when the Camera is moving.
            
            if (IsometricRotation.m_rotationInstance.enabled)
            {
                float distance = Vector3.Distance(Direction.direction, transform.position);                                           
                m_onMove = distance > 0.2f ? true : false;

                if (!OnSlope())
                {
                    if (m_moveType == MoveType.COMBAT)                                                                                    
                    {                                                                                                                            
                        if ((int)m_distanceTravelled < (int)m_moveDistance)                                                                      
                            transform.position = Vector3.MoveTowards(transform.position, Direction.direction, m_movementDelta * Time.deltaTime); 
                    }                                                                                                                            
                    else                                                                                                                         
                        transform.position = Vector3.MoveTowards(transform.position, Direction.direction, m_movementDelta * Time.deltaTime);
                }
                else
                {
                    if (m_moveType == MoveType.COMBAT)
                    {
                        if ((int)m_distanceTravelled < (int)m_moveDistance)
                            transform.position = Vector3.MoveTowards(transform.position, GetSlopeMoveDirection(), m_movementDelta * Time.deltaTime);
                    }
                    else
                        transform.position = Vector3.MoveTowards(transform.position, GetSlopeMoveDirection(), m_movementDelta * Time.deltaTime);
                }
            }
            else
            {
                if (!OnSlope())
                {
                    Direction.direction = new Vector3(p_xAxis, 0, p_zAxis);
                    Direction.righMovement = IsometricRight * m_movementDelta * Time.deltaTime * Direction.direction.x;
                    Direction.slopeMovement = Vector3.zero;
                    Direction.upMovement = IsometricForward * m_movementDelta * Time.deltaTime * Direction.direction.z;

                    Direction.direction = Direction.righMovement + Direction.slopeMovement + Direction.upMovement;
                }
                else
                {
                    Direction.direction = new Vector3(p_xAxis, 0, p_zAxis);
                    Direction.righMovement = IsometricRight * m_movementDelta * Time.deltaTime * Direction.direction.x;
                    Direction.slopeMovement = Vector3.up * m_movementDelta * Time.deltaTime * GetSlopeMoveDirection().y;
                    Direction.upMovement = IsometricForward * m_movementDelta * Time.deltaTime * Direction.direction.z;

                    Direction.direction = Direction.righMovement + Direction.slopeMovement + Direction.upMovement;
                }


                if (m_moveType == MoveType.COMBAT)
                {
                    if ((int)m_distanceTravelled < (int)m_moveDistance)
                        transform.position += Direction.direction;
                }
                else  
                    transform.position += Direction.direction;

                m_onMove = true;                                                                                        
            }

            if (!IsometricRotation.m_rotationInstance.enabled)
            {
                Direction.heading = Vector3.Normalize(Direction.righMovement + Direction.upMovement);
                if (Direction.heading != Vector3.zero)                                                
                    transform.forward = Vector3.Lerp(transform.forward, Direction.heading, 0.40f);    
            }
        }

        /// <summary>
        /// Resolve the movement in Isometric Oriented Perspective with Physics.
        /// </summary>
        public void Move(float p_xAxis, float p_zAxis, Rigidbody p_rigidbody)
        {
            if (IsometricCamera.m_instance.MovingCamera) return; // Prevents that the movement happens when the Camera is moving.

            #region DEPRECATED
            //if (IsometricRotation.m_rotationInstance.enabled)
            //{
            //    float distance = Vector3.Distance(Direction.direction, transform.position);
            //    m_onMove = distance > 0.2f ? true : false;

            //    if (!OnSlope())
            //    {
            //        if (m_moveType == MoveType.COMBAT)
            //        {
            //            if ((int)m_distanceTravelled < (int)m_moveDistance)
            //                p_rigidbody.MovePosition(Vector3.MoveTowards(transform.position, Direction.direction, m_movementDelta * Time.deltaTime));
            //        }
            //        else
            //            p_rigidbody.MovePosition(Vector3.MoveTowards(transform.position, Direction.direction, m_movementDelta * Time.deltaTime));
            //    }
            //    else
            //    {
            //        if (m_moveType == MoveType.COMBAT)
            //        {
            //            if ((int)m_distanceTravelled < (int)m_moveDistance)
            //                p_rigidbody.MovePosition(Vector3.MoveTowards(transform.position, GetSlopeMoveDirection(), m_movementDelta * Time.deltaTime));
            //        }
            //        else
            //            p_rigidbody.MovePosition(Vector3.MoveTowards(transform.position, GetSlopeMoveDirection(), m_movementDelta * Time.deltaTime));
            //    }
            //}
            //else
            //{
            //    Direction.direction = new Vector3(p_xAxis, 0, p_zAxis);
            //    Direction.righMovement = IsometricRight * m_movementDelta * Time.fixedDeltaTime * Direction.direction.x;
            //    Direction.slopeMovement = Vector3.zero;
            //    Direction.upMovement = IsometricForward * m_movementDelta * Time.fixedDeltaTime * Direction.direction.z;

            //    Direction.direction = Direction.righMovement + Direction.slopeMovement + Direction.upMovement;

            //    if (!OnSlope())
            //    {

            //        if (m_moveType == MoveType.COMBAT)
            //        {
            //            if ((int)m_distanceTravelled < (int)m_moveDistance)
            //                p_rigidbody.MovePosition(p_rigidbody.position + Direction.direction);
            //        }
            //        else
            //            p_rigidbody.MovePosition(p_rigidbody.position + Direction.direction);

            //        m_onMove = true;
            //    }
            //    else
            //    {
            //        if (m_moveType == MoveType.COMBAT)
            //        {
            //            if ((int)m_distanceTravelled < (int)m_moveDistance)
            //                p_rigidbody.MovePosition(transform.position + GetSlopeMoveDirection().normalized);
            //        }
            //        else
            //            p_rigidbody.MovePosition(transform.position + GetSlopeMoveDirection().normalized);

            //        m_onMove = true;
            //    }
            //}
            #endregion

            Direction.direction = new Vector3(p_xAxis, 0, p_zAxis);
            Direction.righMovement = IsometricRight * m_movementDelta * Time.fixedDeltaTime * Direction.direction.x;
            Direction.slopeMovement = Vector3.zero;
            Direction.upMovement = IsometricForward * m_movementDelta * Time.fixedDeltaTime * Direction.direction.z;

            Direction.direction = Direction.righMovement + Direction.slopeMovement + Direction.upMovement;

            if (!OnSlope())
            {

                if (m_moveType == MoveType.COMBAT)
                {
                    if ((int)m_distanceTravelled < (int)m_moveDistance)
                        p_rigidbody.MovePosition(p_rigidbody.position + Direction.direction);
                }
                else
                    p_rigidbody.MovePosition(p_rigidbody.position + Direction.direction);

                m_onMove = true;
            }
            else
            {
                if (m_moveType == MoveType.COMBAT)
                {
                    if ((int)m_distanceTravelled < (int)m_moveDistance)
                        p_rigidbody.MovePosition(transform.position + GetSlopeMoveDirection().normalized / 5);
                }
                else
                    p_rigidbody.MovePosition(transform.position + GetSlopeMoveDirection().normalized / 5);

                m_onMove = true;
            }

            if (Direction.direction != Vector3.zero)
                p_rigidbody.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Direction.direction, transform.up), 0.8f);
        }

        /// <summary>
        /// Defines the input direction after changing the camera position.
        /// </summary>
        public void SetInputMoveDelta(ControllType p_controllerType)
        {
            switch (p_controllerType)
            {
                case ControllType.PointAndClick:
                    m_moveDelta = new Vector2(HorizontalMovement, VerticalMovement);
                    break;
                case ControllType.KeyBoard:
                    if (IsometricCamera.m_instance.CamPosition == IsometricCamera.CameraPosition.SOUTH)
                        m_moveDelta = new Vector2(HorizontalMovement, VerticalMovement);

                    if (IsometricCamera.m_instance.CamPosition == IsometricCamera.CameraPosition.WEST)
                        m_moveDelta = new Vector2(VerticalMovement, -HorizontalMovement);

                    if (IsometricCamera.m_instance.CamPosition == IsometricCamera.CameraPosition.NORTH)
                        m_moveDelta = new Vector2(-HorizontalMovement, -VerticalMovement);

                    if (IsometricCamera.m_instance.CamPosition == IsometricCamera.CameraPosition.EAST)
                        m_moveDelta = new Vector2(-VerticalMovement, HorizontalMovement);
                    break;
                case ControllType.Joystick:
                    break;
            }
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

        public bool OnSlope()
        {
            if (Physics.Raycast(transform.position, Vector3.down, out m_slopeHit, transform.localScale.y + 0.25f))
            {
                m_slopeAngle = Vector3.Angle(Vector3.up, m_slopeHit.normal);
                return m_slopeAngle < m_maxSlopeAngle && m_slopeAngle != 0;
            }
            return false;
        }

        public Vector3 GetSlopeMoveDirection()
        {
            return Vector3.ProjectOnPlane(new Vector3(Direction.direction.x, (int)(Direction.direction.y * (int)m_slopeAngle), Direction.direction.z), m_slopeHit.normal).normalized;
        }
    }
}
