using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

namespace CustomRPGSystem
{
    public class UIPlayerPref : MonoBehaviour
    {
        [SerializeField] private Sprite m_toggleOn, m_toggleOff;
        [SerializeField] private Toggle m_toggleSelect;
        [SerializeField] private TMP_Text m_name, m_class, m_level;
        [SerializeField] private Button m_deleteButton, m_editButton;

        private string m_characterId = "";

        #region Properties
        public Toggle ToggleSelect
        {
            get
            {
                return m_toggleSelect;
            }
        }
        public bool IsSelected
        {
            get
            {
                return m_toggleSelect.isOn;
            }
        }
        public string CharacterId
        {
            get
            {
                return m_characterId;
            }
        }
        #endregion

        public void SetPlayerPref(string p_id, string p_name, string p_class, string p_level)
        {
            m_characterId = p_id;
            m_name.text = p_name;
            m_class.text = p_class;
            m_level.text = p_level;

            m_toggleSelect.onValueChanged.AddListener(delegate
            {
                SetToggleGraphic();
            });

            SetToggleGraphic();

            m_toggleSelect.onValueChanged.AddListener(delegate
            {
                CheckCharacter(m_toggleSelect.isOn, m_characterId);
            });

            CheckCharacter(m_toggleSelect.isOn, m_characterId);
        }

        private void SetToggleGraphic()
        {
            if (m_toggleSelect.isOn)
            {
                m_toggleSelect.targetGraphic.GetComponent<Image>().sprite = m_toggleOn;
            }
            else
            {
                m_toggleSelect.targetGraphic.GetComponent<Image>().sprite = m_toggleOff;
            }
        }

        private void CheckCharacter(bool p_toggleCheck, string p_id)
        {
            CharacterCreator.Instance.EditingCharacter = null;
            m_editButton.onClick.RemoveAllListeners();

            if (p_toggleCheck)
            {
                m_deleteButton.gameObject.SetActive(true);
                m_deleteButton.onClick.AddListener(delegate
                {
                    PlayerCharacterData data = CharacterCreator.Instance.SavedCharacters.Find(x => x.info.id == p_id);

                    if (!CharacterCreator.Instance.m_popUpHelper.IsOn)
                    {
                        Button bt1 = Instantiate(CharacterCreator.Instance.m_popUpHelper.m_prefButton);
                        bt1.transform.SetParent(CharacterCreator.Instance.m_popUpHelper.m_buttonHolder);
                        bt1.gameObject.SetActive(true);
                        bt1.gameObject.GetComponent<RectTransform>().localScale = Vector3.one;

                        CharacterCreator.Instance.m_popUpHelper.m_buttonText = bt1.GetComponentInChildren<TMP_Text>();
                        CharacterCreator.Instance.m_popUpHelper.m_buttonText.text = "Confirm";

                        bt1.onClick.AddListener(delegate
                        {
                            int index = CharacterCreator.Instance.SavedCharacters.IndexOf(data);
                            CharacterCreator.Instance.SavedCharacters.RemoveAt(index);

                            CharacterCreator.Instance.DeleteCharacter(CharacterCreator.Instance.MainCharacterDirectory + "/" + data.info.name + ".json");

                            Destroy(gameObject);

                            CharacterCreator.Instance.m_popUpHelper.HidePopUp();

                            CharacterListing.Instance.SetCharacterListing();
                        });

                        CharacterCreator.Instance.m_popUpHelper.m_buttons.Add(bt1);

                        Button bt2 = Instantiate(CharacterCreator.Instance.m_popUpHelper.m_prefButton);
                        bt2.transform.SetParent(CharacterCreator.Instance.m_popUpHelper.m_buttonHolder);
                        bt2.gameObject.SetActive(true);
                        bt2.gameObject.GetComponent<RectTransform>().localScale = Vector3.one;

                        CharacterCreator.Instance.m_popUpHelper.m_buttonText = bt2.GetComponentInChildren<TMP_Text>();
                        CharacterCreator.Instance.m_popUpHelper.m_buttonText.text = "Cancel";

                        bt2.onClick.AddListener(delegate
                        {
                            CharacterCreator.Instance.m_popUpHelper.HidePopUp();
                        });

                        CharacterCreator.Instance.m_popUpHelper.m_buttons.Add(bt2);

                        CharacterCreator.Instance.m_popUpHelper.ShowPopUp("Are you sure to delete your character?");
                    }
                });

                PlayerCharacterData data = CharacterCreator.Instance.SavedCharacters.Find(x => x.info.id == p_id);

                if (data.HasAvailablePoints )
                {
                    m_editButton.gameObject.SetActive(true);
                    m_editButton.onClick.AddListener(delegate
                    {
                        CharacterCreator.Instance.EditingCharacter = data;

                        CharacterAttributeEditor.Instance.IsSet = false;
                        CharacterExtraPointEditor.Instance.IsSet = false;
                        CharacterSkillEditor.Instance.IsSet = false;

                        CharacterCreator.Instance.SetPage(CharacterCreator.Instance.CharacterAttributeEditorPage);

                        CharacterCreator.Instance.m_backButton.GetComponentInChildren<TMP_Text>(true).text = "Back";
                        CharacterCreator.Instance.m_backButton.gameObject.SetActive(false);
                        CharacterCreator.Instance.m_backButton.onClick.RemoveAllListeners();

                        CharacterCreator.m_editingNewCharacter = false;
                        CharacterCreator.m_editingLoadedCharacter = true;
                    });
                }
            }
            else
            {
                m_deleteButton.gameObject.SetActive(false);
                m_editButton.gameObject.SetActive(false);
            }
        }
    }
}
