using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace CustomRPGSystem
{
    public class UIAttributeScore : MonoBehaviour
    {
        [System.Serializable]
        public class AttributesValue
        {
            public bool inUse;
            public int attributeValue;
            public int cost;
        }

        public PlayerCharacterData.AbilityScore.Ability m_ability = PlayerCharacterData.AbilityScore.Ability.Strenght;
        public TMP_Text m_abilityDescription;
        public TMP_Text m_abilityValue;

        [Header("Attributes Points UI")]
        public TMP_Dropdown m_attributeDrop;
        public List<AttributesValue> m_attributesValue = new List<AttributesValue>();

        private int m_standardScore;
        private int m_currentScore;

        #region Properties
        public int StandardScore
        {
            get
            {
                return m_standardScore;
            }
        }
        public int CurrentScore
        {
            get
            {
                return m_currentScore;
            }
        }
        #endregion

        private void Awake()
        {
            UpdateUIAttributeDropdown();
        }

        public void SetUIAttributeScore(PlayerCharacterData.AbilityScore.Ability ability, int value)
        {
            m_ability = ability;
            m_abilityDescription.text = ability.ToString();

            m_standardScore = value;
            m_abilityValue.text = m_standardScore.ToString();
            m_currentScore = m_standardScore;
        }

        void UpdateUIAttributeDropdown()
        {
            m_attributeDrop.ClearOptions();

            for (int i = 0; i < m_attributesValue.Count; i++)
            {
                if (!m_attributesValue[i].inUse)
                {
                    m_attributeDrop.AddOptions(new List<TMP_Dropdown.OptionData> 
                    {
                        new TMP_Dropdown.OptionData() {text = " " + m_attributesValue[i].attributeValue.ToString() + " ("+ "Cost: " + m_attributesValue[i].cost + ") "},
                    });
                }
            }
        }
    }
}
