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

        void Awake()
        {
            if (m_playerPref.gameObject.activeInHierarchy)
            {
                m_playerPref.gameObject.SetActive(false);
            }
        }

        void OnEnable()
        {
            CharacterCreator.Instance.m_nextButton.gameObject.SetActive(false);
            CharacterCreator.Instance.m_backButton.GetComponentInChildren<TMP_Text>(true).text = "Main Title";
            CharacterCreator.Instance.m_backButton.onClick.RemoveAllListeners();
            CharacterCreator.Instance.m_backButton.onClick.AddListener(delegate
            {
                CharacterCreator.Instance.StartCreator();
            });

            CharacterCreator.Instance.LoadCharacters(CharacterCreator.Instance.MainCharacterDirectory + "/");

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
    }
}
