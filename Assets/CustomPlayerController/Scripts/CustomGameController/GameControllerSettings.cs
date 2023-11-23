using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CustomGameController
{
    [CreateAssetMenu]
    public class GameControllerSettings : ScriptableObject
    {
        [SerializeField] private PlayerCharacterSettings characterSettings;
        [SerializeField] private string mainTitleScene = "";
        [SerializeField] private List<string> gameplayScenes = new List<string>();

        public PlayerCharacterSettings CharacterSettings
        {
            get
            {
                return characterSettings;
            }
        }
        public string MainTitleScene
        {
            get
            {
                return mainTitleScene;
            }
        }
        public List<string> GameplayScenes
        {
            get
            {
                return gameplayScenes;
            }
        }
        public string CurrentScene { get; set; }

        public void CheckSettingsFolder()
        {
            if (!Directory.Exists(characterSettings.PlayerCharacterSettingsPath))
                Directory.CreateDirectory(characterSettings.PlayerCharacterSettingsPath);
        }
    }
}
