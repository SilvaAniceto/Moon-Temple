using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace CustomRPGSystem
{
    public class CharacterSkillEditor : MonoBehaviour
    {
        [SerializeField] private TMP_Text m_proficiencyPoints;
        [SerializeField] private TMP_Text m_race;
        [SerializeField] private TMP_Text m_class;

        public Button m_raceButton;
        public Button m_classButton;

        [SerializeField] private List<UISkill> m_UISkill = new List<UISkill>();

        void OnEnable()
        {
            m_raceButton.onClick.AddListener(delegate
            {
                ShowRaceSkill(CharacterCreator.CharacterData);
            });

            m_classButton.onClick.AddListener(delegate
            {
                ShowClassSkill(CharacterCreator.CharacterData);
            });

            UpdateUIText();
            ShowRaceSkill(CharacterCreator.CharacterData);
        }

        void ShowRaceSkill(PlayerCharacterData player)
        {
            m_proficiencyPoints.text = CharacterCreator.CharacterData.info.raceProficiencyPoints.ToString();
            for (int i = 0; i < player.raceSkills.Count; i++)
            {
                m_UISkill[i].SetUISkill(player.raceSkills[i], player.raceSkills[i].proficient, player.raceSkills[i].isChangable);
            }
        }

        void ShowClassSkill(PlayerCharacterData player)
        {
            m_proficiencyPoints.text = CharacterCreator.CharacterData.info.classProficiencyPoints.ToString();
            for (int i = 0; i < player.classSkills.Count; i++)
            {
                m_UISkill[i].SetUISkill(player.classSkills[i], player.classSkills[i].proficient, player.classSkills[i].isChangable);
            }
        }

        private void UpdateUIText()
        {
            m_race.text = CharacterCreator.CharacterData.info.race.ToString();
            m_class.text = CharacterCreator.CharacterData.info.classes.ToString();
        }
    }
}
