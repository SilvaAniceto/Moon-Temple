using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CustomRPGSystem
{
    public class CharacterCreator : MonoBehaviour
    {
        public static CharacterCreator Instance;

        [Header("Main Title")]
        [SerializeField] private GameObject m_UIButtonLayout;
        [SerializeField] private Button m_NewCharacterButton;
        [SerializeField] private Button m_LoadCharacterButton;

        [Header("Character Editor")]
        [SerializeField] private CharacterEditor m_characterEditor;

        [Header("Character Ability Editor")]
        [SerializeField] private CharacterAbilityEditor m_characterAbilityEditor;

        public static string m_playerName = "";
        public static string m_characterName = "";
        public static int m_levelValue;
        public static int m_raceValue;
        public static int m_classValue;

        public static List<PlayerCharacterData> CharacterList = new List<PlayerCharacterData>();
        public static PlayerCharacterData CharacterData;

        #region PROPERTIES
        public string PlayerDirectory
        {
            get
            {
                return Application.persistentDataPath + "/Player";
            }
        }
        public string MainCharacterDirectory
        {
            get
            {
                return PlayerDirectory + "/MainCharacter";
            }
        }
        #endregion

        private void Awake()
        {
            if (Instance == null) Instance = this;

            m_UIButtonLayout.SetActive(true);
            m_characterEditor.gameObject.SetActive(false);
            m_characterAbilityEditor.gameObject.SetActive(false);

            m_characterEditor.createSave.onClick.AddListener(Create);

            if (!Directory.Exists(PlayerDirectory))
            {
                Directory.CreateDirectory(PlayerDirectory);
                Directory.CreateDirectory(MainCharacterDirectory);
            }
            else
            {
                if (!Directory.Exists(MainCharacterDirectory))
                {
                    Directory.CreateDirectory(MainCharacterDirectory);
                }
            }

            if (Directory.GetFiles(MainCharacterDirectory + "/", "*.json").Length == 0)
            {
                m_LoadCharacterButton.gameObject.SetActive(false);
                m_NewCharacterButton.onClick.AddListener(delegate
                {
                    m_UIButtonLayout.SetActive(false);
                    m_characterEditor.gameObject.SetActive(true);
                });
            }
        }

        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        private void Create()
        {
            if (string.IsNullOrEmpty(m_characterName)) return;

            CharacterData = new PlayerCharacterData(m_characterName, m_levelValue, (PlayerCharacterData.CharacterInfo.Race)m_raceValue, (PlayerCharacterData.CharacterInfo.Class)m_classValue);

            CharacterList.Add(CharacterData);
            
            //FileHandler.SaveToJSON<PlayerCharacterData>(CharacterList, MainCharacterDirectory + "/" + m_playerName);

            ManagerCreatorPages();
        }

        void ManagerCreatorPages()
        {
            m_characterEditor.gameObject.SetActive(false);
            m_characterAbilityEditor.gameObject.SetActive(true);
        }
    }
}
