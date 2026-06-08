using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

namespace QuizSystem.UI
{
    /// <summary>
    /// Handles the final score display.
    /// </summary>
    public class QuizResultView : MonoBehaviour
    {
        [SerializeField] private GameObject scoreCanvas;
        [SerializeField] private TextMeshProUGUI scoreText;
        [SerializeField] private Button homeButton;

        public event Action OnHomeClicked;

        private void Awake()
        {
            if (homeButton != null)
                homeButton.onClick.AddListener(() => OnHomeClicked?.Invoke());
            
            if (scoreCanvas != null)
                scoreCanvas.SetActive(false);
        }

        public void ShowResult(int score)
        {
            if (scoreCanvas != null)
            {
                scoreCanvas.SetActive(true);
                if (scoreText != null)
                {
                    scoreText.text = score.ToString();
                }
            }
        }

        public void Hide()
        {
            if (scoreCanvas != null)
                scoreCanvas.SetActive(false);
        }
    }
}
