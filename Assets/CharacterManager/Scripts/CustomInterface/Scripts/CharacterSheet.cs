using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CustomInterface
{
    public class CharacterSheet : MonoBehaviour
    {
        public static CharacterSheet Instance;

        public List<AbilityScore> abilities = new List<AbilityScore>();
        public CharacterInfo infos = new CharacterInfo();

        [SerializeField] private int m_proficienceBonus;
        [SerializeField] private int m_initiative;
        [SerializeField] private int m_inspirationBonus;
        [SerializeField] private int m_AvailablePoints;

        public int ProficienceBonus
        {
            get
            {
                return infos.m_infos.m_proficienceBonus;
            }
        }

        void Awake()
        {
            if (Instance == null) Instance = this;
        }

        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
