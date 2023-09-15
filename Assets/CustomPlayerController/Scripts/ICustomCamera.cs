using UnityEngine;

namespace CustomGameController
{
    public enum CameraPerspective
    {
        None = 0,
        Isometric,
        Third_Person,
        Over_Shoulder,
        First_Person
    }

    public interface ICustomCamera
    {
        Camera PlayerCamera { get; set; }
        CameraPerspective CameraPerspective { get; set; }
        Transform CameraTarget { get; set; }
        //Transform CameraPivot { get; set; }
        float CameraTargetHeight { get; set; }
        Vector3 CameraHeightOfftset { get; }
        float CameraSensibility { get; set; }
        float CameraPan { get; set; }
        float CameraTilt { get; set; }
        bool ChangeCameraPerspective { get; set; }

        void SetInput(CustomPlayerInputHandler inputs);
    }
}
