using CybersecurityChatbot;
using Xunit;

namespace CyberAwareBot.Tests;

public class ChatBotTests
{
    [Fact]
    public void ProcessInput_ReturnsPasswordGuidance()
    {
        var bot = new ChatBot();

        string response = bot.ProcessInput("How do I make a strong password?");

        Assert.Contains("password", response, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void ProcessInput_StoresNameAndRecallsConversation()
    {
        var bot = new ChatBot();

        bot.ProcessInput("My name is Alex");
        string recall = bot.ProcessInput("What did we talk about?");

        Assert.Contains("Alex", recall, StringComparison.OrdinalIgnoreCase);
    }
}
