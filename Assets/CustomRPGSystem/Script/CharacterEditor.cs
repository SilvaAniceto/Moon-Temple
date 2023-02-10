using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CustomRPGSystem
{
    public class CharacterEditor : MonoBehaviour
    {
        [SerializeField] InputField name;
        [SerializeField] Dropdown level;
        [SerializeField] Dropdown race;
        [SerializeField] Dropdown classes;

 
        public List<PlayerCharacterData> CharacterData = new List<PlayerCharacterData>();
        // Start is called before the first frame update
        public void Create()
        {
            if (string.IsNullOrEmpty(name.text)) return;

            CharacterData.Add(new PlayerCharacterData(name.text, level.value, (PlayerCharacterData.CharacterInfo.Race)race.value, (PlayerCharacterData.CharacterInfo.Class)classes.value));           
            FileHandler.SaveToJSON<PlayerCharacterData>(CharacterData, name.text);
        }
    }
}
