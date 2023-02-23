using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace CustomRPGSystem
{
    public class UIAbilityScore : MonoBehaviour
    {
        public TMP_Text m_abilityDescription;
        public TMP_Text m_abilityValue;
        public TMP_Text m_abilityModifier;
        public Button m_minusButton;
        public Button m_plusButton;
        public PlayerCharacterData.AbilityScore.Ability m_ability = PlayerCharacterData.AbilityScore.Ability.Strenght;

        public void SetUIAbilityScore(PlayerCharacterData.AbilityScore.Ability ability, string value, string modifier, bool hasPoint)
        {
            m_ability = ability;
            m_abilityDescription.text = ability.ToString();
            m_abilityValue.text = value;
            m_abilityModifier.text = modifier;

            m_minusButton.onClick.AddListener(delegate 
            {
                SubtractPoints(m_ability);
            });
            m_plusButton.onClick.AddListener(delegate
            {
                AddPoints(m_ability);
            });

            if (hasPoint)
            {
                m_minusButton.gameObject.SetActive(true);
                m_plusButton.gameObject.SetActive(true);
            }
            else
            {
                m_minusButton.gameObject.SetActive(false);
                m_plusButton.gameObject.SetActive(false);
            }
        }

        public void AddPoints(PlayerCharacterData.AbilityScore.Ability ability)
        {
            CharacterAbilityEditor.m_spentPoints++;

            for (int i = 0; i < CharacterCreator.CharacterData.abilityScore.Length; i++)
            {
                if (CharacterCreator.CharacterData.abilityScore[i].ability == ability)
                {
                    int value = CharacterCreator.CharacterData.abilityScore[i].score + 1;
                    CharacterCreator.CharacterData.SetAbilityScore(CharacterCreator.CharacterData, ability, value);

                    m_abilityValue.text = CharacterCreator.CharacterData.abilityScore[i].score.ToString();
                    m_abilityModifier.text = CharacterCreator.CharacterData.abilityScore[i].modifier.ToString();
                    
                }
            }

            CharacterCreator.CharacterData.info.abilityPoints--;
        }

        public void SubtractPoints(PlayerCharacterData.AbilityScore.Ability ability)
        {
            CharacterAbilityEditor.m_spentPoints--;

            for (int i = 0; i < CharacterCreator.CharacterData.abilityScore.Length; i++)
            {
                if (CharacterCreator.CharacterData.abilityScore[i].ability == ability)
                {
                    int value = CharacterCreator.CharacterData.abilityScore[i].score - 1;
                    CharacterCreator.CharacterData.SetAbilityScore(CharacterCreator.CharacterData, ability, value);

                    m_abilityValue.text = CharacterCreator.CharacterData.abilityScore[i].score.ToString();
                    m_abilityModifier.text = CharacterCreator.CharacterData.abilityScore[i].modifier.ToString();
                }
            }

            CharacterCreator.CharacterData.info.abilityPoints++;
        }
    }
}
