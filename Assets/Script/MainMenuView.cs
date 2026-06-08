using UnityEngine;
using UnityEngine.UI;
using QuizSystem.Core;

namespace QuizSystem.UI
{
    /// <summary>
    /// Controller for the Main Menu UI.
    /// Handles navigation to the Quiz and AR Camera scenes.
    /// </summary>
    public class MainMenuView : MonoBehaviour
    {
        [Header("── Buttons ─────────────────────────────")]
        [SerializeField] private Button quizButton;
        [SerializeField] private Button arCameraButton;

        private void Awake()
        {
            if (quizButton != null)
                quizButton.onClick.AddListener(OnQuizButtonClicked);

            if (arCameraButton != null)
                arCameraButton.onClick.AddListener(OnARCameraButtonClicked);
        }

        private void Start()
        {
            if (AudioManager.Instance != null)
                AudioManager.Instance.PlayMainMenuBGM();
        }

        private void OnQuizButtonClicked()
        {
            if (SceneLoader.Instance != null)
                SceneLoader.Instance.LoadQuiz();

        }

        private void OnARCameraButtonClicked()
        {
            if (SceneLoader.Instance != null)
                SceneLoader.Instance.LoadARCamera();

        }
    }
}
