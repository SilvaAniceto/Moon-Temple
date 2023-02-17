using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CustomInterface
{
    public class ToggleSkill : MonoBehaviour
    {
        [SerializeField] public Toggle m_proficient;
        [SerializeField] public Text m_description;
        [SerializeField] public Text m_value;

        private int m_abilityModifier;

        public int AbilityModifier
        {
            get
            {
                return m_abilityModifier;
            }

            set
            {
                if (m_abilityModifier == value) return;

                m_abilityModifier = value;
            }
        }

        public bool Proficient
        {
            get
            {
                return m_proficient.isOn;
            }
        }       
    }
}
