using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IsometricOrientedPerspective
{
    public class IsometricMove : IsometricOrientedPerspective
    {
        public static IsometricMove m_moveInstance;

        [SerializeField] private bool m_isPhysicsMovement, m_onMove;
        [SerializeField] private float m_moveDistance = 9f;
        [Range(1f, 10f)][SerializeField] private float m_movementDelta = 4f;
        private Rigidbody m_Rigidbody;
        private Vector2 m_moveDelta;
        private MoveDirection m_moveDirection;

        [SerializeField] LineRenderer lineRenderer; 

        #region Properties
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

            private set
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
            private set
            {
                if (m_moveDelta == value)
                    return;

                m_moveDelta = value;
            }
        }
        #endregion

        #region Classes
        private class MoveDirection
        {
           public Vector3 direction = new Vector3();
           public Vector3 righMovement = new Vector3();
           public Vector3 upMovement = new Vector3();
           public Vector3 heading = new Vector3();
        }
        #endregion

        new void Awake()
        {
            base.Awake();

            if (m_moveInstance == null)
                m_moveInstance = this;

            if (m_moveDirection == null)
                m_moveDirection = new MoveDirection();

            m_Rigidbody = GetComponent<Rigidbody>();
        }

        //private void Start()
        //{
        //    DrawCircle(100, 9);
        //}

        new void Update()
        {
            base.Update();

            SetInputMoveDelta();

            if (IsometricRotation.m_rotationInstance.enabled)
            {
                if (Physics.Raycast(RaycastHit, out RaycastHit raycastHit, float.MaxValue, IsometricRotation.m_rotationInstance.LayerMask))
                    if (LeftClick && !m_onMove)
                    {
                        m_moveDirection.direction = new Vector3(IsometricRotation.m_rotationInstance.MouseCursor.position.x, transform.position.y, IsometricRotation.m_rotationInstance.MouseCursor.position.z);
                    
                        m_onMove = true;
                    }
                
                if (m_onMove)
                    Move(m_moveDirection.direction.x, m_moveDirection.direction.z);
            }

            if (IsPhysicsMovement) return;

            IsometricCamera.m_instance.MoveBase();

            if (m_moveDelta != Vector2.zero && !IsometricRotation.m_rotationInstance.enabled)
                Move(m_moveDelta.x, m_moveDelta.y);
            else if (m_moveDelta == Vector2.zero && !IsometricRotation.m_rotationInstance.enabled)
                m_onMove = false;
        }

        private void FixedUpdate()
        {
            if (!IsPhysicsMovement) return;

            IsometricCamera.m_instance.MoveBase();

            if (IsometricRotation.m_rotationInstance.enabled)
                if (m_onMove)
                    Move(m_moveDirection.direction.x, m_moveDirection.direction.z);

            if (m_moveDelta != Vector2.zero && !IsometricRotation.m_rotationInstance.enabled)
                Move(m_moveDelta.x, m_moveDelta.y);
            else if (m_moveDelta == Vector2.zero && !IsometricRotation.m_rotationInstance.enabled)
                m_onMove = false;
        }

        protected virtual void Move(float p_xAxis, float p_zAxis)
        {
            if (!IsometricCamera.m_instance.MovingCamera) 
            { 
                if (IsPhysicsMovement)
                {
                    if (IsometricRotation.m_rotationInstance.enabled)
                    {
                        float distance = Vector3.Distance(m_moveDirection.direction, transform.position);
                        m_onMove = distance > 0.2f ? true : false;

                        m_Rigidbody.MovePosition(Vector3.MoveTowards(transform.position, m_moveDirection.direction, m_movementDelta * Time.deltaTime));
                    }
                    else
                    {
                        m_moveDirection.direction = new Vector3(p_xAxis * m_movementDelta * Time.fixedDeltaTime, 0, p_zAxis * m_movementDelta * Time.fixedDeltaTime);

                        m_moveDirection.direction = Camera.main.transform.TransformDirection(m_moveDirection.direction);
                        m_moveDirection.direction.y = 0;

                        m_Rigidbody.MovePosition(m_Rigidbody.position + m_moveDirection.direction);
                        m_onMove = true;
                    }

                    if (!IsometricRotation.m_rotationInstance.enabled)
                        if (m_moveDirection.direction != Vector3.zero)
                            m_Rigidbody.rotation = Quaternion.Slerp(m_Rigidbody.rotation, Quaternion.LookRotation(m_moveDirection.direction), 0.5f);
                }
                else
                {
                    if (IsometricRotation.m_rotationInstance.enabled)
                    {
                        float distance = Vector3.Distance(m_moveDirection.direction, transform.position);
                        m_onMove = distance > 0.2f ? true : false;

                        transform.position = Vector3.MoveTowards(transform.position, m_moveDirection.direction, m_movementDelta * Time.deltaTime);
                    }
                    else
                    {
                        m_moveDirection.direction = new Vector3(p_xAxis, 0, p_zAxis);
                        m_moveDirection.righMovement = IsometricRight * m_movementDelta * Time.deltaTime * m_moveDirection.direction.x;
                        m_moveDirection.upMovement = IsometricForward * m_movementDelta * Time.deltaTime * m_moveDirection.direction.z;

                        transform.position += m_moveDirection.righMovement;
                        transform.position += m_moveDirection.upMovement;
                        m_onMove = true;
                    }


                    if (!IsometricRotation.m_rotationInstance.enabled)
                    {
                        m_moveDirection.heading = Vector3.Normalize(m_moveDirection.righMovement + m_moveDirection.upMovement);
                        if (m_moveDirection.heading != Vector3.zero)
                            transform.forward = Vector3.Lerp(transform.forward, m_moveDirection.heading, 0.40f);
                    }
                }
            }
        }

        public void SetInputMoveDelta()
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

        private void GetMoveDistance(float p_distanceDelta)
        {
            
        }

        //private void DrawCircle(int steps, float radius)
        //{
        //    lineRenderer.positionCount = steps;

        //    for (int currentStep = 0; currentStep < steps; currentStep++)
        //    {
        //        float circunferenceProgress = (float)currentStep / steps;

        //        float currentRadian = circunferenceProgress * 2 * Mathf.PI;

        //        float xScaled = Mathf.Cos(currentRadian);
        //        float zScaled = Mathf.Sin(currentRadian);

        //        float x = xScaled * radius;
        //        float z = zScaled * radius;

        //        Vector3 currentPosition = new Vector3(transform.position.x + x,transform.position.y, transform.position.z + z);

        //        lineRenderer.SetPosition(currentStep, currentPosition);
        //    }
        //}
    }
}
