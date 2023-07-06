using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace IOP
{
    //public class IsometricMove : IsometricOrientedPerspective
    //{
    //    public static IsometricMove m_moveInstance;

    //    private bool m_leftClick, m_onMove;
    //    private float m_limitDistance = 9f;
    //    private float m_movementDelta = 4f;
    //    private float m_distanceToTravel, m_distanceTravelled, m_maxSlopeAngle, m_slopeAngle;
    //    private Vector2 m_moveDelta, m_startPosition;
    //    private Vector3 m_direction, m_auxDirection;
    //    private RaycastHit m_slopeHit;
    //    public enum MoveType { FREE, COMBAT}
    //    private MoveType m_moveType = MoveType.FREE;
    //    private GameObject m_slopeCheck, m_obstacleCheck;
    //    private LayerMask m_layerMask;

    //    public UnityEvent<MoveType> OnMoveTypeChange;

    //    #region Properties
    //    /// <summary>
    //    /// Define the limit distance that the movement can happen.
    //    /// </summary>
    //    public float LimitDistance
    //    {
    //        get
    //        {
    //            return m_limitDistance;
    //        }

    //        set
    //        {
    //            if (m_limitDistance == value)
    //                return;

    //            m_limitDistance = value;
    //        }
    //    }
    //    /// <summary>
    //    /// Define the speed delta of the movement.
    //    /// </summary>
    //    public float MovementDelta
    //    {
    //        get
    //        {
    //            return m_movementDelta;
    //        }

    //        set
    //        {
    //            if (m_movementDelta == value)
    //                return;

    //            m_movementDelta = value;
    //        }
    //    }
    //    /// <summary>
    //    /// Input for left mouse button click.
    //    /// </summary>
    //    public bool LeftClick
    //    {
    //        get
    //        {
    //            return m_leftClick;
    //        }

    //        set
    //        {
    //            if (m_leftClick == value)
    //                return;

    //            m_leftClick = value;
    //        }
    //    }
    //    /// <summary>
    //    /// Define wheter on not the movement is happening.
    //    /// </summary>
    //    public bool OnMove
    //    {
    //        get
    //        {
    //            return m_onMove;
    //        }

    //        set
    //        {
    //            if (m_onMove == value)
    //                return;

    //            m_onMove = value;
    //        }
    //    }
    //    /// <summary>
    //    /// Delta of the axis of movement input.
    //    /// </summary>
    //    public Vector2 MoveDelta
    //    {
    //        get
    //        {
    //            return m_moveDelta;
    //        }
            
    //        set
    //        {
    //            if (m_moveDelta == value)
    //                return;

    //            m_moveDelta = value;
    //        }
    //    }
    //    /// <summary>
    //    /// The start position when the movement begins.
    //    /// </summary>
    //    public Vector2 StartPosition
    //    {
    //        get
    //        {
    //            return m_startPosition;
    //        }

    //        set
    //        {
    //            if (m_startPosition == value)
    //                return;

    //            m_startPosition = value;
    //        }
    //    }                        
    //    public MoveType MoveContext
    //    {
    //        get
    //        {
    //            return m_moveType;
    //        }

    //        set
    //        {
    //            if (m_moveType == value)
    //                return;

    //            m_moveType = value;
    //            OnMoveTypeChange?.Invoke(m_moveType);
    //        } 
    //    }
    //    public RaycastHit SlopeHit
    //    {
    //        get
    //        {
    //            return m_slopeHit;
    //        }
    //    }
    //    public float MaxSlopeAngle
    //    {
    //        get
    //        {
    //            return m_maxSlopeAngle;
    //        }
    //        set
    //        {
    //            if (m_maxSlopeAngle == value)
    //                return;

    //            m_maxSlopeAngle = value;
    //        }
    //    }
    //    public float SlopeAngle
    //    {
    //        get
    //        {
    //            return m_slopeAngle;
    //        }
    //        set
    //        {
    //            if (m_slopeAngle == value)
    //                return;

    //            m_slopeAngle = value;
    //        }
    //    }
    //    public LayerMask LayerMask
    //    {
    //        get
    //        {
    //            return m_layerMask;
    //        }

    //        set
    //        {
    //            if (m_layerMask == value)
    //                return;

    //            m_layerMask = value;
    //        }
    //    }
    //    #endregion

    //    public void Setup(ControllType p_value)
    //    {
    //        if (m_moveInstance == null)
    //        {
    //            m_moveInstance = this;
    //            IsometricSetup();
    //        }

    //        if (m_slopeCheck == null)
    //        {
    //            m_slopeCheck = new GameObject("Slope Check");
    //            m_slopeCheck.transform.SetParent(transform);
    //            m_slopeCheck.transform.localPosition = new Vector3(0, 0, GetComponent<CapsuleCollider>().bounds.extents.z - 0.25f);
    //        }

    //        if (m_obstacleCheck == null)
    //        {
    //            m_obstacleCheck = new GameObject("Obstacle Check");
    //            m_obstacleCheck.transform.SetParent(transform);
    //            m_obstacleCheck.transform.localPosition = new Vector3(0, GetComponent<CapsuleCollider>().bounds.extents.y / 2, GetComponent<CapsuleCollider>().bounds.extents.z / 2);
    //        }

    //        m_onMove = false;
    //    }

    //    /// <summary>
    //    /// Resolve the Point&Click movement in Isometric Oriented Perspective.
    //    /// </summary>
    //    public void Move(Vector3 p_vector3, bool p_click, Rigidbody p_rigidbody)
    //    {
    //        if (IsometricCamera.m_instance.MovingCamera) return; // Prevents that the movement happens when the Camera is moving.

    //        if (!m_onMove)
    //        {
    //            if (p_click)
    //            {                    
    //                m_direction = new Vector3(p_vector3.x, transform.position.y, p_vector3.z);
    //                m_auxDirection = m_direction;
    //                m_onMove = true;

    //                m_direction = m_direction.normalized;

    //                Direction.righMovement = Vector3.zero;
    //                Direction.slopeMovement = Vector3.zero;
    //                Direction.upMovement = transform.forward * m_movementDelta * Time.fixedDeltaTime * m_direction.z;

    //                m_direction = Direction.righMovement + Direction.slopeMovement + Direction.upMovement;
    //            }

    //            m_startPosition = new Vector2(transform.position.x, transform.position.z);

    //            m_distanceToTravel = Mathf.FloorToInt(Vector2.Distance(m_startPosition, new Vector2(m_auxDirection.x, m_auxDirection.z)));
    //        }

    //        if (m_onMove)
    //            GetMoveDistance(m_startPosition);
    //        else
    //            return;
            
    //        if (!OnSlope())
    //        {
    //            if (m_moveType == MoveType.COMBAT)
    //            {
    //                if (Mathf.FloorToInt(Vector3.Distance(m_auxDirection, transform.position)) > m_limitDistance)
    //                {
    //                    m_onMove = false;
    //                    return;
    //                }

    //                if (m_distanceTravelled == m_distanceToTravel)
    //                    m_onMove = false;
    //                else if ((int)m_distanceTravelled < (int)m_limitDistance)
    //                    p_rigidbody.MovePosition(p_rigidbody.position + m_direction);
    //                else
    //                    m_onMove = false;
    //            }
    //            else
    //            {
    //                if (m_onMove)
    //                    p_rigidbody.MovePosition(p_rigidbody.position + m_direction);
    //            }
    //        }
    //        else
    //        {
    //            if (m_moveType == MoveType.COMBAT)
    //            {
    //                if (Mathf.FloorToInt(Vector3.Distance(m_auxDirection, transform.position)) > m_limitDistance)
    //                {
    //                    m_onMove = false;
    //                    return;
    //                }

    //                if (m_distanceTravelled == m_distanceToTravel)
    //                    m_onMove = false;
    //                else if ((int)m_distanceTravelled < (int)m_limitDistance)
    //                    p_rigidbody.MovePosition(transform.position + GetSlopeMoveDirection().normalized / (m_movementDelta + 2));
    //                else
    //                    m_onMove = false;
    //            }
    //            else
    //            {
    //                if (m_onMove)
    //                    p_rigidbody.MovePosition(transform.position + GetSlopeMoveDirection().normalized / (m_movementDelta + 2));
    //            }
    //        }
    //    }

    //    /// <summary>
    //    /// Resolve the Keyboard movement in Isometric Oriented Perspective.
    //    /// </summary>
    //    public void Move(Vector2 p_vector, Rigidbody p_rigidbody)
    //    {
    //        if (IsometricCamera.m_instance.MovingCamera) return; // Prevents that the movement happens when the Camera is moving.

    //        switch (m_moveType)
    //        {
    //            case MoveType.FREE:
    //                if (p_vector != Vector2.zero)
    //                {
    //                    if (!m_onMove)
    //                        m_onMove = true;
    //                }
    //                else if (p_vector == Vector2.zero)
    //                {
    //                    m_onMove = false;
    //                }
    //                break;
    //            case MoveType.COMBAT:
    //                if (p_vector != Vector2.zero)
    //                {
    //                    if ((int)m_distanceTravelled < (int)m_limitDistance)
    //                    {
    //                        if (!m_onMove)
    //                        {
    //                            m_onMove = true;
    //                            m_startPosition = new Vector2(transform.position.x, transform.position.z); ;
    //                        }
    //                    }
    //                }
    //                else if (p_vector == Vector2.zero)
    //                {
    //                    m_onMove = false;
    //                    m_distanceTravelled = 0;
    //                }
    //                break;
    //        }

    //        m_direction = new Vector3(p_vector.x, 0, p_vector.y);
    //        Direction.righMovement = IsometricRight * m_movementDelta * Time.fixedDeltaTime * m_direction.x;
    //        Direction.slopeMovement = Vector3.zero;
    //        Direction.upMovement = IsometricForward * m_movementDelta * Time.fixedDeltaTime * m_direction.z;

    //        m_direction = Direction.righMovement + Direction.slopeMovement + Direction.upMovement;

    //        if (m_direction != Vector3.zero)
    //            p_rigidbody.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(m_direction, transform.up), 0.8f);

    //        FindObstacle();

    //        if (m_onMove) GetMoveDistance(m_startPosition);
    //        else return;

    //        //if (m_direction.normalized == Vector3.zero) return;

    //        if (!OnSlope())
    //        {
    //            if (m_moveType == MoveType.COMBAT)
    //            {
    //                if ((int)m_distanceTravelled < (int)m_limitDistance)
    //                    p_rigidbody.MovePosition(p_rigidbody.position + m_direction);
    //            }
    //            else
    //                p_rigidbody.MovePosition(p_rigidbody.position + m_direction);                
    //        }
    //        else
    //        {
    //            if (m_moveType == MoveType.COMBAT)
    //            {
    //                if ((int)m_distanceTravelled < (int)m_limitDistance)
    //                    p_rigidbody.MovePosition(transform.position + GetSlopeMoveDirection() / (m_movementDelta + 2));
    //            }
    //            else
    //                p_rigidbody.MovePosition(transform.position + GetSlopeMoveDirection() / (m_movementDelta + 2));
    //        }
    //    }

    //    /// <summary>
    //    /// Defines the input direction after changing the camera position.
    //    /// </summary>
    //    public void SetInputMoveDelta(ControllType p_controllerType, float p_horizontalAxis, float p_vertivalAxis)
    //    {
    //        switch (p_controllerType)
    //        {
    //            case ControllType.PointAndClick:
    //                m_moveDelta = new Vector2(p_horizontalAxis, p_vertivalAxis);
    //                break;
    //            case ControllType.KeyBoard:
    //                if (IsometricCamera.m_instance.CamPosition == IsometricCamera.CameraPosition.SOUTH)
    //                    m_moveDelta = new Vector2(p_horizontalAxis, p_vertivalAxis);

    //                if (IsometricCamera.m_instance.CamPosition == IsometricCamera.CameraPosition.WEST)
    //                    m_moveDelta = new Vector2(p_vertivalAxis, -p_horizontalAxis);

    //                if (IsometricCamera.m_instance.CamPosition == IsometricCamera.CameraPosition.NORTH)
    //                    m_moveDelta = new Vector2(-p_horizontalAxis, -p_vertivalAxis);

    //                if (IsometricCamera.m_instance.CamPosition == IsometricCamera.CameraPosition.EAST)
    //                    m_moveDelta = new Vector2(-p_vertivalAxis, p_horizontalAxis);
    //                break;
    //            case ControllType.Joystick:
    //                break;
    //        }
    //    }

    //    /// <summary>
    //    /// Calculate the distance travelled when the movement begins.
    //    /// </summary>
    //    public void GetMoveDistance(Vector2 p_startPosition)
    //    {
    //        m_distanceTravelled = Mathf.FloorToInt(Vector2.Distance(p_startPosition, new Vector2(transform.position.x, transform.position.z)));

    //        switch (m_moveType)
    //        {
    //            case MoveType.FREE:
    //                if (m_distanceTravelled == m_distanceToTravel)
    //                    m_onMove = false;
    //                break;
    //            case MoveType.COMBAT:
    //                if (m_distanceTravelled >= m_limitDistance)
    //                    m_onMove = false;
    //                break;
    //            default:
    //                break;
    //        }

            
    //    }

    //    public bool OnSlope(int value = 0)
    //    {
    //        if (Physics.Raycast(m_slopeCheck.transform.position, Vector3.down, out m_slopeHit, GetComponent<CapsuleCollider>().bounds.extents.y + 1f))
    //        {
    //            m_slopeAngle = Vector3.Angle(Vector3.up, m_slopeHit.normal);
    //            return m_slopeAngle < m_maxSlopeAngle && m_slopeAngle != 0;
    //        }
    //        return false;
    //    }

    //    public static bool OnSlope(Transform transform, CapsuleCollider capsuleCollider, LayerMask layerMask, float maxSlopeAngle)
    //    {
    //        if (Physics.SphereCast(transform.position - new Vector3(0, 0.5f, 0), capsuleCollider.radius / 0.85f, Vector3.down, out RaycastHit hitInfo, Mathf.Infinity, layerMask, QueryTriggerInteraction.Collide))
    //        {
    //            float slopeAngle = Vector3.Angle(Vector3.up, hitInfo.normal);
    //            return slopeAngle < maxSlopeAngle && slopeAngle != 0;
    //        }
    //        return false;
    //    }

    //    public static float GetSlopeAngle(Transform transform, CapsuleCollider capsuleCollider, LayerMask layerMask, float maxSlopeAngle)
    //    {
    //        if (Physics.SphereCast(transform.position - new Vector3(0, 0.5f, 0), capsuleCollider.radius / 0.85f, Vector3.down, out RaycastHit hitInfo, Mathf.Infinity, layerMask, QueryTriggerInteraction.Collide))
    //        {
    //            float slopeAngle = Vector3.Angle(Vector3.up, hitInfo.normal);
    //            return slopeAngle;
    //        }
    //        return 0;
    //    }

    //    void FindObstacle()
    //    {
    //        if (Physics.CheckSphere(m_obstacleCheck.transform.position, GetComponent<CapsuleCollider>().radius - 0.15f, m_layerMask))
    //            m_onMove = false;
    //    }

    //    public Vector3 GetSlopeMoveDirection()
    //    {
    //        return Vector3.ProjectOnPlane(new Vector3(m_direction.x, (int)(m_direction.y * (int)m_slopeAngle), m_direction.z), m_slopeHit.normal).normalized;
    //    }

    //    private void OnDrawGizmos()
    //    {
    //        //Gizmos.color = Color.red;
    //        //Gizmos.DrawLine(m_slopeCheck.transform.position, new Vector3(m_slopeCheck.transform.position.x, transform.position.y - (GetComponent<CapsuleCollider>().bounds.extents.y + 1f), m_slopeCheck.transform.position.z));
    //        //Gizmos.DrawWireSphere(m_obstacleCheck.transform.position, GetComponent<CapsuleCollider>().radius -0.15f);
    //    }
    //}
}
