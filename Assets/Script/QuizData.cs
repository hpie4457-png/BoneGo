using System.Collections.Generic;

namespace QuizSystem.Models
{
    [System.Serializable]
    public class QuizData
    {
        public List<QuestionData> questions;
    }

    [System.Serializable]
    public class QuestionData
    {
        public int id;
        public string question;
        public List<string> choices;
        public int correctAnswerIndex; // 0-based index in the original choices list
        public string image;
    }
}