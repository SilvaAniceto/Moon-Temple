using UnityEngine;
using UnityEngine.UI;

namespace IsometricOrientedPerspective
{
    public class UICameraController : UIPopUpController
    {
        [SerializeField] private Slider UICameraSensibility, UIZoomSensibility, UIZoomMultiplier, UIVerticalOffSet;

        new void Awake()
        {
            base.Awake();

            UICameraSensibility.onValueChanged.AddListener((float p_sensibility) =>
            {
                IsometricCamera.m_instance.Sensibility = p_sensibility;
            });

            UIZoomSensibility.onValueChanged.AddListener((float p_zoom) =>
            {
                IsometricCamera.m_instance.ZoomSensibility = p_zoom;
            });

            UIZoomMultiplier.onValueChanged.AddListener((float p_multiplier) =>
            {
                IsometricCamera.m_instance.ZoomMultiplier = (int)p_multiplier;
            });

            UIVerticalOffSet.onValueChanged.AddListener((float p_offset) =>
            {
                IsometricCamera.m_instance.VerticalOffset = p_offset;
            });
        }

        private void Start()
        {
            UICameraSensibility.value = IsometricCamera.m_instance.Sensibility;
            UIZoomSensibility.value = IsometricCamera.m_instance.ZoomSensibility;
            UIZoomMultiplier.value = IsometricCamera.m_instance.ZoomMultiplier;
            UIVerticalOffSet.value = IsometricCamera.m_instance.VerticalOffset;

            if (isOpen)
                Hide();
        }
    }
}
