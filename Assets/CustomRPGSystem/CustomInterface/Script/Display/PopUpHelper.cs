using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace CustomRPGSystem
{
    public class PopUpHelper : MonoBehaviour
    {
        public TMP_Text m_messageText;
        public TMP_Text m_buttonText;
        public Button m_prefButton;
        public Transform m_buttonHolder;

        public List<Button> m_buttons = new List<Button>();

        public bool IsOn
        {
            get
            {
                if (gameObject.activeSelf) 
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        private void Start()
        {
            if (gameObject.activeInHierarchy)
            {
                m_prefButton.gameObject.SetActive(false);
            }
        }

        public void ShowPopUp(string msg = "")
        {
            gameObject.SetActive(true);
            m_messageText.text = msg;
        }

        public void HidePopUp()
        {
            m_messageText.text = "";

            for (int i = 0; i < m_buttons.Count; i++)
            {
                DestroyImmediate(m_buttons[i].gameObject);
            }

            m_buttons.Clear();
            
            gameObject.SetActive(false);
        }
    }
}
