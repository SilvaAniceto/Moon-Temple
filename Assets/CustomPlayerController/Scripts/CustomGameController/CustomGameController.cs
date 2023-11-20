using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CustomGameController
{
    public class CustomGameController : MonoBehaviour
    {
        public static CustomGameController Instance;

        [SerializeField] private GameObject PauseMenu;

        private Animator UIAnimator;
        private CustomInputActions InputActions;

        private const string FADE_IN_TRANSITION = "Fade_In_Transition";
        private const string FADE_OUT_TRANSITION = "Fade_Out_Transition";
        
        private bool isPaused;
        public bool IsPaused
        {
            get
            {
                return isPaused;
            }
            set
            {
                if (value == IsPaused) return;

                isPaused = value;

                ShowPauseMenu(isPaused);
            }
        }

        void Awake()
        {
            if (Instance == null) Instance = this;

            UIAnimator = GetComponent<Animator>();

            InputActions = new CustomInputActions();
            InputActions.Enable();

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

        void PlayTrasition(bool state)
        {
            if (state)
                UIAnimator.Play(FADE_OUT_TRANSITION);
            else
                UIAnimator.Play(FADE_IN_TRANSITION);
        }

        void ShowPauseMenu(bool state)
        {
            PauseMenu.SetActive(state);

            Time.timeScale = state ? 0f : 1f;
        }
    }
}
