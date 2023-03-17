using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace CustomRPGSystem
{
    public class CharacterAttributeEditor : MonoBehaviour
    {
        [SerializeField] private TMP_Text m_name;
        [SerializeField] private TMP_Text m_level;
        [SerializeField] private TMP_Text m_race;
        [SerializeField] private TMP_Text m_class;

        [Header("Extra Points Editor")]
        [SerializeField] private TMP_Text m_extraPointsText;
        [SerializeField] private List<UIAbilityScore> m_UIAbility = new List<UIAbilityScore>();

        public Button editSkills;
        private int m_currentExtraPoints;

        #region PROPERTIES
        public int ExtraPoints
        {
            get
            {
                return m_currentExtraPoints;
            }
        }
        public bool HasAvailablePoints
        {
            get
            {
                return ExtraPoints > 0 ? true : false;
            }
        }
        public List<UIAbilityScore> Ability
        {
            get
            {
                return m_UIAbility;
            }
        }
        public int CurrentExtraPoints
        {
            get
            {
                return m_currentExtraPoints;
            }
            set
            {
                m_currentExtraPoints = value;
            }
        }
        #endregion

        private void OnEnable()
        {
            m_extraPointsText.text = m_currentExtraPoints.ToString();

            for (int i = 0; i < m_UIAbility.Count; i++)
            {
                m_UIAbility[i].OnPointsChanged.RemoveAllListeners();

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
                else if (!HasAvailablePoints && uIAbility.CurrentScore == uIAbility.StandardScore)
                {
                    uIAbility.m_plusButton.gameObject.SetActive(false);
                    uIAbility.m_minusButton.gameObject.SetActive(false);
                }
                else if (HasAvailablePoints && uIAbility.CurrentScore == uIAbility.StandardScore)
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

            m_extraPointsText.text = m_currentExtraPoints.ToString();
        }

        private void UpdateCurrentPoints(int p_value)
        {
            m_currentExtraPoints = m_currentExtraPoints + p_value;

            UpdateUIPoints();
        }

        private void UpdateUIText()
        {
            m_name.text = CharacterCreator.Instance.EditingCharacter.info.name;
            m_level.text = CharacterCreator.Instance.EditingCharacter.info.level.ToString();
            m_race.text = CharacterCreator.Instance.EditingCharacter.info.race.ToString();
            m_class.text = CharacterCreator.Instance.EditingCharacter.info.classes.ToString();
        }
    }
}
