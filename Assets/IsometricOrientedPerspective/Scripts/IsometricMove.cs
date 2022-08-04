using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IsometricOrientedPerspective
{
    public class IsometricMove : IsometricPerspective
    {
        public static IsometricMove m_moveInstance;

        private bool m_isPhysicsMovement;
        private float m_moveDistance = 9f;
        private float m_movementDelta = 4f;
        private Rigidbody m_Rigidbody;
        private Vector2 m_moveDelta;
        private Vector3 m_startPosition;
        private float m_distanceTravelled;
        private bool m_onMove;

        #region Properties
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

        public void MovementSetup()
        {
            Setup();
        }

        public void Move(float p_xAxis, float p_zAxis)
        {
            if (!IsometricCamera.m_instance.MovingCamera) 
            { 
                if (IsPhysicsMovement)
                {
                    if (IsometricRotation.m_rotationInstance.enabled)
                    {
                        float distance = Vector3.Distance(Direction.direction, transform.position);
                        m_onMove = distance > 0.2f ? true : false;

                        if ((int)m_distanceTravelled < (int)m_moveDistance)
                            m_Rigidbody.MovePosition(Vector3.MoveTowards(transform.position, Direction.direction, m_movementDelta * Time.deltaTime));
                    }
                    else
                    {
                        Direction.direction = new Vector3(p_xAxis * m_movementDelta * Time.fixedDeltaTime, 0, p_zAxis * m_movementDelta * Time.fixedDeltaTime);

                        Direction.direction = Camera.main.transform.TransformDirection(Direction.direction);
                        Direction.direction.y = 0;

                        if ((int)m_distanceTravelled < (int)m_moveDistance)
                            m_Rigidbody.MovePosition(m_Rigidbody.position + Direction.direction);

                        m_onMove = true;
                    }

                    if (!IsometricRotation.m_rotationInstance.enabled)
                        if (Direction.direction != Vector3.zero)
                            m_Rigidbody.rotation = Quaternion.Slerp(m_Rigidbody.rotation, Quaternion.LookRotation(Direction.direction), 0.5f);
                }
                else
                {
                    if (IsometricRotation.m_rotationInstance.enabled)
                    {
                        float distance = Vector3.Distance(Direction.direction, transform.position);
                        m_onMove = distance > 0.2f ? true : false;

                        if ((int)m_distanceTravelled < (int)m_moveDistance)
                            transform.position = Vector3.MoveTowards(transform.position, Direction.direction, m_movementDelta * Time.deltaTime);
                    }
                    else
                    {
                        Direction.direction = new Vector3(p_xAxis, 0, p_zAxis);
                        Direction.righMovement = IsometricRight * m_movementDelta * Time.deltaTime * Direction.direction.x;
                        Direction.upMovement = IsometricForward * m_movementDelta * Time.deltaTime * Direction.direction.z;

                        if ((int)m_distanceTravelled < (int)m_moveDistance)
                        {
                            transform.position += Direction.righMovement;
                            transform.position += Direction.upMovement;
                        }

                        m_onMove = true;
                    }

                    if (!IsometricRotation.m_rotationInstance.enabled)
                    {
                        Direction.heading = Vector3.Normalize(Direction.righMovement + Direction.upMovement);
                        if (Direction.heading != Vector3.zero)
                            transform.forward = Vector3.Lerp(transform.forward, Direction.heading, 0.40f);
                    }
                }
            }
        }

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

        public void GetMoveDistance(Vector3 p_startPosition)
        {
            m_distanceTravelled = Vector3.Distance(p_startPosition, transform.position);

            if (m_distanceTravelled >= m_moveDistance)
                m_onMove = false;
        }
    }
}
