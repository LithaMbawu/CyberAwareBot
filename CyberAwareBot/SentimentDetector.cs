using System;
using System.Linq;

namespace CybersecurityChatbot
{
    /// <summary>
    /// Analyzes sentiment in user input to provide dynamic, mood-aware responses.
    /// </summary>
    public class SentimentDetector
    {
        private string[] positiveWords = { "good", "great", "excellent", "happy", "safe", "secure", "thanks", "thank" };
        private string[] negativeWords = { "bad", "worried", "scared", "unsafe", "hacked", "problem", "issue", "help" };
        private string[] neutralWords = { "what", "how", "why", "tell", "explain" };

        public SentimentDetector()
        {
        }

        /// <summary>
        /// Analyzes the sentiment of the input text.
        /// </summary>
        /// <param name="text">The text to analyze.</param>
        /// <returns>Sentiment type: Positive, Negative, or Neutral.</returns>
        public string AnalyzeSentiment(string text)
        {
            string lowerText = text.ToLower();
            int positiveCount = positiveWords.Count(word => lowerText.Contains(word));
            int negativeCount = negativeWords.Count(word => lowerText.Contains(word));
            int neutralCount = neutralWords.Count(word => lowerText.Contains(word));

            if (positiveCount >= negativeCount + 2)
                return "Positive";
            else if (negativeCount >= positiveCount + 2)
                return "Negative";
            else if (negativeCount > positiveCount)
                return "Concerned";
            else
                return "Neutral";
        }

        /// <summary>
        /// Gets a sentiment-based response prefix.
        /// </summary>
        /// <param name="sentiment">The detected sentiment.</param>
        /// <returns>An opening phrase for mood-aware responses.</returns>
        public string GetSentimentPrefix(string sentiment)
        {
            switch (sentiment)
            {
                case "Positive":
                    return "That’s great to hear! ";
                case "Negative":
                    return "I can help you through that. ";
                case "Concerned":
                    return "I understand your concern. ";
                default:
                    return string.Empty;
            }
        }

        /// <summary>
        /// Gets a sentiment-based response modifier.
        /// </summary>
        /// <param name="sentiment">The detected sentiment.</param>
        /// <returns>A string to append for mood-aware responses.</returns>
        public string GetSentimentModifier(string sentiment)
        {
            switch (sentiment)
            {
                case "Positive":
                    return " Keep up the good security habits.";
                case "Negative":
                    return " Let’s review a safety step together.";
                case "Concerned":
                    return " Small changes can make a big difference.";
                default:
                    return " I’m here when you want to explore another cybersecurity topic.";
            }
        }
    }
}

