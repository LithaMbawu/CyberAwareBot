# Cybersecurity Awareness Chatbot

A polished WPF chatbot designed to teach cybersecurity best practices through a natural conversational experience.

## Features

- **Engaging chat flow** with varied, natural responses
- **Voice greeting** via text-to-speech on startup
- **ASCII art header** for a professional cyber style
- **Keyword-based advice** on passwords, phishing, malware, updates, VPNs, backups, and more
- **Sentiment-aware responses** that adapt tone to user mood
- **Conversation memory** for recall and context-aware follow-ups
- **Personalization** with name recognition and dynamic replies
- **Clean dark UI** with modern layout and readable styling

## Sample Interaction

- User: "How do I create a strong password?"
- Bot: "Strong passwords are your first line of defense. Use a mix of letters, numbers, and symbols, and change them regularly. Keep up the good security habits."

- User: "My name is Alex"
- Bot: "Nice to meet you, Alex! I’ll remember your name so our conversation feels more personal."

- User: "Can you recall what we discussed?"
- Bot: "So far we’ve talked about passwords. What would you like to focus on next?"

## Installation and Setup

### Prerequisites
- Windows 10 or later
- .NET 8.0 SDK installed
- A Windows-compatible environment for WPF apps

### Build Steps
1. Open the `CyberAwareBot` project folder.

### Running the App
1. Launch the application.
2. Listen for the voice greeting.
3. Type a cybersecurity question in the input box.
4. Press Enter or click **Send**.
5. Read the chatbot's answer in the history pane.

## How to Use

- Ask about **password safety**, **phishing**, **malware**, **software updates**, **VPN use**, and **backups**.
- Use phrases like **"my name is"** or **"call me"** to enable the chatbot to remember your name.
- Ask **"what did we talk about"** or **"remember"** to trigger conversation recall.
- Try **friendly greetings** and **thank-you phrases** to see varied response styles.

## Project Structure

```
CyberAwareBot/
├── .gitignore             # Ignore build and system files
├── ChatBot.cs             # Core conversation engine
├── CyberAwareBot.csproj   # Project configuration
├── KeywordResponder.cs    # Keyword-based response library
├── MainWindow.xaml        # WPF UI layout
├── MainWindow.xaml.cs     # UI interaction logic
├── MemoryStore.cs         # Session memory and preference storage
├── README.md              # Project documentation
├── SentimentDetector.cs   # Mood detection and response tone
└── bin/ obj/              # Build outputs (ignored)
```

## Architecture

- **ChatBot.cs**: Orchestrates input evaluation, sentiment handling, memory recall, and response generation
- **KeywordResponder.cs**: Supplies varied responses for cybersecurity topics
- **SentimentDetector.cs**: Identifies user mood and adjusts language tone
- **MemoryStore.cs**: Preserves recent chat history and user preferences
- **MainWindow.xaml / MainWindow.xaml.cs**: Presents the UI and handles user interaction

## GitHub and ARC Submission

GitHub Repository: https://github.com/LithaMbawu/CyberAbot

If submitting to ARC, include the above GitHub link and your YouTube demo link in the ARC submission form.

## Video Demo

YouTube Video Demo: https://youtu.be/PCWVSgsUzIA?si=J9yzzO7IJ4WFapTH


## Notes

- The project is configured to target Windows using `EnableWindowsTargeting` for cross-platform build tooling on macOS.
- This app is intended as an educational cybersecurity awareness tool.

## License

Educational project - feel free to use and modify for learning and demonstration purposes.

## Purpose

This project demonstrates object-oriented programming, human-computer interaction, and cybersecurity awareness through an interactive chatbot system.