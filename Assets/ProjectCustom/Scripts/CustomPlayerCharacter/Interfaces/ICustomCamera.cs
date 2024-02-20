using System;
using UnityEngine;

namespace CustomGameController
{
    public enum CameraPerspective
    {
        First_Person = 0,
        Isometric,
        Third_Person,
        Over_Shoulder
    }

    [Serializable]
    public struct CameraPerspectiveSettings
    {
        public Vector2 XRotationRange;
        public bool ClampXRotation;
        public Vector2 YRotationRange;
        public bool ClampYRotation;
        public Quaternion Rotation;
        public Vector3 Damping;
        public Vector3 ShoulderOffset;
        public bool OrthographicPerspective;
        public float ViewSize;
        public float CameraDistance;

        public CameraPerspectiveSettings(Vector2 xRotationRange, bool clampXRotation, Vector2 yRotationRange, bool clampYRotation, Quaternion rotation, Vector3 damping, Vector3 shoulderOffset, bool orthographicperspective, float viewSize, float cameradistance)
        {
            this.XRotationRange = xRotationRange;
            this.ClampXRotation = clampXRotation;
            this.YRotationRange = yRotationRange;
            this.ClampYRotation = clampYRotation;
            this.Rotation = rotation;
            this.Damping = damping;
            this.ShoulderOffset = shoulderOffset;
            this.OrthographicPerspective = orthographicperspective;
            this.ViewSize = viewSize;
            this.CameraDistance = cameradistance;
        }
    }

    public interface ICustomCamera
    {
        #region CAMERA PROPERTIES
        Camera PlayerCamera { get; set; }
        LayerMask ThirdPersonCollisionFilter {  get; set; }
        LayerMask IsometricCollisionFilter { get; set; }
        CustomCharacterController CustomController { get; set; }
        Vector3 CameraOfftset { get; set; }
        Vector3 WalkOfftset { get; set; }
        Vector3 SprintOfftset { get; set; }
        Vector3 HoverFlightOfftset { get; set; }
        Vector3 SpeedFlightOfftset { get; set; }
        int VerticalCameraDirection { get; }
        #endregion

        #region CAMERA SETTINGS
        Transform CameraTarget { get; set; }
        float CameraTargetHeight { get; set; }
        float CameraSensibility { get; set; }
        float CurrentCameraDistance { get; set; }
        Vector3 CurrentDamping { get; set; }
        Vector3 CurrentShoulderOffset { get; set; }
        #endregion

        #region CAMERA INPUTS VALUES & METHODS
        float CameraPan { get; set; }
        float CameraTilt { get; set; }
        float CameraZoom { get; set; }
        void SetInput(CustomPlayerInputHandler inputs);
        #endregion

        #region CAMERA METHODS
        void UpdateCamera(float cameraTilt, float cameraPan, float cameraZoom);
        void UpdateCameraFollow(float cameraDistance, Vector3 damping, Vector3 shoulderOffset);
        #endregion
    }
}
