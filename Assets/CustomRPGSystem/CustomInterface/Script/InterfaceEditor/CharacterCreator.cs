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
        [SerializeField] private GameObject m_mainTitle;
        [SerializeField] private Button m_NewCharacterButton;
        [SerializeField] private Button m_LoadCharacterButton;
        [SerializeField] private Button m_backButton;
        public Button m_nextButton;

        [Header("Character Editor")]
        [SerializeField] private CharacterEditor m_characterEditor;

        [Header("Character Attribute Editor")]
        [SerializeField] private CharacterAttributeEditor m_characterAttributeEditor;

        [Header("Character Extra Points Editor")]
        [SerializeField] private CharacterExtraPointEditor m_characterExtraPointEditor;

        [Header("Character Skill Editor")]
        [SerializeField] private CharacterSkillEditor m_characterSkillEditor;

        [Header("Character Sheet")]
        [SerializeField] private CharacterSheet m_characterSheet;

        [Header("Character Lists")]
        [SerializeField] private CharacterListing m_characterListing;

        [Header("PopUp Helper")]
        public PopUpHelper m_popUpHelper;

        public static string m_playerName = "";
        public static string m_characterName = "";
        public static int m_levelValue;
        public static int m_raceValue;
        public static int m_classValue;

        private List<PlayerCharacterData> CharacterList = new List<PlayerCharacterData>();
        public static PlayerCharacterData CharacterData;

        public PlayerCharacterData EditingCharacter;

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
                return PlayerDirectory + "/Characters";
            }
        }
        public List<PlayerCharacterData> SavedCharacters
        {
            get
            {
                return CharacterList;
            }
        }
        #endregion

        private void Awake()
        {
            if (Instance == null) Instance = this;
            if (CharacterAttributeEditor.Instance == null) CharacterAttributeEditor.Instance = m_characterAttributeEditor;
            if (CharacterExtraPointEditor.Instance == null) CharacterExtraPointEditor.Instance = m_characterExtraPointEditor;
            if (CharacterSkillEditor.Instance == null) CharacterSkillEditor.Instance = m_characterSkillEditor;
            if (CharacterListing.Instance == null) CharacterListing.Instance = m_characterListing;

            UIPages.Add(m_mainTitle);
            UIPages.Add(m_characterEditor.gameObject);
            UIPages.Add(m_characterAttributeEditor.gameObject);
            UIPages.Add(m_characterExtraPointEditor.gameObject);
            UIPages.Add(m_characterSkillEditor.gameObject);
            UIPages.Add(m_characterSheet.gameObject);
            UIPages.Add(m_characterListing.gameObject);

            m_backButton.onClick.AddListener(PreviousPage);
            m_nextButton.gameObject.SetActive(false);

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
                    m_nextButton.gameObject.SetActive(true);
                });
            }
            else
            {
                m_LoadCharacterButton.gameObject.SetActive(true);
                m_NewCharacterButton.onClick.AddListener(delegate
                {
                    NextPage();
                    m_nextButton.gameObject.SetActive(true);
                });
            }

            ManagerCreatorPages(0);
        }

        public void SaveCharacter(PlayerCharacterData player)
        {
            for (int i = 0; i < player.classSkills.Count; i++)
            {
                for (int j = 0; j < player.skills.Length; j++)
                {
                    if (player.classSkills[i].skill == player.skills[i].skill && player.classSkills[i].proficient)
                    {
                        player.skills[i] = player.classSkills[i];
                    }
                }
            }

            for (int i = 0; i < player.raceSkills.Count; i++)
            {
                for (int j = 0; j < player.skills.Length; j++)
                {
                    if (player.raceSkills[i].skill == player.skills[i].skill && player.raceSkills[i].proficient)
                    {
                        player.skills[i] = player.raceSkills[i];
                    }
                }
            }

            CharacterList.Add(player);

            FileHandler.SaveToJSON<PlayerCharacterData>(player, MainCharacterDirectory + "/" + player.info.name + ".json");
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

        void PreviousPage()
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

        public void NextPage()
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
