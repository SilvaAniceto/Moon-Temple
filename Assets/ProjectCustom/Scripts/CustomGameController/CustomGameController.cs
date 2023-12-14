using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace CustomGameController
{
    public enum GameState
    {
        None,
        MainTitle,
        InGamePlay
    }
    public class CustomGameController : MonoBehaviour
    {
        public static CustomGameController Instance;

        #region INSPECTOR FIELDS
        [Header("GameSettings")]
        [SerializeField] GameControllerSettings m_gameControllerSettings;
        [Space(10)]

        [SerializeField] private GameObject m_pauseMenu;
        [SerializeField] private GameObject m_mainTitle;
        #endregion

        #region PRIVATE FIELDS
        [SerializeField] private GameState m_gameState;
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

            m_gameState = GameState.MainTitle;

            UIAnimator = GetComponent<Animator>();

            InputActions = new CustomInputActions();
            InputActions.Enable();

            m_gameControllerSettings.CurrentScene = m_gameControllerSettings.GameplayScenes[0];

            SceneManager.sceneLoaded += OnLoadedScene;

            DontDestroyOnLoad(this);
        }

        void Start()
        {
            ShowPauseMenu(false);
        }
        void Update()
        {
            //Debug.Log(m_mainTitle);
            switch (m_gameState)
            {
                case GameState.None:
                    break;
                case GameState.MainTitle:
                    break;
                case GameState.InGamePlay:
                    if (InputActions.GameControllerActions.Pause.WasPressedThisFrame())
                        IsPaused = !IsPaused;
                    break;
            }
        }
        #endregion

        public void StartFadeInTransition()
        {
            UIAnimator.Play(FADE_IN_TRANSITION);
        }

        public void StartFadeOutTransition()
        {
            UIAnimator.Play(FADE_OUT_TRANSITION);
        }

        public void LoadScene()
        {
            SceneManager.LoadSceneAsync(m_gameControllerSettings.CurrentScene);
        }

        void OnLoadedScene(Scene loadedScene, LoadSceneMode sceneMode)
        {
            if (loadedScene.name != m_gameControllerSettings.MainTitleScene)
            {
                m_gameState = GameState.InGamePlay;
                m_mainTitle.SetActive(false);
            }
            else
            {
                m_gameState = GameState.MainTitle;
                m_mainTitle.SetActive(true);
            }
            StartFadeOutTransition();
        }

        #region MENU METHODS
        void ShowPauseMenu(bool state)
        {
            m_pauseMenu.SetActive(state);
        }
        public void SetCameraSensibility(float value)
        {
            m_gameControllerSettings.CharacterSettings.CameraSensibility = value;
        }
        public void SetCameraHeight(float value)
        {
            m_gameControllerSettings.CharacterSettings.CameraHeight = value;
        }
        #endregion
    }
}
