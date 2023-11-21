using System.IO;
using UnityEngine;

namespace CustomGameController
{
    [CreateAssetMenu]
    public class GameControllerSettings : ScriptableObject
    {
        public PlayerCharacterSettings characterSettings;

        public void CheckSettingsFolder()
        {
            if (!Directory.Exists(characterSettings.PlayerCharacterSettingsPath))
                Directory.CreateDirectory(characterSettings.PlayerCharacterSettingsPath);
        }
    }
}
