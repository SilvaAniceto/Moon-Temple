using UnityEngine;
using UnityEngine.UI;

namespace CustomGameController
{
    public class UISliderItem : MonoBehaviour
    {
        public Slider ItemSlider
        { 
            get
            {
                return GetComponentInChildren<Slider>();
            }
        }
        public Animator ItemAnimator
        {
            get
            {
                return GetComponent<Animator>();
            }
        }
        public bool IsActive { get; set; }

        public void ActiveAnimation()
        {
            IsActive = true;
            ItemAnimator.Play("UISliderItem_Selected");
        }
        public void DeactiveAnimation()
        {
            IsActive = false;
            ItemAnimator.Play("UISliderItem_Deselected");
        }
    }
}
