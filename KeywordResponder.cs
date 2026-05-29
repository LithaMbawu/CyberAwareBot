using System;
using System.Collections.Generic;

namespace CybersecurityChatbot
{
    /// <summary>
    /// Provides varied, engaging responses based on keywords for natural conversation flow.
    /// </summary>
    public class KeywordResponder
    {
        private Dictionary<string, List<string>> responses;
        private Random random;

        public KeywordResponder()
        {
            random = new Random();
            responses = new Dictionary<string, List<string>>();

            // Initialize responses for various keywords
            responses["password"] = new List<string>
            {
                "Strong passwords are your first line of defense. Use a mix of letters, numbers, and symbols, and change them regularly.",
                "Remember: never share your passwords, even with friends. Use a password manager to keep them safe.",
                "A good password is like a good lock - it keeps the bad guys out!"
            };

            responses["phishing"] = new List<string>
            {
                "Phishing attacks trick you into giving away information. Always check the sender's email and hover over links before clicking.",
                "If an email seems too good to be true, it probably is. Report suspicious emails to your IT department.",
                "Stay vigilant! Phishing is one of the most common cyber threats."
            };

            responses["malware"] = new List<string>
            {
                "Malware can sneak onto your device through downloads or emails. Keep your antivirus updated and scan regularly.",
                "Be cautious with attachments and downloads. If in doubt, don't open it!",
                "Malware comes in many forms - viruses, trojans, ransomware. Prevention is key."
            };

            responses["hello"] = new List<string>
            {
                "Hello! Ready to learn about cybersecurity?",
                "Hi there! What cybersecurity topic interests you today?",
                "Greetings! I'm here to help you stay cyber-safe."
            };

            responses["thanks"] = new List<string>
            {
                "You're welcome! Stay safe out there.",
                "Glad I could help. Remember, cybersecurity is everyone's responsibility.",
                "No problem! Knowledge is the best defense."
            };

            responses["security"] = new List<string>
            {
                "Security starts with good habits: strong passwords, automatic updates, and cautious clicking.",
                "Think of cybersecurity as everyday hygiene: routine checks keep threats away.",
                "A layered defense is strongest: secure devices, secure accounts, and secure behavior."
            };

            responses["update"] = new List<string>
            {
                "Keep your software updated. Updates patch vulnerabilities before attackers can exploit them.",
                "Automatic updates are one of the easiest ways to stay protected.",
                "Install updates regularly and restart when prompted so your system stays secure."
            };

            responses["vpn"] = new List<string>
            {
                "A VPN can help protect your privacy on public Wi-Fi, but it is one layer in a broader security strategy.",
                "Use trusted VPN services when you connect from cafes or airports to keep your traffic private.",
                "A VPN helps encrypt your data, especially on networks you do not control."
            };

            responses["backup"] = new List<string>
            {
                "Regular backups are essential. If ransomware or hardware failure happens, you can recover safely.",
                "Store backups separately from your main device and test restore procedures occasionally.",
                "Backups give you peace of mind. Keep them current and offline when possible."
            };

            responses["howareyou"] = new List<string>
            {
                "I’m ready to help you stay cybersafe. What do you want to learn today?",
                "Feeling focused on security—let’s talk about your concerns.",
                "I’m here to support your cybersecurity journey with clear, practical advice."
            };

            responses["fallback"] = new List<string>
            {
                "Great question. What part of cybersecurity would you like to explore?",
                "I can help with passwords, phishing, malware, software updates, and more.",
                "Tell me more about your concern and I’ll give you a security-friendly answer.",
                "I’m here to help you build safer habits online—what would you like to know?"
            };
        }

        /// <summary>
        /// Gets a random response for a given keyword.
        /// </summary>
        /// <param name="keyword">The keyword to respond to.</param>
        /// <returns>A random response or a default message.</returns>
        public string GetResponse(string keyword)
        {
            string lowerKeyword = keyword.ToLower();
            if (responses.ContainsKey(lowerKeyword))
            {
                var responseList = responses[lowerKeyword];
                return responseList[random.Next(responseList.Count)];
            }
            else
            {
                return responses["fallback"][random.Next(responses["fallback"].Count)];
            }
        }

        /// <summary>
        /// Checks if a keyword has responses.
        /// </summary>
        /// <param name="keyword">The keyword to check.</param>
        /// <returns>True if responses exist.</returns>
        public bool HasResponse(string keyword)
        {
            return responses.ContainsKey(keyword.ToLower());
        }
    }
}

