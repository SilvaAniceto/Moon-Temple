using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

namespace CustomRPGSystem
{
    public class UIAttributeScore : MonoBehaviour
    {
        [SerializeField] private PlayerCharacterData.AbilityScore.Ability m_ability = PlayerCharacterData.AbilityScore.Ability.Strenght;
        [SerializeField] private TMP_Text m_abilityDescription;
        [SerializeField] private TMP_Text m_abilityValue;
        [SerializeField] private TMP_Text m_modifierValue;

        [Header("Attributes Points UI")]
        [SerializeField] private List<CharacterAttributeEditor.AttributesValue> m_values = new List<CharacterAttributeEditor.AttributesValue>();
        public TMP_Dropdown m_attributeDrop;

        [HideInInspector] public UnityEvent OnPointsChanged = new UnityEvent();

        private int m_standardScore;
        private int m_currentScore;
        [SerializeField] private List<int> m_score = new List<int>();

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

        public void SetUIAttributeScore(PlayerCharacterData.AbilityScore.Ability ability, int value)
        {
            m_ability = ability;
            m_abilityDescription.text = ability.ToString();

            m_standardScore = value;
            m_abilityValue.text = m_standardScore.ToString();
            m_currentScore = m_standardScore;

            for (int i = 0; i < CharacterCreator.Instance.EditingCharacter.abilityScore.Length; i++)
            {
                if (CharacterCreator.Instance.EditingCharacter.abilityScore[i].ability == ability)
                {
                    m_modifierValue.text = CharacterCreator.Instance.EditingCharacter.abilityScore[i].modifier.ToString();
                }
            }

            m_attributeDrop.value = 0;

            CharacterAttributeEditor.Instance.m_attributesValue[0].inUse = true;

            m_attributeDrop.onValueChanged.AddListener(delegate
            {
                DistributeAttribute(m_ability, m_attributeDrop.value);
            });

            UpdateUIAttributeDropdown();
        }

        void DistributeAttribute(PlayerCharacterData.AbilityScore.Ability ability, int index)
        {
            CharacterAttributeEditor.AttributesValue attValue = m_values.Find(x => x.attributeValue == m_score[index]);

            for (int i = 0; i < m_values.Count; i++)
            {
                if (m_values[i].inUse)
                {
                    if (m_values[i].attributeValue != attValue.attributeValue)
                    {
                        m_currentScore -= m_values[i].attributeValue;
                        CharacterAttributeEditor.Instance.CurrentAvailablePoints = CharacterAttributeEditor.Instance.AvailablePoints + m_values[i].cost;

                        m_currentScore += attValue.attributeValue;
                        CharacterAttributeEditor.Instance.CurrentAvailablePoints = CharacterAttributeEditor.Instance.AvailablePoints - attValue.cost;

                        int indexOf = m_values.IndexOf(attValue);

                        m_values[i].inUse = false;
                        m_values[indexOf].inUse = true;

                        CharacterAttributeEditor.Instance.m_attributesValue[indexOf] = m_values[indexOf];
                    }
                }
            }

            for (int i = 0; i < CharacterCreator.Instance.EditingCharacter.abilityScore.Length; i++)
            {
                if (CharacterCreator.Instance.EditingCharacter.abilityScore[i].ability == ability)
                {
                    CharacterCreator.Instance.EditingCharacter.SetAbilityScore(CharacterCreator.Instance.EditingCharacter, CharacterCreator.Instance.EditingCharacter.abilityScore[i].ability, m_currentScore);
                    m_abilityValue.text = CharacterCreator.Instance.EditingCharacter.abilityScore[i].score.ToString();

                    m_modifierValue.text = CharacterCreator.Instance.EditingCharacter.abilityScore[i].modifier.ToString();
                }
            }

            OnPointsChanged?.Invoke();
        }

        public void UpdateUIAttributeDropdown()
        {
            m_attributeDrop.ClearOptions();
            m_score.Clear();

            m_attributeDrop.AddOptions(new List<TMP_Dropdown.OptionData>
            {
                new TMP_Dropdown.OptionData() {text = " "},
            });

            m_score.Add(0);

            for (int i = 0; i < CharacterAttributeEditor.Instance.m_attributesValue.Count; i++)
            {
                if (CharacterAttributeEditor.Instance.m_attributesValue[i].attributeValue == 0)
                {
                    m_score.Add(CharacterAttributeEditor.Instance.m_attributesValue[i].attributeValue);
                    m_attributeDrop.AddOptions(new List<TMP_Dropdown.OptionData>
                    {
                        new TMP_Dropdown.OptionData() {text = " " + CharacterAttributeEditor.Instance.m_attributesValue[i].attributeValue.ToString() + " ("+ "Cost: " + CharacterAttributeEditor.Instance.m_attributesValue[i].cost + ") "},
                    });
                }
                if (!CharacterAttributeEditor.Instance.m_attributesValue[i].inUse && CharacterAttributeEditor.Instance.m_attributesValue[i].attributeValue != 0 && CharacterAttributeEditor.Instance.AvailablePoints - CharacterAttributeEditor.Instance.m_attributesValue[i].cost >= 0)
                {
                    m_score.Add(CharacterAttributeEditor.Instance.m_attributesValue[i].attributeValue);
                    m_attributeDrop.AddOptions(new List<TMP_Dropdown.OptionData> 
                    {
                        new TMP_Dropdown.OptionData() {text = " " + CharacterAttributeEditor.Instance.m_attributesValue[i].attributeValue.ToString() + " ("+ "Cost: " + CharacterAttributeEditor.Instance.m_attributesValue[i].cost + ") "},
                    });
                }
            }

            m_attributeDrop.captionText.text = m_values.Find(x => x.inUse).attributeValue.ToString();
        }
    }
}
