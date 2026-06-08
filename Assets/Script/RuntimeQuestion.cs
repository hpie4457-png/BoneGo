using System.Collections.Generic;

namespace QuizSystem.Models
{
    /// <summary>
    /// A question ready for gameplay — choices are already shuffled,
    /// and CorrectAnswerIndex reflects the new position after shuffling.
    /// </summary>
    public class RuntimeQuestion
    {
        public int Id { get; private set; }
        public string QuestionText { get; private set; }
        public string ImageName { get; private set; }
        public List<string> ShuffledChoices { get; private set; }
        public int CorrectAnswerIndex { get; private set; }

        public RuntimeQuestion(QuestionData source, List<string> shuffledChoices, int correctIndex)
        {
            Id = source.id;
            QuestionText = source.question;
            ImageName = source.image;
            ShuffledChoices = shuffledChoices;
            CorrectAnswerIndex = correctIndex;
        }

        public bool IsCorrect(int selectedIndex) => selectedIndex == CorrectAnswerIndex;
    }
}