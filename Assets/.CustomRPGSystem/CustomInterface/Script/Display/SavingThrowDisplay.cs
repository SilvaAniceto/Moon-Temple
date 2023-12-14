using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace CustomRPGSystem
{
    public class SavingThrowDisplay : MonoBehaviour
    {
        [SerializeField] private TMP_Text m_abilitySave;
        [SerializeField] private TMP_Text m_savingBonus;

        [SerializeField] private Image m_image;
        [SerializeField] private Color m_proficientColor, m_defaultColor;

        public void SetSavingThrowDisplay(string abilitySave, int bonus, int score, bool proficient)
        {
            if (proficient)
            {
                m_abilitySave.fontStyle = FontStyles.Bold;
                m_savingBonus.fontStyle = FontStyles.Bold;

                m_savingBonus.text = (bonus + score).ToString();

                m_image.color = m_proficientColor;
            }
            else
            {
                m_abilitySave.fontStyle = FontStyles.Normal;
                m_savingBonus.fontStyle = FontStyles.Normal;

                m_savingBonus.text = (score).ToString();

                m_image.color = m_defaultColor;
            }

            m_abilitySave.text = abilitySave;
        }
    }
}
