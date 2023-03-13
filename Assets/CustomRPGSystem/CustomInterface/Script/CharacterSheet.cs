using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace CustomRPGSystem
{
    public class CharacterSheet : MonoBehaviour
    {
        [Header("Character Identification")]
        [SerializeField] private TMP_Text m_nameText;
        [SerializeField] private TMP_Text m_raceText, m_classText;

        [Header("Character Hit Points")]
        [SerializeField] private HitPointDisplay m_hitPointDisplay;

        [Header("Character Info Values")]
        [SerializeField] private TMP_Text m_levelText;
        [SerializeField] private TMP_Text m_proficiencyBonusText, m_initiativeText, m_armorClassText, m_speedText;

        [Header("Abilities Score")]
        [SerializeField] private List<AbilityScoreDisplay> m_abilityScoreDisplays = new List<AbilityScoreDisplay>();

        [Header("Saving Throws")]
        [SerializeField] private List<SavingThrowDisplay> m_savingThrowDisplays = new List<SavingThrowDisplay>();

        [Header("Passive Sense")]
        [SerializeField] private PassiveSenseDisplay m_perception;
        [SerializeField] private PassiveSenseDisplay m_insight;
        [SerializeField] private PassiveSenseDisplay m_investigation;

        [Header("Skills")]
        [SerializeField] private List<SkillDisplay> m_skillDisplay = new List<SkillDisplay>();

        #region PROPERTIES

        #endregion

        void Start()
        {
        
        }

        void Update()
        {
        
        }

        private void OnEnable()
        {
            SetInfoSheet();
        }

        public void SetInfoSheet()
        {
            m_nameText.text = CharacterCreator.Instance.EditingCharacter.info.name;
            m_raceText.text = CharacterCreator.Instance.EditingCharacter.info.race.ToString();
            m_classText.text = CharacterCreator.Instance.EditingCharacter.info.classes.ToString();

            m_hitPointDisplay.SetHitPointsDisplay(CharacterCreator.Instance.EditingCharacter);

            m_levelText.text = CharacterCreator.Instance.EditingCharacter.info.level.ToString();
            m_proficiencyBonusText.text = CharacterCreator.Instance.EditingCharacter.info.proficiencyBonus.ToString();
            m_speedText.text = CharacterCreator.Instance.EditingCharacter.info.speed.ToString();

            for (int i = 0; i < CharacterCreator.Instance.EditingCharacter.abilityScore.Length; i++)
            {
                m_abilityScoreDisplays[i].SetAbilityDisplay(CharacterCreator.Instance.EditingCharacter.abilityScore[i].ability.ToString(),
                                                            CharacterCreator.Instance.EditingCharacter.abilityScore[i].score,
                                                            CharacterCreator.Instance.EditingCharacter.abilityScore[i].modifier);

                m_savingThrowDisplays[i].SetSavingThrowDisplay(CharacterCreator.Instance.EditingCharacter.abilityScore[i].ability.ToString(),
                                                               CharacterCreator.Instance.EditingCharacter.info.proficiencyBonus,
                                                               CharacterCreator.Instance.EditingCharacter.abilityScore[i].modifier,
                                                               CharacterCreator.Instance.EditingCharacter.abilityScore[i].savingThrows);


                if (CharacterCreator.Instance.EditingCharacter.abilityScore[i].ability == PlayerCharacterData.AbilityScore.Ability.Dexterity)
                {
                    CharacterCreator.Instance.EditingCharacter.info.initiative = CharacterCreator.Instance.EditingCharacter.abilityScore[i].modifier;

                    m_initiativeText.text = CharacterCreator.Instance.EditingCharacter.info.initiative.ToString();

                    m_armorClassText.text = (CharacterCreator.Instance.EditingCharacter.abilityScore[i].modifier + CharacterCreator.Instance.EditingCharacter.info.armorClass).ToString();
                }

                if (CharacterCreator.Instance.EditingCharacter.abilityScore[i].ability == PlayerCharacterData.AbilityScore.Ability.Wisdom)
                {
                    for (int j = 0; j < CharacterCreator.Instance.EditingCharacter.skills.Length; j++)
                    {
                        if (CharacterCreator.Instance.EditingCharacter.skills[j].skill == PlayerCharacterData.Skills.Skill.Perception)
                        {
                            m_perception.SetPassiveSenseDisplay(CharacterCreator.Instance.EditingCharacter.abilityScore[i].modifier,
                                                                CharacterCreator.Instance.EditingCharacter.info.proficiencyBonus);
                        }

                        if (CharacterCreator.Instance.EditingCharacter.skills[j].skill == PlayerCharacterData.Skills.Skill.Insight)
                        {
                            m_insight.SetPassiveSenseDisplay(CharacterCreator.Instance.EditingCharacter.abilityScore[i].modifier,
                                                                CharacterCreator.Instance.EditingCharacter.info.proficiencyBonus);
                        }
                    }
                }

                if (CharacterCreator.Instance.EditingCharacter.abilityScore[i].ability == PlayerCharacterData.AbilityScore.Ability.Intelligence)
                {
                    for (int j = 0; j < CharacterCreator.Instance.EditingCharacter.skills.Length; j++)
                    {
                        if (CharacterCreator.Instance.EditingCharacter.skills[j].skill == PlayerCharacterData.Skills.Skill.Investigation)
                        {
                            m_investigation.SetPassiveSenseDisplay(CharacterCreator.Instance.EditingCharacter.abilityScore[i].modifier,
                                                                CharacterCreator.Instance.EditingCharacter.info.proficiencyBonus);
                        }
                    }
                }
            } 
        }
        public void SetSkillSheet()
        {
            
        }
    }
}
