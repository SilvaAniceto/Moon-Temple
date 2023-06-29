using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CustomRPGSystem
{
    public class CharacterListing : MonoBehaviour
    {
        public static CharacterListing Instance;

        [SerializeField] UIPlayerPref m_playerPref;
        [SerializeField] private Transform m_holder;

        private List<UIPlayerPref> m_prefList = new List<UIPlayerPref>();
        private List<PlayerCharacterData> m_characterList = new List<PlayerCharacterData>();

        void Awake()
        {
            if (m_playerPref.gameObject.activeInHierarchy)
            {
                m_playerPref.gameObject.SetActive(false);
            }
        }

        void OnEnable()
        {
            SetCharacterListing();
        }

        public void SetCharacterListing()
        {
            m_characterList.Clear();
            m_characterList = CharacterCreator.Instance.LoadCharacters(CharacterCreator.Instance.MainCharacterDirectory + "/");

            if (m_characterList.Count <= 0)
            {
                CharacterCreator.Instance.m_nextButton.onClick.RemoveAllListeners();
                CharacterCreator.Instance.m_nextButton.gameObject.SetActive(false);
            }
            else
            {
                CharacterCreator.Instance.m_nextButton.gameObject.SetActive(true);
                CharacterCreator.Instance.m_nextButton.GetComponentInChildren<TMP_Text>(true).text = "Select Character";
                CharacterCreator.Instance.m_nextButton.onClick.RemoveAllListeners();
                CharacterCreator.Instance.m_nextButton.onClick.AddListener(delegate
                {
                    SelectCharacter();
                });
            }

            CharacterCreator.Instance.m_backButton.GetComponentInChildren<TMP_Text>(true).text = "Main Title";
            CharacterCreator.Instance.m_backButton.onClick.RemoveAllListeners();
            CharacterCreator.Instance.m_backButton.onClick.AddListener(delegate
            {
                CharacterCreator.Instance.StartCreator();
            });

            foreach (UIPlayerPref pref in m_prefList)
            {
                Destroy(pref.gameObject);
            }
            m_prefList.Clear();

            for (int i = 0; i < CharacterCreator.Instance.SavedCharacters.Count; i++)
            {
                UIPlayerPref pref = Instantiate(m_playerPref);
                pref.transform.SetParent(m_holder);
                pref.gameObject.GetComponent<RectTransform>().localScale = Vector3.one;
                pref.ToggleSelect.group = m_holder.GetComponent<ToggleGroup>();

                pref.SetPlayerPref(CharacterCreator.Instance.SavedCharacters[i].info.id, CharacterCreator.Instance.SavedCharacters[i].info.name, CharacterCreator.Instance.SavedCharacters[i].info.classes.ToString(), CharacterCreator.Instance.SavedCharacters[i].info.level.ToString());

                pref.gameObject.SetActive(true);

                m_prefList.Add(pref);
            }
        }

        private void OnDisable()
        {
            for (int i = 0; i < m_prefList.Count; i++)
            {
                if (m_prefList[i] == null)
                {
                    m_prefList.RemoveAt(i);
                }
                else
                {
                    Destroy(m_prefList[i].gameObject);
                }
            }

            m_prefList.Clear();
        }

        private void SelectCharacter()
        {
            UIPlayerPref pref = m_prefList.Find(x => x.IsSelected);
            PlayerCharacterData character = m_characterList.Find(x => x.info.id == pref.CharacterId);

            CharacterSheet.Instance.IsEditing = false;
            CharacterSheet.Instance.CurrentCharacter = character;

            CharacterCreator.Instance.SetPage(CharacterCreator.Instance.CharacterSheetPage);
        }
    }
}
