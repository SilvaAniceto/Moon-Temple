using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

namespace CustomRPGSystem
{
    public class CharacterSheet : MonoBehaviour
    {
        public static CharacterSheet Instance;

        [Header("Panels")]
        [SerializeField] private GameObject m_attributePanel;
        [SerializeField] private GameObject m_skillPanel;
        [SerializeField] private GameObject m_spellcastPanel;

        [Header("Panel Buttons")]
        [SerializeField] private Button m_skillButton;
        [SerializeField] private Button m_attributeButton;
        [SerializeField] private Button m_spellcastButton;

        [Header("Character Identification")]
        [SerializeField] private TMP_Text m_nameText;
        [SerializeField] private TMP_Text m_raceText, m_classText;

        [Header("Character Hit Points")]
        [SerializeField] private HitPointDisplay m_hitPointDisplay;

        [Header("Character Info Values")]
        [SerializeField] private TMP_Text m_levelText;
        [SerializeField] private TMP_Text m_inspirationText, m_proficiencyBonusText, m_initiativeText, m_armorClassText, m_speedText;

        [Header("Abilities Score")]
        [SerializeField] private List<AbilityScoreDisplay> m_abilityScoreDisplays = new List<AbilityScoreDisplay>();

        [Header("Saving Throws")]
        [SerializeField] private List<SavingThrowDisplay> m_savingThrowDisplays = new List<SavingThrowDisplay>();

        [Header("Passive Sense")]
        [SerializeField] private PassiveSenseDisplay m_perception;
        [SerializeField] private PassiveSenseDisplay m_insight;
        [SerializeField] private PassiveSenseDisplay m_investigation;

        [Header("Skills")]
        [SerializeField] private List<SkillDisplay> m_skillDisplay = new List<SkillDisplay>();

        [Header("SpellCasting")]
        [SerializeField] private SpellCastingDisplay m_spellCastingDisplay;

        private bool m_isEditing = true;
        private PlayerCharacterData m_currentCharacter;
        private List<PlayerCharacterData.Skills> m_editingSkills = new List<PlayerCharacterData.Skills>();

        #region PROPERTIES
        public bool IsEditing
        {
            set 
            {
                if (value == m_isEditing) return;

                m_isEditing = value;
            }
        }
        public PlayerCharacterData CurrentCharacter
        {
            get
            {
                if (m_isEditing) return CharacterCreator.Instance.EditingCharacter;
                else return m_currentCharacter;
            }
            set
            {
                m_currentCharacter = value;
            }
        }
        #endregion

        void Awake()
        {
            m_skillButton.onClick.AddListener(delegate
            {
                m_attributePanel.SetActive(false);
                m_skillPanel.SetActive(true);
                m_spellcastPanel.SetActive(false);

                PanelButtonHandler();
            });

            m_attributeButton.onClick.AddListener(delegate
            {
                m_attributePanel.SetActive(true);
                m_skillPanel.SetActive(false);
                m_spellcastPanel.SetActive(false);

                PanelButtonHandler();
            });

            m_spellcastButton.onClick.AddListener(delegate
            {
                m_attributePanel.SetActive(false);
                m_skillPanel.SetActive(false);
                m_spellcastPanel.SetActive(true);

                PanelButtonHandler();
            });
        }

        private void OnEnable()
        {
            ConfigureCharacterSheetPage();

            CurrentCharacter.SetSpellCasting(CurrentCharacter, CurrentCharacter.info.level);
            CurrentCharacter.SetHitPoints(CurrentCharacter);

            PrepareSkills(CurrentCharacter);
            SetInfoSheet(CurrentCharacter, m_editingSkills);
            SetSpellCastingSheet(CurrentCharacter);

            PanelButtonHandler();
        }

        private void ConfigureCharacterSheetPage()
        {
            if (m_isEditing)
            {
                if (!CharacterCreator.Instance.m_nextButton.gameObject.activeInHierarchy)
                {
                    CharacterCreator.Instance.m_nextButton.gameObject.SetActive(true);
                }

                CharacterCreator.Instance.m_nextButton.GetComponentInChildren<TMP_Text>(true).text = "Next";
                CharacterCreator.Instance.m_nextButton.onClick.RemoveAllListeners();
                CharacterCreator.Instance.m_nextButton.onClick.AddListener(delegate
                {
                    if (!CharacterCreator.Instance.m_popUpHelper.IsOn)
                    {
                        Button bt1 = Instantiate(CharacterCreator.Instance.m_popUpHelper.m_prefButton);
                        bt1.transform.SetParent(CharacterCreator.Instance.m_popUpHelper.m_buttonHolder);
                        bt1.gameObject.SetActive(true);
                        bt1.gameObject.GetComponent<RectTransform>().localScale = Vector3.one;

                        CharacterCreator.Instance.m_popUpHelper.m_buttonText = bt1.GetComponentInChildren<TMP_Text>();
                        CharacterCreator.Instance.m_popUpHelper.m_buttonText.text = "Continue Editing";

                        bt1.onClick.AddListener(CharacterCreator.Instance.m_popUpHelper.HidePopUp);

                        CharacterCreator.Instance.m_popUpHelper.m_buttons.Add(bt1);

                        Button bt2 = Instantiate(CharacterCreator.Instance.m_popUpHelper.m_prefButton);
                        bt2.transform.SetParent(CharacterCreator.Instance.m_popUpHelper.m_buttonHolder);
                        bt2.gameObject.SetActive(true);
                        bt2.gameObject.GetComponent<RectTransform>().localScale = Vector3.one;

                        CharacterCreator.Instance.m_popUpHelper.m_buttonText = bt2.GetComponentInChildren<TMP_Text>();
                        CharacterCreator.Instance.m_popUpHelper.m_buttonText.text = "Finish & Save";

                        bt2.onClick.AddListener(delegate
                        {
                            CharacterCreator.Instance.SaveCharacter(CharacterCreator.Instance.EditingCharacter);
                            CharacterCreator.Instance.m_popUpHelper.HidePopUp();
                            CharacterCreator.Instance.NextPage();
                        });

                        CharacterCreator.Instance.m_popUpHelper.m_buttons.Add(bt2);

                        CharacterCreator.Instance.m_popUpHelper.ShowPopUp("Finish creating your character and save?");
                    }
                });

                CharacterCreator.Instance.m_backButton.GetComponentInChildren<TMP_Text>(true).text = "Back";
                CharacterCreator.Instance.m_backButton.gameObject.SetActive(true);
                CharacterCreator.Instance.m_backButton.onClick.RemoveAllListeners();
                CharacterCreator.Instance.m_backButton.onClick.AddListener(CharacterCreator.Instance.PreviousPage);
            }
            else
            {
                if (!CharacterCreator.Instance.m_nextButton.gameObject.activeInHierarchy)
                {
                    CharacterCreator.Instance.m_nextButton.gameObject.SetActive(true);
                }

                CharacterCreator.Instance.m_nextButton.GetComponentInChildren<TMP_Text>(true).text = "Change Character";
                CharacterCreator.Instance.m_nextButton.onClick.RemoveAllListeners();
                CharacterCreator.Instance.m_nextButton.onClick.AddListener(delegate
                {
                    CharacterCreator.Instance.SetPage(CharacterCreator.Instance.CharacterListingPage);
                });

                CharacterCreator.Instance.m_backButton.onClick.RemoveAllListeners();
                CharacterCreator.Instance.m_backButton.gameObject.SetActive(false);
            }
        }

        public void SetInfoSheet(PlayerCharacterData player, List<PlayerCharacterData.Skills> skills)
        {
            m_nameText.text = player.info.name;
            m_raceText.text = player.info.race.ToString();
            m_classText.text = player.info.classes.ToString();

            m_hitPointDisplay.SetHitPointsDisplay(player);

            m_levelText.text = player.info.level.ToString();
            m_inspirationText.text = player.info.inspirationPoints.ToString();
            m_proficiencyBonusText.text = player.info.proficiencyBonus.ToString();
            m_speedText.text = player.info.speed.ToString();

            for (int i = 0; i < player.abilityScore.Length; i++)
            {
                m_abilityScoreDisplays[i].SetAbilityDisplay(player.abilityScore[i].ability.ToString(),
                                                            player.abilityScore[i].score,
                                                            player.abilityScore[i].modifier);

                m_savingThrowDisplays[i].SetSavingThrowDisplay(player.abilityScore[i].ability.ToString(),
                                                               player.info.proficiencyBonus,
                                                               player.abilityScore[i].modifier,
                                                               player.abilityScore[i].savingThrows);


                if (player.abilityScore[i].ability == PlayerCharacterData.AbilityScore.Ability.Dexterity)
                {
                    player.info.initiative = player.abilityScore[i].modifier;

                    m_initiativeText.text = player.info.initiative.ToString();

                    m_armorClassText.text = (player.abilityScore[i].modifier + player.info.armorClass).ToString();
                }

                if (player.abilityScore[i].ability == PlayerCharacterData.AbilityScore.Ability.Intelligence)
                {
                    for (int j = 0; j < skills.Count; j++)
                    {
                        if (skills[j].skill == PlayerCharacterData.Skills.Skill.Investigation)
                        {
                            m_investigation.SetPassiveSenseDisplay(player.abilityScore[i].modifier,
                                                                   player.info.proficiencyBonus,
                                                                   skills[j].proficient);
                        }
                    }
                }

                if (player.abilityScore[i].ability == PlayerCharacterData.AbilityScore.Ability.Wisdom)
                {
                    for (int j = 0; j < skills.Count; j++)
                    {
                        if (skills[j].skill == PlayerCharacterData.Skills.Skill.Perception)
                        {
                            m_perception.SetPassiveSenseDisplay(player.abilityScore[i].modifier,
                                                                player.info.proficiencyBonus,
                                                                skills[j].proficient) ;
                        }

                        if (player.skills[j].skill == PlayerCharacterData.Skills.Skill.Insight)
                        {
                            m_insight.SetPassiveSenseDisplay(player.abilityScore[i].modifier,
                                                             player.info.proficiencyBonus,
                                                             skills[j].proficient);
                        }
                    }
                }
            }

            for (int i = 0; i < m_skillDisplay.Count; i++)
            {
                m_skillDisplay[i].SetSkillDisplay(m_editingSkills[i], player.abilityScore, player.info.proficiencyBonus);
            }
        }

        public void SetSpellCastingSheet(PlayerCharacterData player)
        {
            m_spellCastingDisplay.SetSpellCastingDisplay(player);
        }

        void PanelButtonHandler()
        {
            if (m_attributePanel.activeInHierarchy) m_attributeButton.gameObject.SetActive(false);
            else m_attributeButton.gameObject.SetActive(true);

            if (m_skillPanel.activeInHierarchy) m_skillButton.gameObject.SetActive(false);
            else m_skillButton.gameObject.SetActive(true);

            if (m_spellcastPanel.activeInHierarchy) m_spellcastButton.gameObject.SetActive(false);
            else m_spellcastButton.gameObject.SetActive(true);
        }
        
        void PrepareSkills(PlayerCharacterData player)
        {
            m_editingSkills.Clear();

            for (int i = 0; i < player.raceSkills.Count; i++)
            {
                m_editingSkills.Add(player.raceSkills[i]);
            }

            for (int i = 0; i < player.classSkills.Count; i++)
            {
                if (player.classSkills[i].proficient)
                {
                    PlayerCharacterData.Skills s = m_editingSkills.Find(x => x.skill == player.classSkills[i].skill);

                    int index = m_editingSkills.IndexOf(s);

                    m_editingSkills[i] = player.classSkills[i];
                }
            }
        }
    }
}
