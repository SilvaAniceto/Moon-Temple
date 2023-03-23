using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace CustomRPGSystem
{
    public class CharacterExtraPointEditor : MonoBehaviour
    {
        public static CharacterExtraPointEditor Instance;

        [SerializeField] private TMP_Text m_name;
        [SerializeField] private TMP_Text m_level;
        [SerializeField] private TMP_Text m_race;
        [SerializeField] private TMP_Text m_class;

        [SerializeField] private List<UIExtraAttributeScore> m_UIExtraAttributes = new List<UIExtraAttributeScore>();
        [SerializeField] private TMP_Text m_extraPointsText;

        private int m_currentExtraPoints;

        #region PROPERTIES
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
        public bool HasExtraPoints
        {
            get
            {
                return ExtraPoints > 0 ? true : false;
            }
        }
        #endregion

        private void OnEnable()
        {
            m_extraPointsText.text = m_currentExtraPoints.ToString();

            for (int i = 0; i < m_UIExtraAttributes.Count; i++)
            {
                m_UIExtraAttributes[i].OnPointsChanged.RemoveAllListeners();

                m_UIExtraAttributes[i].OnPointsChanged.AddListener(UpdateCurrentExtraPoints);
            }

            UpdateUIPoints();
            UpdateUIText();
        }

        public void SetExtraPointEditor()
        {
            for (int i = 0; i < CharacterCreator.Instance.EditingCharacter.abilityScore.Length; i++)
            {
                ExtraAttributePoints[i].SetUIExtraAttributeScore(CharacterCreator.Instance.EditingCharacter.abilityScore[i].ability, CharacterCreator.Instance.EditingCharacter.abilityScore[i].score, HasExtraPoints);
            }
        }

        private void UpdateUIPoints()
        {
            foreach (UIExtraAttributeScore uIAbility in m_UIExtraAttributes)
            {
                if (!HasExtraPoints && uIAbility.CurrentScore > uIAbility.StandardScore)
                {
                    uIAbility.m_plusButton.gameObject.SetActive(false);
                    uIAbility.m_minusButton.gameObject.SetActive(true);
                }
                else if (!HasExtraPoints && uIAbility.CurrentScore == uIAbility.StandardScore)
                {
                    uIAbility.m_plusButton.gameObject.SetActive(false);
                    uIAbility.m_minusButton.gameObject.SetActive(false);
                }
                else if (HasExtraPoints && uIAbility.CurrentScore == uIAbility.StandardScore)
                {
                    uIAbility.m_plusButton.gameObject.SetActive(true);
                    uIAbility.m_minusButton.gameObject.SetActive(false);
                }
                else if (HasExtraPoints && uIAbility.CurrentScore > uIAbility.StandardScore)
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
