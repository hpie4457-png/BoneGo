using UnityEngine;
using UnityEngine.UI;
using System;

namespace QuizSystem.UI
{
    /// <summary>
    /// Handles the pause menu UI and state.
    /// </summary>
    public class QuizPauseManager : MonoBehaviour
    {
        [SerializeField] private GameObject pauseCanvas;
        [SerializeField] private Button pauseButton;
        [SerializeField] private Button resumeButton;
        [SerializeField] private Button exitButton;

        public event Action<bool> OnPauseToggled;
        public event Action OnExitClicked;

        private void Awake()
        {
            if (pauseButton != null) pauseButton.onClick.AddListener(() => TogglePause(true));
            if (resumeButton != null) resumeButton.onClick.AddListener(() => TogglePause(false));
            if (exitButton != null) exitButton.onClick.AddListener(() => OnExitClicked?.Invoke());

            if (pauseCanvas != null)
                pauseCanvas.SetActive(false);
        }

        public void TogglePause(bool pause)
        {
            if (pauseCanvas != null)
                pauseCanvas.SetActive(pause);
            
            OnPauseToggled?.Invoke(pause);
        }

        public void SetPauseButtonActive(bool active)
        {
            if (pauseButton != null)
                pauseButton.gameObject.SetActive(active);
        }
    }
}
