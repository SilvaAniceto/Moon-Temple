using UnityEngine;

namespace CustomGameController
{
    public interface ICustomCamera
    {
        Vector2 CameraLook { get; set; }
        bool CameraAimInput { get; set; }

        void SetInput(CustomPlayerInputHandler inputs);
    }
    public class CustomCamera : MonoBehaviour, ICustomCamera
    {
        public static CustomCamera Instance;

        public enum CameraPerspective
        {
            None = 0,
            Isometric,
            Third_Person,
            Over_Shoulder,
            First_Person
        }

        [SerializeField] private Camera m_camera;

        [Header("Camera Settings")]
        [Range(10f, 100f)][SerializeField] private float m_cameraSpeed;
        [Range(1f, 3f)][SerializeField] private float m_verticalOffset;

        [Header("Camera Target")]
        [SerializeField] private Transform m_target;

        public Vector2 CameraLook { get; set; }
        public bool CameraAimInput { get; set; }
        public CameraPerspective Perspective { get; set; }

        private void Awake()
        {
            if (Instance == null) Instance = this;
        }

        void Update()
        {
            if (Perspective != CameraPerspective.First_Person)
            {
                var targetRotation = Quaternion.LookRotation(m_target.position - transform.position);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 1);
            }
            else if (Perspective == CameraPerspective.First_Person)
            {
                transform.rotation = m_target.rotation;
            }
        }

        public void SetInput(CustomPlayerInputHandler inputs)
        {

        }

        public void SetCameraPerspective(CameraPerspective perspective)
        {
            Perspective = perspective;
            switch (perspective)
            {
                case CameraPerspective.None:
                    m_camera.transform.localPosition = Vector3.zero;
                    break;
                case CameraPerspective.Isometric:
                    m_camera.transform.localPosition = new Vector3(-25.0f, 15.0f, -25.0f);
                    m_camera.transform.rotation = Quaternion.Euler( new Vector3(0.0f, 45.0f, 0.0f));

                    m_camera.orthographic = true;
                    m_camera.orthographicSize = 10;
                    break;
                case CameraPerspective.Third_Person:
                    m_camera.transform.localPosition = new Vector3(0.0f, 4.5f, -10.0f);
                    m_camera.transform.rotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, 0.0f));

                    m_camera.orthographic = false;
                    break;
                case CameraPerspective.Over_Shoulder:
                    break;
                case CameraPerspective.First_Person:
                    m_camera.transform.localPosition = new Vector3(0.0f, 1.5f, 0f);
                    m_camera.transform.rotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, 0.0f));

                    m_camera.orthographic = false;
                    break;
            }
        }
    }
}