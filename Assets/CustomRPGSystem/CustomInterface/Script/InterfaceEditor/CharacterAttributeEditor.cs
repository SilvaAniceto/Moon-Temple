using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace CustomRPGSystem
{
    public class CharacterAttributeEditor : MonoBehaviour
    {
        public static CharacterAttributeEditor Instance;

        [System.Serializable]
        public class AttributesValue
        {
            public bool inUse;
            public int attributeValue;
            public int cost;
        }

        [SerializeField] private TMP_Text m_name;
        [SerializeField] private TMP_Text m_level;
        [SerializeField] private TMP_Text m_race;
        [SerializeField] private TMP_Text m_class;

        [Header("Available Points Editor")]
        public List<AttributesValue> m_attributesValue = new List<AttributesValue>();
        [SerializeField] private List<UIAttributeScore> m_UIAttributes = new List<UIAttributeScore>();
        [SerializeField] private TMP_Text m_currentAvailablePointsText;


        [Header("Extra Points Editor")]
        [SerializeField] private List<UIExtraAttributeScore> m_UIExtraAttributes = new List<UIExtraAttributeScore>();
        [SerializeField] private TMP_Text m_extraPointsText;

        public Button editSkills;
        private int m_currentAvailablePoints;
        private int m_currentExtraPoints;

        #region PROPERTIES
        public List<UIAttributeScore> AttributePoints
        {
            get 
            { 
                return m_UIAttributes;
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
        public List<UIExtraAttributeScore> ExtraAttributePoints
        {
            get
            {
                return m_UIExtraAttributes;
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
            m_currentAvailablePointsText.text = m_currentAvailablePoints.ToString();

            for (int i = 0; i < m_UIExtraAttributes.Count; i++)
            {
                m_UIExtraAttributes[i].OnPointsChanged.RemoveAllListeners();

                m_UIExtraAttributes[i].OnPointsChanged.AddListener(UpdateCurrentExtraPoints);
            }

            for (int i = 0; i < m_UIAttributes.Count; i++)
            {
                m_UIAttributes[i].OnPointsChanged.RemoveAllListeners();

                m_UIAttributes[i].OnPointsChanged.AddListener(UpdateUIDropdown);
            }

            UpdateUIPoints();
            UpdateUIDropdown();
            UpdateUIText();
        }

        private void UpdateUIPoints()
        {
            foreach (UIExtraAttributeScore uIAbility in m_UIExtraAttributes)
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

        private void UpdateUIDropdown()
        {
            foreach (UIAttributeScore uIAttributeScore in m_UIAttributes)
            {
                uIAttributeScore.UpdateUIAttributeDropdown();
            }
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
