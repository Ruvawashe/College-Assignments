using System;
using System.Drawing;
using System.Windows.Forms;

namespace CybersecurityChatbotGUI
{
    public partial class ChatForm : Form
    {
        private readonly ChatbotEngine _engine;
        private string _userName = "";

        //Controls
        private Panel        pnlHeader;
        private Label        lblTitle;
        private Label        lblSubtitle;
        private PictureBox   picLogo;
        private RichTextBox  rtbChat;
        private Panel        pnlInput;
        private TextBox      txtInput;
        private Button       btnSend;
        private Button       btnClear;
        private Panel        pnlTopics;
        private Label        lblTopicsTitle;
        private FlowLayoutPanel flpTopics;
        private StatusStrip  statusStrip;
        private ToolStripStatusLabel lblStatus;
        private Panel        pnlNameEntry;
        private Label        lblNamePrompt;
        private TextBox      txtName;
        private Button       btnStart;

        public ChatForm()
        {
            _engine = new ChatbotEngine();
            InitializeComponent();
            ShowNamePanel();
        }

        //Designer-equivalent setup
        private void InitializeComponent()
        {
            this.SuspendLayout();

            // Form
            Text            = "🔐 Cybersecurity Awareness Chatbot";
            Size            = new Size(900, 700);
            MinimumSize     = new Size(800, 600);
            BackColor       = Color.FromArgb(15, 20, 40);
            Font            = new Font("Segoe UI", 10f);
            StartPosition   = FormStartPosition.CenterScreen;

            //Header
            pnlHeader = new Panel
            {
                Dock      = DockStyle.Top,
                Height    = 90,
                BackColor = Color.FromArgb(20, 30, 60),
                Padding   = new Padding(15, 10, 15, 10)
            };

            lblTitle = new Label
            {
                Text      = "🔐  CYBERSECURITY AWARENESS BOT",
                ForeColor = Color.FromArgb(0, 200, 255),
                Font      = new Font("Segoe UI", 16f, FontStyle.Bold),
                AutoSize  = true,
                Location  = new Point(15, 12)
            };

            lblSubtitle = new Label
            {
                Text      = "Keeping you safe online — ask me anything about cybersecurity!",
                ForeColor = Color.FromArgb(150, 180, 220),
                Font      = new Font("Segoe UI", 9f, FontStyle.Italic),
                AutoSize  = true,
                Location  = new Point(18, 50)
            };

            pnlHeader.Controls.AddRange(new Control[] { lblTitle, lblSubtitle });

            //Status bar
            statusStrip = new StatusStrip { BackColor = Color.FromArgb(20, 30, 60) };
            lblStatus   = new ToolStripStatusLabel
            {
                Text      = "Welcome! Enter your name to begin.",
                ForeColor = Color.FromArgb(0, 200, 255)
            };
            statusStrip.Items.Add(lblStatus);

            //Topics sidebar
            pnlTopics = new Panel
            {
                Dock      = DockStyle.Right,
                Width     = 185,
                BackColor = Color.FromArgb(18, 26, 52),
                Padding   = new Padding(8)
            };

            lblTopicsTitle = new Label
            {
                Text      = "💡 Quick Topics",
                ForeColor = Color.FromArgb(0, 200, 255),
                Font      = new Font("Segoe UI", 10f, FontStyle.Bold),
                Dock      = DockStyle.Top,
                Height    = 30,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding   = new Padding(4, 0, 0, 0)
            };

            flpTopics = new FlowLayoutPanel
            {
                Dock          = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                WrapContents  = false,
                AutoScroll    = true,
                Padding       = new Padding(0, 5, 0, 0)
            };

            string[] topics = {
                "🎣 Phishing", "🔑 Passwords", "🦠 Malware",
                "💰 Ransomware", "🕵️ Spyware", "🌐 VPN",
                "🔒 Encryption", "📱 Mobile Security",
                "🛡️ Firewall", "📶 Public Wi-Fi",
                "🔐 2FA", "🗄️ Data Backup",
                "👁️ Privacy", "🚨 Online Scams",
                "💬 Social Engineering"
            };

            foreach (string t in topics)
            {
                Button btn = new Button
                {
                    Text      = t,
                    Width     = 163,
                    Height    = 30,
                    FlatStyle = FlatStyle.Flat,
                    BackColor = Color.FromArgb(30, 45, 80),
                    ForeColor = Color.FromArgb(180, 210, 255),
                    Font      = new Font("Segoe UI", 8.5f),
                    Margin    = new Padding(0, 2, 0, 2),
                    Tag       = t
                };
                btn.FlatAppearance.BorderColor = Color.FromArgb(50, 80, 130);
                btn.Click += TopicButton_Click;
                flpTopics.Controls.Add(btn);
            }

            pnlTopics.Controls.Add(flpTopics);
            pnlTopics.Controls.Add(lblTopicsTitle);

            //Chat display
            rtbChat = new RichTextBox
            {
                Dock       = DockStyle.Fill,
                ReadOnly   = true,
                BackColor  = Color.FromArgb(15, 20, 40),
                ForeColor  = Color.White,
                Font       = new Font("Segoe UI", 10f),
                BorderStyle= BorderStyle.None,
                ScrollBars = RichTextBoxScrollBars.Vertical,
                Padding    = new Padding(10)
            };

            //Input panel
            pnlInput = new Panel
            {
                Dock      = DockStyle.Bottom,
                Height    = 60,
                BackColor = Color.FromArgb(20, 30, 60),
                Padding   = new Padding(10, 8, 10, 8)
            };

            txtInput = new TextBox
            {
                Dock        = DockStyle.Fill,
                BackColor   = Color.FromArgb(30, 45, 80),
                ForeColor   = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Font        = new Font("Segoe UI", 10.5f),
                PlaceholderText = "Type your message here..."
            };
            txtInput.KeyDown += TxtInput_KeyDown;

            btnSend = new Button
            {
                Text      = "Send ➤",
                Dock      = DockStyle.Right,
                Width     = 90,
                BackColor = Color.FromArgb(0, 120, 200),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font      = new Font("Segoe UI", 10f, FontStyle.Bold),
                Cursor    = Cursors.Hand
            };
            btnSend.FlatAppearance.BorderSize = 0;
            btnSend.Click += BtnSend_Click;

            btnClear = new Button
            {
                Text      = "Clear",
                Dock      = DockStyle.Right,
                Width     = 65,
                BackColor = Color.FromArgb(60, 30, 30),
                ForeColor = Color.FromArgb(255, 120, 120),
                FlatStyle = FlatStyle.Flat,
                Font      = new Font("Segoe UI", 9f),
                Cursor    = Cursors.Hand,
                Margin    = new Padding(0, 0, 5, 0)
            };
            btnClear.FlatAppearance.BorderSize = 0;
            btnClear.Click += (s, e) => { rtbChat.Clear(); AppendBotMessage("Chat cleared. How can I help you?"); };

            pnlInput.Controls.Add(txtInput);
            pnlInput.Controls.Add(btnClear);
            pnlInput.Controls.Add(btnSend);

            //Name Entry Overlay
            pnlNameEntry = new Panel
            {
                Size      = new Size(420, 200),
                BackColor = Color.FromArgb(20, 30, 60),
                BorderStyle = BorderStyle.FixedSingle
            };

            lblNamePrompt = new Label
            {
                Text      = "🔐  Welcome to the Cybersecurity Awareness Bot!\n\nPlease enter your name to begin:",
                ForeColor = Color.FromArgb(180, 220, 255),
                Font      = new Font("Segoe UI", 10f),
                Location  = new Point(20, 20),
                Size      = new Size(380, 75),
                TextAlign = ContentAlignment.TopCenter
            };

            txtName = new TextBox
            {
                Location    = new Point(50, 110),
                Size        = new Size(220, 30),
                BackColor   = Color.FromArgb(30, 45, 80),
                ForeColor   = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Font        = new Font("Segoe UI", 11f),
                PlaceholderText = "Your name..."
            };
            txtName.KeyDown += TxtName_KeyDown;

            btnStart = new Button
            {
                Text      = "Start Chat ➤",
                Location  = new Point(280, 108),
                Size      = new Size(105, 32),
                BackColor = Color.FromArgb(0, 120, 200),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font      = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                Cursor    = Cursors.Hand
            };
            btnStart.FlatAppearance.BorderSize = 0;
            btnStart.Click += BtnStart_Click;

            pnlNameEntry.Controls.AddRange(new Control[] { lblNamePrompt, txtName, btnStart });

            //Assemble
            Controls.Add(rtbChat);
            Controls.Add(pnlTopics);
            Controls.Add(pnlInput);
            Controls.Add(pnlHeader);
            Controls.Add(statusStrip);
            Controls.Add(pnlNameEntry);

            this.Load  += ChatForm_Load;
            this.Resize += ChatForm_Resize;

            this.ResumeLayout(false);
        }

        //Name Panel Logic
        private void ShowNamePanel()
        {
            pnlNameEntry.Visible = true;
            CenterNamePanel();
            txtName.Focus();
        }

        private void CenterNamePanel()
        {
            pnlNameEntry.Location = new Point(
                (ClientSize.Width  - pnlNameEntry.Width)  / 2,
                (ClientSize.Height - pnlNameEntry.Height) / 2);
        }

        private void ChatForm_Load(object sender, EventArgs e) => CenterNamePanel();
        private void ChatForm_Resize(object sender, EventArgs e) => CenterNamePanel();

        private void TxtName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) BtnStart_Click(sender, e);
        }

        private void BtnStart_Click(object sender, EventArgs e)
        {
            string name = txtName.Text.Trim();
            if (string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show("Please enter your name to continue.", "Name Required",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtName.Focus();
                return;
            }
            _userName = name;
            _engine.SetUserName(name);
            pnlNameEntry.Visible = false;
            lblStatus.Text = $"Chatting as: {_userName}";
            PrintWelcome();
            txtInput.Focus();
        }

        //Chat Logic
        private void PrintWelcome()
        {
            AppendAsciiArt();
            AppendBotMessage($"Hello {_userName}! 👋 I'm your Cybersecurity Awareness Bot. " +
                "I'm here to help you stay safe online.\n\n" +
                "You can ask me about phishing, passwords, malware, VPNs, encryption, and much more. " +
                "Use the quick topic buttons on the right, or just type your question below!");
        }

        private void AppendAsciiArt()
        {
            rtbChat.SelectionColor = Color.FromArgb(0, 180, 220);
            rtbChat.SelectionFont  = new Font("Courier New", 8f);
            rtbChat.AppendText(
                "  ██████╗██╗   ██╗██████╗ ███████╗██████╗ \n" +
                " ██╔════╝╚██╗ ██╔╝██╔══██╗██╔════╝██╔══██╗\n" +
                " ██║      ╚████╔╝ ██████╔╝█████╗  ██████╔╝\n" +
                " ██║       ╚██╔╝  ██╔══██╗██╔══╝  ██╔══██╗\n" +
                " ╚██████╗   ██║   ██████╔╝███████╗██║  ██║\n" +
                "  ╚═════╝   ╚═╝   ╚═════╝ ╚══════╝╚═╝  ╚═╝\n\n");
            rtbChat.SelectionFont = new Font("Segoe UI", 10f);
        }

        private void AppendUserMessage(string msg)
        {
            rtbChat.SelectionColor = Color.FromArgb(100, 200, 255);
            rtbChat.SelectionFont  = new Font("Segoe UI", 10f, FontStyle.Bold);
            rtbChat.AppendText($"\n  👤 {_userName}: ");
            rtbChat.SelectionColor = Color.FromArgb(200, 220, 255);
            rtbChat.SelectionFont  = new Font("Segoe UI", 10f);
            rtbChat.AppendText(msg + "\n");
            rtbChat.ScrollToCaret();
        }

        private void AppendBotMessage(string msg)
        {
            rtbChat.SelectionColor = Color.FromArgb(0, 220, 180);
            rtbChat.SelectionFont  = new Font("Segoe UI", 10f, FontStyle.Bold);
            rtbChat.AppendText("\n  🤖 Bot: ");
            rtbChat.SelectionColor = Color.FromArgb(220, 240, 255);
            rtbChat.SelectionFont  = new Font("Segoe UI", 10f);
            rtbChat.AppendText(msg + "\n");
            rtbChat.ScrollToCaret();
        }

        private void AppendSentimentNote(string note)
        {
            rtbChat.SelectionColor = Color.FromArgb(255, 180, 80);
            rtbChat.SelectionFont  = new Font("Segoe UI", 9f, FontStyle.Italic);
            rtbChat.AppendText($"  [{note}]\n");
        }

        private void SendMessage()
        {
            string userInput = txtInput.Text.Trim();
            if (string.IsNullOrWhiteSpace(userInput)) return;

            txtInput.Clear();

            if (userInput.ToLower() == "exit" || userInput.ToLower() == "quit" || userInput.ToLower() == "bye")
            {
                AppendUserMessage(userInput);
                AppendBotMessage($"Goodbye, {_userName}! Stay safe online. 👋");
                btnSend.Enabled = false;
                txtInput.Enabled = false;
                return;
            }

            AppendUserMessage(userInput);

            // Sentiment detection note
            string sentiment = _engine.DetectSentiment(userInput);
            if (!string.IsNullOrEmpty(sentiment))
                AppendSentimentNote($"Sentiment detected: {sentiment}");

            string response = _engine.GetResponse(userInput);
            AppendBotMessage(response);
            lblStatus.Text = $"Chatting as: {_userName}  |  Topic: {_engine.LastTopic}";
        }

        //Event Handlers
        private void BtnSend_Click(object sender, EventArgs e)  => SendMessage();

        private void TxtInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                SendMessage();
            }
        }

        private void TopicButton_Click(object sender, EventArgs e)
        {
            if (pnlNameEntry.Visible) return;
            string topic = ((Button)sender).Text;
            // Strip the emoji prefix
            string keyword = topic.Length > 3 ? topic.Substring(2).Trim().ToLower() : topic.ToLower();
            txtInput.Text = keyword;
            SendMessage();
        }
    }
}
