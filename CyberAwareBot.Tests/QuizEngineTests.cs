using CybersecurityChatbot;
using Xunit;

namespace CyberAwareBot.Tests;

public class QuizEngineTests
{
    [Fact]
    public void Start_ResetsScoreAndActivatesQuiz()
    {
        var quiz = new QuizEngine();

        quiz.Start();

        Assert.True(quiz.IsActive);
        Assert.Equal(0, quiz.Score);
        Assert.NotNull(quiz.CurrentQuestion);
    }

    [Fact]
    public void SubmitAnswer_IncrementsScoreForCorrectAnswer()
    {
        var quiz = new QuizEngine();
        quiz.Start();

        int correctIndex = quiz.CurrentQuestion!.CorrectOptionIndex;
        QuizResult result = quiz.SubmitAnswer(correctIndex);

        Assert.True(result.WasCorrect);
        Assert.Equal(1, quiz.Score);
    }
}
