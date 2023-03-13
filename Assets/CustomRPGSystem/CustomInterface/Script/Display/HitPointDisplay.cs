using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace CustomRPGSystem
{
    public class HitPointDisplay : MonoBehaviour
    {
        [SerializeField] private TMP_Text m_currentPoints, m_maxPoints, m_hitDie; 

        public void SetHitPointsDisplay(PlayerCharacterData player)
        {
            m_currentPoints.text = player.info.currentHitPoints.ToString();
            m_maxPoints.text = player.info.maxHitPoints.ToString();
            m_hitDie.text = player.info.dice.ToString() + "d" + player.info.hitDie.ToString();
        }
    }
}
