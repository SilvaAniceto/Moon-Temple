using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static UnityEditor.Experimental.GraphView.GraphView;

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
        private bool isSet = false;
        
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
        public bool IsSet
        {
            set
            {
                isSet = value;
            }
        }
        #endregion
           
        private void OnEnable()
        {
            CharacterCreator.Instance.m_backButton.gameObject.SetActive(true);
            CharacterCreator.Instance.m_backButton.onClick.RemoveAllListeners();
            CharacterCreator.Instance.m_backButton.onClick.AddListener(CharacterCreator.Instance.PreviousPage);

            CharacterCreator.Instance.m_nextButton.gameObject.SetActive(true);
            CharacterCreator.Instance.m_nextButton.onClick.RemoveAllListeners();
            CharacterCreator.Instance.m_nextButton.onClick.AddListener(CharacterCreator.Instance.NextPage);

            if (!isSet) SetAttributeEditor(CharacterCreator.Instance.EditingCharacter);

            m_currentAvailablePointsText.text = m_currentAvailablePoints.ToString();

            for (int i = 0; i < m_UIAttributes.Count; i++)
            {
                m_UIAttributes[i].OnPointsChanged.RemoveAllListeners();

                m_UIAttributes[i].OnPointsChanged.AddListener(UpdateUIDropdown);
            }

            UpdateUIText(CharacterCreator.Instance.EditingCharacter);
        }

        public void SetAttributeEditor(PlayerCharacterData player)
        {
            m_attributesValue = new List<AttributesValue>
            {
                new AttributesValue() {inUse = true, attributeValue = 0, cost = 0},
                new AttributesValue() {inUse = false, attributeValue = 8, cost = 0},
                new AttributesValue() {inUse = false, attributeValue = 9, cost = 1},
                new AttributesValue() {inUse = false, attributeValue = 10, cost = 2},
                new AttributesValue() {inUse = false, attributeValue = 11, cost = 3},
                new AttributesValue() {inUse = false, attributeValue = 12, cost = 4},
                new AttributesValue() {inUse = false, attributeValue = 13, cost = 5},
                new AttributesValue() {inUse = false, attributeValue = 14, cost = 7},
                new AttributesValue() {inUse = false, attributeValue = 15, cost = 9}
            };

            for (int i = 0; i < player.abilityScore.Length; i++)
            {
                AttributePoints[i].SetUIAttributeScore(player.abilityScore[i].ability, player.abilityScore[i].score);
            }

            UpdateUIDropdown();

            isSet = true;
        }

        private void UpdateUIDropdown()
        {
            foreach (UIAttributeScore uIAttributeScore in m_UIAttributes)
            {
                uIAttributeScore.UpdateUIAttributeDropdown();
            }

            m_currentAvailablePointsText.text = m_currentAvailablePoints.ToString();
        }

        private void UpdateUIText(PlayerCharacterData player)
        {
            m_name.text = player.info.name;
            m_level.text =player.info.level.ToString();
            m_race.text = player.info.race.ToString();
            m_class.text = player.info.classes.ToString();
        }
    }
}
