using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace CustomRPGSystem
{
    public class SkillDisplay : MonoBehaviour
    {
        [SerializeField] private Sprite m_proficientSkill;
        [SerializeField] private Sprite m_nonProficientSkill;

        [SerializeField] private TMP_Text m_skillDescription;
        [SerializeField] private TMP_Text m_skillBonus;

        [SerializeField] private Image m_proficientImage;

        private int m_skillTotalBonus;

        public void SetSkillDisplay(PlayerCharacterData.Skills skill, int modifier, int bonus)
        {
            m_skillDescription.text = skill.skill.ToString();

            if (skill.proficient)
            {
                m_proficientImage.sprite = m_proficientSkill;
                m_skillTotalBonus = bonus + modifier;
            }
            else 
            {
                m_proficientImage.sprite = m_nonProficientSkill;
                m_skillTotalBonus = modifier;
            }

            m_skillBonus.text = m_skillTotalBonus.ToString();   
        }

    }
}
