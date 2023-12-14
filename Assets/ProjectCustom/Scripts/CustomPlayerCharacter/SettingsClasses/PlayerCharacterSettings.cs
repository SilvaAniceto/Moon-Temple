using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CustomGameController
{
    [System.Serializable]
    public class PlayerCharacterSettings
    {
        public string PlayerCharacterSettingsPath => Application.persistentDataPath + "/PlayerSettings";
        public string FileSettings => "/setting";

        [SerializeField][Range(1f, 5)] private float m_CameraTargetHeight = 1.8f;
        [SerializeField][Range(0.5f, 1.5f)] private float m_cameraSensibility = 1.25f;

        public float CameraHeight
        {
            get
            {
                return m_CameraTargetHeight;
            }
            set
            {
                if (m_CameraTargetHeight == value) return;

                m_CameraTargetHeight = value;
            }
        }
        public float CameraSensibility
        {
            get
            {
                return m_cameraSensibility;
            }
            set
            {
                if (m_cameraSensibility == value) return;

                m_cameraSensibility = value;
            }
        }
    }
}
