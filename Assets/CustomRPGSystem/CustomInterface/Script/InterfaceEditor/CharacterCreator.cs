using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

namespace CustomRPGSystem
{
    public class CharacterCreator : MonoBehaviour
    {
        public static CharacterCreator Instance;

        [Header("Main Title")]
        [SerializeField] private GameObject m_mainTitle;
        [SerializeField] private Button m_NewCharacterButton;
        [SerializeField] private Button m_LoadCharacterButton;
        public Button m_backButton;
        public Button m_nextButton;
        public Button m_closeButton;

        [Header("Character Editor")]
        [SerializeField] private CharacterEditor m_characterEditor;

        [Header("Character Attribute Editor")]
        [SerializeField] private CharacterAttributeEditor m_characterAttributeEditor;

        [Header("Character Extra Points Editor")]
        [SerializeField] private CharacterExtraPointEditor m_characterExtraPointEditor;

        [Header("Character Skill Editor")]
        [SerializeField] private CharacterSkillEditor m_characterSkillEditor;

        [Header("Character Sheet")]
        [SerializeField] private CharacterSheet m_characterSheet;

        [Header("Character Lists")]
        [SerializeField] private CharacterListing m_characterListing;

        [Header("PopUp Helper")]
        public PopUpHelper m_popUpHelper;

        public static string m_playerName = "";
        public static string m_characterName = "";
        public static int m_levelValue;
        public static int m_raceValue;
        public static int m_classValue;
        public static bool m_editingNewCharacter = false;
        public static bool m_editingLoadedCharacter = false;

        private List<PlayerCharacterData> CharacterList = new List<PlayerCharacterData>();
        public static PlayerCharacterData CharacterData;

        public PlayerCharacterData EditingCharacter;

        [SerializeField] private List<GameObject> UIPages = new List<GameObject>();

        public string[] array;

        #region PROPERTIES
        public string PlayerDirectory
        {
            get
            {
                return Application.persistentDataPath + "/Player";
            }
        }
        public string MainCharacterDirectory
        {
            get
            {
                return PlayerDirectory + "/Characters";
            }
        }
        public List<PlayerCharacterData> SavedCharacters
        {
            get
            {
                return CharacterList;
            }
        }
        public GameObject CharacterEditorPage
        {
            get
            {
                return m_characterEditor.gameObject;
            }
        }
        public GameObject CharacterAttributeEditorPage
        {
            get
            {
                return m_characterAttributeEditor.gameObject;
            }
        }
        public GameObject CharacterExtraPointEditorPage
        {
            get
            {
                return m_characterExtraPointEditor.gameObject;
            }
        }
        public GameObject CharacterSkillEditorPage
        {
            get
            {
                return m_characterSkillEditor.gameObject;
            }
        }
        public GameObject CharacterSheetPage
        {
            get
            {
                return m_characterSheet.gameObject;
            }
        }
        public GameObject CharacterListingPage
        {
            get
            {
                return m_characterListing.gameObject;
            }
        }
        #endregion

        private void Awake()
        {
            StartCreator();
        }

        public void StartCreator()
        {
            if (Instance == null) Instance = this;
            if (CharacterAttributeEditor.Instance == null) CharacterAttributeEditor.Instance = m_characterAttributeEditor;
            if (CharacterExtraPointEditor.Instance == null) CharacterExtraPointEditor.Instance = m_characterExtraPointEditor;
            if (CharacterSkillEditor.Instance == null) CharacterSkillEditor.Instance = m_characterSkillEditor;
            if (CharacterSheet.Instance == null) CharacterSheet.Instance = m_characterSheet;
            if (CharacterListing.Instance == null) CharacterListing.Instance = m_characterListing;

            UIPages.Clear();

            UIPages.Add(m_mainTitle);
            UIPages.Add(m_characterEditor.gameObject);
            UIPages.Add(m_characterAttributeEditor.gameObject);
            UIPages.Add(m_characterExtraPointEditor.gameObject);
            UIPages.Add(m_characterSkillEditor.gameObject);
            UIPages.Add(m_characterSheet.gameObject);
            UIPages.Add(m_characterListing.gameObject);

            m_backButton.onClick.RemoveAllListeners();
            m_backButton.onClick.AddListener(PreviousPage);

            m_nextButton.gameObject.SetActive(false);

            m_closeButton.gameObject.SetActive(true);
            m_closeButton.onClick.RemoveAllListeners();
            m_closeButton.onClick.AddListener(delegate
            {
                if (!Instance.m_popUpHelper.IsOn)
                {
                    Button bt1 = Instantiate(Instance.m_popUpHelper.m_prefButton);
                    bt1.transform.SetParent(Instance.m_popUpHelper.m_buttonHolder);
                    bt1.gameObject.SetActive(true);
                    bt1.gameObject.GetComponent<RectTransform>().localScale = Vector3.one;

                    Instance.m_popUpHelper.m_buttonText = bt1.GetComponentInChildren<TMP_Text>();
                    Instance.m_popUpHelper.m_buttonText.text = "Confirm";

                    bt1.onClick.AddListener(delegate
                    {
                        Application.Quit();
                    });

                    Instance.m_popUpHelper.m_buttons.Add(bt1);

                    Button bt2 = Instantiate(Instance.m_popUpHelper.m_prefButton);
                    bt2.transform.SetParent(Instance.m_popUpHelper.m_buttonHolder);
                    bt2.gameObject.SetActive(true);
                    bt2.gameObject.GetComponent<RectTransform>().localScale = Vector3.one;

                    Instance.m_popUpHelper.m_buttonText = bt2.GetComponentInChildren<TMP_Text>();
                    Instance.m_popUpHelper.m_buttonText.text = "Cancel";

                    bt2.onClick.AddListener(delegate
                    {
                        Instance.m_popUpHelper.HidePopUp();
                    });

                    Instance.m_popUpHelper.m_buttons.Add(bt2);

                    Instance.m_popUpHelper.ShowPopUp("Are you sure you want to quit?");
                }
            });

            m_NewCharacterButton.onClick.RemoveAllListeners();
            m_LoadCharacterButton.onClick.RemoveAllListeners();

            if (!Directory.Exists(PlayerDirectory))
            {
                Directory.CreateDirectory(PlayerDirectory);
                Directory.CreateDirectory(MainCharacterDirectory);
            }
            else
            {
                if (!Directory.Exists(MainCharacterDirectory))
                {
                    Directory.CreateDirectory(MainCharacterDirectory);
                }
            }

            if (Directory.GetFiles(MainCharacterDirectory + "/", "*.json").Length == 0)
            {
                m_LoadCharacterButton.gameObject.SetActive(false);
                m_NewCharacterButton.onClick.AddListener(delegate
                {
                    SetPage(m_characterEditor.gameObject);
                    m_nextButton.gameObject.SetActive(true);
                    m_closeButton.gameObject.SetActive(false);

                    m_editingNewCharacter = true;
                    m_editingLoadedCharacter = false;
                });
            }
            else
            {
                LoadCharacters(MainCharacterDirectory + "/");

                m_LoadCharacterButton.gameObject.SetActive(true);
                m_LoadCharacterButton.onClick.AddListener(delegate
                {
                    SetPage(m_characterListing.gameObject);
                    m_closeButton.gameObject.SetActive(false);

                    m_backButton.onClick.RemoveAllListeners();
                    m_backButton.onClick.AddListener(delegate
                    {
                        StartCreator();
                    });

                    m_editingNewCharacter = false;
                    m_editingLoadedCharacter = true;
                });

                if (CharacterList.Count > 0 && CharacterList.Count < 4)
                {
                    m_NewCharacterButton.gameObject.SetActive(true);
                    m_NewCharacterButton.onClick.AddListener(delegate
                    {
                        SetPage(m_characterEditor.gameObject);
                        m_nextButton.gameObject.SetActive(true);
                        m_closeButton.gameObject.SetActive(false);

                        m_editingNewCharacter = true;
                        m_editingLoadedCharacter = false;
                    });
                }
                else
                {
                    m_NewCharacterButton.gameObject.SetActive(false);
                    m_NewCharacterButton.onClick.RemoveAllListeners();
                }
            }

            ManagerCreatorPages(0);
        }

        public void SaveCharacter(PlayerCharacterData player)
        {
            for (int i = 0; i < player.classSkills.Count; i++)
            {
                for (int j = 0; j < player.skills.Length; j++)
                {
                    if (player.classSkills[i].skill == player.skills[i].skill && player.classSkills[i].proficient)
                    {
                        player.skills[i] = player.classSkills[i];
                    }
                }
            }

            for (int i = 0; i < player.raceSkills.Count; i++)
            {
                for (int j = 0; j < player.skills.Length; j++)
                {
                    if (player.raceSkills[i].skill == player.skills[i].skill && player.raceSkills[i].proficient)
                    {
                        player.skills[i] = player.raceSkills[i];
                    }
                }
            }

            PlayerCharacterData characterData = CharacterList.Find(x => x.info.name == player.info.name);

            if (characterData != null)
            {
                CharacterList[CharacterList.IndexOf(characterData)] = player;
            }
            else
            {
                CharacterList.Add(player);
            }

            FileHandler.SaveToJSON<PlayerCharacterData>(player, MainCharacterDirectory + "/" + player.info.name + ".json");
        }

        public List<PlayerCharacterData> LoadCharacters(string p_path)
        {
            array = null;
            CharacterList.Clear();

            array = Directory.GetFiles(p_path, "*.json");

            for (int i = 0; i < array.Length; i++)
            {
                PlayerCharacterData sc = FileHandler.ReadFromJSON<PlayerCharacterData>(array[i]);

                CharacterList.Add(sc);
            }

            return CharacterList;
        }

        public void DeleteCharacter(string p_path)
        {
            File.Delete(p_path);

#if UNITY_EDITOR
            UnityEditor.AssetDatabase.Refresh();
#endif
        }

        void ManagerCreatorPages(int p_pageIndex)
        {
            if (p_pageIndex == 0)
            {
                m_backButton.gameObject.SetActive(false);
                m_nextButton.gameObject.SetActive(false);
            }
            else m_backButton.gameObject.SetActive(true);

            for (int i = 0; i < UIPages.Count; i++)
            {
                UIPages[i].SetActive(false);
            }

            UIPages[p_pageIndex].SetActive(true);
        }

        public void PreviousPage()
        {
            int index = 0;

            for (int i = 0; i < UIPages.Count; i++)
            {
                if (UIPages[i].activeInHierarchy)
                {
                    index = i;
                }
            }

            ManagerCreatorPages(index - 1);
        }

        public void NextPage()
        {
            int index = 0;

            for (int i = 0; i < UIPages.Count; i++)
            {
                if (UIPages[i].activeInHierarchy)
                {
                    index = i;
                }
            }

            ManagerCreatorPages(index + 1);
        }

        public void SetPage(GameObject p_page)
        {
            if (UIPages.Contains(p_page))
            {
                ManagerCreatorPages(UIPages.IndexOf(p_page));
            }
        }
    }
}
