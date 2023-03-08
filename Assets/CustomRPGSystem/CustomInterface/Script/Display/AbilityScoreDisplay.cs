using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace CustomRPGSystem
{
    public class AbilityScoreDisplay : MonoBehaviour
    {
        [SerializeField] private TMP_Text m_ability;
        [SerializeField] private TMP_Text m_score;
        [SerializeField] private TMP_Text m_modifier;

        public void SetAbilityDisplay(string ability, int score, int modifier)
        {
            m_ability.text = ability;
            m_score.text = score.ToString();
            m_modifier.text = modifier.ToString();
        }
        
    }
}
