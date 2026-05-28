// ============================================================
// PROG6221POE - Part 2
// Cybersecurity Awareness Chatbot - GUI Version
// Student: Pearl
// Student Number: ST10474866
// Date: May 2026
// Description: A WinForms GUI chatbot that educates users on
// cybersecurity topics using keyword recognition, sentiment
// detection, memory recall and random responses.
// ============================================================

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Media;
using System.Windows.Forms;


namespace CybersecurityChatbotPart2_Fixed
{
    public partial class Form1 : Form
    {
        private string userName = "";
        private string favouriteTopic = "";
        private string lastTopic = "";
        private Random random = new Random();

        private Dictionary<string, List<string>> responses = new Dictionary<string, List<string>>
        {
            { "password", new List<string> {
                "Use strong passwords with uppercase, lowercase, numbers and symbols!",
                "Never reuse the same password across multiple accounts!",
                "Consider using a password manager to securely store your passwords!",
                "Change your passwords regularly, especially after a data breach!"
            }},
            { "phishing", new List<string> {
                "Phishing tricks you into revealing personal info. Never click suspicious links!",
                "Always verify the sender's email address before clicking any links!",
                "Legitimate companies will never ask for your password via email!",
                "Look for spelling mistakes and urgency in emails as signs of phishing!"
            }},
            { "scam", new List<string> {
                "Scammers disguise themselves as trusted organisations. Be cautious!",
                "If something seems too good to be true, it probably is a scam!",
                "Never send money to someone you have not met in person!",
                "Report scams to the South African Police Service cybercrime unit!"
            }},
            { "privacy", new List<string> {
                "Review your privacy settings on social media regularly!",
                "Be careful about what personal information you share online!",
                "Use a VPN to protect your privacy on public networks!",
                "Read privacy policies before signing up for new services!"
            }},
            { "malware", new List<string> {
                "Always use antivirus software and keep it updated!",
                "Never download software from unknown or untrusted sources!",
                "Always scan downloads before opening them!",
                "Keep your operating system updated to protect against malware!"
            }},
            { "vpn", new List<string> {
                "A VPN encrypts your internet connection on public Wi-Fi!",
                "VPNs protect your privacy by hiding your IP address!",
                "Always use a reputable VPN service for the best protection!",
                "A VPN can help you access content securely from different locations!"
            }},
            { "2fa", new List<string> {
                "Two factor authentication adds an extra layer of security!",
                "Always enable 2FA where possible for better account security!",
                "2FA makes it harder for attackers to access your accounts!",
                "Use an authenticator app like Google Authenticator for stronger 2FA!"
            }},
            { "firewall", new List<string> {
                "A firewall monitors and controls incoming and outgoing network traffic!",
                "Always keep your firewall enabled to protect against unauthorised access!",
                "Both hardware and software firewalls provide important protection!"
            }},
            { "encryption", new List<string> {
                "Encryption converts your data into code to prevent unauthorised access!",
                "Always use encrypted connections (HTTPS) when browsing sensitive websites!",
                "Encrypt sensitive files on your device to protect them if stolen!"
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
            this.BackColor = Color.Black;

            Label lblTitle = new Label();
            lblTitle.Text = "🔒 CYBERSECURITY AWARENESS BOT 🔒";
            lblTitle.ForeColor = Color.Cyan;
            lblTitle.Font = new Font("Consolas", 16, FontStyle.Bold);
            lblTitle.Size = new Size(860, 45);
            lblTitle.Location = new Point(10, 10);
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;
            this.Controls.Add(lblTitle);

            Label lblSubtitle = new Label();
            lblSubtitle.Text = "Your Personal Cybersecurity Assistant | Stay Safe Online!";
            lblSubtitle.ForeColor = Color.Yellow;
            lblSubtitle.Font = new Font("Consolas", 9);
            lblSubtitle.Size = new Size(860, 25);
            lblSubtitle.Location = new Point(10, 55);
            lblSubtitle.TextAlign = ContentAlignment.MiddleCenter;
            this.Controls.Add(lblSubtitle);

            RichTextBox rtbChat = new RichTextBox();
            rtbChat.Name = "rtbChat";
            rtbChat.Size = new Size(860, 420);
            rtbChat.Location = new Point(10, 85);
            rtbChat.BackColor = Color.Black;
            rtbChat.ForeColor = Color.Lime;
            rtbChat.Font = new Font("Consolas", 10);
            rtbChat.ReadOnly = true;
            rtbChat.ScrollBars = RichTextBoxScrollBars.Vertical;
            rtbChat.BorderStyle = BorderStyle.FixedSingle;
            this.Controls.Add(rtbChat);

            TextBox txtInput = new TextBox();
            txtInput.Name = "txtInput";
            txtInput.Size = new Size(680, 35);
            txtInput.Location = new Point(10, 515);
            txtInput.BackColor = Color.DarkSlateGray;
            txtInput.ForeColor = Color.White;
            txtInput.Font = new Font("Consolas", 11);
            txtInput.KeyPress += TxtInput_KeyPress;
            this.Controls.Add(txtInput);

            Button btnSend = new Button();
            btnSend.Name = "btnSend";
            btnSend.Text = "SEND";
            btnSend.Size = new Size(170, 35);
            btnSend.Location = new Point(700, 515);
            btnSend.BackColor = Color.DarkCyan;
            btnSend.ForeColor = Color.White;
            btnSend.Font = new Font("Consolas", 11, FontStyle.Bold);
            btnSend.FlatStyle = FlatStyle.Flat;
            btnSend.Click += BtnSend_Click;
            this.Controls.Add(btnSend);

            Label lblStatus = new Label();
            lblStatus.Name = "lblStatus";
            lblStatus.Text = "Type your message and press SEND or Enter";
            lblStatus.ForeColor = Color.Yellow;
            lblStatus.Font = new Font("Consolas", 9);
            lblStatus.Size = new Size(860, 25);
            lblStatus.Location = new Point(10, 560);
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
            catch (Exception ex)
            {
                Console.WriteLine("Audio error: " + ex.Message);
            }
        }

        private void WelcomeUser()
        {
            string name = "";
            using (Form inputForm = new Form())
            {
                inputForm.Text = "Welcome";
                inputForm.Size = new Size(400, 200);
                inputForm.BackColor = Color.Black;
                inputForm.StartPosition = FormStartPosition.CenterScreen;

                Label lbl = new Label();
                lbl.Text = "Welcome! What is your name?";
                lbl.ForeColor = Color.Cyan;
                lbl.Font = new Font("Consolas", 10);
                lbl.Location = new Point(20, 20);
                lbl.Size = new Size(350, 25);

                TextBox txt = new TextBox();
                txt.Location = new Point(20, 55);
                txt.Size = new Size(340, 25);
                txt.BackColor = Color.DarkSlateGray;
                txt.ForeColor = Color.White;
                txt.Font = new Font("Consolas", 10);

                Button btn = new Button();
                btn.Text = "OK";
                btn.Location = new Point(150, 100);
                btn.Size = new Size(80, 30);
                btn.BackColor = Color.DarkCyan;
                btn.ForeColor = Color.White;
                btn.Click += (s, ev) => { name = txt.Text; inputForm.Close(); };

                inputForm.Controls.AddRange(new Control[] { lbl, txt, btn });
                inputForm.ShowDialog();
            }

            if (string.IsNullOrWhiteSpace(name))
                name = "User";

            userName = name;
            AppendMessage("Bot", $"Hello {userName}! Welcome to the Cybersecurity Awareness Bot!", Color.Cyan);
            AppendMessage("Bot", "I can help you with: passwords, phishing, scams, privacy, malware, vpn, 2fa, firewall, encryption", Color.Cyan);
            AppendMessage("Bot", $"What would you like to learn about today, {userName}?", Color.Cyan);
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
                AppendMessage("Bot", "I did not quite understand that. Could you please rephrase?", Color.Red);
                return;
            }

            string input = txtInput.Text.Trim();
            AppendMessage(userName, input, Color.Yellow);
            txtInput.Clear();

            string response = GetResponse(input.ToLower());
            if (!string.IsNullOrEmpty(response))
                AppendMessage("Bot", response, Color.Lime);
        }

        private string GetResponse(string input)
        {
            if (input.Contains("worried") || input.Contains("scared") || input.Contains("anxious"))
                return $"I completely understand your concern, {userName}. Cybersecurity can feel overwhelming, but you are taking the right steps! What specific topic are you worried about?";

            if (input.Contains("frustrated") || input.Contains("angry") || input.Contains("annoyed"))
                return $"I hear your frustration, {userName}. Let me help you find exactly what you need. Try asking about passwords, phishing, scams, privacy or malware!";

            if (input.Contains("curious") || input.Contains("interested") || input.Contains("want to know"))
                return $"I love your curiosity, {userName}! Ask me about any cybersecurity topic and I will share what I know!";

            if (input.Contains("happy") || input.Contains("great") || input.Contains("excited"))
                return $"That is wonderful to hear, {userName}! Let us use that positive energy to learn more about staying safe online!";

            if (input.Contains("my name is"))
            {
                string newName = input.Replace("my name is", "").Trim();
                if (!string.IsNullOrEmpty(newName))
                {
                    userName = newName;
                    return $"Nice to meet you, {userName}! I will remember your name throughout our conversation.";
                }
            }

            if (input.Contains("i'm interested in") || input.Contains("i am interested in") || input.Contains("i like"))
            {
                favouriteTopic = input;
                return $"Great, {userName}! I will remember that you are interested in that topic!";
            }

            if (input.Contains("tell me more") || input.Contains("explain more") || input.Contains("give me another tip") || input.Contains("more info"))
            {
                if (!string.IsNullOrEmpty(lastTopic) && responses.ContainsKey(lastTopic))
                {
                    var tips = responses[lastTopic];
                    return $"Here is another tip about {lastTopic}: {tips[random.Next(tips.Count)]}";
                }
                return $"Please ask me about a specific topic like passwords, phishing or privacy, {userName}!";
            }

            foreach (var keyword in responses.Keys)
            {
                if (input.Contains(keyword))
                {
                    lastTopic = keyword;
                    var tipList = responses[keyword];
                    string tip = tipList[random.Next(tipList.Count)];
                    return $"Great question about {keyword}, {userName}! {tip} Type 'tell me more' for another tip!";
                }
            }

            if (input.Contains("how are you"))
                return $"I am doing great and ready to help you stay safe online, {userName}!";

            if (input.Contains("what can you do") || input.Contains("help"))
                return $"I can help you with: passwords, phishing, scams, privacy, malware, vpn, 2fa, firewall, or encryption, {userName}!";

            if (input.Contains("thank") || input.Contains("thanks"))
                return $"You are very welcome, {userName}! Feel free to ask me anything else!";

            if (input.Contains("bye") || input.Contains("exit") || input.Contains("goodbye"))
            {
                AppendMessage("Bot", $"Goodbye {userName}! Remember to stay safe online! 🔒", Color.Cyan);
                return "";
            }

            return $"I am not sure about that, {userName}. Try asking about passwords, phishing, scams, privacy, malware, vpn, 2fa, firewall, or encryption!";
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }
    }
}