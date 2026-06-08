using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace QuizSystem.Core
{
    /// <summary>
    /// Countdown timer that drives a UI Slider and a TMP text label.
    /// Fires events on tick and on expiry so other systems can react.
    /// 
    /// Assign in Inspector:
    ///   - timerSlider   : the Slider component
    ///   - timerText     : TextMeshProUGUI showing remaining seconds
    ///   - questionTime  : total seconds per question (default 15)
    /// </summary>
    public class QuizTimer : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private Slider timerSlider;
        [SerializeField] private TextMeshProUGUI timerText;

        [Header("Settings")]
        [SerializeField] private float questionTime = 15f;

        // Subscibe to know when time runs out
        public event Action OnTimeExpired;

        private float _remaining;
        private bool _isRunning;

        // ── Public API ───────────────────────────────────────────────

        public void StartTimer()
        {
            _remaining = questionTime;
            _isRunning = true;
            UpdateUI();
        }

        public void StopTimer()
        {
            _isRunning = false;
        }

        public void ResumeTimer()
        {
            _isRunning = true;
        }

        public void ResetTimer()
        {
            StopTimer();
            _remaining = questionTime;
            UpdateUI();
        }

        // ── Unity lifecycle ──────────────────────────────────────────

        private void Update()
        {
            if (!_isRunning) return;

            _remaining -= Time.deltaTime;

            if (_remaining <= 0f)
            {
                _remaining = 0f;
                _isRunning = false;
                UpdateUI();
                OnTimeExpired?.Invoke();
                return;
            }

            UpdateUI();
        }

        // ── Private helpers ──────────────────────────────────────────

        private void UpdateUI()
        {
            if (timerSlider != null)
                timerSlider.value = _remaining / questionTime;

            if (timerText != null)
                timerText.text = Mathf.CeilToInt(_remaining).ToString();
        }
    }
}