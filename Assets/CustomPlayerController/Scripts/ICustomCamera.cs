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
        public Vector2 YRotationRange;
        public Quaternion Rotation;
        public Vector3 Damping;
        public Vector3 ShoulderOffset;
        public bool OrthographicPerspective;
        public float CameraDistance;

        public CameraPerspectiveSettings(Vector2 xRotationRange, Vector2 yRotationRange, Quaternion rotation, Vector3 damping, Vector3 shoulderOffset, bool orthographicperspective, float cameradistance)
        {
            this.XRotationRange = xRotationRange;
            this.YRotationRange = yRotationRange;
            this.Rotation = rotation;
            this.Damping = damping;
            this.ShoulderOffset = shoulderOffset;
            this.OrthographicPerspective = orthographicperspective;
            this.CameraDistance = cameradistance;
        }
    }

    public interface ICustomCamera
    {
        CustomCharacterController CustomController { get; set; }
        Camera PlayerCamera { get; set; }
        CameraPerspective CameraPerspective { get; set; }
        CameraPerspectiveSettings FirstPerson { get; }
        CameraPerspectiveSettings Isometric { get; }
        CameraPerspectiveSettings ThirdPerson { get; }
        Transform CameraTarget { get; set; }
        float CameraTargetHeight { get; set; }
        Vector3 CameraHeightOfftset { get; }
        float CameraSensibility { get; set; }
        float CameraPan { get; set; }
        float CameraTilt { get; set; }
        bool ChangeCameraPerspective { get; set; }

        void SetInput(CustomPlayerInputHandler inputs);
    }
}
