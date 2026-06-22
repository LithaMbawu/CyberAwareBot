# Cybersecurity Awareness Chatbot

A polished cross-platform Avalonia chatbot designed to teach cybersecurity best practices through a natural conversational experience on macOS and Windows.

## Features

- **Engaging chat flow** with varied, natural responses
- **Voice greeting** via text-to-speech on startup
- **ASCII art header** for a professional cyber style
- **Keyword-based advice** on passwords, phishing, malware, updates, VPNs, backups, and more
- **Sentiment-aware responses** that adapt tone to user mood
- **Conversation memory** for recall and context-aware follow-ups
- **Personalization** with name recognition and dynamic replies
- **Task assistant** with reminders, completion tracking, and optional MySQL-backed storage
- **Cybersecurity quiz game** with more than 10 questions and instant feedback
- **NLP-style command detection** for tasks, reminders, quiz commands, and activity log requests
- **Activity log** for recent bot actions
- **Modern tabbed Avalonia UI** with controls for each feature

## Sample Interaction

- User: "How do I create a strong password?"
- Bot: "Strong passwords are your first line of defense. Use a mix of letters, numbers, and symbols, and change them regularly. Keep up the good security habits."

- User: "My name is Alex"
- Bot: "Nice to meet you, Alex! I’ll remember your name so our conversation feels more personal."

- User: "Can you recall what we discussed?"
- Bot: "So far we’ve talked about passwords. What would you like to focus on next?"

## Installation and Setup

### Prerequisites
- macOS or Windows
- .NET 8.0 SDK installed
- Desktop runtime support for Avalonia

### Build Steps
1. Open the `CyberAwareBot` project folder.

### Running the App
1. Launch the application.
2. Listen for the voice greeting.
3. Type a cybersecurity question, task request, quiz request, or activity log command in the input box.
4. Press Enter or click **Send**.
5. Use the **Tasks**, **Quiz**, and **Activity Log** tabs for the richer GUI features.

## How to Use

- Ask about **password safety**, **phishing**, **malware**, **software updates**, **VPN use**, and **backups**.
- Use phrases like **"my name is"** or **"call me"** to enable the chatbot to remember your name.
- Ask **"what did we talk about"** or **"remember"** to trigger conversation recall.
- Try **"add task to enable 2FA"**, **"remind me to review privacy settings in 3 days"**, **"start quiz"**, or **"show activity log"**.

## Project Structure

```
CyberAwareBot/
├── .gitignore             # Ignore build and system files
├── ActivityLog.cs         # Recent bot action history
├── ChatBot.cs             # Core conversation engine
├── CyberAwareBot.csproj   # Project configuration
├── CyberSecurityTask.cs   # Task model
├── CyberSecurityTaskStore.cs # Task persistence layer
├── KeywordResponder.cs    # Keyword-based response library
├── MainWindow.axaml       # Avalonia UI layout
├── MainWindow.xaml.cs     # UI interaction logic
├── MemoryStore.cs         # Session memory and preference storage
├── QuizEngine.cs          # Quiz logic and scoring
├── QuizQuestion.cs        # Quiz question model
├── README.md              # Project documentation
├── SentimentDetector.cs   # Mood detection and response tone
└── bin/ obj/              # Build outputs (ignored)
```

## Architecture

- **ChatBot.cs**: Orchestrates input evaluation, sentiment handling, memory recall, and response generation
- **KeywordResponder.cs**: Supplies varied responses for cybersecurity topics
- **SentimentDetector.cs**: Identifies user mood and adjusts language tone
- **MemoryStore.cs**: Preserves recent chat history and user preferences
- **App.axaml / App.xaml.cs**: Bootstraps the Avalonia desktop app
- **MainWindow.axaml / MainWindow.xaml.cs**: Presents the Avalonia UI and handles chat, tasks, quiz, and logs

## GitHub and ARC Submission

GitHub Repository: https://github.com/LithaMbawu/AwarenessChatBot.git

If submitting to ARC, include the above GitHub link and your YouTube demo link in the ARC submission form.

## Video Demo

YouTube Video Demo: https://youtu.be/PCWVSgsUzIA?si=J9yzzO7IJ4WFapTH


## Notes

- The project now targets `net8.0` with Avalonia so it runs on macOS.
- The task assistant stores data locally by default and can use MySQL when the `CYBERAWAREBOT_MYSQL_CONNECTION` environment variable is set.
- This app is intended as an educational cybersecurity awareness tool.

## License

Educational project - feel free to use and modify for learning and demonstration purposes.

## Purpose

This project demonstrates object-oriented programming, human-computer interaction, and cybersecurity awareness through an interactive chatbot system.