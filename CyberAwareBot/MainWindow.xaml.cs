using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using CyberAwareBot;

namespace CybersecurityChatbot
{
    /// <summary>
    /// Main window for the Cybersecurity Awareness Chatbot application.
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ChatBot _chatBot;
        private readonly CyberSecurityTaskStore _taskStore;
        private readonly ActivityLog _activityLog;
        private readonly QuizEngine _quizEngine;
        private readonly AudioPlayer _audioPlayer;

        public MainWindow()
        {
            InitializeComponent();

            _chatBot = new ChatBot();
            _taskStore = new CyberSecurityTaskStore();
            _activityLog = new ActivityLog();
            _quizEngine = new QuizEngine();
            _audioPlayer = new AudioPlayer();

            PlayVoiceGreeting();
            LoadAsciiArt();

            AppendBotMessage(_chatBot.GetGreeting());
            RefreshTaskList();
            RefreshActivityLog();
            LoadQuizQuestionPreview();
            ScrollChatToBottom();
            UserInputTextBox.Focus();
        }

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            SendMessage();
        }

        private void UserInputTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SendMessage();
            }
        }

        private void SendMessage()
        {
            string input = UserInputTextBox.Text.Trim();
            if (string.IsNullOrWhiteSpace(input))
            {
                return;
            }

            AppendUserMessage(input);
            string response = HandleUserRequest(input);
            AppendBotMessage(response);

            UserInputTextBox.Clear();
            ScrollChatToBottom();
        }

        private string HandleUserRequest(string input)
        {
            string normalized = NormalizeText(input);

            if (IsActivityLogRequest(normalized))
            {
                RefreshActivityLog();
                MainTabControl.SelectedIndex = 3;
                return BuildActivitySummary();
            }

            if (IsQuizRequest(normalized))
            {
                StartQuiz();
                MainTabControl.SelectedIndex = 2;
                return "Quiz started. Answer the first question in the Quiz tab.";
            }

            if (TryHandleTaskCommand(input, normalized, out string taskResponse))
            {
                MainTabControl.SelectedIndex = 1;
                return taskResponse;
            }

            return _chatBot.ProcessInput(input);
        }

        private bool TryHandleTaskCommand(string originalInput, string normalizedInput, out string response)
        {
            response = string.Empty;

            if (IsShowTasksRequest(normalizedInput))
            {
                RefreshTaskList();
                response = BuildTaskSummary();
                return true;
            }

            if (IsCompleteTaskRequest(normalizedInput))
            {
                CyberSecurityTask? task = FindTaskFromCommand(normalizedInput);
                if (task == null)
                {
                    response = "Select a task in the Tasks tab or mention the task title so I can mark it complete.";
                    return true;
                }

                task.IsCompleted = true;
                _taskStore.UpdateTask(task);
                _activityLog.Add($"Task completed: '{task.Title}'.");
                RefreshTaskList();
                RefreshActivityLog();
                response = $"Task marked complete: '{task.Title}'.";
                return true;
            }

            if (IsDeleteTaskRequest(normalizedInput))
            {
                CyberSecurityTask? task = FindTaskFromCommand(normalizedInput);
                if (task == null)
                {
                    response = "Select a task in the Tasks tab or mention the task title so I can delete it.";
                    return true;
                }

                _taskStore.DeleteTask(task.Id);
                _activityLog.Add($"Task deleted: '{task.Title}'.");
                RefreshTaskList();
                RefreshActivityLog();
                response = $"Task deleted: '{task.Title}'.";
                return true;
            }

            if (IsTaskRequest(normalizedInput) || IsReminderRequest(normalizedInput))
            {
                CyberSecurityTask createdTask = CreateTaskFromMessage(originalInput);
                _taskStore.AddTask(createdTask);
                _activityLog.Add($"Task added: '{createdTask.Title}'.");

                if (!string.IsNullOrWhiteSpace(createdTask.ReminderText))
                {
                    _activityLog.Add($"Reminder set for '{createdTask.Title}': {createdTask.ReminderText}.");
                }

                RefreshTaskList();
                RefreshActivityLog();
                response = createdTask.ReminderText == null
                    ? $"Task added: '{createdTask.Title}'. Would you like to set a reminder for it?"
                    : $"Task added: '{createdTask.Title}'. Reminder set for {createdTask.ReminderText}.";
                return true;
            }

            return false;
        }

        private bool IsTaskRequest(string normalizedInput)
        {
            return normalizedInput.Contains("task")
                || normalizedInput.Contains("todo")
                || normalizedInput.Contains("to do")
                || normalizedInput.Contains("add task")
                || normalizedInput.Contains("new task");
        }

        private bool IsReminderRequest(string normalizedInput)
        {
            return normalizedInput.Contains("remind")
                || normalizedInput.Contains("reminder")
                || normalizedInput.Contains("set reminder");
        }

        private bool IsShowTasksRequest(string normalizedInput)
        {
            return normalizedInput.Contains("show tasks")
                || normalizedInput.Contains("list tasks")
                || normalizedInput.Contains("my tasks")
                || normalizedInput.Contains("view tasks");
        }

        private bool IsCompleteTaskRequest(string normalizedInput)
        {
            return normalizedInput.Contains("complete task")
                || normalizedInput.Contains("mark task complete")
                || (normalizedInput.Contains("complete") && normalizedInput.Contains("task"))
                || normalizedInput.Contains("mark as done");
        }

        private bool IsDeleteTaskRequest(string normalizedInput)
        {
            return normalizedInput.Contains("delete task")
                || normalizedInput.Contains("remove task")
                || (normalizedInput.Contains("delete") && normalizedInput.Contains("task"));
        }

        private bool IsQuizRequest(string normalizedInput)
        {
            return normalizedInput.Contains("quiz")
                || normalizedInput.Contains("game")
                || normalizedInput.Contains("test me")
                || normalizedInput.Contains("start quiz")
                || normalizedInput.Contains("play quiz");
        }

        private bool IsActivityLogRequest(string normalizedInput)
        {
            return normalizedInput.Contains("activity log")
                || normalizedInput.Contains("show log")
                || normalizedInput.Contains("what have you done")
                || normalizedInput.Contains("recent actions")
                || normalizedInput.Contains("what have you done for me");
        }

        private CyberSecurityTask? FindTaskFromCommand(string normalizedInput)
        {
            List<CyberSecurityTask> tasks = _taskStore.GetAllTasks();
            if (tasks.Count == 0)
            {
                return null;
            }

            string taskFragment = ExtractTaskFragment(normalizedInput);
            if (!string.IsNullOrWhiteSpace(taskFragment))
            {
                CyberSecurityTask? found = tasks.FirstOrDefault(task => task.Title.Contains(taskFragment, StringComparison.OrdinalIgnoreCase));
                if (found != null)
                {
                    return found;
                }
            }

            if (TaskListBox.SelectedItem is CyberSecurityTask selectedTask)
            {
                return selectedTask;
            }

            return tasks.FirstOrDefault();
        }

        private string ExtractTaskFragment(string normalizedInput)
        {
            Match match = Regex.Match(normalizedInput, @"(?:task|complete|delete|remove|mark)\s+(?:the\s+)?(?<title>.+)", RegexOptions.IgnoreCase);
            return match.Success ? match.Groups["title"].Value.Trim() : string.Empty;
        }

        private CyberSecurityTask CreateTaskFromMessage(string originalInput)
        {
            string normalized = NormalizeText(originalInput);
            string title = ExtractTaskTitle(normalized, originalInput);
            string description = GenerateTaskDescription(title);
            string? reminderText = ExtractReminderText(originalInput);
            DateTime? reminderDate = ParseReminderDate(reminderText);

            return new CyberSecurityTask
            {
                Title = title,
                Description = description,
                ReminderText = reminderText,
                ReminderDate = reminderDate,
                IsCompleted = false
            };
        }

        private string ExtractTaskTitle(string normalizedInput, string originalInput)
        {
            string title = originalInput.Trim();
            string[] patterns =
            {
                @"add(?: a)?(?: new)? task(?: to)? (?<title>.+)",
                @"remind me to (?<title>.+)",
                @"set(?: a)? reminder(?: to)? (?<title>.+)",
                @"task(?: is|:)? (?<title>.+)"
            };

            foreach (string pattern in patterns)
            {
                Match match = Regex.Match(normalizedInput, pattern, RegexOptions.IgnoreCase);
                if (match.Success)
                {
                    title = match.Groups["title"].Value.Trim();
                    break;
                }
            }

            title = Regex.Replace(title, @"\b(in \d+ days?|tomorrow|today|next week|on .+)$", string.Empty, RegexOptions.IgnoreCase).Trim();
            title = title.Trim('"', '\'', '.', '-', ':');

            if (string.IsNullOrWhiteSpace(title))
            {
                title = "Cybersecurity task";
            }

            return CultureTitleCase(title);
        }

        private string GenerateTaskDescription(string title)
        {
            string lowerTitle = title.ToLowerInvariant();

            if (lowerTitle.Contains("2fa") || lowerTitle.Contains("two-factor"))
            {
                return "Enable two-factor authentication to add an extra security step to your account.";
            }

            if (lowerTitle.Contains("password"))
            {
                return "Review password strength, reuse, and manager usage so your accounts stay protected.";
            }

            if (lowerTitle.Contains("privacy"))
            {
                return "Review account privacy settings to reduce exposure of personal information.";
            }

            if (lowerTitle.Contains("phishing"))
            {
                return "Practice spotting phishing attempts and reporting suspicious messages.";
            }

            return $"Cybersecurity follow-up task: {title}.";
        }

        private string? ExtractReminderText(string originalInput)
        {
            string lowerInput = originalInput.ToLowerInvariant();

            Match inDaysMatch = Regex.Match(lowerInput, @"in\s+(\d+)\s+days?", RegexOptions.IgnoreCase);
            if (inDaysMatch.Success)
            {
                return $"in {inDaysMatch.Groups[1].Value} days";
            }

            if (lowerInput.Contains("tomorrow"))
            {
                return "tomorrow";
            }

            Match onDateMatch = Regex.Match(originalInput, @"on\s+([A-Za-z0-9,/-]+)", RegexOptions.IgnoreCase);
            if (onDateMatch.Success)
            {
                return $"on {onDateMatch.Groups[1].Value.Trim()}";
            }

            return null;
        }

        private DateTime? ParseReminderDate(string? reminderText)
        {
            if (string.IsNullOrWhiteSpace(reminderText))
            {
                return null;
            }

            if (reminderText.StartsWith("in ", StringComparison.OrdinalIgnoreCase) &&
                int.TryParse(Regex.Match(reminderText, @"\d+").Value, out int days))
            {
                return DateTime.Today.AddDays(days);
            }

            if (reminderText.Equals("tomorrow", StringComparison.OrdinalIgnoreCase))
            {
                return DateTime.Today.AddDays(1);
            }

            if (DateTime.TryParse(reminderText.Replace("on ", string.Empty, StringComparison.OrdinalIgnoreCase), out DateTime parsedDate))
            {
                return parsedDate;
            }

            return null;
        }

        private string BuildTaskSummary()
        {
            List<CyberSecurityTask> tasks = _taskStore.GetAllTasks();
            if (tasks.Count == 0)
            {
                return "No tasks are saved yet. You can add a cybersecurity task from the Chat or Tasks tab.";
            }

            return "Current tasks:\n\n" + string.Join("\n\n", tasks.Take(10).Select((task, index) => $"{index + 1}. {task}"));
        }

        private string BuildActivitySummary()
        {
            List<string> entries = _activityLog.GetRecentEntries(10).ToList();
            if (entries.Count == 0)
            {
                return "No activity has been recorded yet.";
            }

            return "Here’s a summary of recent actions:\n\n" + string.Join("\n\n", entries.Select((entry, index) => $"{index + 1}. {entry}"));
        }

        private void RefreshTaskList()
        {
            TaskListBox.ItemsSource = null;
            TaskListBox.ItemsSource = _taskStore.GetAllTasks();
        }

        private void RefreshActivityLog()
        {
            ActivityLogListBox.ItemsSource = null;
            ActivityLogListBox.ItemsSource = _activityLog.GetRecentEntries(10).ToList();
        }

        private void LoadQuizQuestionPreview()
        {
            QuizProgressTextBlock.Text = "Quiz is idle.";
            QuizQuestionTextBlock.Text = "Click Start Quiz or type 'quiz' in the chat to begin.";
            QuizScoreTextBlock.Text = $"Score: 0/{_quizEngine.TotalQuestions}";
            QuizFeedbackTextBlock.Text = string.Empty;
            SetQuizOptionVisibility(false);
            SubmitQuizAnswerButton.IsEnabled = false;
            NextQuizQuestionButton.IsEnabled = false;
        }

        private void StartQuiz()
        {
            _quizEngine.Start();
            _activityLog.Add("Quiz started.");
            RefreshActivityLog();
            LoadCurrentQuizQuestion();
        }

        private void LoadCurrentQuizQuestion(bool clearFeedback = true)
        {
            QuizQuestion? question = _quizEngine.CurrentQuestion;
            if (question == null)
            {
                QuizProgressTextBlock.Text = "Quiz complete.";
                QuizQuestionTextBlock.Text = _quizEngine.GetFinalSummary();
                QuizFeedbackTextBlock.Text = _quizEngine.GetFinalFeedback();
                QuizScoreTextBlock.Text = $"Score: {_quizEngine.Score}/{_quizEngine.TotalQuestions}";
                SetQuizOptionVisibility(false);
                SubmitQuizAnswerButton.IsEnabled = false;
                NextQuizQuestionButton.IsEnabled = false;
                _activityLog.Add($"Quiz completed with score {_quizEngine.Score}/{_quizEngine.TotalQuestions}.");
                RefreshActivityLog();
                return;
            }

            QuizProgressTextBlock.Text = $"Question {_quizEngine.CurrentQuestionNumber} of {_quizEngine.TotalQuestions}";
            QuizQuestionTextBlock.Text = question.QuestionText;
            if (clearFeedback)
            {
                QuizFeedbackTextBlock.Text = string.Empty;
            }
            QuizScoreTextBlock.Text = $"Score: {_quizEngine.Score}/{_quizEngine.TotalQuestions}";
            SetQuizOptions(question.Options);
            SubmitQuizAnswerButton.IsEnabled = true;
            NextQuizQuestionButton.IsEnabled = true;
        }

        private void SetQuizOptions(IReadOnlyList<string> options)
        {
            RadioButton[] radios = { QuizOption1Radio, QuizOption2Radio, QuizOption3Radio, QuizOption4Radio };
            for (int i = 0; i < radios.Length; i++)
            {
                if (i < options.Count)
                {
                    radios[i].IsVisible = true;
                    radios[i].Content = options[i];
                    radios[i].IsChecked = false;
                }
                else
                {
                    radios[i].IsVisible = false;
                    radios[i].IsChecked = false;
                }
            }
        }

        private void SetQuizOptionVisibility(bool isVisible)
        {
            QuizOption1Radio.IsVisible = isVisible;
            QuizOption2Radio.IsVisible = isVisible;
            QuizOption3Radio.IsVisible = isVisible;
            QuizOption4Radio.IsVisible = isVisible;
        }

        private int? GetSelectedQuizIndex()
        {
            RadioButton[] radios = { QuizOption1Radio, QuizOption2Radio, QuizOption3Radio, QuizOption4Radio };
            for (int i = 0; i < radios.Length; i++)
            {
                if (radios[i].IsVisible && radios[i].IsChecked == true)
                {
                    return i;
                }
            }

            return null;
        }

        private string NormalizeText(string input)
        {
            string lowered = input.ToLowerInvariant();
            string withoutSymbols = Regex.Replace(lowered, @"[^a-z0-9\s/-]", " ");
            return Regex.Replace(withoutSymbols, @"\s+", " ").Trim();
        }

        private string CultureTitleCase(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return value;
            }

            TextInfo textInfo = CultureInfo.CurrentCulture.TextInfo;
            string[] tokens = value.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < tokens.Length; i++)
            {
                string token = tokens[i];
                if (token.Equals("2fa", StringComparison.OrdinalIgnoreCase) || token.Contains("2fa", StringComparison.OrdinalIgnoreCase))
                {
                    tokens[i] = token.ToUpperInvariant();
                }
                else
                {
                    tokens[i] = textInfo.ToTitleCase(token.ToLowerInvariant());
                }
            }

            return string.Join(' ', tokens);
        }

        private void AddTaskButton_Click(object sender, RoutedEventArgs e)
        {
            string title = TaskTitleTextBox.Text.Trim();
            string description = TaskDescriptionTextBox.Text.Trim();
            string reminderText = TaskReminderTextBox.Text.Trim();

            if (string.IsNullOrWhiteSpace(title))
            {
                AppendBotMessage("Please enter a task title before saving it.");
                return;
            }

            CyberSecurityTask task = new CyberSecurityTask
            {
                Title = CultureTitleCase(title),
                Description = string.IsNullOrWhiteSpace(description) ? GenerateTaskDescription(title) : description,
                ReminderText = string.IsNullOrWhiteSpace(reminderText) ? null : reminderText,
                ReminderDate = ParseReminderDate(string.IsNullOrWhiteSpace(reminderText) ? null : reminderText),
                IsCompleted = false
            };

            _taskStore.AddTask(task);
            _activityLog.Add($"Task added: '{task.Title}'.");
            if (!string.IsNullOrWhiteSpace(task.ReminderText))
            {
                _activityLog.Add($"Reminder set for '{task.Title}': {task.ReminderText}.");
            }

            TaskTitleTextBox.Clear();
            TaskDescriptionTextBox.Clear();
            TaskReminderTextBox.Clear();
            RefreshTaskList();
            RefreshActivityLog();
            AppendBotMessage($"Task saved: '{task.Title}'.");
        }

        private void CompleteTaskButton_Click(object sender, RoutedEventArgs e)
        {
            if (TaskListBox.SelectedItem is not CyberSecurityTask task)
            {
                AppendBotMessage("Please select a task first.");
                return;
            }

            task.IsCompleted = true;
            _taskStore.UpdateTask(task);
            _activityLog.Add($"Task completed: '{task.Title}'.");
            RefreshTaskList();
            RefreshActivityLog();
            AppendBotMessage($"Task marked complete: '{task.Title}'.");
        }

        private void DeleteTaskButton_Click(object sender, RoutedEventArgs e)
        {
            if (TaskListBox.SelectedItem is not CyberSecurityTask task)
            {
                AppendBotMessage("Please select a task first.");
                return;
            }

            _taskStore.DeleteTask(task.Id);
            _activityLog.Add($"Task deleted: '{task.Title}'.");
            RefreshTaskList();
            RefreshActivityLog();
            AppendBotMessage($"Task deleted: '{task.Title}'.");
        }

        private void RefreshTasksButton_Click(object sender, RoutedEventArgs e)
        {
            RefreshTaskList();
            AppendBotMessage(BuildTaskSummary());
        }

        private void StartQuizButton_Click(object sender, RoutedEventArgs e)
        {
            StartQuiz();
        }

        private void SubmitQuizAnswerButton_Click(object sender, RoutedEventArgs e)
        {
            int? selectedIndex = GetSelectedQuizIndex();
            if (selectedIndex == null)
            {
                QuizFeedbackTextBlock.Text = "Please choose an answer before submitting.";
                return;
            }

            int answeredQuestionNumber = _quizEngine.CurrentQuestionNumber;
            QuizResult result = _quizEngine.SubmitAnswer(selectedIndex.Value);
            QuizFeedbackTextBlock.Text = result.Feedback;
            QuizScoreTextBlock.Text = $"Score: {_quizEngine.Score}/{_quizEngine.TotalQuestions}";
            _activityLog.Add($"Quiz answer submitted for question {answeredQuestionNumber}. Correct: {result.WasCorrect}.");
            RefreshActivityLog();

            if (result.IsFinished)
            {
                QuizProgressTextBlock.Text = "Quiz complete.";
                QuizQuestionTextBlock.Text = _quizEngine.GetFinalSummary();
                SetQuizOptionVisibility(false);
                SubmitQuizAnswerButton.IsEnabled = false;
                NextQuizQuestionButton.IsEnabled = false;
                return;
            }

            LoadCurrentQuizQuestion(false);
        }

        private void NextQuizQuestionButton_Click(object sender, RoutedEventArgs e)
        {
            LoadCurrentQuizQuestion(false);
        }

        private void RestartQuizButton_Click(object sender, RoutedEventArgs e)
        {
            StartQuiz();
        }

        private void RefreshLogButton_Click(object sender, RoutedEventArgs e)
        {
            RefreshActivityLog();
            AppendBotMessage(BuildActivitySummary());
        }
        private void AppendUserMessage(string message)
        {
            ChatHistoryTextBlock.Text += $"You: {message}\n\n";
        }

        private void AppendBotMessage(string message)
        {
            ChatHistoryTextBlock.Text += $"CyberBot: {message}\n\n";
        }

        private void LoadAsciiArt()
        {
            AsciiArtText.Text =
@"
   ________
  / ____  /\\
 / / __ \/  \\
| | |  | |   |
| | |__| |   |
| |_____/    |
 \___________/
     ||  ||
     ||  ||
    /_\\ /_\\
";
        }

        private void PlayVoiceGreeting()
        {
            try
            {
                _audioPlayer.PlayGreeting();
            }
            catch
            {
                AppendBotMessage("Voice greeting not available on this system.");
            }
        }

        private void ScrollChatToBottom()
        {
            ChatScrollViewer.Offset = new Vector(0, double.MaxValue);
        }
    }
}
