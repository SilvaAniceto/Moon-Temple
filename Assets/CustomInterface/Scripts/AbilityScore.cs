using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace CustomInterface
{
    public class AbilityScore : MonoBehaviour
    {
        [System.Serializable]
        public class Skills
        {
            [HideInInspector] public string m_id = "";
            public bool m_proficient = false;
            public string m_description;
        }

        [System.Serializable]
        public class Abilities
        {
            [HideInInspector] public string m_id = "";
            public int m_abilityScore;
            public int m_abilityModifier;

            public UnityEvent<int> onScoreChanged = new UnityEvent<int>();
        }

        public enum Ability
        {
            Strenght,
            Dexterity,
            Constitution,
            Intelligence,
            Wisdom,
            Charisma
        }

        [Header("Definições de Habilidades")]
        [SerializeField] private Ability m_ability = Ability.Strenght;
        [SerializeField] public Abilities m_values = new Abilities();
        [SerializeField] private List<Skills> m_skill = new List<Skills>();
        [Header("Interface da Habilidade")]
        [SerializeField] private Toggle m_abilityToggle;
        [SerializeField] private Text m_title;
        [SerializeField] private Text m_Score;
        [SerializeField] private Text m_modifierValue;
        [SerializeField] private ToggleSkill m_prefSkill;
        [SerializeField] private Transform m_skillPanel;
        [SerializeField] private UIButton m_uiButton;

        private int abilityValue;
        private int modifierValue;
        private List<ToggleSkill> skillList = new List<ToggleSkill>();

        private void Awake()
        {
            m_abilityToggle.onValueChanged.AddListener(delegate
            {
                SetSkillPanel(m_abilityToggle.isOn);
            });

            m_uiButton.m_increaseButton.onClick.AddListener(delegate
            {
                AddScore();
            });
            m_uiButton.m_decreaseButton.onClick.AddListener(delegate 
            {
               RemoveScore();
            });

            for (int i = 0; i < m_skill.Count; i++)
            {
                m_skill[i].m_id = m_ability.ToString().Trim().ToLower();
                m_values.m_id = m_skill[i].m_id;
            }

        }

        // Start is called before the first frame update
        void Start()
        {
            SetAbilityScore();            

            m_title.text = m_ability.ToString();

            SetSkillPanel(m_abilityToggle.isOn);

            CharacterSheet.Instance.infos.m_infos.onProficienceChanged.AddListener(delegate
            {
                SetModifierValue(abilityValue);
            });
        }

        void AddScore()
        {
            m_values.m_abilityScore++;
            abilityValue = m_values.m_abilityScore;
            SetAbilityScore();
        }

        void RemoveScore()
        {
            if (m_values.m_abilityScore == 0) return;

            m_values.m_abilityScore--;
            abilityValue = m_values.m_abilityScore;
            SetAbilityScore();
        }

        void SetAbilityScore()
        {
            m_Score.text = abilityValue.ToString();
            SetModifierValue(abilityValue);
        }

        public void SetModifierValue(int p_value)
        {
            if (p_value > 30)
            {
                modifierValue = 10;
                m_modifierValue.text = (modifierValue + CharacterSheet.Instance.ProficienceBonus).ToString(); 
            }

            switch (p_value)
            {
                case 1:
                    modifierValue = -5;
                break;
                case 2:
                    modifierValue = -4;
                break;
                case 3:
                    modifierValue = -4;
                break;
                case 4:
                    modifierValue = -3;
                    break;
                case 5:
                    modifierValue = -3;
                    break;
                case 6:
                    modifierValue = -2;
                    break;
                case 7:
                    modifierValue = -2;
                    break;
                case 8:
                    modifierValue = -1;
                    break;
                case 9:
                    modifierValue = -1;
                    break;
                case 10:
                    modifierValue = 0;
                    break;
                case 11:
                    modifierValue = 0;
                    break;
                case 12:
                    modifierValue = 1;
                    break;
                case 13:
                    modifierValue = 1;
                    break;
                case 14:
                    modifierValue = 2;
                    break;
                case 15:
                    modifierValue = 2;
                    break;
                case 16:
                    modifierValue = 3;
                    break;
                case 17:
                    modifierValue = 3;
                    break;
                case 18:
                    modifierValue = 4;
                    break;
                case 19:
                    modifierValue = 4;
                    break;
                case 20:
                    modifierValue = 5;
                    break;
                case 21:
                    modifierValue = 5;
                    break;
                case 22:
                    modifierValue = 6;
                    break;
                case 23:
                    modifierValue = 6;
                    break;
                case 24:
                    modifierValue = 7;
                    break;
                case 25:
                    modifierValue = 7;
                    break;
                case 26:
                    modifierValue = 8;
                    break;
                case 27:
                    modifierValue = 8;
                    break;
                case 28:
                    modifierValue = 9;
                    break;
                case 29:
                    modifierValue = 9;
                    break;
                case 30:
                    modifierValue = 10;
                    break;
            }

            m_values.m_abilityModifier = modifierValue;
            m_modifierValue.text = (modifierValue + CharacterSheet.Instance.ProficienceBonus).ToString();

            m_values.onScoreChanged?.Invoke(modifierValue + CharacterSheet.Instance.ProficienceBonus);
        }

        void SetSkillScore(bool p_newPref)
        {
            if (p_newPref)
            {
                for (int i = 0; i < m_skill.Count; i++)
                {
                    ToggleSkill ts = Instantiate(m_prefSkill, m_skillPanel);
                    ts.gameObject.SetActive(true);
                    ts.m_proficient.isOn = m_skill[i].m_proficient;
                    ts.m_description.text = m_skill[i].m_description;
                    ts.AbilityModifier = modifierValue + CharacterSheet.Instance.ProficienceBonus;

                    CalculateSkillBonus(ts.AbilityModifier, ts.m_value, ts.m_proficient.isOn);

                    ts.m_proficient.onValueChanged.AddListener(delegate
                    {
                        CalculateSkillBonus(ts.AbilityModifier, ts.m_value, ts.m_proficient.isOn);
                    });

                    m_skill[i].m_id = m_ability.ToString().Trim().ToLower();
                    m_values.m_id = m_skill[i].m_id;

                    skillList.Add(ts);
                }
            }
            else
            {
                for (int i = 0; i < skillList.Count; i++)
                {
                    skillList[i].AbilityModifier = modifierValue + CharacterSheet.Instance.ProficienceBonus;              
                    
                    CalculateSkillBonus(skillList[i].AbilityModifier, skillList[i].m_value, skillList[i].m_proficient.isOn);
                }
            }
        }

        void CalculateSkillBonus(int p_skillValue, Text p_text, bool p_plus)
        {
            if (p_plus)
                p_text.text = (p_skillValue + CharacterSheet.Instance.ProficienceBonus).ToString();
            else
                p_text.text = p_skillValue.ToString();


            for (int i = 0; i < skillList.Count; i++)
            {
                Skills s = m_skill.Find(x => x.m_description.Trim().ToLower() == skillList[i].m_description.text.Trim().ToLower());

                s.m_proficient = skillList[i].m_proficient.isOn;                
            }
        }

        void SetSkillPanel(bool p_value)
        {
            if (p_value)
                SetSkillScore(p_value);
            else
            {
                for (int i = 0; i < skillList.Count; i++)
                {
                    Destroy(skillList[i].gameObject);                    
                }
                skillList.Clear();
            }

            m_skillPanel.gameObject.SetActive(p_value);
        }
    }
}
