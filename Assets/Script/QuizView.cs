using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using QuizSystem.Core;
using QuizSystem.Models;
using QuizSystem.UI;

namespace QuizSystem.UI
{
    /// <summary>
    /// Handles the visual representation of a question and its options.
    /// </summary>
    public class QuizView : MonoBehaviour
    {
        [Header("── Question UI ──────────────────────────")]
        [SerializeField] private TextMeshProUGUI questionNumberText;
        [SerializeField] private TextMeshProUGUI questionBodyText;
        [SerializeField] private Image questionImage;
        [SerializeField] private GameObject headerPanel;
        [SerializeField] private GameObject spacer;
        [SerializeField] private LayoutElement imageWrapperLayout;
        [SerializeField] private RectTransform circleTransform;

        [Header("── Options ─────────────────────────────")]
        [SerializeField] private Transform optionContainer;
        [SerializeField] private GameObject optionPrefab;

        [Header("── Navigation ──────────────────────────")]
        [SerializeField] private Button nextButton;

        public event Action OnNextClicked;
        public event Action<int> OnOptionSelected;

        private readonly List<OptionView> _spawnedOptions = new();

        private void Awake()
        {
            if (nextButton != null)
                nextButton.onClick.AddListener(() => OnNextClicked?.Invoke());
        }

        public void DisplayQuestion(int index, int total, RuntimeQuestion question)
        {
            // Update Text
            questionNumberText.text = $"{index + 1} / {total}";
            questionBodyText.text = question.QuestionText;

            // Update Image
            UpdateQuestionImage(question.ImageName);

            // Setup Layout
            SetResultState(false);

            // Spawn Options
            SpawnOptions(question);
        }

        public void SetResultState(bool isRevealed)
        {
            if (headerPanel != null) headerPanel.SetActive(!isRevealed);
            if (spacer != null) spacer.SetActive(!isRevealed);
            nextButton.gameObject.SetActive(isRevealed);

            // Adjust image wrapper and circle size
            if (imageWrapperLayout != null)
            {
                imageWrapperLayout.preferredHeight = isRevealed ? 250f : 350f;
            }

            if (circleTransform != null)
            {
                float width = isRevealed ? 250f : 350f;
                circleTransform.sizeDelta = new Vector2(width, 0f);
            }
        }

        public void LockOptions()
        {
            foreach (var option in _spawnedOptions)
            {
                option.Lock();
            }
        }

        public void HighlightOption(int index, bool correct)
        {
            foreach (var option in _spawnedOptions)
            {
                option.Lock();
                if (option.OptionIndex == index)
                {
                    option.SetState(correct ? OptionState.Correct : OptionState.Wrong);
                }
            }
        }

        private void SpawnOptions(RuntimeQuestion question)
        {
            ClearOptions();

            for (int i = 0; i < question.ShuffledChoices.Count; i++)
            {
                var go = Instantiate(optionPrefab, optionContainer);
                var view = go.GetComponent<OptionView>();

                if (view == null)
                {
                    Debug.LogError("[QuizView] optionPrefab is missing OptionView component.");
                    continue;
                }

                view.Setup(i, question.ShuffledChoices[i]);

                int captured = i;
                view.OnSelected += _ => OnOptionSelected?.Invoke(captured);

                _spawnedOptions.Add(view);
            }
        }

        private void ClearOptions()
        {
            foreach (var old in _spawnedOptions)
            {
                if (old != null) Destroy(old.gameObject);
            }
            _spawnedOptions.Clear();
        }

        private void UpdateQuestionImage(string imageName)
        {
            if (questionImage == null) return;

            if (string.IsNullOrEmpty(imageName))
            {
                questionImage.gameObject.SetActive(false);
                return;
            }

            string nameNoExt = System.IO.Path.GetFileNameWithoutExtension(imageName);
            string dir = System.IO.Path.GetDirectoryName(imageName);
            Sprite sprite = null;

            if (!string.IsNullOrEmpty(dir))
            {
                // If the JSON includes a folder path (e.g., "Tengkorak/image.png")
                sprite = Resources.Load<Sprite>($"QuizImages/{dir}/{nameNoExt}");
            }
            else
            {
                // Search in root and all known subfolders
                string[] searchFolders = new string[]
                {
                    "",
                    "Tengkorak",
                    "Tulang Dada",
                    "Tulang Kaki",
                    "Tulang Pelvis",
                    "Tulang Punggung",
                    "Tulang Tangan"
                };

                foreach (string folder in searchFolders)
                {
                    string path = string.IsNullOrEmpty(folder)
                        ? $"QuizImages/{nameNoExt}"
                        : $"QuizImages/{folder}/{nameNoExt}";

                    sprite = Resources.Load<Sprite>(path);
                    if (sprite != null) break;
                }
            }

            if (sprite != null)
            {
                questionImage.sprite = sprite;
                questionImage.gameObject.SetActive(true);
            }
            else
            {
                Debug.LogWarning($"[QuizView] Image not found in any QuizImages folders: {nameNoExt}");
                questionImage.gameObject.SetActive(false);
            }
        }
    }
}
