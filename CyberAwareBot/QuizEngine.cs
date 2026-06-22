using System;
using System.Collections.Generic;

namespace CybersecurityChatbot
{
    public sealed class QuizEngine
    {
        private readonly List<QuizQuestion> _questions = new();
        private int _currentIndex;

        public QuizEngine()
        {
            SeedQuestions();
        }

        public int CurrentQuestionNumber => IsActive ? _currentIndex + 1 : 0;

        public int TotalQuestions => _questions.Count;

        public int Score { get; private set; }

        public bool IsActive { get; private set; }

        public QuizQuestion? CurrentQuestion => IsActive && _currentIndex < _questions.Count ? _questions[_currentIndex] : null;

        public void Start()
        {
            _currentIndex = 0;
            Score = 0;
            IsActive = true;
        }

        public QuizResult SubmitAnswer(int selectedIndex)
        {
            if (!IsActive || CurrentQuestion == null)
            {
                return new QuizResult
                {
                    Feedback = "Start the quiz first.",
                    IsFinished = true,
                    WasCorrect = false
                };
            }

            QuizQuestion question = CurrentQuestion;
            bool wasCorrect = selectedIndex == question.CorrectOptionIndex;
            if (wasCorrect)
            {
                Score++;
            }

            string feedback = wasCorrect
                ? $"Correct! {question.Explanation}"
                : $"Not quite. {question.Explanation} The correct answer was '{question.Options[question.CorrectOptionIndex]}'.";

            _currentIndex++;
            bool finished = _currentIndex >= _questions.Count;
            if (finished)
            {
                IsActive = false;
            }

            return new QuizResult
            {
                WasCorrect = wasCorrect,
                Feedback = feedback,
                IsFinished = finished
            };
        }

        public string GetFinalSummary()
        {
            if (_questions.Count == 0)
            {
                return "No quiz questions are available.";
            }

            if (Score >= 10)
            {
                return $"Great job! You're a cybersecurity pro. Final score: {Score}/{_questions.Count}.";
            }

            if (Score >= 6)
            {
                return $"Good work. Keep practicing to strengthen your cybersecurity skills. Final score: {Score}/{_questions.Count}.";
            }

            return $"Keep learning to stay safe online. Final score: {Score}/{_questions.Count}.";
        }

        public string GetFinalFeedback()
        {
            return Score >= 10
                ? "You answered most questions confidently and showed strong security awareness."
                : Score >= 6
                    ? "You have a solid base. Review the explanations and try again to improve your score."
                    : "Review the key ideas: phishing, passwords, safe browsing, and social engineering.";
        }

        private void SeedQuestions()
        {
            _questions.Add(new QuizQuestion
            {
                QuestionText = "What should you do if an email asks for your password?",
                Options = new[] { "Reply with your password", "Delete or report it as phishing", "Forward it to friends", "Ignore it and click the link" },
                CorrectOptionIndex = 1,
                Explanation = "Reporting phishing helps stop scammers and protects others."
            });

            _questions.Add(new QuizQuestion
            {
                QuestionText = "True or false: Reusing the same password everywhere is safe.",
                Options = new[] { "True", "False" },
                CorrectOptionIndex = 1,
                Explanation = "Unique passwords reduce the damage if one account is exposed."
            });

            _questions.Add(new QuizQuestion
            {
                QuestionText = "Which feature adds an extra login step after your password?",
                Options = new[] { "Two-factor authentication", "Browser cache", "Incognito mode", "Cookie sync" },
                CorrectOptionIndex = 0,
                Explanation = "Two-factor authentication makes stolen passwords less useful."
            });

            _questions.Add(new QuizQuestion
            {
                QuestionText = "True or false: Public Wi-Fi can expose your traffic if it is not protected.",
                Options = new[] { "True", "False" },
                CorrectOptionIndex = 0,
                Explanation = "Untrusted networks can be monitored, so use caution and secure connections."
            });

            _questions.Add(new QuizQuestion
            {
                QuestionText = "What is phishing?",
                Options = new[] { "A virus scanner", "A scam that tricks you into giving information", "A browser update", "A password manager" },
                CorrectOptionIndex = 1,
                Explanation = "Phishing is designed to steal data by pretending to be trustworthy."
            });

            _questions.Add(new QuizQuestion
            {
                QuestionText = "What is the safest action for unknown attachments?",
                Options = new[] { "Open them quickly", "Scan or ignore them unless verified", "Rename them", "Send them back" },
                CorrectOptionIndex = 1,
                Explanation = "Attachments can contain malware, so verify them first."
            });

            _questions.Add(new QuizQuestion
            {
                QuestionText = "True or false: Software updates often contain security fixes.",
                Options = new[] { "True", "False" },
                CorrectOptionIndex = 0,
                Explanation = "Updates patch known weaknesses before attackers can use them."
            });

            _questions.Add(new QuizQuestion
            {
                QuestionText = "Which behavior is a sign of social engineering?",
                Options = new[] { "Carefully written URLs", "Urgent pressure to share secrets", "Automatic updates", "Encrypted websites" },
                CorrectOptionIndex = 1,
                Explanation = "Attackers often rush people to bypass their judgment."
            });

            _questions.Add(new QuizQuestion
            {
                QuestionText = "What helps you create and store strong unique passwords?",
                Options = new[] { "A password manager", "Writing them on sticky notes", "Reusing one password", "Sharing with friends" },
                CorrectOptionIndex = 0,
                Explanation = "Password managers help you use strong, unique credentials safely."
            });

            _questions.Add(new QuizQuestion
            {
                QuestionText = "True or false: HTTPS helps protect data in transit.",
                Options = new[] { "True", "False" },
                CorrectOptionIndex = 0,
                Explanation = "HTTPS encrypts traffic between your browser and the site."
            });

            _questions.Add(new QuizQuestion
            {
                QuestionText = "What should you do with suspicious links?",
                Options = new[] { "Click first, check later", "Verify the source before opening", "Share them publicly", "Open them in private mode" },
                CorrectOptionIndex = 1,
                Explanation = "Checking the source helps avoid malware and phishing pages."
            });

            _questions.Add(new QuizQuestion
            {
                QuestionText = "True or false: Backups can help you recover after ransomware.",
                Options = new[] { "True", "False" },
                CorrectOptionIndex = 0,
                Explanation = "Reliable backups reduce the impact of ransomware and device failure."
            });
        }
    }
}
