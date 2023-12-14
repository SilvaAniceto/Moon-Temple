using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace CustomRPGSystem
{
    public class UISlotPref : MonoBehaviour
    {
        [SerializeField] public TMP_Text m_magicTier;
        [SerializeField] public GameObject m_slot, m_slotText;
        [SerializeField] public List<GameObject> m_availableSlots = new List<GameObject>();
        [SerializeField] public Transform m_slotHolder;
    }
}
