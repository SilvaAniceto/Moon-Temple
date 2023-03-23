using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace CustomRPGSystem
{
    public class CharacterSkillEditor : MonoBehaviour
    {
        public static CharacterSkillEditor Instance;

        [SerializeField] private TMP_Text m_proficiencyPoints;
        [SerializeField] private TMP_Text m_race;
        [SerializeField] private TMP_Text m_class;
        //public Button m_reviewButton;

        public Button m_raceButton;
        public Button m_classButton;

        [SerializeField] private List<UISkill> m_UISkill = new List<UISkill>();
        [HideInInspector] public List<PlayerCharacterData.Skills> m_raceSkills = new List<PlayerCharacterData.Skills>();
        [HideInInspector] public List<PlayerCharacterData.Skills> m_classSkills = new List<PlayerCharacterData.Skills>();
        [HideInInspector] public List<PlayerCharacterData.Skills> m_skills = new List<PlayerCharacterData.Skills>();

        private int m_currentRacePoints;
        private int m_currentClassPoints;

        #region PROPERTIES
        public int CurrentRacePoints
        {
            get
            {
                return m_currentRacePoints;
            }
            set
            {
                m_currentRacePoints = value;
            }
        }
        public int CurrentClassPoints
        {
            get
            {
                return m_currentClassPoints;
            }
            set
            {
                m_currentClassPoints = value;
            }
        }
        public bool HasAvailableRacePoints
        {
            get
            {
                return m_currentRacePoints == 0 ? false : true;
            }
        }
        public bool HasAvailableClassPoints
        {
            get
            {
                return m_currentClassPoints == 0 ? false : true;
            }
        }
        #endregion

        void OnEnable()
        {
            SetSkillEditor();

            m_raceButton.onClick.RemoveAllListeners();
            m_classButton.onClick.RemoveAllListeners();

            m_raceButton.onClick.AddListener(delegate
            {
                ShowRaceSkill();
            });

            m_classButton.onClick.AddListener(delegate
            {
                ShowClassSkill();
            });

            UpdateUIText();
            ShowRaceSkill();
        }

        void SetSkillEditor()
        {
            if (Instance == null)
            {
                Instance = this;
                CurrentRacePoints = CharacterCreator.Instance.EditingCharacter.info.raceProficiencyPoints;
                CurrentClassPoints = CharacterCreator.Instance.EditingCharacter.info.classProficiencyPoints;

                m_raceSkills.Clear();
                m_classSkills.Clear();

                foreach (PlayerCharacterData.Skills raceSkill in CharacterCreator.Instance.EditingCharacter.raceSkills)
                {
                    m_raceSkills.Add(raceSkill);
                }

                foreach (PlayerCharacterData.Skills classSkill in CharacterCreator.Instance.EditingCharacter.classSkills)
                {
                    m_classSkills.Add(classSkill);
                }

                SetCharacterSkills();
            }
        }

        public void SetCharacterSkills()
        {
            m_skills.Clear();

            for (int i = 0; i < CharacterCreator.Instance.EditingCharacter.skills.Length; i++)
            {
                m_skills.Add(CharacterCreator.Instance.EditingCharacter.skills[i]);
            }

            for (int i = 0; i < m_raceSkills.Count; i++)
            {
                if (m_raceSkills[i].proficient)
                {
                    for (int j = 0; j < m_skills.Count; j++)
                    {
                        if (m_raceSkills[i].skill == m_skills[j].skill)
                        {
                            m_skills[j] = m_raceSkills[i];
                        }
                    }
                }
            }

            for (int i = 0; i < m_classSkills.Count; i++)
            {
                if (m_classSkills[i].proficient)
                {
                    for (int j = 0; j < m_skills.Count; j++)
                    {
                        if (m_classSkills[i].skill == m_skills[j].skill)
                        {
                            m_skills[j] = m_classSkills[i];
                        }
                    }
                }
            }
        }

        void ShowRaceSkill()
        {
            m_proficiencyPoints.text = m_currentRacePoints.ToString();
            for (int i = 0; i < m_raceSkills.Count; i++)
            {
                m_UISkill[i].SetUISkill(m_raceSkills[i], m_raceSkills[i].proficient, m_raceSkills[i].isChangable, HasAvailableRacePoints);
                m_UISkill[i].OnProficiencySet.RemoveAllListeners();
                m_UISkill[i].OnProficiencySet.AddListener(UpdateRacePoints);

                for (int j = 0; j < m_classSkills.Count; j++)
                {
                    if (m_raceSkills[i].skill == m_classSkills[j].skill)
                    {
                        if (m_classSkills[j].proficient)
                        {
                            m_UISkill[i].SetProficiencyToggle(m_classSkills[j].proficient);
                        }
                    }
                }
            }
        }

        void ShowClassSkill()
        {
            m_proficiencyPoints.text = m_currentClassPoints.ToString();
            for (int i = 0; i < m_classSkills.Count; i++)
            {
                m_UISkill[i].SetUISkill(m_classSkills[i], m_classSkills[i].proficient, m_classSkills[i].isChangable, HasAvailableClassPoints);
                m_UISkill[i].OnProficiencySet.RemoveAllListeners();
                m_UISkill[i].OnProficiencySet.AddListener(UpdateClassPoints);

                for (int j = 0; j < m_raceSkills.Count; j++)
                {
                    if (m_classSkills[i].skill == m_raceSkills[j].skill)
                    {
                        if (m_raceSkills[j].proficient)
                        {
                            m_UISkill[i].SetProficiencyToggle(m_raceSkills[j].proficient);
                        }
                    }
                }
            }
        }

        void UpdateRacePoints(PlayerCharacterData.Skills p_skill, bool p_proficiency)
        {
            if (p_proficiency) m_currentRacePoints--;
            else m_currentRacePoints++;

            ShowRaceSkill();
        }

        void UpdateClassPoints(PlayerCharacterData.Skills p_skill, bool p_proficiency)
        {
            if (p_proficiency) m_currentClassPoints--;
            else m_currentClassPoints++;

            ShowClassSkill();
        }

        void UpdateUIText()
        {
            m_race.text = CharacterCreator.Instance.EditingCharacter.info.race.ToString();
            m_class.text = CharacterCreator.Instance.EditingCharacter.info.classes.ToString();
        }
    }
}
