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
        [SerializeField] private UISlotPref m_slotPref;
        [SerializeField] private Transform m_magicSlotPanel;
        List<UISlotPref> m_prefs = new List<UISlotPref>();
        
        public void SetSpellCastingDisplay(PlayerCharacterData player)
        {
            if (m_prefs.Count != 0)
            {
                for (int i = 0; i < m_prefs.Count; i++)
                {
                    Destroy(m_prefs[i].gameObject);
                }

                m_prefs.Clear();
            }

            m_conjuringAbility.text = player.spellCasting.conjuringAbility.ToString();
            m_magicResistance.text = player.spellCasting.magicResistance.ToString();
            m_magicAttackModifier.text = player.spellCasting.magicAttackModifier.ToString();

            for (int i = 0; i < player.spellCasting.magicSlots.Count; i++)
            {
                UISlotPref pref = Instantiate(m_slotPref);
                m_prefs.Add(pref);
                pref.gameObject.SetActive(true); 
                pref.transform.SetParent(m_magicSlotPanel);
                pref.gameObject.GetComponent<RectTransform>().localScale = Vector3.one;

                if (player.info.classes == PlayerCharacterData.CharacterInfo.Class.Barbarian && player.spellCasting.magicSlots[i].tier == PlayerCharacterData.SpellCasting.MagicTier.None)
                {
                    pref.m_magicTier.text = "Rage";

                    if (player.spellCasting.magicSlots[i].currentAvailableSlots == 0)
                    {
                        pref.m_slotText.SetActive(true);
                        pref.m_slotText.GetComponent<TMP_Text>().text = "Unlimited";
                    }
                }
                else if (player.info.classes == PlayerCharacterData.CharacterInfo.Class.Monk && player.spellCasting.magicSlots[i].tier == PlayerCharacterData.SpellCasting.MagicTier.None)
                {
                    pref.m_magicTier.text = "Chi Points";
                }
                else if (player.info.classes == PlayerCharacterData.CharacterInfo.Class.Warlock && player.spellCasting.magicSlots[i].tier == PlayerCharacterData.SpellCasting.MagicTier.None)
                {
                    pref.m_magicTier.text = "Invocations";
                }
                else
                {
                    pref.m_magicTier.text = player.spellCasting.magicSlots[i].tier.ToString();
                }

                for (int j = 0; j < player.spellCasting.magicSlots[i].currentAvailableSlots; j++)
                {
                    GameObject slot = Instantiate(pref.m_slot);
                    slot.SetActive(true);
                    pref.m_availableSlots.Add(slot);
                    slot.transform.SetParent(pref.m_slotHolder);
                    slot.GetComponent<RectTransform>().localScale = Vector3.one;
                }
            }
        }
    }
}
