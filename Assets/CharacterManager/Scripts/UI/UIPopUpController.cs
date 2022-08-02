using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace IsometricOrientedPerspective
{
    public class UIPopUpController : MonoBehaviour
    {
        public static UIPopUpController m_popUpInstance;

        [SerializeField] private RectTransform open;
        [SerializeField] private RectTransform close;
        [SerializeField] private RectTransform panel;
        [SerializeField] private Button btOpenClose;
        protected bool isOpen = true;

        protected virtual void Awake()
        {
            if (m_popUpInstance == null)
                m_popUpInstance = this;

            btOpenClose?.onClick.AddListener(OnOpenCLose);
        }

        protected virtual void OnOpenCLose()
        {
            if (isOpen)
                Hide();
            else
                Show();
        }

        public virtual void Show(bool now = false)
        {
            LeanTween.cancel(panel.gameObject);

            LeanTween.move(panel.gameObject, open.transform.position, 0.3f).setEase(LeanTweenType.easeOutQuad).setOnComplete(() =>
            {
                btOpenClose.GetComponentInChildren<Text>().text = "<<";
            });

            isOpen = true;
        }
        public virtual void Hide(bool now = false)
        {
            LeanTween.cancel(panel.gameObject);

            LeanTween.move(panel.gameObject, close.transform.position, 0.3f).setEase(LeanTweenType.easeOutQuad).setOnComplete(() =>
            {
                btOpenClose.GetComponentInChildren<Text>().text = ">>";
            });

            isOpen = false;
        }
    }
}
