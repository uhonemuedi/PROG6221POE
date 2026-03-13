using System;
using System.Media;
using System.Threading;

class Program
{
    static void Main(string[] args)
    {
        Console.BackgroundColor = ConsoleColor.Black;
        Console.ForegroundColor = ConsoleColor.White;
        Console.Clear();
        PlayGreeting();
        DisplayAsciiArt();
        GreetUser();
        StartChat();
    }

    static void PlayGreeting()
    {
        try
        {
            string audioPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets", "greeting.wav");
            SoundPlayer player = new SoundPlayer(audioPath);
            player.PlaySync();
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Audio could not be played: " + ex.Message);
            Console.ResetColor();
        }
    }

    static void DisplayAsciiArt()
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine(@"
  ██████╗██╗   ██╗██████╗ ███████╗██████╗ 
 ██╔════╝╚██╗ ██╔╝██╔══██╗██╔════╝██╔══██╗
 ██║      ╚████╔╝ ██████╔╝█████╗  ██████╔╝
 ██║       ╚██╔╝  ██╔══██╗██╔══╝  ██╔══██╗
 ╚██████╗   ██║   ██████╔╝███████╗██║  ██║
  ╚═════╝   ╚═╝   ╚═════╝ ╚══════╝╚═╝  ╚═╝
        ");
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("  === Cybersecurity Awareness Bot ===");
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine();
    }

    static void GreetUser()
    {
        Console.ForegroundColor = ConsoleColor.Magenta;
        TypeText("What is your name? ");
        Console.ForegroundColor = ConsoleColor.White;
        string name = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(name))
            name = "User";

        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("╔══════════════════════════════════════════╗");
        Console.ForegroundColor = ConsoleColor.Green;
        TypeText($"  Welcome, {name}! I'm your Cybersecurity Assistant.");
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("╚══════════════════════════════════════════╝");
        Console.WriteLine();
    }

    static void StartChat()
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        TypeText("You can ask me about: passwords, phishing, safe browsing, or just say 'how are you'.");
        Console.WriteLine();

        while (true)
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Write("You: ");
            Console.ForegroundColor = ConsoleColor.White;
            string input = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(input))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                TypeText("Bot: I didn't quite understand that. Can you rephrase?");
                Console.WriteLine();
                continue;
            }

            string response = GetResponse(input.ToLower());
            Console.ForegroundColor = ConsoleColor.Green;
            TypeText("Bot: " + response);
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine();

            if (input.ToLower() == "exit")
                break;
        }
    }

    static string GetResponse(string input)
    {
        if (input.Contains("how are you"))
            return "I'm doing great and ready to help you stay safe online!";
        else if (input.Contains("purpose") || input.Contains("what can you do"))
            return "I'm here to educate you on cybersecurity topics like phishing, passwords, and safe browsing.";
        else if (input.Contains("what can i ask"))
            return "You can ask me about: passwords, phishing, safe browsing, or general cybersecurity tips!";
        else if (input.Contains("password"))
            return "Use strong passwords with a mix of letters, numbers and symbols. Never reuse passwords!";
        else if (input.Contains("phishing"))
            return "Phishing is when attackers trick you into revealing personal info. Never click suspicious links!";
        else if (input.Contains("safe browsing") || input.Contains("browsing"))
            return "Always check for HTTPS in the URL, avoid unknown sites, and keep your browser updated.";
        else if (input.Contains("malware"))
            return "Malware is malicious software. Always use antivirus and avoid downloading unknown files.";
        else if (input.Contains("vpn"))
            return "A VPN encrypts your internet connection. Use one on public Wi-Fi to stay safe.";
        else if (input.Contains("two factor") || input.Contains("2fa"))
            return "Two-factor authentication adds an extra layer of security. Always enable it where possible!";
        else if (input.Contains("exit"))
            return "Goodbye! Stay safe online!";
        else
            return "I don't quite understand that. Can you rephrase?";
    }

    static void TypeText(string text)
    {
        foreach (char c in text)
        {
            Console.Write(c);
            Thread.Sleep(15);
        }
        Console.WriteLine();
    }
}