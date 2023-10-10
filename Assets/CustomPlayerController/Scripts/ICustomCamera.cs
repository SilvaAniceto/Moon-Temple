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

        public CameraPerspectiveSettings(Vector2 xRotationRange, bool clampXRotation, Vector2 yRotationRange, bool clamYRotation, Quaternion rotation, Vector3 damping, Vector3 shoulderOffset, bool orthographicperspective, float viewSize, float cameradistance)
        {
            this.XRotationRange = xRotationRange;
            this.ClampXRotation = clampXRotation;
            this.YRotationRange = yRotationRange;
            this.ClampYRotation = clamYRotation;
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
        CameraPerspectiveSettings FirstPerson { get; }
        CameraPerspectiveSettings Isometric { get; }
        CameraPerspectiveSettings ThirdPerson { get; }
        CameraPerspectiveSettings OverShoulder { get; }
        Vector3 CameraHeightOfftset { get; }
        #endregion

        #region CAMERA SETTINGS
        Transform CameraTarget { get; set; }
        float CameraTargetHeight { get; set; }
        float CameraSensibility { get; set; }
        CameraPerspective CameraPerspective { get; set; }
        #endregion

        #region CAMERA INPUTS VALUES & METHODS
        float CameraPan { get; set; }
        float CameraTilt { get; set; }
        bool ChangeCameraPerspective { set; }
        void SetInput(CustomPlayerInputHandler inputs);
        #endregion

        #region CAMERA METHODS
        void SetCameraPerspective(CameraPerspective perspective);
        void UpdateCamera(float cameraTilt, float cameraPan);
        void SetPerspectiveSettings();
        #endregion
    }
}
