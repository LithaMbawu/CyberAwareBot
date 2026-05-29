using System.Windows;
using System.Windows.Input;
using System.Speech.Synthesis;

namespace CybersecurityChatbot
{
    /// <summary>
    /// Main window for the Cybersecurity Awareness Chatbot application.
    /// Provides an intuitive interface for users to interact with the chatbot.
    /// </summary>
    public partial class MainWindow : Window
    {
        private ChatBot _chatBot;

        /// <summary>
        /// Initializes the main window and sets up the chatbot.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            // Initialize chatbot with memory and sentiment capabilities
            _chatBot = new ChatBot();

            // Perform startup tasks
            PlayVoiceGreeting();
            LoadAsciiArt();

            // Display opening greeting and scroll into view
            AppendBotMessage(_chatBot.GetGreeting());
            ChatScrollViewer.ScrollToBottom();
            UserInputTextBox.Focus();
        }

        /// <summary>
        /// Handles the send button click event.
        /// </summary>
        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            SendMessage();
        }

        /// <summary>
        /// Handles the Enter key press in the input textbox.
        /// </summary>
        private void UserInputTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SendMessage();
            }
        }

        /// <summary>
        /// Processes and sends the user's message to the chatbot.
        /// </summary>
        private void SendMessage()
        {
            string input = UserInputTextBox.Text.Trim();

            if (string.IsNullOrWhiteSpace(input))
                return;

            // Display user message
            AppendUserMessage(input);

            // Get bot response
            string response = _chatBot.ProcessInput(input);
            AppendBotMessage(response);

            // Clear input and scroll to bottom
            UserInputTextBox.Clear();
            ChatScrollViewer.ScrollToBottom();
        }

        /// <summary>
        /// Appends a user message to the chat history.
        /// </summary>
        /// <param name="message">The user's message.</param>
        private void AppendUserMessage(string message)
        {
            ChatHistoryTextBlock.Text += $"You: {message}\n\n";
        }

        /// <summary>
        /// Appends a bot message to the chat history.
        /// </summary>
        /// <param name="message">The bot's message.</param>
        private void AppendBotMessage(string message)
        {
            ChatHistoryTextBlock.Text += $"CyberBot: {message}\n\n";
        }

        /// <summary>
        /// Loads and displays ASCII art in the header.
        /// </summary>
        private void LoadAsciiArt()
        {
            AsciiArtText.Text =
@"
   _______
  /       \
 /  Cyber   \
|   Secure   |
 \  Chatbot /
  \_______/
     ||
     ||
    /  \
   |    |
   |    |
  /      \
 /        \
";
        }

        /// <summary>
        /// Plays a voice greeting using text-to-speech.
        /// </summary>
        private void PlayVoiceGreeting()
        {
            try
            {
                using (SpeechSynthesizer synthesizer = new SpeechSynthesizer())
                {
                    synthesizer.Speak("Welcome to the Cybersecurity Awareness Chatbot. How can I help you stay secure today?");
                }
            }
            catch
            {
                // Fallback if TTS not available
                MessageBox.Show("Voice greeting not available on this system.");
            }
        }
    }
}
