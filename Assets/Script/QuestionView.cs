using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using QuizSystem.Models;

namespace QuizSystem.UI
{
    /// <summary>
    /// Renders the current question: number, text, image, and spawns option prefabs.
    ///
    /// Assign in Inspector:
    ///   - questionNumberText  : "Question 1 / 5"
    ///   - questionText        : the question body
    ///   - questionImage       : the question illustration
    ///   - optionContainer     : parent transform for spawned options
    ///   - optionPrefab        : OptionView prefab
    /// </summary>
    public class QuestionView : MonoBehaviour
    {
        [Header("Question UI")]
        [SerializeField] private TextMeshProUGUI questionNumberText;
        [SerializeField] private TextMeshProUGUI questionText;
        [SerializeField] private Image questionImage;

        [Header("Options")]
        [SerializeField] private Transform optionContainer;
        [SerializeField] private GameObject optionPrefab;

        private readonly List<OptionView> _activeOptions = new();
        private static readonly string[] OptionLabels = { "A", "B", "C", "D", "E" };

        // Fired when the player selects an option
        public System.Action<int> OnOptionSelected;

        // ── Public API ───────────────────────────────────────────────

        public void Display(RuntimeQuestion question, int questionNumber, int totalQuestions)
        {
            questionNumberText.text = $"Question {questionNumber} / {totalQuestions}";
            questionText.text = question.QuestionText;

            LoadImage(question.ImageName);
            SpawnOptions(question.ShuffledChoices);
        }

        /// <summary>
        /// Reveal which answer was correct and mark the player's selection.
        /// </summary>
        public void RevealResult(int selectedIndex, int correctIndex)
        {
            foreach (var option in _activeOptions)
            {
                option.Lock();

                if (option.OptionIndex == correctIndex)
                {
                    option.SetState(OptionState.Correct);
                }
                else if (option.OptionIndex == selectedIndex)
                {
                    option.SetState(OptionState.Wrong);
                }
            }
        }

        /// <summary>
        /// When time expires with no selection, just reveal the correct answer.
        /// </summary>
        public void RevealCorrectOnly(int correctIndex)
        {
            foreach (var option in _activeOptions)
            {
                option.Lock();
                if (option.OptionIndex == correctIndex)
                    option.SetState(OptionState.Correct);
            }
        }

        // ── Private helpers ──────────────────────────────────────────

        private void SpawnOptions(List<string> choices)
        {
            // Clear old options
            foreach (var old in _activeOptions)
                Destroy(old.gameObject);
            _activeOptions.Clear();

            for (int i = 0; i < choices.Count; i++)
            {
                var go = Instantiate(optionPrefab, optionContainer);
                var view = go.GetComponent<OptionView>();

                if (view == null)
                {
                    Debug.LogError("[QuestionView] optionPrefab is missing OptionView component.");
                    continue;
                }

                view.Setup(i, choices[i]);

                int captured = i; // closure-safe copy
                view.OnSelected += _ => OnOptionSelected?.Invoke(captured);

                _activeOptions.Add(view);
            }
        }

        private void LoadImage(string imageName)
        {
            if (questionImage == null) return;

            if (string.IsNullOrEmpty(imageName))
            {
                questionImage.gameObject.SetActive(false);
                return;
            }

            // Images must be placed in Assets/Resources/QuizImages/
            var sprite = Resources.Load<Sprite>($"QuizImages/{System.IO.Path.GetFileNameWithoutExtension(imageName)}");

            if (sprite != null)
            {
                questionImage.sprite = sprite;
                questionImage.gameObject.SetActive(true);
            }
            else
            {
                Debug.LogWarning($"[QuestionView] Image not found: QuizImages/{imageName}");
                questionImage.gameObject.SetActive(false);
            }
        }
    }
}