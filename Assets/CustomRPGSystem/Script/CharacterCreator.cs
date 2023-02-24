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
        [SerializeField] private Button m_backButton;

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

        public PlayerCharacterData c;

        [SerializeField] private List<GameObject> UIPages = new List<GameObject>();

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

            UIPages.Add(m_UIButtonLayout);
            UIPages.Add(m_characterEditor.gameObject);
            UIPages.Add(m_characterAbilityEditor.gameObject);

            m_characterEditor.editAbilities.onClick.AddListener(Create);

            m_characterAbilityEditor.editSkills.onClick.AddListener(SetAbilities);

            m_backButton.onClick.AddListener(OnBackButton);

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
                    NextPage();
                });
            }

            ManagerCreatorPages(0);
        }

        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        void Create()
        {
            if (string.IsNullOrEmpty(m_characterName)) return;

            CharacterData = new PlayerCharacterData(m_characterName, m_levelValue, (PlayerCharacterData.CharacterInfo.Race)m_raceValue, (PlayerCharacterData.CharacterInfo.Class)m_classValue);

            c = CharacterData;

            //CharacterList.Add(CharacterData);

            //FileHandler.SaveToJSON<PlayerCharacterData>(CharacterList, MainCharacterDirectory + "/" + m_playerName);

            NextPage();
        }

        void SetAbilities()
        {
            for (int i = 0; i < CharacterData.abilityScore.Length; i++)
            {
                for (int j = 0; j < m_characterAbilityEditor.Ability.Count; j++)
                {
                    if (CharacterData.abilityScore[i].ability == m_characterAbilityEditor.Ability[j].m_ability)
                    {
                        CharacterData.SetAbilityScore(CharacterData, m_characterAbilityEditor.Ability[j].m_ability, m_characterAbilityEditor.Ability[j].CurrentScore);
                    }
                }
            }
        }

        void ManagerCreatorPages(int p_pageIndex)
        {
            if (p_pageIndex == 0) m_backButton.gameObject.SetActive(false);
            else m_backButton.gameObject.SetActive(true);

            for (int i = 0; i < UIPages.Count; i++)
            {
                UIPages[i].SetActive(false);
            }

            UIPages[p_pageIndex].SetActive(true);
        }

        void OnBackButton()
        {
            int index = 0;

            for (int i = 0; i < UIPages.Count; i++)
            {
                if (UIPages[i].activeInHierarchy)
                {
                    index = i;
                }
            }

            ManagerCreatorPages(index - 1);
        }

        void NextPage()
        {
            int index = 0;

            for (int i = 0; i < UIPages.Count; i++)
            {
                if (UIPages[i].activeInHierarchy)
                {
                    index = i;
                }
            }

            ManagerCreatorPages(index + 1);
        }
    }
}
