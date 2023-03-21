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

        [Header("Available Points Editor")]
        [SerializeField] private TMP_Text m_currentAvailablePointsText;


        [Header("Extra Points Editor")]
        [SerializeField] private TMP_Text m_extraPointsText;
        [SerializeField] private List<UIExtraAttributeScore> m_UIAbility = new List<UIExtraAttributeScore>();

        public Button editSkills;
        private int m_currentAvailablePoints;
        private int m_currentExtraPoints;

        #region PROPERTIES
        public List<UIExtraAttributeScore> Ability
        {
            get
            {
                return m_UIAbility;
            }
        }
        public int AvailablePoints
        {
            get
            {
                return m_currentAvailablePoints;
            }
        }
        public int CurrentAvailablePoints
        {
            set
            {
                m_currentAvailablePoints = value;
            }
        }
        public bool HasAvailablePoints
        {
            get
            {
                return AvailablePoints > 0 ? true : false;
            }
        }
        public int ExtraPoints
        {
            get
            {
                return m_currentExtraPoints;
            }
        }
        public int CurrentExtraPoints
        {
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

                m_UIAbility[i].OnPointsChanged.AddListener(UpdateCurrentExtraPoints);
            }

            UpdateUIPoints();
            UpdateUIText();
        }

        private void UpdateUIPoints()
        {
            foreach (UIExtraAttributeScore uIAbility in m_UIAbility)
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

        private void UpdateCurrentExtraPoints(int p_value)
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
