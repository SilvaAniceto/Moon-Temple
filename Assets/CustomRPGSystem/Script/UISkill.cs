using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
using System;

namespace CustomRPGSystem
{
    public class UISkill : MonoBehaviour
    {
        [SerializeField] private Toggle m_skillToggle;
        [SerializeField] private TMP_Text m_skillDescription;
        [SerializeField] private Sprite m_toggleChangable;
        [SerializeField] private Sprite m_toggleUnchangable;
        [SerializeField] private Image m_background;

        [HideInInspector] public UnityEvent<bool> OnProficiencySet = new UnityEvent<bool>();

        public void SetUISkill(PlayerCharacterData.Skills skill, bool isProficient, bool isChangable, bool hasAvailablePoints)
        {
            m_skillToggle.onValueChanged.RemoveAllListeners();

            m_skillDescription.text = skill.skill.ToString();

            if (!hasAvailablePoints)
            {
                if (isProficient)
                {
                    m_skillToggle.interactable = true;
                    m_background.sprite = m_toggleChangable;

                    m_skillToggle.isOn = isProficient;
                }
                else
                {
                    m_skillToggle.interactable = false;
                    m_background.sprite = m_toggleUnchangable;

                    m_skillToggle.isOn = isProficient;
                }
            }
            else
            {
                if (isChangable)
                {
                    m_skillToggle.interactable = isChangable;
                    m_background.sprite = m_toggleChangable;

                    m_skillToggle.isOn = isProficient;
                }
                else
                {
                    m_skillToggle.interactable = false;
                    m_background.sprite = m_toggleUnchangable;

                    m_skillToggle.isOn = isProficient;
                }
            }

            m_skillToggle.onValueChanged.AddListener(delegate
            {
                SetProficientSkill(skill, m_skillToggle.isOn);
            });
        }

        private void SetProficientSkill(PlayerCharacterData.Skills skill, bool isProficient)
        {
            skill.proficient = isProficient;
            OnProficiencySet?.Invoke(skill.proficient);
        }

    }
}
