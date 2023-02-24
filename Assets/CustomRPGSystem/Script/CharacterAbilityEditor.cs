using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace CustomRPGSystem
{
    public class CharacterAbilityEditor : MonoBehaviour
    {
        [SerializeField] private TMP_Text m_name;
        [SerializeField] private TMP_Text m_level;
        [SerializeField] private TMP_Text m_race;
        [SerializeField] private TMP_Text m_class;
        [SerializeField] private TMP_Text m_availablePoints;
        public Button editSkills;

        [SerializeField] private List<UIAbilityScore> m_UIAbility = new List<UIAbilityScore>();

        #region PROPERTIES
        public int AvailablePoints
        {
            get
            {
                return CharacterCreator.CharacterData.info.abilityPoints;
            }
        }
        public bool HasAvailablePoints
        {
            get
            {
                return AvailablePoints > 0 ? true : false;
            }
        }
        public List<UIAbilityScore> Ability
        {
            get
            {
                return m_UIAbility;
            }
        }
        #endregion

        private void Awake()
        {
            m_availablePoints.text = CharacterCreator.CharacterData.info.abilityPoints.ToString();

            for (int i = 0; i < CharacterCreator.CharacterData.abilityScore.Length; i++)
            {
                m_UIAbility[i].SetUIAbilityScore(CharacterCreator.CharacterData.abilityScore[i].ability,
                                                 CharacterCreator.CharacterData.abilityScore[i].score,
                                                 HasAvailablePoints);
                m_UIAbility[i].m_plusButton.onClick.AddListener(UpdateUIPoints);
                m_UIAbility[i].m_minusButton.onClick.AddListener(UpdateUIPoints);
            }

            UpdateUIPoints();
        }

        private void Start()
        {
            m_name.text = CharacterCreator.CharacterData.info.name;
            m_level.text = CharacterCreator.CharacterData.info.level.ToString();
            m_race.text = CharacterCreator.CharacterData.info.race.ToString();
            m_class.text = CharacterCreator.CharacterData.info.classes.ToString();
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
                else if(!HasAvailablePoints && uIAbility.CurrentScore == uIAbility.StandardScore)
                {
                    uIAbility.m_plusButton.gameObject.SetActive(false);
                    uIAbility.m_minusButton.gameObject.SetActive(false);
                }
                else if(HasAvailablePoints && uIAbility.CurrentScore == uIAbility.StandardScore)
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

            m_availablePoints.text = CharacterCreator.CharacterData.info.abilityPoints.ToString();
        }
    }
}
