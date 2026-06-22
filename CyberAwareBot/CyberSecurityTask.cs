using System;

namespace CybersecurityChatbot
{
    public sealed class CyberSecurityTask
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string? ReminderText { get; set; }

        public DateTime? ReminderDate { get; set; }

        public bool IsCompleted { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public override string ToString()
        {
            string status = IsCompleted ? "Done" : "Open";
            string reminder = string.IsNullOrWhiteSpace(ReminderText) ? "No reminder" : ReminderText!;
            return $"[{status}] {Title} - {Description} ({reminder})";
        }
    }
}
