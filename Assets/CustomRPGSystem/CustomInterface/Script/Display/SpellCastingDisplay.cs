using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace CustomRPGSystem
{
    public class SpellCastingDisplay : MonoBehaviour
    {
        [SerializeField] private TMP_Text m_conjuringAbility;
        [SerializeField] private TMP_Text m_magicResistance;
        [SerializeField] private TMP_Text m_magicAttackModifier;
        
        public void SetSpellCastingDisplay(PlayerCharacterData.SpellCasting spellCasting)
        {
            m_conjuringAbility.text = spellCasting.conjuringAbility.ToString();
            m_magicResistance.text = spellCasting.magicResistance.ToString();
            m_magicAttackModifier.text = spellCasting.magicAttackModifier.ToString();
        }
    }
}
