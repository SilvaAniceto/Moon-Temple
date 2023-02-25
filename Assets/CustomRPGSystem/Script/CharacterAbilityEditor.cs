using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace CustomRPGSystem
{
    public class CharacterAbilityEditor : MonoBehaviour
    {
        [SerializeField] private TMP_Text m_name;
        [SerializeField] private TMP_Text m_level;
        [SerializeField] private TMP_Text m_race;
        [SerializeField] private TMP_Text m_class;
        [SerializeField] private TMP_Text m_availablePointsText;
        public Button editSkills;

        [SerializeField] private List<UIAbilityScore> m_UIAbility = new List<UIAbilityScore>();
        private int m_currentAvailablePoints;

        #region PROPERTIES
        public int AvailablePoints
        {
            get
            {
                return CharacterCreator.CharacterData.info.abilityPoints;
            }
        }
        public bool HasAvailablePoints
        {
            get
            {
                return AvailablePoints > 0 ? true : false;
            }
        }
        public List<UIAbilityScore> Ability
        {
            get
            {
                return m_UIAbility;
            }
        }
        public int CurrentAvailablePoints
        {
            get
            {
                return m_currentAvailablePoints;
            }
        }
        #endregion

        private void OnEnable()
        {
            m_currentAvailablePoints = CharacterCreator.CharacterData.info.abilityPoints;
            m_availablePointsText.text = m_currentAvailablePoints.ToString();

            for (int i = 0; i < CharacterCreator.CharacterData.abilityScore.Length; i++)
            {
                m_UIAbility[i].SetUIAbilityScore(CharacterCreator.CharacterData.abilityScore[i].ability,
                                                 CharacterCreator.CharacterData.abilityScore[i].score,
                                                 HasAvailablePoints);
                m_UIAbility[i].OnPointsChanged.AddListener(UpdateCurrentPoints);
            }

            UpdateUIPoints();
            UpdateUIText();
        }

        private void UpdateUIPoints()
        {
            foreach (UIAbilityScore uIAbility in m_UIAbility)
            {
                if (!HasAvailablePoints && uIAbility.CurrentScore > uIAbility.StandardScore)
                {
                    uIAbility.m_plusButton.gameObject.SetActive(false);
                    uIAbility.m_minusButton.gameObject.SetActive(true);
                }
                else if(!HasAvailablePoints && uIAbility.CurrentScore == uIAbility.StandardScore)
                {
                    uIAbility.m_plusButton.gameObject.SetActive(false);
                    uIAbility.m_minusButton.gameObject.SetActive(false);
                }
                else if(HasAvailablePoints && uIAbility.CurrentScore == uIAbility.StandardScore)
                {
                    uIAbility.m_plusButton.gameObject.SetActive(true);
                    uIAbility.m_minusButton.gameObject.SetActive(false);
                }
                else if (HasAvailablePoints && uIAbility.CurrentScore > uIAbility.StandardScore)
                {
                    uIAbility.m_plusButton.gameObject.SetActive(true);
                    uIAbility.m_minusButton.gameObject.SetActive(true);
                }
            }

            m_availablePointsText.text = m_currentAvailablePoints.ToString();
        }

        private void UpdateCurrentPoints(int p_value)
        {
            m_currentAvailablePoints = m_currentAvailablePoints + p_value;

            UpdateUIPoints();
        }

        private void UpdateUIText()
        {
            m_name.text = CharacterCreator.CharacterData.info.name;
            m_level.text = CharacterCreator.CharacterData.info.level.ToString();
            m_race.text = CharacterCreator.CharacterData.info.race.ToString();
            m_class.text = CharacterCreator.CharacterData.info.classes.ToString();
        }
    }
}
