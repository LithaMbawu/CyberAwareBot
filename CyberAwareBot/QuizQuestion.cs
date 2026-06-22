namespace CybersecurityChatbot
{
    public sealed class QuizQuestion
    {
        public string QuestionText { get; set; } = string.Empty;

        public string[] Options { get; set; } = System.Array.Empty<string>();

        public int CorrectOptionIndex { get; set; }

        public string Explanation { get; set; } = string.Empty;
    }

    public sealed class QuizResult
    {
        public bool WasCorrect { get; set; }

        public string Feedback { get; set; } = string.Empty;

        public bool IsFinished { get; set; }
    }
}
