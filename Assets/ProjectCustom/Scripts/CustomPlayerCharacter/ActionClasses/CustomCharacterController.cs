using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace CustomGameController
{
    public struct CustomPlayerInputHandler
    {
        public Vector3 MoveDirectionInput;
        public bool VerticalActionInput;
        public bool SpeedUpInput;

        public Vector2 CameraAxis;
    }

    public enum VerticalState
    {
        Idle,
        Jumping,
        Falling,
        InFlight
    }

    [RequireComponent(typeof(CharacterController))]

    public class CustomCharacterController : MonoBehaviour
    {
        #region CHARACTER GENERAL PROPERTIES
        public CharacterController CharacterController { get => GetComponent<CharacterController>(); }
        public VerticalState VerticalState
        {
            get
            {
                if (!OnGround && !InFlight)
                {
                    if (CurrentPosition.y - PreviousPosition.y < -0.02f) return VerticalState.Falling;
                    else if (CurrentPosition.y - PreviousPosition.y > 0.02f) return VerticalState.Jumping;
                    else if (CurrentPosition.y - PreviousPosition.y == 0.0f) return VerticalState.Idle;
                }
                if (!OnGround && InFlight)
                {
                    return VerticalState.InFlight;
                }

                return VerticalState.Idle;
            }
        }
        private Vector3 CharacterAnchorPosition { get => new Vector3(CharacterController.bounds.center.x, CharacterController.bounds.center.y - CharacterController.bounds.extents.y, CharacterController.bounds.center.z); }
        private Vector3 CharacterCenterPosition {  get =>  CharacterController.bounds.center; }
        public Transform ArchorReference { get; private set; }
        private Animator CharacterAnimator { get => GetComponent<Animator>(); }
        private Vector3 PreviousPosition { get; set; }
        private Vector3 CurrentPosition { get => transform.position; }
        #endregion

        #region CHARACTER SETUPS
        public void SetupCharacter(LayerMask groundLayer)
        {
            if (CharacterController == null)
            { 
                CharacterController.slopeLimit = MaxSlopeAngle;
            }

            GroundLayer = groundLayer;

            GameObject t = new GameObject("~GroundCheckOrigin");
            GroundCheckOrigin = t.transform;
            GroundCheckOrigin.transform.SetParent(transform);

            t = new GameObject("~WallCheckOrigin");
            WallCheckOrigin = t.transform;
            WallCheckOrigin.transform.SetParent(transform);

            t = new GameObject("~ArchorReference");
            ArchorReference = t.transform;
            ArchorReference.transform.SetParent(transform);

            SetPlayerPhysicsSimulation(ApplyGravity);

            CustomPlayer.OnSpeedUpAction.AddListener(UpdateCharacterSpeed);
            CustomPlayer.CharacterCheckSlopeAndGround.AddListener(() => 
            {
                SetCheckersLocation();
                CheckGroundLevel();
            });
            CustomPlayer.UpdateCharacterAnimation.AddListener(Animate);

            StartCoroutine("CheckingUngroundedStates");

            SetSlopeCheckSystem(SlopeCheckCount, CharacterController.radius);
        }
        private void SetPlayerPhysicsSimulation(UnityAction actionSimulated)
        {
            CustomPlayer.PlayerPhysicsSimulation.RemoveAllListeners();
            if (actionSimulated != null) CustomPlayer.PlayerPhysicsSimulation.AddListener(actionSimulated);
        }
        private void SetCheckersLocation()
        {
            GroundCheckOrigin.position = CharacterAnchorPosition;

            ArchorReference.position = CharacterAnchorPosition;
            ArchorReference.localRotation = transform.localRotation;

            WallCheckOrigin.position = CharacterCenterPosition;
        }
        #endregion

        #region CHARACTER GRAVITY
        private Vector3 Gravity { get => Vector3.up * 9.81f; }
        private float GravityMultiplierFactor 
        { 
            get
            {
                if (!OnGround && !InFlight)
                {
                    if (VerticalState == VerticalState.Jumping) return 0.9f;
                    if (VerticalState == VerticalState.Falling) return 2.2f;
                    return 1.0f;
                }
                if (!OnGround && InFlight)
                {
                    if (Gliding) return 0.05f;

                    return 1.2f;
                }

                return 0.0f;
            }
        }
        private float Drag { get => 1.4f; }
        private Vector3 GravityVelocity { get; set; }

        private float ApplyDrag(float velocity, float drag)
        {
            return velocity * (1 / (1 + drag * Time.deltaTime));
        }
        private void ApplyGravity()
        {
            GravityVelocity -= Gravity * GravityMultiplierFactor * Time.deltaTime;

            if (SlopeAngle() <= MaxSlopeAngle)
                CharacterController.Move(GravityVelocity * Time.deltaTime);
            else
                CharacterController.Move(GetSlopeMoveDirection(GravityVelocity + SlopeHit().transform.forward) * Time.deltaTime * CurrentSpeed);
        }
        #endregion

        #region CHARACTER GROUND DETECTION
        private bool onGround;
        private bool OnGround
        {
            get => onGround;
            set
            {
                if (value)
                {
                    InFlight = false;
                    Gliding = false;
                    FlightHeight = 0.0f;
                    FlightVelocity = Vector3.zero;
                }

                if (onGround == value) return;

                onGround = value;

                if (!onGround && SlopeHit().collider == null)
                {
                    StopCoroutine("CheckingUngroundedStates");
                    StartCoroutine("CheckingUngroundedStates");
                }
            }
        }
        private Transform GroundCheckOrigin { get; set; }
        private LayerMask GroundLayer { get; set; }

        private void CheckGroundLevel()
        {
            bool ground;

            ground = Physics.CheckSphere(GroundCheckOrigin.position, CharacterController.radius, GroundLayer, QueryTriggerInteraction.Collide);

            OnGround = ground;

            if (ground && GravityVelocity.y < 0.0f)
            {
                GravityVelocity = Vector3.zero;
            }
        }
        IEnumerator CheckingUngroundedStates()
        {
            PreviousPosition = transform.position;

            AllowJump = false;

            CustomPlayer.OnVerticalAction.AddListener(CheckEnterFlightState);            

            yield return new WaitUntil(() => OnGround);

            FlightGravityDirection = 0.0f;

            CurrentyVelocity = CurrentyVelocity / Drag * 0.85f;

            CustomPlayer.OnVerticalAction.RemoveAllListeners();
            CustomPlayer.OnVerticalAction.AddListener(Jump);
            CustomPlayer.OnCharacterDirection.RemoveAllListeners();
            CustomPlayer.OnCharacterDirection.AddListener(UpdateThirdPersonMovePosition);

            yield return new WaitForSeconds(Time.fixedDeltaTime);
            AllowJump = true;
        }
        #endregion

        #region CHARACTER SLOPE DETECTION
        private float MaxSlopeAngle { get => 45.0f; }
        private int SlopeCheckCount { get => 6; }
        private List<Transform> SlopeCheckList { get; set; }

        private void SetSlopeCheckSystem(int checkCount, float radius)
        {
            SlopeCheckList = new List<Transform>();

            for (int i = 0; i < checkCount; i++)
            {
                GameObject obj = new GameObject("~SlopeOrigin");
                Transform t = obj.transform;
                t.SetParent(transform);
                t.localPosition = Vector3.zero;

                SlopeCheckList.Add(t);

                float circunferenceProgress = (float)i / checkCount;

                float currentRadian = circunferenceProgress * 2 * Mathf.PI;

                float xScaled = Mathf.Cos(currentRadian);
                float zScaled = Mathf.Sin(currentRadian);

                float x = xScaled * radius;
                float z = zScaled * radius;

                Vector3 currentPosition = new Vector3(t.localPosition.x + z, (CharacterController.height / 2) - 0.2f, t.localPosition.z + x);

                t.localPosition = currentPosition;
            }
        }
        private bool OnSlope()
        {
            if (!OnGround) return false;

            foreach (Transform t in SlopeCheckList)
            {
                if (Physics.Raycast(t.position, Vector3.down, out RaycastHit hitInfo, CharacterController.height / 2 + 0.5f, GroundLayer, QueryTriggerInteraction.Collide))
                {
                    return hitInfo.normal != Vector3.up;
                }
            }
            return false;
        }
        private float SlopeAngle()
        {
            if (!OnGround) return 0;

            foreach (Transform t in SlopeCheckList)
            {
                if (Physics.Raycast(t.position, Vector3.down, out RaycastHit hitInfo, CharacterController.height / 2 + 0.5f, GroundLayer, QueryTriggerInteraction.Collide))
                {
                    float slopeAngle = Mathf.RoundToInt(Vector3.Angle(Vector3.up, hitInfo.normal));
                    return slopeAngle;
                }
            }
            return 0;
        }
        private RaycastHit SlopeHit()
        {
            if (!OnGround) return new RaycastHit();

            foreach (Transform t in SlopeCheckList)
            {
                Physics.Raycast(t.position, Vector3.down, out RaycastHit hitInfo, CharacterController.height / 2, GroundLayer, QueryTriggerInteraction.Collide);

                if (hitInfo.collider != null) return hitInfo;
            }
            return new RaycastHit();
        }
        private Vector3 GetSlopeMoveDirection(Vector3 direction)
        {
            return Vector3.ProjectOnPlane(new Vector3(direction.x, (int)(direction.y * (int)SlopeAngle()), direction.z), SlopeHit().normal).normalized;
        }
        #endregion

        #region CHARACTER WALL DETECTION
        private Transform WallCheckOrigin { get; set; }

        private bool CheckWallHit()
        {
            return Physics.Raycast(WallCheckOrigin.position, transform.forward, CharacterController.radius + 0.1f, GroundLayer, QueryTriggerInteraction.Collide);
        }
        #endregion

        #region CHARACTER MOVEMENT
        public float BaseSpeed
        {
            get => 0.415f * CharacterController.height * 3.2f;
        }
        public float CurrentSpeed { get; set; }
        public Vector3 CurrentyVelocity { get; set; }
        private float BaseAcceleration { get => 2.0f; }
        private float UngroundAcceleration { get => BaseAcceleration * 0.8f; }
        private float InFlightAcceleration { get => BaseAcceleration * 2.5f; }
        private float CurrentAcceleration
        {
            get
            {
                if (OnGround && !InFlight) return BaseAcceleration;
                else if (!OnGround && !InFlight) return UngroundAcceleration;
                else return InFlightAcceleration;
            }
        }

        private void UpdateCharacterSpeed(bool speedAction, float speed)
        {
            if (speedAction)
            {
                if (OnGround || !InFlight)
                {
                    CurrentSpeed = speed * 3.6f;
                    return;
                }
                if (Gliding)
                {
                    CurrentSpeed = speed * 5.2f;
                    return;
                }
            }
            else
            {
                if (OnGround || !InFlight)
                {
                    CurrentSpeed = speed;
                    return;
                }
                if (InFlight)
                {
                    CurrentSpeed = speed * 3.6f;
                    return;
                }
            }
        }
        private void UpdateThirdPersonMovePosition(Vector3 inputDirection, float movementSpeed)
        {
            if (VerticalState == VerticalState.InFlight) return;

            inputDirection = inputDirection.normalized;

            Vector3 right = inputDirection.x * CustomPerspective.CustomRight;
            Vector3 forward = inputDirection.z * CustomPerspective.CustomForward;

            Vector3 move = right + forward + Vector3.zero;

            if (move != Vector3.zero) transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(move), Time.deltaTime * 4.5f);

            if (CheckWallHit())
            {
                CharacterController.Move(Vector3.zero);
                return;
            }

            if (OnGround)
            {
                CurrentyVelocity = Vector3.MoveTowards(CurrentyVelocity, move, CurrentAcceleration * Time.deltaTime);

                if (OnSlope())
                {
                    if (SlopeAngle() <= MaxSlopeAngle)
                    {
                        CharacterController.Move(GetSlopeMoveDirection(CurrentyVelocity) * Time.deltaTime * movementSpeed);
                    }
                    else
                    {
                        CharacterController.Move(Vector3.zero * Time.deltaTime * movementSpeed * 0.5f);
                    }
                }
                else
                    CharacterController.Move(CurrentyVelocity * Time.deltaTime * movementSpeed);
            }
            else
            {
                CurrentyVelocity = Vector3.MoveTowards(CurrentyVelocity, move, CurrentAcceleration * Time.deltaTime);

                float x = ApplyDrag(CurrentyVelocity.x, Drag);
                float z = ApplyDrag(CurrentyVelocity.z, Drag);

                CurrentyVelocity = new Vector3(x, 0, z);

                CharacterController.Move(CurrentyVelocity * Time.deltaTime * movementSpeed);
            }
        }
        //public void UpdateFirstPersonMovePosition(Vector3 inputDirection, float movementSpeed)
        //{
        //    inputDirection = inputDirection.normalized;

        //    Vector3 move = new Vector3();
        //    Vector3 right = inputDirection.x * Right;
        //    Vector3 forward = inputDirection.z * Forward;

        //    move = right + forward + Vector3.zero;

        //    if (OnGround)
        //    {
        //        CurrentyVelocity = Vector3.MoveTowards(CurrentyVelocity, move, CurrentAcceleration * Time.deltaTime);

        //        if (OnSlope())
        //        {
        //            if (SlopeAngle() <= MaxSlopeAngle)
        //            {
        //                CharacterController.Move(GetSlopeMoveDirection(CurrentyVelocity) * Time.deltaTime * movementSpeed);
        //            }
        //            else
        //            {
        //                CharacterController.Move(Vector3.zero * Time.deltaTime * movementSpeed * 0.5f);
        //            }
        //        }
        //        else
        //            CharacterController.Move(CurrentyVelocity * Time.deltaTime * movementSpeed);
        //    }
        //    else
        //    {
        //        CurrentyVelocity = Vector3.MoveTowards(CurrentyVelocity, move, CurrentAcceleration * Time.deltaTime);

        //        float x = ApplyDrag(CurrentyVelocity.x, Drag);
        //        float z = ApplyDrag(CurrentyVelocity.z, Drag);

        //        CurrentyVelocity = new Vector3(x, 0, z);

        //        CharacterController.Move(CurrentyVelocity * Time.deltaTime * movementSpeed);
        //    }

        //    Quaternion Rot = CustomCamera.Instance.CameraTarget.transform.rotation;

        //    Rot.x = 0.0f;
        //    Rot.z = 0.0f;

        //    transform.rotation = Rot;
        //}
        #endregion

        #region CHARACTER FLIGHT
        private float MaximunFlightHeight { get => JumpSpeed * 1.4f; }
        private bool InFlight { get; set; }
        private bool Gliding { get; set; }
        private float FlightGravityDirection { get; set; }

        private float flightHeight;
        private float FlightHeight
        {
            get
            {
                return flightHeight;
            }
            set
            {
                if (OnGround) flightHeight = value;

                if (flightHeight < 0.8f && value < 1.0f)
                {
                    flightHeight = value;
                    Gliding = false;
                }
                else if (flightHeight >= 0.8f && !Gliding)
                {
                    Gliding = true;
                }
                else if (value <= 0.05f && Gliding)
                {
                    flightHeight = value;
                    Gliding = false;
                }
            }
        }
        private Vector3 FlightVelocity { get; set; }

        private void CheckEnterFlightState(bool value)
        {
            if (value)
            {
                InFlight = true;
                CustomPlayer.OnCharacterDirection.RemoveAllListeners();
                CustomPlayer.OnCharacterDirection.AddListener(UpdateFlightPosition);
                CustomPlayer.OnVerticalAction.RemoveAllListeners();
                CustomPlayer.OnVerticalAction.AddListener(UpdateFlightHeightPosition);
            }
        }
        private void UpdateFlightPosition(Vector3 inputDirection, float movementSpeed)
        {
            inputDirection = inputDirection.normalized;

            Vector3 right = inputDirection.x * CustomPerspective.CustomRight;
            Vector3 forward = inputDirection.z * CustomPerspective.CustomForward;

            Vector3 move = right + forward + Vector3.zero;

            if (move != Vector3.zero) transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(move), Time.deltaTime * 4.5f);

            CurrentyVelocity = Vector3.MoveTowards(CurrentyVelocity, move, CurrentAcceleration * Time.deltaTime);

            CharacterController.Move(CurrentyVelocity * Time.deltaTime * movementSpeed);
        }
        private void UpdateFlightHeightPosition(bool verticalAction)
        {
            if (!Gliding)
            {
                if (verticalAction)
                {
                    InFlight = true;
                    float flightHeight = Mathf.Clamp(Mathf.Round(Mathf.InverseLerp(PreviousPosition.y, PreviousPosition.y + MaximunFlightHeight, CurrentPosition.y) * 100.0f) / 100, 0.0f, 1.0f);
                    FlightHeight = flightHeight;
                    FlightGravityDirection = Mathf.Lerp(Gravity.y, 0.0f, flightHeight);
                }
                else
                {
                    InFlight = false;
                    FlightGravityDirection = 0.0f;
                }
            }
            else
            {
                float flightHeight = Mathf.Clamp(Mathf.Round(Mathf.InverseLerp(PreviousPosition.y + MaximunFlightHeight, CurrentPosition.y, CurrentPosition.y - PreviousPosition.y) * 100.0f) / 100, 0.0f, 1.0f);
                FlightHeight = flightHeight;

                if (verticalAction)
                {
                    InFlight = true;
                }
                else
                {
                    InFlight = false;
                }
            }

            Vector3 up = FlightGravityDirection * Vector3.up;

            Vector3 move = up + new Vector3(FlightVelocity.x, 0.0f, FlightVelocity.z);

            FlightVelocity = Vector3.MoveTowards(FlightVelocity, move, Time.deltaTime * Gravity.y);

            CharacterController.Move(FlightVelocity * Gravity.y * Time.deltaTime);
        }
        #endregion

        #region CHARACTER JUMP
        private float JumpHeight { get => CharacterController.height * 0.4f; }
        private float JumpSpeed { get => Mathf.Sqrt(2.0f * JumpHeight * Gravity.y); }
        private bool AllowJump { get; set; }

        private void Jump(bool jumpInput)
        {
            if (jumpInput)
            {
                if (AllowJump)
                {
                    GravityVelocity = new Vector3(0.0f, JumpSpeed, 0.0f);

                    AllowJump = false;
                }
            }

            CharacterController.Move(GravityVelocity * Time.deltaTime);
        }
        #endregion

        #region CHARACTER ANIMATION
        private void Animate()
        {
            if (CheckWallHit()) CharacterAnimator.SetFloat("MoveSpeed", 0.0f, 0.1f, Time.deltaTime);
            else CharacterAnimator.SetFloat("MoveSpeed", CurrentyVelocity.magnitude * (CurrentSpeed / BaseSpeed), 0.1f, Time.deltaTime);

            CharacterAnimator.SetBool("Jumping", VerticalState == VerticalState.Jumping);
            CharacterAnimator.SetBool("OnGround", VerticalState == VerticalState.Idle);
            CharacterAnimator.SetBool("Falling", VerticalState == VerticalState.Falling);
            CharacterAnimator.SetBool("InFlight", VerticalState == VerticalState.InFlight);
        }
        #endregion 
    }
}
