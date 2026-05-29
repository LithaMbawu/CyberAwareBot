using System;
using System.Collections.Generic;

namespace CybersecurityChatbot
{
    /// <summary>
    /// Manages conversation memory for consistent recall and engagement.
    /// Stores chat history and user preferences.
    /// </summary>
    public class MemoryStore
    {
        private List<string> conversationHistory;
        private Dictionary<string, string> userPreferences;

        public MemoryStore()
        {
            conversationHistory = new List<string>();
            userPreferences = new Dictionary<string, string>();
        }

        /// <summary>
        /// Adds a message to the conversation history.
        /// </summary>
        /// <param name="message">The message to add.</param>
        public void AddToHistory(string message)
        {
            conversationHistory.Add($"{DateTime.Now}: {message}");
            if (conversationHistory.Count > 100) // Limit history size
            {
                conversationHistory.RemoveAt(0);
            }
        }

        /// <summary>
        /// Retrieves the recent conversation history.
        /// </summary>
        /// <returns>List of recent messages.</returns>
        public List<string> GetRecentHistory()
        {
            return conversationHistory.GetRange(Math.Max(0, conversationHistory.Count - 10), Math.Min(10, conversationHistory.Count));
        }

        /// <summary>
        /// Gets a list of recent user topics from history.
        /// </summary>
        /// <param name="count">Maximum number of recent items.</param>
        /// <returns>Recent user message topics.</returns>
        public List<string> GetRecentTopics(int count = 3)
        {
            var topics = new List<string>();
            for (int i = conversationHistory.Count - 1; i >= 0 && topics.Count < count; i--)
            {
                string entry = conversationHistory[i];
                int separatorIndex = entry.IndexOf("User:", StringComparison.OrdinalIgnoreCase);
                if (separatorIndex >= 0)
                {
                    string topic = entry.Substring(separatorIndex + 5).Trim();
                    if (!string.IsNullOrWhiteSpace(topic) && !topics.Contains(topic))
                    {
                        topics.Add(topic);
                    }
                }
            }

            topics.Reverse();
            return topics;
        }

        /// <summary>
        /// Retrieves the last user message if available.
        /// </summary>
        /// <returns>The last user message or null.</returns>
        public string? GetLastUserMessage()
        {
            for (int i = conversationHistory.Count - 1; i >= 0; i--)
            {
                if (conversationHistory[i].IndexOf("User:", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    return conversationHistory[i];
                }
            }

            return null;
        }

        /// <summary>
        /// Stores a user preference.
        /// </summary>
        /// <param name="key">Preference key.</param>
        /// <param name="value">Preference value.</param>
        public void SetPreference(string key, string value)
        {
            userPreferences[key] = value;
        }

        /// <summary>
        /// Retrieves a user preference.
        /// </summary>
        /// <param name="key">Preference key.</param>
        /// <returns>Preference value or null if not set.</returns>
        public string? GetPreference(string key)
        {
            return userPreferences.ContainsKey(key) ? userPreferences[key] : null;
        }
    }
}

