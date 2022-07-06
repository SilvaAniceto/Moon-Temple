using UnityEngine;
using UnityEngine.UI;

namespace IsometricOrientedPerspective
{
    public class UIPopUpController : MonoBehaviour
    {
        [SerializeField] private RectTransform open;
        [SerializeField] private RectTransform close;
        [SerializeField] private RectTransform panel;
        [SerializeField] private Button btOpenClose;
        protected bool isOpen = true;

        protected virtual void Awake()
        {
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

            //if (now)
            //    panel.transform.localPosition = open.transform.localPosition;
            //else
                LeanTween.move(panel.gameObject, open.transform.position, 0.3f).setEase(LeanTweenType.easeOutQuad).setOnComplete(() =>
                {
                    btOpenClose.GetComponentInChildren<Text>().text = "<<";
                });

            isOpen = true;
        }
        public virtual void Hide(bool now = false)
        {
            LeanTween.cancel(panel.gameObject);

            //if (now)
            //    panel.transform.localPosition = close.transform.localPosition;
            //else
            LeanTween.move(panel.gameObject, close.transform.position, 0.3f).setEase(LeanTweenType.easeOutQuad).setOnComplete(() =>
            {
                btOpenClose.GetComponentInChildren<Text>().text = ">>";
            });

            isOpen = false;
        }
    }
}
