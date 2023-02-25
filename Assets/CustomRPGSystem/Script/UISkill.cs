using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace CustomRPGSystem
{
    public class UISkill : MonoBehaviour
    {
        [SerializeField] private Toggle m_skillToggle;
        [SerializeField] private TMP_Text m_skillDescription;

        public void SetUISkill(PlayerCharacterData.Skills skill, bool isProficient, bool isChangable)
        {
            m_skillDescription.text = skill.skill.ToString();

            m_skillToggle.gameObject.SetActive(isChangable);

            m_skillToggle.isOn = isProficient;

            m_skillToggle.onValueChanged.AddListener(delegate
            {
                SetProficientSkill(skill, m_skillToggle.isOn);
            });
        }

        private void SetProficientSkill(PlayerCharacterData.Skills skill, bool isProficient)
        {
            skill.proficient = isProficient;
        }

    }
}
