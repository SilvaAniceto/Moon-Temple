using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace CustomRPGSystem
{
    public class CharacterSkillEditor : MonoBehaviour
    {
        [SerializeField] private TMP_Text m_proficiencyPoints;
        [SerializeField] private TMP_Text m_race;
        [SerializeField] private TMP_Text m_class;

        public Button m_raceButton;
        public Button m_classButton;

        [SerializeField] private List<UISkill> m_UISkill = new List<UISkill>();
        private List<PlayerCharacterData.Skills> m_raceSkills = new List<PlayerCharacterData.Skills>();
        private List<PlayerCharacterData.Skills> m_classSkills = new List<PlayerCharacterData.Skills>();

        private int m_currentRacePoints;
        private int m_currentClassPoints;

        #region PROPERTIES
        public int CurrentRacePoints
        {
            get
            {
                return m_currentRacePoints;
            }
        }
        public int CurrentClassPoints
        {
            get
            {
                return m_currentClassPoints;
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
            m_currentRacePoints = CharacterCreator.CharacterData.info.raceProficiencyPoints;
            m_currentClassPoints = CharacterCreator.CharacterData.info.classProficiencyPoints;

            m_raceSkills.Clear();
            m_classSkills.Clear();

            foreach (PlayerCharacterData.Skills raceSkill in CharacterCreator.CharacterData.raceSkills)
            {
                m_raceSkills.Add(raceSkill);
            }

            foreach (PlayerCharacterData.Skills classSkill in CharacterCreator.CharacterData.classSkills)
            {
                m_classSkills.Add(classSkill);
            }

            m_raceButton.onClick.AddListener(delegate
            {
                ShowRaceSkill(m_raceSkills);
            });

            m_classButton.onClick.AddListener(delegate
            {
                ShowClassSkill(m_classSkills);
            });

            UpdateUIText();
            ShowRaceSkill(m_raceSkills);
        }

        void ShowRaceSkill(List<PlayerCharacterData.Skills> skills)
        {
            m_proficiencyPoints.text = m_currentRacePoints.ToString();
            for (int i = 0; i < skills.Count; i++)
            {
                m_UISkill[i].SetUISkill(skills[i], skills[i].proficient, skills[i].isChangable, HasAvailableRacePoints);
                m_UISkill[i].OnProficiencySet.RemoveAllListeners();
                m_UISkill[i].OnProficiencySet.AddListener(UpdateRacePoints);
            }
        }

        void ShowClassSkill(List<PlayerCharacterData.Skills> skills)
        {
            m_proficiencyPoints.text = m_currentClassPoints.ToString();
            for (int i = 0; i < skills.Count; i++)
            {
                m_UISkill[i].SetUISkill(skills[i], skills[i].proficient, skills[i].isChangable, HasAvailableClassPoints);
                m_UISkill[i].OnProficiencySet.RemoveAllListeners();
                m_UISkill[i].OnProficiencySet.AddListener(UpdateClassPoints);
            }
        }

        void UpdateRacePoints(bool p_proficiency)
        {
            if (p_proficiency) m_currentRacePoints--;
            else m_currentRacePoints++;

            ShowRaceSkill(m_raceSkills);
        }

        void UpdateClassPoints(bool p_proficiency)
        {
            if (p_proficiency) m_currentClassPoints--;
            else m_currentClassPoints++;

            ShowClassSkill(m_classSkills);
        }

        private void UpdateUIText()
        {
            m_race.text = CharacterCreator.CharacterData.info.race.ToString();
            m_class.text = CharacterCreator.CharacterData.info.classes.ToString();
        }
    }
}
