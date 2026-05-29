using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace CyberAwareBot
{
    public class AudioPlayer
    {
        public void PlayGreeting()
        {
            try
            {
                string fileName = "greeting.wav";
                string basePath = AppDomain.CurrentDomain.BaseDirectory ?? string.Empty;
                string projectRoot = Path.GetFullPath(Path.Combine(basePath, "..", "..", ".."));
                string userProfile = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) ?? string.Empty;

                string[] searchPaths = new[]
                {
                    Path.Combine(projectRoot, fileName),
                    Path.Combine(userProfile, "Downloads", "WPTest", fileName),
                    Path.Combine(userProfile, "Downloads", fileName),
                    Path.Combine(basePath, fileName)
                };

                string filePath = searchPaths.FirstOrDefault(File.Exists) ?? string.Empty;

                if (string.IsNullOrEmpty(filePath))
                {
                    Console.WriteLine("Audio file not found. Tried:");
                    foreach (var path in searchPaths)
                    {
                        Console.WriteLine(" - " + path);
                    }
                    Console.WriteLine("Please place greeting.wav in the project root, output folder, or Downloads/WPTest.");
                    return;
                }

                Console.WriteLine("Playing greeting from: " + filePath);

                Process.Start(new ProcessStartInfo
                {
                    FileName = "afplay",
                    Arguments = $"\"{filePath}\"",
                    UseShellExecute = false,
                    CreateNoWindow = true
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error playing greeting: " + ex.Message);
            }
        }
    }
}

