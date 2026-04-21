using System;
using System.Collections.Generic;
using System.Drawing;
using System.Media;
using System.Windows.Forms;

namespace PROG6221POE_Part2
{
    public partial class Form1 : Form
    {
        // Memory variables
        private string userName = "";
        private string favouriteTopic = "";
        private string lastTopic = "";

        // Random responses
        private Random random = new Random();

        // Keyword responses dictionary
        private Dictionary<string, List<string>> responses = new Dictionary<string, List<string>>
        {
            { "password", new List<string> {
                "Use strong passwords with a mix of letters, numbers and symbols!",
                "Never reuse passwords across multiple accounts!",
                "Consider using a password manager to keep track of your passwords!"
            }},
            { "phishing", new List<string> {
                "Phishing is when attackers trick you into revealing personal info. Never click suspicious links!",
                "Always verify the sender's email address before clicking any links!",
                "Legitimate companies will never ask for your password via email!"
            }},
            { "scam", new List<string> {
                "Be cautious of emails asking for personal information. Scammers often disguise themselves as trusted organisations.",
                "If something seems too good to be true, it probably is a scam!",
                "Never send money to someone you haven't met in person!"
            }},
            { "privacy", new List<string> {
                "Review your privacy settings on social media regularly!",
                "Be careful about what personal information you share online!",
                "Use a VPN to protect your privacy on public networks!"
            }},
            { "malware", new List<string> {
                "Always use antivirus software and keep it updated!",
                "Never download software from unknown sources!",
                "Malware can steal your personal information. Always scan downloads!"
            }},
            { "vpn", new List<string> {
                "A VPN encrypts your internet connection. Use one on public Wi-Fi!",
                "VPNs help protect your privacy by hiding your IP address!",
                "Always use a reputable VPN service for best protection!"
            }},
            { "2fa", new List<string> {
                "Two-factor authentication adds an extra layer of security!",
                "Always enable 2FA where possible for better account security!",
                "2FA makes it much harder for attackers to access your accounts!"
            }}
        };

        public Form1()
        {
            InitializeComponent();
            SetupUI();
            PlayGreeting();
            WelcomeUser();
        }

        private void SetupUI()
        {
            // Form settings
            this.Text = "Cybersecurity Awareness Chatbot";
            this.Size = new Size(800, 600);
            this.BackColor = Color.Black;
            this.MinimumSize = new Size(800, 600);

            // Title label
            Label lblTitle = new Label();
            lblTitle.Text = "🔒 CYBERSECURITY AWARENESS BOT 🔒";
            lblTitle.ForeColor = Color.Cyan;
            lblTitle.Font = new Font("Consolas", 14, FontStyle.Bold);
            lblTitle.Size = new Size(760, 40);
            lblTitle.Location = new Point(10, 10);
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;
            this.Controls.Add(lblTitle);

            // Chat display
            RichTextBox rtbChat = new RichTextBox();
            rtbChat.Name = "rtbChat";
            rtbChat.Size = new Size(760, 400);
            rtbChat.Location = new Point(10, 55);
            rtbChat.BackColor = Color.Black;
            rtbChat.ForeColor = Color.Lime;
            rtbChat.Font = new Font("Consolas", 10);
            rtbChat.ReadOnly = true;
            rtbChat.ScrollBars = RichTextBoxScrollBars.Vertical;
            this.Controls.Add(rtbChat);

            // Input textbox
            TextBox txtInput = new TextBox();
            txtInput.Name = "txtInput";
            txtInput.Size = new Size(620, 35);
            txtInput.Location = new Point(10, 465);
            txtInput.BackColor = Color.DarkGray;
            txtInput.ForeColor = Color.White;
            txtInput.Font = new Font("Consolas", 11);
            txtInput.KeyPress += TxtInput_KeyPress;
            this.Controls.Add(txtInput);

            // Send button
            Button btnSend = new Button();
            btnSend.Name = "btnSend";
            btnSend.Text = "SEND";
            btnSend.Size = new Size(140, 35);
            btnSend.Location = new Point(640, 465);
            btnSend.BackColor = Color.DarkCyan;
            btnSend.ForeColor = Color.White;
            btnSend.Font = new Font("Consolas", 11, FontStyle.Bold);
            btnSend.Click += BtnSend_Click;
            this.Controls.Add(btnSend);

            // Status label
            Label lblStatus = new Label();
            lblStatus.Name = "lblStatus";
            lblStatus.Text = "Type your message and press SEND or Enter";
            lblStatus.ForeColor = Color.Yellow;
            lblStatus.Font = new Font("Consolas", 9);
            lblStatus.Size = new Size(760, 25);
            lblStatus.Location = new Point(10, 510);
            this.Controls.Add(lblStatus);
        }

        private void PlayGreeting()
        {
            try
            {
                string audioPath = System.IO.Path.Combine(
                    AppDomain.CurrentDomain.BaseDirectory, "Assets", "greeting.wav");
                if (System.IO.File.Exists(audioPath))
                {
                    SoundPlayer player = new SoundPlayer(audioPath);
                    player.Play();
                }
            }
            catch { }
        }

        private void WelcomeUser()
        {
            string name = Microsoft.VisualBasic.Interaction.InputBox(
                "Welcome to the Cybersecurity Awareness Bot!\nWhat is your name?",
                "Welcome", "");

            if (string.IsNullOrWhiteSpace(name))
                name = "User";

            userName = name;
            AppendMessage("Bot", $"Hello {userName}! I'm your Cybersecurity Awareness Assistant.", Color.Cyan);
            AppendMessage("Bot", "I can help you with: passwords, phishing, scams, privacy, malware, vpn, 2fa", Color.Cyan);
            AppendMessage("Bot", "What would you like to know about today?", Color.Cyan);
        }

        private void AppendMessage(string sender, string message, Color color)
        {
            RichTextBox rtbChat = this.Controls["rtbChat"] as RichTextBox;
            if (rtbChat != null)
            {
                rtbChat.SelectionStart = rtbChat.TextLength;
                rtbChat.SelectionLength = 0;
                rtbChat.SelectionColor = color;
                rtbChat.AppendText($"[{sender}]: {message}\n\n");
                rtbChat.ScrollToCaret();
            }
        }

        private void BtnSend_Click(object sender, EventArgs e)
        {
            ProcessInput();
        }

        private void TxtInput_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                ProcessInput();
                e.Handled = true;
            }
        }

        private void ProcessInput()
        {
            TextBox txtInput = this.Controls["txtInput"] as TextBox;
            if (txtInput == null || string.IsNullOrWhiteSpace(txtInput.Text))
            {
                AppendMessage("Bot", "I didn't quite understand that. Can you rephrase?", Color.Red);
                return;
            }

            string input = txtInput.Text.Trim();
            AppendMessage(userName, input, Color.Yellow);
            txtInput.Clear();

            string response = GetResponse(input.ToLower());
            AppendMessage("Bot", response, Color.Lime);
        }

        private string GetResponse(string input)
        {
            // Sentiment detection
            if (input.Contains("worried") || input.Contains("scared") || input.Contains("anxious"))
                return $"I understand you're feeling worried, {userName}. Cybersecurity can be overwhelming, but I'm here to help! Let's tackle your concerns together.";

            if (input.Contains("frustrated") || input.Contains("angry") || input.Contains("annoyed"))
                return $"I hear your frustration, {userName}. Let me help you find the information you need quickly!";

            if (input.Contains("curious") || input.Contains("interested") || input.Contains("want to know"))
                return $"Great curiosity, {userName}! Learning about cybersecurity is the first step to staying safe online. What topic interests you?";

            // Memory features
            if (input.Contains("my name is"))
            {
                string newName = input.Replace("my name is", "").Trim();
                userName = newName;
                return $"Nice to meet you, {userName}! I'll remember your name.";
            }

            if (input.Contains("i'm interested in") || input.Contains("i like"))
            {
                favouriteTopic = input;
                return $"Great! I'll remember that you're interested in {favouriteTopic}. It's a crucial part of staying safe online!";
            }

            // Follow up responses
            if (input.Contains("tell me more") || input.Contains("explain more") || input.Contains("give me another tip"))
            {
                if (!string.IsNullOrEmpty(lastTopic) && responses.ContainsKey(lastTopic))
                {
                    var tips = responses[lastTopic];
                    return tips[random.Next(tips.Count)];
                }
                return "Please ask me about a specific topic like passwords, phishing, or privacy!";
            }

            // Keyword recognition with random responses
            foreach (var keyword in responses.Keys)
            {
                if (input.Contains(keyword))
                {
                    lastTopic = keyword;
                    var tipList = responses[keyword];
                    return tipList[random.Next(tipList.Count)];
                }
            }

            // General responses
            if (input.Contains("how are you"))
                return $"I'm doing great and ready to help you stay safe online, {userName}!";

            if (input.Contains("what can you do") || input.Contains("help"))
                return "I can help you with: passwords, phishing, scams, privacy, malware, VPN, and 2FA topics!";

            if (input.Contains("bye") || input.Contains("exit") || input.Contains("goodbye"))
            {
                AppendMessage("Bot", $"Goodbye {userName}! Stay safe online! 🔒", Color.Cyan);
                return "";
            }

            return $"I'm not sure about that, {userName}. Try asking about passwords, phishing, scams, privacy, malware, vpn, or 2fa!";
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }
    }
}