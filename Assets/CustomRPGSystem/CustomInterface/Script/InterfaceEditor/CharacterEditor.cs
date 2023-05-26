using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace CustomRPGSystem
{
    public class CharacterEditor : MonoBehaviour
    {
        [SerializeField] private TMP_InputField playerName;
        [SerializeField] private TMP_InputField characterName;
        [SerializeField] private TMP_Dropdown level;
        [SerializeField] private TMP_Dropdown race;
        [SerializeField] private TMP_Dropdown classes;

        private void Awake()
        {
            level.AddOptions(new List<TMP_Dropdown.OptionData>
            {
                new TMP_Dropdown.OptionData() {text = "1"},
                new TMP_Dropdown.OptionData() {text = "2"},
                new TMP_Dropdown.OptionData() {text = "3"},
                new TMP_Dropdown.OptionData() {text = "4"},
                new TMP_Dropdown.OptionData() {text = "5"},
                new TMP_Dropdown.OptionData() {text = "6"},
                new TMP_Dropdown.OptionData() {text = "7"},
                new TMP_Dropdown.OptionData() {text = "8"},
                new TMP_Dropdown.OptionData() {text = "9"},
                new TMP_Dropdown.OptionData() {text = "10"},
                new TMP_Dropdown.OptionData() {text = "11"},
                new TMP_Dropdown.OptionData() {text = "12"},
                new TMP_Dropdown.OptionData() {text = "13"},
                new TMP_Dropdown.OptionData() {text = "14"},
                new TMP_Dropdown.OptionData() {text = "15"},
                new TMP_Dropdown.OptionData() {text = "16"},
                new TMP_Dropdown.OptionData() {text = "17"},
                new TMP_Dropdown.OptionData() {text = "18"},
                new TMP_Dropdown.OptionData() {text = "19"},
                new TMP_Dropdown.OptionData() {text = "20"}
            });

            race.AddOptions(new List<TMP_Dropdown.OptionData>
            {
                new TMP_Dropdown.OptionData() {text = "None"},
                new TMP_Dropdown.OptionData() {text = "Dragonborn"},
                new TMP_Dropdown.OptionData() {text = "Hill Dwarf"},
                new TMP_Dropdown.OptionData() {text = "Montain Dwarf"},
                new TMP_Dropdown.OptionData() {text = "High Elf"},
                new TMP_Dropdown.OptionData() {text = "Wood Elf"},
                new TMP_Dropdown.OptionData() {text = "Shadow Elf"},
                new TMP_Dropdown.OptionData() {text = "Forest Gnome"},
                new TMP_Dropdown.OptionData() {text = "Rock Gnome"},
                new TMP_Dropdown.OptionData() {text = "Half Elf"},
                new TMP_Dropdown.OptionData() {text = "Half Orc"},
                new TMP_Dropdown.OptionData() {text = "Lighfoot Halfling"},
                new TMP_Dropdown.OptionData() {text = "Stout Halfling"},
                new TMP_Dropdown.OptionData() {text = "Human"},
                new TMP_Dropdown.OptionData() {text = "Tiefling"}
            });

            classes.AddOptions(new List<TMP_Dropdown.OptionData>
            {
                new TMP_Dropdown.OptionData() {text = "None"},
                new TMP_Dropdown.OptionData() {text = "Barbarian"},
                new TMP_Dropdown.OptionData() {text = "Bard"},
                new TMP_Dropdown.OptionData() {text = "Cleric"},
                new TMP_Dropdown.OptionData() {text = "Druid"},
                new TMP_Dropdown.OptionData() {text = "Fighter"},
                new TMP_Dropdown.OptionData() {text = "Monk"},
                new TMP_Dropdown.OptionData() {text = "Paladin"},
                new TMP_Dropdown.OptionData() {text = "Ranger"},
                new TMP_Dropdown.OptionData() {text = "Rogue"},
                new TMP_Dropdown.OptionData() {text = "Sorcerer"},
                new TMP_Dropdown.OptionData() {text = "Warlock"},
                new TMP_Dropdown.OptionData() {text = "Wizard"}
            });
        }

        private void OnEnable()
        {
            playerName.onValueChanged.AddListener(delegate
            {
                CharacterCreator.m_playerName = playerName.text;
            });

            characterName.onValueChanged.AddListener(delegate
            {
                CharacterCreator.m_characterName = characterName.text;
            });

            level.onValueChanged.AddListener(delegate {
                CharacterCreator.m_levelValue = level.value;
            });

            race.onValueChanged.AddListener(delegate {
                CharacterCreator.m_raceValue = race.value;
            });

            classes.onValueChanged.AddListener(delegate {
                CharacterCreator.m_classValue = classes.value;
            });

            CharacterCreator.Instance.m_nextButton.onClick.RemoveAllListeners();
            CharacterCreator.Instance.m_nextButton.onClick.AddListener(delegate
            {
                if (string.IsNullOrEmpty(CharacterCreator.m_characterName)) 
                {
                    Button bt = Instantiate(CharacterCreator.Instance.m_popUpHelper.m_prefButton);
                    bt.transform.SetParent(CharacterCreator.Instance.m_popUpHelper.m_buttonHolder);
                    bt.gameObject.SetActive(true);
                    bt.gameObject.GetComponent<RectTransform>().localScale = Vector3.one;

                    CharacterCreator.Instance.m_popUpHelper.m_buttonText = bt.GetComponentInChildren<TMP_Text>();
                    CharacterCreator.Instance.m_popUpHelper.m_buttonText.text = "Ok";

                    bt.onClick.AddListener(CharacterCreator.Instance.m_popUpHelper.HidePopUp);

                    CharacterCreator.Instance.m_popUpHelper.m_buttons.Add(bt);
                    CharacterCreator.Instance.m_popUpHelper.ShowPopUp("Insert a character name.");
                    return;
                } 

                CharacterCreator.CharacterData = null;
                CharacterCreator.Instance.EditingCharacter = null;

                CharacterCreator.CharacterData = new PlayerCharacterData(CharacterCreator.m_characterName, CharacterCreator.m_levelValue, (PlayerCharacterData.CharacterInfo.Race)CharacterCreator.m_raceValue, (PlayerCharacterData.CharacterInfo.Class)CharacterCreator.m_classValue);

                CharacterCreator.Instance.EditingCharacter = CharacterCreator.CharacterData;

                CharacterAttributeEditor.Instance.CurrentAvailablePoints = CharacterCreator.Instance.EditingCharacter.info.availablePoints;
                CharacterExtraPointEditor.Instance.CurrentExtraPoints = CharacterCreator.Instance.EditingCharacter.info.extraPoints;

                CharacterAttributeEditor.Instance.IsSet = false;
                CharacterExtraPointEditor.Instance.IsSet = false;
                CharacterSkillEditor.Instance.IsSet = false;

                CharacterCreator.Instance.NextPage();
            });
        }
    }
}
