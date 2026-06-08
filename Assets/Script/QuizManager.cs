using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using QuizSystem.Core;
using QuizSystem.Models;
using QuizSystem.UI;

namespace QuizSystem
{
    /// <summary>
    /// The Orchestrator of the Quiz system. 
    /// Coordinates between data, UI components, and game state.
    /// </summary>
    public class QuizManager : MonoBehaviour
    {
        [Header("── Components ──────────────────────────")]
        [SerializeField] private QuizView quizView;
        [SerializeField] private QuizAlertManager alertManager;
        [SerializeField] private QuizResultView resultView;
        [SerializeField] private QuizPauseManager pauseManager;
        [SerializeField] private QuizTimer quizTimer;

        [Header("── Settings ────────────────────────────")]
        [SerializeField] private GameObject quizCanvas;
        [SerializeField] private int maxQuestions = 10;

        // ═══════════════════════════════════════════════════════════════
        // Private state
        // ═══════════════════════════════════════════════════════════════

        private List<RuntimeQuestion> _questions;
        private int _currentIndex;
        private int _score;
        private bool _answerLocked;

        // ═══════════════════════════════════════════════════════════════
        // Unity lifecycle
        // ═══════════════════════════════════════════════════════════════

        private void Awake()
        {
            // Wire up UI events
            if (quizView != null)
            {
                quizView.OnOptionSelected += HandleOptionSelected;
                quizView.OnNextClicked += HandleNextClicked;
            }

            if (pauseManager != null)
            {
                pauseManager.OnPauseToggled += HandlePauseToggled;
                pauseManager.OnExitClicked += ExitQuiz;
            }

            if (resultView != null)
            {
                resultView.OnHomeClicked += ExitQuiz;
            }

            if (quizTimer != null)
            {
                quizTimer.OnTimeExpired += HandleTimeExpired;
            }
        }

        private void Start()
        {
            // Transition from Main Menu music to Quiz music
            if (AudioManager.Instance != null)
            {
                Debug.Log("Playing Quiz BGM");
                AudioManager.Instance.StopBGM(0.5f);
                AudioManager.Instance.PlayQuizBGM(0.5f);
            }

            LoadQuestions();
            ShowQuestion(_currentIndex);
        }

        // ═══════════════════════════════════════════════════════════════
        // Quiz flow
        // ═══════════════════════════════════════════════════════════════

        private void LoadQuestions()
        {
            var loader = new QuizLoader();
            _questions = loader.LoadAndShuffle(maxQuestions);

            if (_questions == null || _questions.Count == 0)
                Debug.LogError("[QuizManager] No questions loaded – check Resources/Quiz/questions.json");
        }

        private void ShowQuestion(int index)
        {
            if (_questions == null || index >= _questions.Count) return;

            _answerLocked = false;
            
            // UI Reset
            alertManager.ClearAlerts();
            quizView.DisplayQuestion(index, _questions.Count, _questions[index]);

            // Timer
            if (quizTimer != null)
            {
                quizTimer.StartTimer();
            }
        }

        private void HandleOptionSelected(int selectedIndex)
        {
            if (_answerLocked) return;
            _answerLocked = true;

            if (quizTimer != null) quizTimer.StopTimer();

            var q = _questions[_currentIndex];
            bool correct = q.IsCorrect(selectedIndex);

            if (correct)
            {
                _score += 10;
                AudioManager.Instance?.PlayCorrectSFX();
            }
            else
            {
                AudioManager.Instance?.PlayWrongSFX();
            }

            // Update View
            quizView.HighlightOption(selectedIndex, correct);
            quizView.SetResultState(true);

            // Show Alert
            alertManager.ShowAlert(correct ? AlertType.Correct : AlertType.Wrong);
        }

        private void HandleTimeExpired()
        {
            if (_answerLocked) return;
            _answerLocked = true;

            quizView.LockOptions();
            quizView.SetResultState(true);

            alertManager.ShowAlert(AlertType.Warning);
        }

        private void HandleNextClicked()
        {
            _currentIndex++;

            if (_currentIndex >= _questions.Count)
            {
                ShowResults();
                return;
            }

            ShowQuestion(_currentIndex);
        }

        // ═══════════════════════════════════════════════════════════════
        // Pause / Navigation
        // ═══════════════════════════════════════════════════════════════

        private void HandlePauseToggled(bool isPaused)
        {
            // Don't allow pausing if answer is already locked (result state)
            if (isPaused && _answerLocked)
            {
                pauseManager.TogglePause(false);
                return;
            }

            if (isPaused)
                quizTimer?.StopTimer();
            else if (!_answerLocked)
                quizTimer?.ResumeTimer();

            if (quizCanvas != null) quizCanvas.SetActive(!isPaused);
        }

        private void ShowResults()
        {
            if (quizTimer != null) quizTimer.StopTimer();
            if (quizCanvas != null) quizCanvas.SetActive(false);
            
            resultView.ShowResult(_score);
        }

        private void ExitQuiz()
        {
            if (SceneLoader.Instance != null)
                SceneLoader.Instance.LoadMainMenu();
            else
                SceneManager.LoadScene("MainMenu"); // Fallback
        }
    }
}