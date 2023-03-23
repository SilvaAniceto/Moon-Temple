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

        public List<AttributesValue> m_attributesValue = new List<AttributesValue>();
        [SerializeField] private List<UIAttributeScore> m_UIAttributes = new List<UIAttributeScore>();
        [SerializeField] private TMP_Text m_currentAvailablePointsText;

        private int m_currentAvailablePoints;
        
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
        #endregion

        private void OnEnable()
        {
            CharacterCreator.Instance.m_nextButton.onClick.RemoveAllListeners();
            CharacterCreator.Instance.m_nextButton.onClick.AddListener(CharacterCreator.Instance.NextPage);
            CharacterCreator.Instance.m_nextButton.onClick.AddListener(CharacterExtraPointEditor.Instance.SetExtraPointEditor);
            

            m_currentAvailablePointsText.text = m_currentAvailablePoints.ToString();

            for (int i = 0; i < m_UIAttributes.Count; i++)
            {
                m_UIAttributes[i].OnPointsChanged.RemoveAllListeners();

                m_UIAttributes[i].OnPointsChanged.AddListener(UpdateUIDropdown);
            }

            UpdateUIDropdown();
            UpdateUIText();
        }

        public void SetAttributeEditor()
        {
            for (int i = 0; i < CharacterCreator.Instance.EditingCharacter.abilityScore.Length; i++)
            {
                AttributePoints[i].SetUIAttributeScore(CharacterCreator.Instance.EditingCharacter.abilityScore[i].ability, CharacterCreator.Instance.EditingCharacter.abilityScore[i].score);
            }
        }

        private void UpdateUIDropdown()
        {
            foreach (UIAttributeScore uIAttributeScore in m_UIAttributes)
            {
                uIAttributeScore.UpdateUIAttributeDropdown();
            }

            m_currentAvailablePointsText.text = m_currentAvailablePoints.ToString();
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
