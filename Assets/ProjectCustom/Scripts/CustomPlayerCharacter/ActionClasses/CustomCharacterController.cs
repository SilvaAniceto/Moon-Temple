using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;
using UnityEngine.Events;

namespace CustomGameController
{
    public enum VerticalState
    {
        Idle,
        Jumping,
        Falling,
        Flighting
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
                if (!OnGround && !Flighting)
                {
                    if (CurrentPosition.y - PreviousPosition.y < -0.02f) return VerticalState.Falling;
                    else if (CurrentPosition.y - PreviousPosition.y > 0.02f) return VerticalState.Jumping;
                    else if (CurrentPosition.y - PreviousPosition.y == 0.0f) return VerticalState.Idle;
                }
                if (!OnGround && Flighting)
                {
                    return VerticalState.Flighting;
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
        public float VerticalLocalEuler { get => transform.localEulerAngles.y > 180 ? transform.localEulerAngles.y - 360 : transform.localEulerAngles.y; }
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
                if (VerticalState == VerticalState.Jumping) return 0.9f;
                else if (VerticalState == VerticalState.Falling) return 2.2f;
                else if (VerticalState == VerticalState.Flighting)
                {
                    if (SpeedingUpAction)
                    {
                        float value = Mathf.Lerp(-0.7f, 1.3f, VerticalDirection);
                        return Mathf.Clamp(value, 0.002f, 1.0f);
                    }

                    return 0.02f;
                }
                return 1.0f;
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

            if (VerticalState == VerticalState.Flighting) GravityVelocity = new Vector3(0.0f, Mathf.Clamp(GravityVelocity.y, -Gravity.y * 2.0f, 0.0f), 0.0f);

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
                    CustomPlayer.OnFlightPropulsion.RemoveAllListeners();
                    PropulsionForce = 0.0f;
                    Flighting = false;
                    FlightVelocity = Vector3.zero;
                    transform.localRotation = Quaternion.Euler(0.0f, transform.localEulerAngles.y, 0.0f);
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

            CustomPlayer.OnFlightPropulsion.AddListener(CheckEnterFlightState);

            yield return new WaitUntil(() => OnGround);

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
        public bool SpeedingUpAction { get; set; }
        public Vector3 CurrentyVelocity { get; set; }
        private float BaseAcceleration { get => 2.0f; }
        private float UngroundAcceleration { get => BaseAcceleration * 0.8f; }
        private float FlightingAcceleration { get => BaseAcceleration * 2.5f; }
        private float CurrentAcceleration
        {
            get
            {
                if (OnGround && !Flighting) return BaseAcceleration;
                else if (!OnGround && !Flighting) return UngroundAcceleration;
                else return FlightingAcceleration;
            }
        }

        private void UpdateCharacterSpeed(bool speedAction, float speed)
        {
            SpeedingUpAction = speedAction;

            if (speedAction)
            {
                if (OnGround)
                {
                    CurrentSpeed = speed * 3.6f;
                    return;
                }
                if (Flighting)
                {
                    CurrentSpeed = speed * 10.8f;
                    return;
                }
            }
            else
            {
                if (OnGround || !Flighting)
                {
                    CurrentSpeed = speed;
                    return; 
                }
                if (Flighting)
                {
                    CurrentSpeed = speed * 1.8f;
                    return;
                }
            }
        }
        private void UpdateThirdPersonMovePosition(Vector3 inputDirection, float movementSpeed)
        {
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
        #endregion

        #region CHARACTER FLIGHT
        private Vector3 FlightVelocity { get; set; }
        private bool Flighting { get; set; }
        private float PropulsionForce {  get; set; }
        private float PropulsionTime { get; set; }
        private float VerticalDirection { get; set; }

        private void CheckEnterFlightState(Vector3 value)
        {
            if (value != Vector3.zero)
            {
                PropulsionForce = value.magnitude;
                PropulsionTime = 5.0f;
                CustomPlayer.OnCharacterDirection.RemoveAllListeners();
                CustomPlayer.OnCharacterDirection.AddListener(UpdateFlightPosition);
                CustomPlayer.OnFlightPropulsion.RemoveAllListeners();
                CustomPlayer.OnFlightPropulsion.AddListener(UpdateFlightHeightPosition);
            }
        }
        private void UpdateFlightPosition(Vector3 inputDirection, float movementSpeed)
        { 
            inputDirection = inputDirection.normalized;

            Vector3 right = inputDirection.x * CustomPerspective.CustomRight;
            Vector3 forward = SpeedingUpAction ? FlightVelocity.y * CustomPerspective.CustomForward : inputDirection.z * CustomPerspective.CustomForward;
            Vector3 up = -inputDirection.z * transform.up;

            VerticalDirection = Mathf.Round(Mathf.InverseLerp(1.0f, -1.0f, up.y) * 100.0f) / 100.0f;

            Vector3 move = SpeedingUpAction ? forward + Vector3.zero : right + forward + Vector3.zero;
            Vector3 lookRotation = SpeedingUpAction ? CustomPerspective.CustomForward + (up * Mathf.Lerp(0.1f, 0.85f, VerticalDirection)) + (right * 0.3f) : CustomPerspective.CustomForward + Vector3.zero - (right * 0.5f);

            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookRotation), Time.deltaTime * 4.5f);

            if (SpeedingUpAction)
            {
                float zRot = transform.localEulerAngles.z;
                
                zRot = -inputDirection.x * 90.0f;

                transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(transform.localEulerAngles.x, transform.localEulerAngles.y, zRot), 3.6f * Time.deltaTime);
            }

            CurrentyVelocity = Vector3.MoveTowards(CurrentyVelocity, move, CurrentAcceleration * Time.deltaTime);

            CharacterController.Move(CurrentyVelocity * Time.deltaTime * movementSpeed);            
        }
        private void UpdateFlightHeightPosition(Vector3 flightPropulsion)
        {
            if (flightPropulsion != Vector3.zero)
            {
                Flighting = true;
                PropulsionForce = Mathf.Lerp(PropulsionForce, 0.1f, Time.deltaTime);

                if (PropulsionTime > 0.0f /*&& SpeedingUpAction*/)
                {
                    PropulsionTime = Mathf.Lerp(PropulsionTime, 0.0f, Time.deltaTime);
                    PropulsionTime = Mathf.Round(PropulsionTime * 100.0f) / 100.0f;
                    PropulsionTime = Mathf.Clamp(PropulsionTime, 0.0f, 5.0f);
                }
            }
            else
            {
                if (PropulsionTime < 5.0f)
                {
                    PropulsionTime = Mathf.Lerp(PropulsionTime, 5.0f, Time.deltaTime);
                    PropulsionTime = Mathf.Round(PropulsionTime * 100.0f) / 100.0f;
                    PropulsionTime = Mathf.Clamp(PropulsionTime, 0.0f, 5.0f);
                }

                PropulsionForce = Mathf.Lerp(PropulsionForce, 1.0f, Time.deltaTime);
                Flighting = false;
            }

            Vector3 right  = flightPropulsion.x * CustomPerspective.CustomRight;
            Vector3 up = flightPropulsion.y * transform.up;

            FlightVelocity = right + up + Vector3.zero;

            CharacterController.Move(FlightVelocity * Gravity.y * PropulsionForce * Time.deltaTime);
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
            CharacterAnimator.SetBool("Flighting", VerticalState == VerticalState.Flighting);
            CharacterAnimator.SetBool("SpeedFlight", VerticalState == VerticalState.Flighting && SpeedingUpAction);
        }
        #endregion         

        private void OnGUI()
        {
            //GUI.Box(new Rect(0, 0, 300, 300), "Debug");
            //GUILayout.Label("");
            //GUILayout.Label("Vetical State :" + VerticalState);
            //GUILayout.Label("Gravity Multiplier: " + GravityMultiplierFactor);
            //GUILayout.Label("Gravity Velocity: " + GravityVelocity);
            //GUILayout.Label("Flight Velocity: " + FlightVelocity);
            //GUILayout.Label("Vertical Direction: " + VerticalDirection);
            //GUILayout.Label("Current Velocity: " + CurrentyVelocity);
            //GUILayout.Label("Propulsion Force: " + PropulsionForce);
            //GUILayout.Label("Y: " + delta);
            //GUILayout.Label("Propulsion Time: " + PropulsionTime);
        }
    }
}
