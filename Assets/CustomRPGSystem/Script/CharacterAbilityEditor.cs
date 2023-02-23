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

        [SerializeField] public List<UIAbilityScore> m_UIAbility = new List<UIAbilityScore>();

        private bool m_hasAvailablePoints = false;
        private int m_startingPoins = 0;
        public static int m_spentPoints = 0;

        #region PROPERTIES
        public int AvailablePoints
        {
            get
            {
                return CharacterCreator.CharacterData.info.abilityPoints;
            }
        }
        #endregion
        private void Awake()
        {
            m_hasAvailablePoints = AvailablePoints > 0 ? true : false;

            m_startingPoins = CharacterCreator.CharacterData.info.abilityPoints;
            m_availablePoints.text = CharacterCreator.CharacterData.info.abilityPoints.ToString();

            for (int i = 0; i < CharacterCreator.CharacterData.abilityScore.Length; i++)
            {
                m_UIAbility[i].SetUIAbilityScore(CharacterCreator.CharacterData.abilityScore[i].ability,
                                                 CharacterCreator.CharacterData.abilityScore[i].score.ToString(),
                                                 CharacterCreator.CharacterData.abilityScore[i].modifier.ToString(),
                                                 m_hasAvailablePoints);
                m_UIAbility[i].m_plusButton.onClick.AddListener(UpdatePoints);
                m_UIAbility[i].m_minusButton.onClick.AddListener(UpdatePoints);
            }

            UpdatePoints();
        }

        private void Start()
        {
            m_name.text = CharacterCreator.CharacterData.info.name;
            m_level.text = CharacterCreator.CharacterData.info.level.ToString();
            m_race.text = CharacterCreator.CharacterData.info.race.ToString();
            m_class.text = CharacterCreator.CharacterData.info.classes.ToString();
        }

        private void UpdatePoints()
        {
            if (m_spentPoints == 0)
            {
                foreach (UIAbilityScore uIAbility in m_UIAbility)
                {
                    uIAbility.m_minusButton.gameObject.SetActive(false);
                }
            }
            else
            {
                foreach (UIAbilityScore uIAbility in m_UIAbility)
                {
                    uIAbility.m_minusButton.gameObject.SetActive(true);
                }
            }

            if (m_spentPoints == m_startingPoins)
            {
                foreach (UIAbilityScore uIAbility in m_UIAbility)
                {
                    uIAbility.m_plusButton.gameObject.SetActive(false);
                }
            }
            else
            {
                foreach (UIAbilityScore uIAbility in m_UIAbility)
                {
                    uIAbility.m_plusButton.gameObject.SetActive(true);
                }
            }

            m_availablePoints.text = CharacterCreator.CharacterData.info.abilityPoints.ToString();
        }
    }
}
