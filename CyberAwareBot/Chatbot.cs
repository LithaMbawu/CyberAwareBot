using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace CybersecurityChatbot
{
    /// <summary>
    /// Main chatbot class that orchestrates conversation flow with memory, sentiment, and keyword responses.
    /// </summary>
    public class ChatBot
    {
        private MemoryStore memory;
        private SentimentDetector sentimentDetector;
        private KeywordResponder keywordResponder;

        public ChatBot()
        {
            memory = new MemoryStore();
            sentimentDetector = new SentimentDetector();
            keywordResponder = new KeywordResponder();
        }

        /// <summary>
        /// Gets the initial greeting message.
        /// </summary>
        /// <returns>Greeting string.</returns>
        public string GetGreeting()
        {
            string? name = memory.GetPreference("name");
            string greeting = string.IsNullOrWhiteSpace(name)
                ? "Hello! I'm your Cybersecurity Awareness Chatbot. I'm here to help you learn about staying safe online. What would you like to know?"
                : $"Hello {name}! I'm your Cybersecurity Awareness Chatbot. I'm ready to help you stay secure today. What would you like to know?";

            memory.AddToHistory("Bot: " + greeting);
            return greeting;
        }

        /// <summary>
        /// Processes user input and generates a response.
        /// </summary>
        /// <param name="input">User's message.</param>
        /// <returns>Bot's response.</returns>
        public string ProcessInput(string input)
        {
            memory.AddToHistory("User: " + input);

            string sentiment = sentimentDetector.AnalyzeSentiment(input);
            string response = GenerateResponse(input, sentiment);

            memory.AddToHistory("Bot: " + response);
            return response;
        }

        /// <summary>
        /// Generates a response based on input and sentiment.
        /// </summary>
        /// <param name="input">User input.</param>
        /// <param name="sentiment">Detected sentiment.</param>
        /// <returns>Generated response.</returns>
        private string GenerateResponse(string input, string sentiment)
        {
            string lowerInput = input.ToLowerInvariant();
            string userName = memory.GetPreference("name") ?? string.Empty;
            string response;

            if (TryStoreUserName(input, out string extractedName))
            {
                response = $"Nice to meet you, {extractedName}! I’ll remember your name so our conversation feels more personal. ";
            }
            else if (lowerInput.Contains("remember") || lowerInput.Contains("recall") || lowerInput.Contains("history") || lowerInput.Contains("previous"))
            {
                response = GetMemoryRecallResponse();
            }
            else if (keywordResponder.HasResponse("password") && lowerInput.Contains("password"))
            {
                response = keywordResponder.GetResponse("password");
            }
            else if (keywordResponder.HasResponse("phishing") && lowerInput.Contains("phishing"))
            {
                response = keywordResponder.GetResponse("phishing");
            }
            else if (keywordResponder.HasResponse("malware") && (lowerInput.Contains("malware") || lowerInput.Contains("virus")))
            {
                response = keywordResponder.GetResponse("malware");
            }
            else if (keywordResponder.HasResponse("vpn") && lowerInput.Contains("vpn"))
            {
                response = keywordResponder.GetResponse("vpn");
            }
            else if (keywordResponder.HasResponse("backup") && lowerInput.Contains("backup"))
            {
                response = keywordResponder.GetResponse("backup");
            }
            else if (keywordResponder.HasResponse("update") && lowerInput.Contains("update"))
            {
                response = keywordResponder.GetResponse("update");
            }
            else if (lowerInput.Contains("hello") || lowerInput.Contains("hi") || lowerInput.Contains("hey"))
            {
                response = keywordResponder.GetResponse("hello");
            }
            else if (lowerInput.Contains("thank"))
            {
                response = keywordResponder.GetResponse("thanks");
            }
            else if (lowerInput.Contains("how are you") || lowerInput.Contains("how are u") || lowerInput.Contains("how r u"))
            {
                response = keywordResponder.GetResponse("howareyou");
            }
            else
            {
                response = keywordResponder.GetResponse("fallback");
            }

            return BuildNaturalResponse(response, sentiment, userName);
        }

        /// <summary>
        /// Adds a friendly sentiment-aware layer to the chatbot response.
        /// </summary>
        /// <param name="response">The base response text.</param>
        /// <param name="sentiment">Detected sentiment.</param>
        /// <param name="userName">User's name if known.</param>
        /// <returns>Final response string.</returns>
        private string BuildNaturalResponse(string response, string sentiment, string userName)
        {
            string prefix = sentimentDetector.GetSentimentPrefix(sentiment);
            string suffix = sentimentDetector.GetSentimentModifier(sentiment);
            string namePrefix = string.IsNullOrWhiteSpace(userName) ? string.Empty : $"{userName}, ";

            return string.Concat(namePrefix, prefix, response, suffix);
        }

        /// <summary>
        /// Stores the user's name if it is present in the input.
        /// </summary>
        /// <param name="input">User input text.</param>
        /// <param name="name">Extracted user name.</param>
        /// <returns>True if a name was extracted and stored.</returns>
        private bool TryStoreUserName(string input, out string name)
        {
            name = string.Empty;
            var match = Regex.Match(input, "\\b(?:my name is|call me|i am|im)\\s+([A-Za-z][A-Za-z0-9_-]{1,19})", RegexOptions.IgnoreCase);
            if (!match.Success)
                return false;

            name = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(match.Groups[1].Value.ToLower());
            memory.SetPreference("name", name);
            memory.AddToHistory($"Bot: Stored user name as {name}.");
            return true;
        }

        /// <summary>
        /// Generates a response that recalls recent conversation topics.
        /// </summary>
        /// <returns>A memory-aware response.</returns>
        private string GetMemoryRecallResponse()
        {
            var topics = memory.GetRecentTopics(4);
            if (topics.Count == 0)
            {
                return "We are just getting started. Tell me what cybersecurity topic you'd like to explore first.";
            }

            string topicList = string.Join(", ", topics);
            return $"So far we’ve talked about {topicList}. What would you like to focus on next?";
        }

        /// <summary>
        /// Retrieves recent conversation history.
        /// </summary>
        /// <returns>Formatted history string.</returns>
        public string GetConversationHistory()
        {
            var history = memory.GetRecentHistory();
            return string.Join("\n", history);
        }
    }
}
