using UnityEngine;

namespace QuizSystem.UI
{
    public enum AlertType { Correct, Wrong, Warning }

    /// <summary>
    /// Handles spawning and clearing of feedback alerts (Correct, Wrong, Time Expired).
    /// </summary>
    public class QuizAlertManager : MonoBehaviour
    {
        [Header("Containers")]
        [SerializeField] private Transform alertContainer;

        [Header("Prefabs")]
        [SerializeField] private GameObject correctPrefab;
        [SerializeField] private GameObject wrongPrefab;
        [SerializeField] private GameObject warningPrefab;

        public void ShowAlert(AlertType type)
        {
            ClearAlerts();

            if (alertContainer == null) return;
            alertContainer.gameObject.SetActive(true);

            GameObject prefab = type switch
            {
                AlertType.Correct => correctPrefab,
                AlertType.Wrong => wrongPrefab,
                AlertType.Warning => warningPrefab,
                _ => null
            };

            if (prefab != null)
            {
                Instantiate(prefab, alertContainer);
            }
        }

        public void ClearAlerts()
        {
            if (alertContainer == null) return;
            
            foreach (Transform child in alertContainer)
            {
                Destroy(child.gameObject);
            }
            
            alertContainer.gameObject.SetActive(false);
        }
    }
}
