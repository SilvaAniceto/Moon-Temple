using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
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
        public UnityEvent<int> OnPointsChanged = new UnityEvent<int>();

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

        public void SetUIAbilityScore(PlayerCharacterData.AbilityScore.Ability ability, int value, bool hasPoint)
        {
            m_ability = ability;
            m_abilityDescription.text = ability.ToString();

            m_standardScore = value;
            m_abilityValue.text = m_standardScore.ToString();
            m_currentScore = m_standardScore;

            m_abilityModifier.text = CharacterCreator.CharacterData.SetAbilityModifier(m_standardScore).ToString();

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
            for (int i = 0; i < CharacterCreator.CharacterData.abilityScore.Length; i++)
            {
                if (CharacterCreator.CharacterData.abilityScore[i].ability == ability)
                {
                    m_currentScore++;

                    m_abilityValue.text = m_currentScore.ToString();
                    m_abilityModifier.text = CharacterCreator.CharacterData.SetAbilityModifier(m_currentScore).ToString();
                }
            }
            OnPointsChanged?.Invoke(+1);
            //CharacterCreator.CharacterData.info.abilityPoints--;
        }

        public void SubtractPoints(PlayerCharacterData.AbilityScore.Ability ability)
        {
            for (int i = 0; i < CharacterCreator.CharacterData.abilityScore.Length; i++)
            {
                if (CharacterCreator.CharacterData.abilityScore[i].ability == ability)
                {
                    m_currentScore--;

                    m_abilityValue.text = m_currentScore.ToString();
                    m_abilityModifier.text = CharacterCreator.CharacterData.SetAbilityModifier(m_currentScore).ToString();
                }
            }
            OnPointsChanged?.Invoke(+1);
            //CharacterCreator.CharacterData.info.abilityPoints++;
        }
    }
}
