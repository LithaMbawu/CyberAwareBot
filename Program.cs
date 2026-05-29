using System;

namespace CyberAwareBot
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Cybersecurity Awareness Bot";

            AudioPlayer audioPlayer = new AudioPlayer();

            UIHelper ui = new UIHelper();
            Console.WriteLine();
            ui.ShowBanner();
            audioPlayer.PlayGreeting();

            string userName = PromptForUserName();
            ui.ShowWelcome(userName);

            bool running = true;
            Chatbot chatbot = new Chatbot(userName);

            while (running)
            {
                ShowMenu();
                Console.Write("Enter your choice (1-6): ");
                string choice = (Console.ReadLine() ?? string.Empty).Trim();

                switch (choice)
                {
                    case "1":
                        RunTopic("Password Safety", GetPasswordQuestions(), GetPasswordAnswers());
                        break;
                    case "2":
                        RunTopic("Phishing", GetPhishingQuestions(), GetPhishingAnswers());
                        break;
                    case "3":
                        RunTopic("Safe Browsing", GetBrowsingQuestions(), GetBrowsingAnswers());
                        break;
                    case "4":
                        chatbot.RunChat();
                        break;
                    case "5":
                        audioPlayer.PlayGreeting();
                        break;
                    case "6":
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("\nGoodbye, {0}! Stay safe online.", userName);
                        Console.ResetColor();
                        running = false;
                        Environment.Exit(0);
                        break;
                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Invalid choice. Please select 1-6.");
                        Console.ResetColor();
                        break;
                }
            }
        }

        static string PromptForUserName()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("Please enter your name: ");
            string name = (Console.ReadLine() ?? string.Empty).Trim();
            Console.ResetColor();

            if (string.IsNullOrWhiteSpace(name))
            {
                name = "Guest";
            }

            return name;
        }

        static void ShowMenu()
        {
            Console.WriteLine("\n────────────────────────────────────────────────");
            Console.WriteLine("|           Cybersecurity Awareness Menu         |");
            Console.WriteLine("────────────────────────────────────────────────");
            Console.WriteLine("1. Password Safety");
            Console.WriteLine("2. Phishing");
            Console.WriteLine("3. Safe Browsing");
            Console.WriteLine("4. Chat with Bot");
            Console.WriteLine("5. Play Greeting");
            Console.WriteLine("6. Exit");
        }

        static void RunTopic(string topic, string[] questions, string[] answers)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"\n--- {topic} Questions ---");
            Console.ResetColor();

            for (int i = 0; i < questions.Length; i++)
            {
                Console.WriteLine($"\n{i + 1}. {questions[i]}");
                Console.Write("Your answer: ");
                string userAnswer = Console.ReadLine() ?? string.Empty;
                string correctAnswer = answers.Length > i ? answers[i] : "No answer available.";

                if (string.IsNullOrWhiteSpace(userAnswer))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Bot: I didn't quite understand that. Could you rephrase?");
                    Console.ResetColor();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Bot: Thanks for your answer.");
                    Console.WriteLine($"Bot: The best answer is: {correctAnswer}");
                    Console.ResetColor();
                }
            }

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"\nYou have completed the {topic} questions!");
            Console.ResetColor();
        }

        static string[] GetPasswordQuestions()
        {
            return new string[]
            {
                "Why is it important to have a strong password?",
                "Should you use the same password for multiple accounts?",
                "What makes a password strong?",
                "How often should you change your passwords?",
                "What is a password manager and why use one?",
                "Is using personal info in your password safe?",
                "What is two-factor authentication (2FA)?",
                "How can you safely store your passwords?"
            };
        }

        static string[] GetPasswordAnswers()
        {
            return new string[]
            {
                "A strong password helps protect your accounts from being hacked or guessed.",
                "No, you should use unique passwords for each account.",
                "A strong password is long, unique, and contains letters, numbers, and symbols.",
                "Change passwords if a service is breached or if you suspect compromise, but generally use strong unique passwords instead of frequent changes.",
                "A password manager stores strong passwords securely so you don't have to remember them all.",
                "No, using personal info makes passwords easier to guess.",
                "Two-factor authentication adds a second verification step after your password.",
                "Use a password manager, avoid writing them down, and never share them with others."
            };
        }

        static string[] GetPhishingQuestions()
        {
            return new string[]
            {
                "What is phishing?",
                "How can you recognize a phishing email?",
                "Why should you avoid clicking unknown links?",
                "What should you do if you suspect a phishing attempt?",
                "Is it safe to reply to suspicious emails?",
                "What is spear phishing?",
                "How do phishing scams affect businesses?",
                "Can phishing happen over phone calls (vishing)?"
            };
        }

        static string[] GetPhishingAnswers()
        {
            return new string[]
            {
                "Phishing is a scam where attackers try to trick you into giving personal information.",
                "You can recognize phishing by checking the sender, looking for poor spelling, and avoiding unexpected links or attachments.",
                "Unknown links may lead to malicious sites or downloads, so avoid clicking them unless you trust the source.",
                "If you suspect phishing, do not click any links, report it, and delete the message.",
                "No, it is not safe to reply to suspicious emails because that can confirm your address to attackers.",
                "Spear phishing is a targeted phishing attack aimed at a specific person or organization.",
                "Phishing scams can cost businesses money, damage reputation, and expose sensitive data.",
                "Yes, phishing can happen over phone calls and is called vishing. Always verify the caller before sharing information."
            };
        }

        static string[] GetBrowsingQuestions()
        {
            return new string[]
            {
                "What is a secure website?",
                "Why is HTTPS important?",
                "What are the risks of using public Wi-Fi?",
                "Why should your browser be updated regularly?",
                "What is safe downloading practice?",
                "How can you check website credibility?",
                "Why should you avoid unknown pop-ups?",
                "What is browser sandboxing and why does it help?"
            };
        }

        static string[] GetBrowsingAnswers()
        {
            return new string[]
            {
                "A secure website uses HTTPS and has a valid certificate to protect your data.",
                "HTTPS encrypts data between your browser and the website so attackers cannot read it.",
                "Public Wi-Fi can be insecure and allow attackers to intercept your traffic or steal your information.",
                "Regular updates fix security flaws and help keep your browser safe from new threats.",
                "Only download files from trusted sources and avoid unknown or suspicious downloads.",
                "Check website credibility by confirming the URL, reading reviews, and verifying security indicators.",
                "Unknown pop-ups can carry malware or phishing attempts, so avoid interacting with them.",
                "Sandboxing isolates the browser from the rest of your system, reducing the impact of attacks."
            };
        }
    }
}

