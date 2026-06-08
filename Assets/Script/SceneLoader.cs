using UnityEngine;
using UnityEngine.SceneManagement;

namespace QuizSystem.Core
{
    /// <summary>
    /// Global manager for scene transitions.
    /// Follows the Singleton pattern to be accessible from anywhere.
    /// </summary>
    public class SceneLoader : MonoBehaviour
    {
        public static SceneLoader Instance { get; private set; }

        [Header("Scene Names")]
        [SerializeField] private string mainMenuScene = "MainMenu";
        [SerializeField] private string quizScene = "QuizScene";
        [SerializeField] private string arCameraScene = "ARCameraScene";

        private void Awake()
        {
            // Singleton Setup
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        // ── Public API ───────────────────────────────────────────────

        public void LoadMainMenu()
        {
            LoadScene(mainMenuScene);
        }

        public void LoadQuiz()
        {
            LoadScene(quizScene);
        }

        public void LoadARCamera()
        {
            LoadScene(arCameraScene);
        }

        public void LoadScene(string sceneName)
        {
            if (string.IsNullOrEmpty(sceneName))
            {
                Debug.LogError("[SceneLoader] Scene name is empty!");
                return;
            }

            SceneManager.LoadScene(sceneName);
        }

        public void ReloadCurrentScene()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        public void QuitGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif
        }
    }
}
