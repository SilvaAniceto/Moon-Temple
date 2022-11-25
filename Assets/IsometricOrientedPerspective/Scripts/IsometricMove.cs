using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace IOP
{
    public class IsometricMove : IsometricOrientedPerspective
    {
        public static IsometricMove m_moveInstance;

        private bool m_leftClick, m_onMove;
        private float m_moveDistance = 9f;
        private float m_movementDelta = 4f;
        private float m_distanceTravelled, m_horizontalMovement, m_verticalMovement, m_maxSlopeAngle, m_slopeAngle;
        private Vector2 m_moveDelta;
        private Vector3 m_startPosition, m_direction, m_auxDirection;
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

        public void Setup(ControllType p_value)
        {
            if (m_moveInstance == null)
            {
                m_moveInstance = this;
                IsometricSetup();
            }

            m_onMove = false;
        }

        /// <summary>
        /// Resolve the movement in Isometric Oriented Perspective with no Physics.
        /// </summary>
        public void Move(Vector3 p_vector3, bool p_click, Rigidbody p_rigidbody)
        {
            if (IsometricCamera.m_instance.MovingCamera) return; // Prevents that the movement happens when the Camera is moving.

            if (!m_onMove)
            {
                if (p_click)
                {
                    //m_direction = new Vector3(p_vector3.x, transform.position.y, p_vector3.z);
                    //m_auxDirection = p_vector3;
                    if (p_vector3.y < transform.position.y)
                    {
                        Vector3 ponto = new Vector3(p_vector3.x, transform.position.y, p_vector3.z);
                        float catetoAdjacente = Vector3.Distance(transform.position, ponto);
                        float tangente = Mathf.Tan(m_slopeAngle);
                        float catetoOposto = catetoAdjacente * tangente;

                        m_auxDirection = new Vector3(ponto.x, ponto.y - catetoOposto, ponto.z);
                    }
                    else if(p_vector3.y > transform.position.y)
                    {
                        Vector3 ponto = new Vector3(p_vector3.x, transform.position.y, p_vector3.z);
                        float catetoAdjacente = Vector3.Distance(transform.position, ponto);
                        float tangente = Mathf.Tan(m_slopeAngle);
                        float catetoOposto = catetoAdjacente * tangente;

                        m_auxDirection = new Vector3(ponto.x, ponto.y + catetoOposto + 0.1f, ponto.z);
                    }
                    else
                        m_auxDirection = new Vector3(p_vector3.x, transform.position.y, p_vector3.z);

                    m_direction = m_auxDirection;
                    m_onMove = true;
                }

                m_startPosition = transform.position;
            }

            if (m_onMove)
                GetMoveDistance(m_startPosition);

            //if (m_direction.normalized != Vector3.zero)
            if (m_auxDirection.normalized != Vector3.zero)
            {
                #region 
                //if (!OnSlope())
                //{
                //    m_direction = new Vector3(m_auxDirection.x, transform.position.y, m_auxDirection.z);

                //    if (m_moveType == MoveType.COMBAT)
                //    {
                //        if ((int)m_distanceTravelled < (int)m_moveDistance)
                //            transform.position = Vector3.MoveTowards(transform.position, m_direction, m_movementDelta * Time.fixedDeltaTime);
                //        else
                //            m_onMove = false;
                //    }
                //    else
                //    {
                //        transform.position = Vector3.MoveTowards(transform.position, m_direction, m_movementDelta * Time.fixedDeltaTime);
                //        float distance = Vector3.Distance(m_direction, transform.position);
                //        m_onMove = distance > 0.2f ? true : false;
                //    }
                //}
                //else
                //{
                //    m_direction = new Vector3(m_auxDirection.x, m_auxDirection.y + GetComponent<CapsuleCollider>().bounds.extents.y + 0.1f, m_auxDirection.z);
                //
                //    if (m_moveType == MoveType.COMBAT)
                //    {
                //        if ((int)m_distanceTravelled < (int)m_moveDistance)
                //            transform.position = Vector3.MoveTowards(transform.position, m_direction, m_movementDelta * Time.fixedDeltaTime);
                //        else
                //            m_onMove = false;
                //    }
                //    else
                //    {
                //        transform.position = Vector3.MoveTowards(transform.position, m_direction, m_movementDelta * Time.fixedDeltaTime);
                //        float distance = Vector3.Distance(m_direction, transform.position);
                //        m_onMove = distance > 0.2f ? true : false;
                //    }
                //}
                #endregion
                if (!OnSlope())
                {
                   // m_direction = new Vector3(m_auxDirection.x, transform.position.y, m_auxDirection.z);

                    if (m_moveType == MoveType.COMBAT)
                    {
                        if ((int)m_distanceTravelled < (int)m_moveDistance)
                            p_rigidbody.MovePosition(Vector3.MoveTowards(p_rigidbody.position, m_direction, m_movementDelta * Time.fixedDeltaTime));
                        else
                            m_onMove = false;
                    }
                    else
                    {
                        p_rigidbody.MovePosition(Vector3.MoveTowards(p_rigidbody.position, m_direction, m_movementDelta * Time.fixedDeltaTime));
                        float distance = Vector3.Distance(m_direction, transform.position);
                        m_onMove = distance > 0.2f ? true : false;
                    }
                }
                else
                {

                    //if (m_auxDirection.y < transform.position.y - GetComponent<CapsuleCollider>().bounds.extents.y)
                    //    m_direction = m_auxDirection;
                    //else
                    //    m_direction = new Vector3(m_auxDirection.x, m_auxDirection.y + GetComponent<CapsuleCollider>().bounds.extents.y + 0.1f, m_auxDirection.z);

                    if (m_moveType == MoveType.COMBAT)
                    {
                        if ((int)m_distanceTravelled < (int)m_moveDistance)
                            p_rigidbody.MovePosition(Vector3.MoveTowards(p_rigidbody.position, m_direction, m_movementDelta * Time.fixedDeltaTime));
                        else
                            m_onMove = false;
                    }
                    else
                    {
                        p_rigidbody.MovePosition(Vector3.MoveTowards(p_rigidbody.position, m_direction, m_movementDelta * Time.fixedDeltaTime));
                        float distance = Vector3.Distance(m_direction, transform.position);
                        m_onMove = distance > 0.2f ? true : false;
                    }
                }
            }
        }

        /// <summary>
        /// Resolve the movement in Isometric Oriented Perspective with Physics.
        /// </summary>
        public void Move(Vector2 p_vector, Rigidbody p_rigidbody)
        {
            if (IsometricCamera.m_instance.MovingCamera) return; // Prevents that the movement happens when the Camera is moving.

            switch (m_moveType)
            {
                case MoveType.FREE:
                    if (p_vector != Vector2.zero)
                    {
                        if (!m_onMove)
                            m_onMove = true;
                    }
                    else if (p_vector == Vector2.zero)
                    {
                        m_onMove = false;
                    }
                    break;
                case MoveType.COMBAT:
                    if (p_vector != Vector2.zero)
                    {
                        if ((int)m_distanceTravelled < (int)m_moveDistance)
                        {
                            if (!m_onMove)
                            {
                                m_onMove = true;
                                m_startPosition = transform.position;
                            }
                        }
                    }
                    else if (p_vector == Vector2.zero)
                    {
                        m_onMove = false;
                        m_distanceTravelled = 0;
                    }
                    break;
            }

            m_direction = new Vector3(p_vector.x, 0, p_vector.y);
            Direction.righMovement = IsometricRight * m_movementDelta * Time.fixedDeltaTime * m_direction.x;
            Direction.slopeMovement = Vector3.zero;
            Direction.upMovement = IsometricForward * m_movementDelta * Time.fixedDeltaTime * m_direction.z;

            m_direction = Direction.righMovement + Direction.slopeMovement + Direction.upMovement;

            if (m_onMove) GetMoveDistance(m_startPosition);

            if (!OnSlope())
            {
                if (m_moveType == MoveType.COMBAT)
                {
                    if ((int)m_distanceTravelled < (int)m_moveDistance)
                        p_rigidbody.MovePosition(p_rigidbody.position + m_direction);
                }
                else
                    p_rigidbody.MovePosition(p_rigidbody.position + m_direction);                
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
            }

            if (m_direction != Vector3.zero)
                p_rigidbody.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(m_direction, transform.up), 0.8f);
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
            return Vector3.ProjectOnPlane(new Vector3(m_direction.x, (int)(m_direction.y * (int)m_slopeAngle), m_direction.z), m_slopeHit.normal).normalized;
        }
    }
}
