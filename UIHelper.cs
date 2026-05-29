using System;

namespace CyberAwareBot
{
    public class UIHelper
    {
        // ASCII art stored as a string
        public string Banner { get; } = @"                                                                                                                                  
    ▄▄▄▄  ▄▄▄    ▄▄▄ ▄▄▄▄▄▄    ▄▄▄▄▄▄▄▄  ▄▄▄▄▄▄      ▄▄▄▄    ▄▄▄▄▄▄▄▄     ▄▄▄▄   ▄▄    ▄▄  ▄▄▄▄▄▄     ▄▄▄▄▄▄   ▄▄▄▄▄▄▄▄ ▄▄▄    ▄▄▄
  ██▀▀▀▀█  ██▄  ▄██  ██▀▀▀▀██  ██▀▀▀▀▀▀  ██▀▀▀▀██  ▄█▀▀▀▀█   ██▀▀▀▀▀▀   ██▀▀▀▀█  ██    ██  ██▀▀▀▀██   ▀▀██▀▀   ▀▀▀██▀▀▀  ██▄  ▄██ 
 ██▀        ██▄▄██   ██    ██  ██        ██    ██  ██▄       ██        ██▀       ██    ██  ██    ██     ██        ██      ██▄▄██  
 ██          ▀██▀    ███████   ███████   ███████    ▀████▄   ███████   ██        ██    ██  ███████      ██        ██       ▀██▀   
 ██▄          ██     ██    ██  ██        ██  ▀██▄       ▀██  ██        ██▄       ██    ██  ██  ▀██▄     ██        ██        ██    
  ██▄▄▄▄█     ██     ██▄▄▄▄██  ██▄▄▄▄▄▄  ██    ██  █▄▄▄▄▄█▀  ██▄▄▄▄▄▄   ██▄▄▄▄█  ▀██▄▄██▀  ██    ██   ▄▄██▄▄      ██        ██    
    ▀▀▀▀      ▀▀     ▀▀▀▀▀▀▀   ▀▀▀▀▀▀▀▀  ▀▀    ▀▀▀  ▀▀▀▀▀    ▀▀▀▀▀▀▀▀     ▀▀▀▀     ▀▀▀▀    ▀▀    ▀▀▀  ▀▀▀▀▀▀      ▀▀        ▀▀
";

        public void ShowBanner()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(Banner);
            Console.ResetColor();
        }

        public void ShowWelcome(string userName)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("====================================================");
            Console.WriteLine($" Welcome, {userName}!");
            Console.WriteLine(" Welcome to the Cybersecurity Awareness Bot.");
            Console.WriteLine(" I’m here to help you stay safer online with expert guidance.");
            Console.WriteLine("====================================================\n");
            Console.ResetColor();
        }
    }
}