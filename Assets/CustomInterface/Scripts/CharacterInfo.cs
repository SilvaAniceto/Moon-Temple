using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace CustomInterface
{
    public class CharacterInfo : MonoBehaviour
    {
        public class CharInfo
        {
            public string m_name = "";
            public int m_level;
            public int m_maxHP;
            public int m_currentHP;
            public int m_armouClass;
            public int m_inspirationPoints;
            public int m_proficienceBonus;
            public int m_initiative;
            public int m_speed;

            public UnityEvent<int> onProficienceChanged = new UnityEvent<int>();
        }


        [Header("Information")]
        [SerializeField] private Text m_name;
        [SerializeField] private Text m_level;
        [SerializeField] private UIButton m_lvlButton;
        [SerializeField] private Text m_maxHPValue;
        [SerializeField] private Text m_currentHPValue;
        [Header("Armour Class")]
        [SerializeField] private Text m_acValue;
        [Header("Bonus/Points")]
        [SerializeField] private Text m_inspirationValue;
        [SerializeField] private UIButton m_inspirationButton;
        [SerializeField] private Text m_proficienceValue;
        [SerializeField] private Text m_initiativeValue;
        [SerializeField] private Text m_speedValue;
        [SerializeField] private UIButton m_speedButton;

        [HideInInspector] public CharInfo m_infos = new CharInfo();
        private AbilityScore dexterity = new AbilityScore();

        // Start is called before the first frame update
        void Start()
        {
            dexterity = CharacterSheet.Instance.abilities.Find(x => x.m_values.m_id == AbilityScore.Ability.Dexterity.ToString().Trim().ToLower());

            dexterity.m_values.onScoreChanged.AddListener(SetInitiative);

            m_infos.onProficienceChanged.AddListener(SetInitiative);

            m_lvlButton.m_increaseButton.onClick.AddListener(AddLevel);
            m_lvlButton.m_decreaseButton.onClick.AddListener(RemoveLevel);

            m_speedButton.m_increaseButton.onClick.AddListener(AddSpeed);
            m_speedButton.m_decreaseButton.onClick.AddListener(RemoveSpeed);

            m_inspirationButton.m_increaseButton.onClick.AddListener(AddInspiration);
            m_inspirationButton.m_decreaseButton.onClick.AddListener(RemoveInspiration);
        }

        // Update is called once per frame
        void Update()
        {
            

            
        }

        void SetInitiative(int p_value)
        {
            m_infos.m_initiative = p_value + CharacterSheet.Instance.ProficienceBonus;

            m_initiativeValue.text = m_infos.m_initiative.ToString();
        }

        void AddLevel()
        {
            m_infos.m_level++;
            m_level.text = m_infos.m_level.ToString();
            SetProficiencePoints(m_infos.m_level);
        }

        void RemoveLevel()
        {
            m_infos.m_level--;
            m_level.text = m_infos.m_level.ToString();
            SetProficiencePoints(m_infos.m_level);
        }

        void SetProficiencePoints(int p_points)
        {
            if (p_points >= 1 && p_points < 5)
                m_infos.m_proficienceBonus = 2;
            if (p_points >= 5 && p_points < 8)
                m_infos.m_proficienceBonus = 3;
            if (p_points >= 8 && p_points < 13)
                m_infos.m_proficienceBonus = 4;
            if (p_points >= 13 && p_points < 17)
                m_infos.m_proficienceBonus = 5;
            if (p_points >= 17 && p_points < 21)
                m_infos.m_proficienceBonus = 6;

            m_infos.onProficienceChanged?.Invoke(m_infos.m_proficienceBonus);

            m_proficienceValue.text = m_infos.m_proficienceBonus.ToString();
        }

        void AddSpeed()
        {
            m_infos.m_speed++;
            m_speedValue.text = m_infos.m_speed.ToString();
        }

        void RemoveSpeed()
        {
            m_infos.m_speed--;
            m_speedValue.text = m_infos.m_speed.ToString();
        }

        void AddInspiration()
        {
            m_infos.m_inspirationPoints++;
            m_inspirationValue.text = m_infos.m_inspirationPoints.ToString();
        }

        void RemoveInspiration()
        {
            m_infos.m_inspirationPoints--;
            m_inspirationValue.text = m_infos.m_inspirationPoints.ToString();
        }
    }
}
