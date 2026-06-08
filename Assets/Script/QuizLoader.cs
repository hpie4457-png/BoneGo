using System.Collections.Generic;
using UnityEngine;
using QuizSystem.Models;

namespace QuizSystem.Core
{
    /// <summary>
    /// Pure C# helper (not a MonoBehaviour).
    /// Loads questions.json from Resources/Quiz/ and returns a
    /// shuffled list of RuntimeQuestions ready for gameplay.
    ///
    /// Place your JSON at: Assets/Resources/Quiz/questions.json
    /// </summary>
    public class QuizLoader
    {
        private const string ResourcePath = "Quiz";

        public List<RuntimeQuestion> LoadAndShuffle(int maxQuestions)
        {
            var raw = LoadJson();
            if (raw?.questions == null || raw.questions.Count == 0)
            {
                Debug.LogError("[QuizLoader] No questions found. Check Resources/Quiz.json");
                return null;
            }

            var list = BuildRuntimeQuestions(raw.questions);

            // Shuffle all questions first
            Shuffle(list);

            // Then take only the requested amount (up to 10 by default)
            if (list.Count > maxQuestions)
            {
                list = list.GetRange(0, maxQuestions);
            }

            return list;
        }

        // ── Private ──────────────────────────────────────────────────

        private QuizData LoadJson()
        {
            var asset = Resources.Load<TextAsset>(ResourcePath);
            if (asset == null)
            {
                Debug.LogError($"[QuizLoader] File not found at Resources/{ResourcePath}");
                return null;
            }
            return JsonUtility.FromJson<QuizData>(asset.text);
        }

        private List<RuntimeQuestion> BuildRuntimeQuestions(List<QuestionData> raw)
        {
            var result = new List<RuntimeQuestion>(raw.Count);
            foreach (var data in raw)
            {
                if (data.choices == null || data.choices.Count == 0)
                {
                    Debug.LogWarning($"[QuizLoader] Question id={data.id} has no choices – skipped.");
                    continue;
                }

                int clampedCorrect = Mathf.Clamp(data.correctAnswerIndex, 0, data.choices.Count - 1);
                string correctText = data.choices[clampedCorrect];

                var shuffled = new List<string>(data.choices);
                Shuffle(shuffled);

                int newCorrectIndex = shuffled.IndexOf(correctText);
                result.Add(new RuntimeQuestion(data, shuffled, newCorrectIndex));
            }
            return result;
        }

        private void Shuffle<T>(List<T> list)
        {
            for (int i = list.Count - 1; i > 0; i--)
            {
                int j = Random.Range(0, i + 1);
                (list[i], list[j]) = (list[j], list[i]);
            }
        }
    }
}