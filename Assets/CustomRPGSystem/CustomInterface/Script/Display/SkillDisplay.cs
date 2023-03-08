using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace CustomRPGSystem
{
    public class SkillDisplay : MonoBehaviour
    {
        [SerializeField] private Sprite m_proficientSkill;
        [SerializeField] private Sprite m_nonProficientSkill;

        [SerializeField] private TMP_Text m_skillDescription;
        [SerializeField] private TMP_Text m_skillBonus;

        private int m_skillTotalBonus;

        void Start()
        {
        
        }

        void Update()
        {
        
        }
    }
}
