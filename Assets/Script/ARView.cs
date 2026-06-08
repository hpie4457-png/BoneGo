using UnityEngine;
using UnityEngine.UI;
using QuizSystem.Core;

namespace QuizSystem.UI
{
    /// <summary>
    /// Controller for the AR View.
    /// Handles navigation back to the Main Menu.
    /// </summary>
    public class ARView : MonoBehaviour
    {
        [Header("── UI Buttons ──────────────────────────")]
        [SerializeField] private Button homeButton;

        private void Awake()
        {
            if (homeButton != null)
            {
                homeButton.onClick.AddListener(OnHomeButtonClicked);
            }
            else
            {
                Debug.LogWarning("[ARView] Home Button is not assigned in the inspector.");
            }
        }

        private void Start()
        {
            if (AudioManager.Instance != null)
            {
                // Smoothly lower the background volume in AR Scene so TTS is clearly audible (1 second fade)
                AudioManager.Instance.SetBGMVolume(0.15f, 1.0f);
            }
        }

        private void OnDestroy()
        {
            if (homeButton != null)
            {
                homeButton.onClick.RemoveListener(OnHomeButtonClicked);
            }
            
            if (AudioManager.Instance != null)
            {
                // Smoothly restore normal background volume when exiting AR Scene (1 second fade)
                AudioManager.Instance.SetBGMVolume(1.0f, 1.0f);
            }
        }

        private void OnHomeButtonClicked()
        {
            if (SceneLoader.Instance != null)
            {
                SceneLoader.Instance.LoadMainMenu();
            }
            else
            {
                Debug.LogWarning("[ARView] SceneLoader.Instance not found. Falling back to direct scene loading.");
                UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
            }
        }
    }
}
