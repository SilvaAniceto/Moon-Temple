using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace CustomRPGSystem
{
    public class UIPlayerPref : MonoBehaviour
    {
        [SerializeField] private Sprite m_toggleOn, m_toggleOff;
        [SerializeField] private Toggle m_toggleSelect;
        [SerializeField] private TMP_Text m_name, m_class, m_level;

        public void SetPlayerPref(string p_name, string p_class, string p_level)
        {
            m_name.text = p_name;
            m_class.text = p_class;
            m_level.text = p_level;

            m_toggleSelect.onValueChanged.AddListener(delegate
            {
                SetToggleGraphic();
            });

            SetToggleGraphic();
        }

        private void SetToggleGraphic()
        {
            if (m_toggleSelect.isOn)
            {
                m_toggleSelect.targetGraphic.GetComponent<Image>().sprite = m_toggleOn;
            }
            else
            {
                m_toggleSelect.targetGraphic.GetComponent<Image>().sprite = m_toggleOff;
            }
        }
    }
}
