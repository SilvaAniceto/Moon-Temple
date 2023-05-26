using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CustomRPGSystem
{
    public class CharacterListing : MonoBehaviour
    {
        public static CharacterListing Instance;

        [SerializeField] UIPlayerPref m_playerPref;
        [SerializeField] private Transform m_holder;

        void Awake()
        {
            if (m_playerPref.gameObject.activeInHierarchy)
            {
                m_playerPref.gameObject.SetActive(false);
            }
        }

        void OnEnable()
        {
            for (int i = 0; i < CharacterCreator.Instance.SavedCharacters.Count; i++)
            {
                UIPlayerPref pref = Instantiate(m_playerPref);
                pref.transform.SetParent(m_holder);
                pref.gameObject.GetComponent<RectTransform>().localScale = Vector3.one;

                pref.SetPlayerPref(CharacterCreator.Instance.SavedCharacters[i].info.name, CharacterCreator.Instance.SavedCharacters[i].info.classes.ToString(), CharacterCreator.Instance.SavedCharacters[i].info.level.ToString());

                pref.gameObject.SetActive(true);
            }
        }
    }
}
