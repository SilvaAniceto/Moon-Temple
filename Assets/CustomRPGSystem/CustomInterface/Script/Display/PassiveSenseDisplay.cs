using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace CustomRPGSystem
{
    public class PassiveSenseDisplay : MonoBehaviour
    {
        [SerializeField] private TMP_Text m_value;
        
        public void SetPassiveSenseDisplay(int modifier, int bonus)
        {
            m_value.text = (10 + modifier + bonus).ToString();
        }
    }
}
