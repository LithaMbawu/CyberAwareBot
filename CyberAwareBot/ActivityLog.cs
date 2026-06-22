using System;
using System.Collections.Generic;
using System.Linq;

namespace CybersecurityChatbot
{
    public sealed class ActivityLog
    {
        private readonly List<string> _entries = new();

        public void Add(string message)
        {
            string entry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}";
            _entries.Insert(0, entry);

            if (_entries.Count > 100)
            {
                _entries.RemoveAt(_entries.Count - 1);
            }
        }

        public IReadOnlyList<string> GetRecentEntries(int count)
        {
            return _entries.Take(count).ToList();
        }
    }
}
