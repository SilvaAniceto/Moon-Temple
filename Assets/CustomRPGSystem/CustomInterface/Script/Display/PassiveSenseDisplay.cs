using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace CustomRPGSystem
{
    public class PassiveSenseDisplay : MonoBehaviour
    {
        [SerializeField] private TMP_Text m_value;
        
        public void SetPassiveSenseDisplay(int modifier, int bonus, bool proficient)
        {
            if (proficient)
            {
                m_value.text = (10 + modifier + bonus).ToString();
            }
            else
            {
                m_value.text = (10 + modifier).ToString();
            }
        }
    }
}
