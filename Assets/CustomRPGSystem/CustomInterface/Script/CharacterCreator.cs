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
        [SerializeField] private CharacterAttributeEditor m_characterAttributeEditor;

        [Header("Character Skill Editor")]
        [SerializeField] private CharacterSkillEditor m_characterSkillEditor;

        [Header("Character Sheet")]
        [SerializeField] private CharacterSheet m_characterSheet;

        public static string m_playerName = "";
        public static string m_characterName = "";
        public static int m_levelValue;
        public static int m_raceValue;
        public static int m_classValue;

        private List<PlayerCharacterData> CharacterList = new List<PlayerCharacterData>();
        public static PlayerCharacterData CharacterData;

        public PlayerCharacterData EditingCharacter;

        private List<GameObject> UIPages = new List<GameObject>();

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
            UIPages.Add(m_characterAttributeEditor.gameObject);
            UIPages.Add(m_characterSkillEditor.gameObject);
            UIPages.Add(m_characterSheet.gameObject);

            m_characterEditor.editAbilities.onClick.AddListener(SetRaceAndClass);

            m_characterAttributeEditor.editSkills.onClick.AddListener(NextPage);
            m_characterSkillEditor.m_reviewButton.onClick.AddListener(NextPage);
            m_characterSkillEditor.m_reviewButton.onClick.AddListener(m_characterSkillEditor.SetCharacterSkills);

            m_backButton.onClick.AddListener(PreviousPage);

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

        void SetRaceAndClass()
        {
            if (string.IsNullOrEmpty(m_characterName)) return;

            CharacterData = null;

            CharacterData = new PlayerCharacterData(m_characterName, m_levelValue, (PlayerCharacterData.CharacterInfo.Race)m_raceValue, (PlayerCharacterData.CharacterInfo.Class)m_classValue);

            EditingCharacter = CharacterData;

            SetAttributeEditor();
            SetSkillEditor();

            //CharacterList.Add(CharacterData);

            //FileHandler.SaveToJSON<PlayerCharacterData>(CharacterList, MainCharacterDirectory + "/" + m_playerName);

            NextPage();
        }

        void SetAttributeEditor()
        {
            m_characterAttributeEditor.CurrentAvailablePoints = EditingCharacter.info.availablePoints;
            m_characterAttributeEditor.CurrentExtraPoints = EditingCharacter.info.extraPoints;

            for (int i = 0; i < EditingCharacter.abilityScore.Length; i++)
            {
                m_characterAttributeEditor.AttributePoints[i].SetUIAttributeScore(EditingCharacter.abilityScore[i].ability, EditingCharacter.abilityScore[i].score);
            }

            for (int i = 0; i < EditingCharacter.abilityScore.Length; i++)
            {
                m_characterAttributeEditor.ExtraAttributePoints[i].SetUIExtraAttributeScore(EditingCharacter.abilityScore[i].ability, EditingCharacter.abilityScore[i].score, m_characterAttributeEditor.HasAvailablePoints);
            }
        }

        void SetSkillEditor()
        {
            m_characterSkillEditor.CurrentRacePoints = EditingCharacter.info.raceProficiencyPoints;
            m_characterSkillEditor.CurrentClassPoints = EditingCharacter.info.classProficiencyPoints;

            m_characterSkillEditor.m_raceSkills.Clear();
            m_characterSkillEditor.m_classSkills.Clear();

            foreach (PlayerCharacterData.Skills raceSkill in EditingCharacter.raceSkills)
            {
                m_characterSkillEditor.m_raceSkills.Add(raceSkill);
            }

            foreach (PlayerCharacterData.Skills classSkill in EditingCharacter.classSkills)
            {
                m_characterSkillEditor.m_classSkills.Add(classSkill);
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
