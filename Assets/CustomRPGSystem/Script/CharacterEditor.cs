using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace CustomRPGSystem
{
    public class CharacterEditor : MonoBehaviour
    {
        [SerializeField] TMP_InputField characterName;
        [SerializeField] TMP_Dropdown level;
        [SerializeField] TMP_Dropdown race;
        [SerializeField] TMP_Dropdown classes;
        
        public List<PlayerCharacterData> CharacterData = new List<PlayerCharacterData>();
        // Start is called before the first frame update
        public void Create()
        {
            if (string.IsNullOrEmpty(characterName.text)) return;

            CharacterData.Add(new PlayerCharacterData(characterName.text, level.value, (PlayerCharacterData.CharacterInfo.Race)race.value, (PlayerCharacterData.CharacterInfo.Class)classes.value));           
            FileHandler.SaveToJSON<PlayerCharacterData>(CharacterData, characterName.text);
        }
    }
}
