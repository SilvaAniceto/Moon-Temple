using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CustomGameController
{
    public class CustomGameController : MonoBehaviour
    {
        public static CustomGameController Instance;

        #region INSPECTOR FIELDS
        [SerializeField] GameControllerSettings m_gameControllerSettings;

        [SerializeField] private GameObject m_pauseMenu;
        [SerializeField] private Slider m_sensibilitySlider;
        [SerializeField] private Slider m_heightSlider;

        [SerializeField] private float s;
        [SerializeField] private float h;
        #endregion

        #region PRIVATE FIELDS
        private static Animator UIAnimator;
        private bool m_isPaused;

        private CustomInputActions InputActions;
        #endregion

        #region ANIMATION FIELDS
        private const string FADE_IN_TRANSITION = "Fade_In_Transition";
        private const string FADE_OUT_TRANSITION = "Fade_Out_Transition";
        #endregion

        #region PROPERTIES
        public bool IsPaused
        {
            get
            {
                return m_isPaused;
            }
            set
            {
                if (value == IsPaused) return;

                m_isPaused = value;

                ShowPauseMenu(m_isPaused);
            }
        }
        #endregion

        #region DEFAULT METHODS
        void Awake()
        {
            if (Instance == null) Instance = this;

            UIAnimator = GetComponent<Animator>();

            InputActions = new CustomInputActions();
            InputActions.Enable();

            m_sensibilitySlider.onValueChanged.AddListener(SetCameraSensibility);
            m_heightSlider.onValueChanged.AddListener(SetCameraHeight);

            DontDestroyOnLoad(this);
        }
        void Start()
        {
            ShowPauseMenu(false);
        }
        void Update()
        {
            if (InputActions.GameControllerActions.Pause.WasPressedThisFrame())
                IsPaused = !IsPaused;
        }
        #endregion

        #region MENU METHODS
        void ShowPauseMenu(bool state)
        {
            m_pauseMenu.SetActive(state);

            Time.timeScale = state ? 0f : 1f;
        }
        void SetCameraSensibility(float value)
        {
            s = value;
        }
        void SetCameraHeight(float value)
        {
            h = value;
        }
        #endregion
    }
}
