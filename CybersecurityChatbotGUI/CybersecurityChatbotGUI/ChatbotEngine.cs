using System;
using System.Collections.Generic;
using System.Linq;

namespace CybersecurityChatbotGUI
{
    //Memory: stores user details recalled during conversation
    public class UserMemory
    {
        public string Name            { get; set; } = "";
        public string FavouriteTopic  { get; set; } = "";
        public string LastTopic       { get; set; } = "";
        public bool   IsWorried       { get; set; } = false;
        public List<string> MentionedInterests { get; } = new();

        public void RecordInterest(string topic)
        {
            if (!MentionedInterests.Contains(topic))
                MentionedInterests.Add(topic);
            LastTopic = topic;
            if (string.IsNullOrEmpty(FavouriteTopic))
                FavouriteTopic = topic;
        }
    }

    //A single keyword-group entry
    public class ResponseEntry
    {
        public string   TopicLabel  { get; init; }
        public string[] Keywords    { get; init; }
        public string[] Responses   { get; init; }   // Multiple → random selection
        public bool     IsRandom    { get; init; }   // True = pick one at random

        public ResponseEntry(string label, string[] keywords, string[] responses, bool isRandom = false)
        {
            TopicLabel = label;
            Keywords   = keywords;
            Responses  = responses;
            IsRandom   = isRandom;
        }
    }

    //Sentiment category
    public enum Sentiment { Neutral, Worried, Curious, Frustrated }

    //Main chatbot engine
    public class ChatbotEngine
    {
        private readonly Random        _rng     = new();
        private readonly UserMemory    _memory  = new();
        private readonly List<ResponseEntry> _responses;
        private string _lastTopicLabel = "General";

        // Exposed to UI for status-bar display
        public string LastTopic => _lastTopicLabel;

        //Sentiment keyword maps
        private static readonly Dictionary<Sentiment, string[]> SentimentKeywords = new()
        {
            [Sentiment.Worried]    = new[] { "worried", "scared", "afraid", "nervous", "panic", "help me", "hacked", "stolen", "compromised", "attacked" },
            [Sentiment.Curious]    = new[] { "curious", "interested", "want to know", "tell me", "explain", "what is", "how does", "i wonder" },
            [Sentiment.Frustrated] = new[] { "frustrated", "confused", "don't understand", "dont understand", "lost", "annoying", "complicated", "too hard" }
        };

        //Empathetic openers based on sentiment
        private static readonly Dictionary<Sentiment, string[]> SentimentResponses = new()
        {
            [Sentiment.Worried]    = new[] {
                "It's completely understandable to feel that way — cybersecurity can feel overwhelming. Let me help! 💙",
                "Don't worry, you're not alone. Many people face this. Here's what you need to know:",
                "I hear you, and it's great that you're taking this seriously. Here's how to stay safe:"
            },
            [Sentiment.Curious]    = new[] {
                "Great curiosity — that's the first step to staying safe! 😊",
                "I love the enthusiasm! Here's what you need to know:",
                "Excellent question! Let me explain:"
            },
            [Sentiment.Frustrated] = new[] {
                "Cybersecurity can feel overwhelming, but I'll break it down simply for you. 👍",
                "No worries at all — let me explain this as clearly as possible:",
                "It's okay to find this confusing! Here's a straightforward explanation:"
            }
        };

        public ChatbotEngine()
        {
            _responses = BuildResponseTable();
        }

        public void SetUserName(string name)
        {
            _memory.Name = name;
        }

        //Public: detect sentiment in input
        public string DetectSentiment(string input)
        {
            string lower = input.ToLower();
            foreach (var kvp in SentimentKeywords)
                if (kvp.Value.Any(k => lower.Contains(k)))
                    return kvp.Key.ToString();
            return "";
        }

        //Internal: get sentiment enum
        private Sentiment ClassifySentiment(string input)
        {
            string lower = input.ToLower();
            foreach (var kvp in SentimentKeywords)
                if (kvp.Value.Any(k => lower.Contains(k)))
                    return kvp.Key;
            return Sentiment.Neutral;
        }

        //Build a prefix for empathetic replies
        private string EmpathyPrefix(Sentiment s)
        {
            if (s == Sentiment.Neutral) return "";
            var lines = SentimentResponses[s];
            return lines[_rng.Next(lines.Length)] + "\n\n";
        }

        //Memory personalisation suffix
        private string MemoryPersonalise(string topic)
        {
            _memory.RecordInterest(topic);
            if (_memory.MentionedInterests.Count > 1 && _rng.Next(3) == 0)
            {
                string prev = _memory.MentionedInterests
                    .Where(t => t != topic)
                    .OrderBy(_ => _rng.Next())
                    .FirstOrDefault();
                if (prev != null)
                    return $"\n\n💡 As someone interested in {prev}, you might also want to ask me about how they relate!";
            }
            return "";
        }

        //Main response dispatcher
        public string GetResponse(string rawInput)
        {
            string input = rawInput.ToLower().Trim();
            string name  = _memory.Name;

            Sentiment sentiment = ClassifySentiment(input);
            string    empathy   = EmpathyPrefix(sentiment);

            //Conversation flow: follow-ups
            if (IsFollowUp(input))
            {
                string followUp = HandleFollowUp(input);
                if (!string.IsNullOrEmpty(followUp))
                    return empathy + followUp;
            }

            //Memory: user mentions interest
            if (input.Contains("interested in") || input.Contains("i like") || input.Contains("i love"))
            {
                foreach (var entry in _responses)
                    foreach (string kw in entry.Keywords)
                        if (input.Contains(kw))
                        {
                            _memory.RecordInterest(entry.TopicLabel);
                            return empathy + $"Great! I'll remember that you're interested in {entry.TopicLabel}. " +
                                   $"It's a crucial part of staying safe online. " +
                                   PickResponse(entry);
                        }
            }

            //Memory: recall
            if (input.Contains("what do you remember") || input.Contains("what do you know about me"))
            {
                _lastTopicLabel = "Memory";
                return RecallMemory(name);
            }

            //Greeting & general
            if (input.Contains("how are you"))
            {
                _lastTopicLabel = "Greeting";
                return $"I'm great, thank you for asking, {name}! I'm fully charged and ready to help you stay cybersafe. " +
                       "How can I assist you today? You can ask about phishing, passwords, malware, and more!";
            }

            if (input.Contains("purpose") || input.Contains("what do you do") || input.Contains("what are you") || input.Contains("how can you help"))
            {
                _lastTopicLabel = "Purpose";
                return "My purpose is to educate you about cybersecurity. I can answer questions about phishing, " +
                       "malware, passwords, safe browsing, encryption, and much more. Ask away!";
            }

            if (input.Contains("topics") || input.Contains("what can i ask"))
            {
                _lastTopicLabel = "Help";
                return "You can ask me about:\n" +
                       "  🎣 Phishing & email scams\n  🔑 Password safety\n  🦠 Malware, viruses, ransomware\n" +
                       "  🕵️ Spyware, trojans, worms\n  🌐 Safe browsing & VPNs\n  🔐 Two-factor authentication (2FA)\n" +
                       "  🔒 Encryption & data backups\n  📶 Public Wi-Fi & privacy\n  🚨 Scams & social engineering\n\n" +
                       "Or click a quick topic on the right panel!";
            }

            if (input.Contains("help"))
            {
                _lastTopicLabel = "Help";
                return $"No worries, {name}! I'm here to guide you. " +
                       "Try asking about phishing, passwords, malware, safe browsing, 2FA, encryption, and more. " +
                       "You can also click the topic buttons on the right!";
            }

            if (input.Contains("thank"))
            {
                _lastTopicLabel = "Greeting";
                return $"You're very welcome, {name}! Staying informed is the first step to staying safe. " +
                       "Is there anything else you'd like to know?";
            }

            //Keyword matching from response table
            foreach (var entry in _responses)
            {
                foreach (string kw in entry.Keywords)
                {
                    if (input.Contains(kw))
                    {
                        _lastTopicLabel = entry.TopicLabel;
                        string core = PickResponse(entry);
                        return empathy + core + MemoryPersonalise(entry.TopicLabel);
                    }
                }
            }

            //Default / error handling
            _lastTopicLabel = "Unknown";
            return $"I didn't quite understand that, {name}. Could you rephrase?\n\n" +
                   "Try asking about: phishing, passwords, malware, ransomware, VPN, encryption, 2FA,\n" +
                   "social engineering, identity theft, data backups, antivirus — or type 'topics' for the full list!";
        }

        //Pick one response (random if flagged, else first)
        private string PickResponse(ResponseEntry entry)
        {
            if (entry.IsRandom || entry.Responses.Length > 1)
                return entry.Responses[_rng.Next(entry.Responses.Length)];
            return entry.Responses[0];
        }

        //Conversation flow helpers
        private bool IsFollowUp(string input)
        {
            string[] followUpPhrases = {
                "tell me more", "give me another", "more info", "explain more",
                "what else", "more details", "another tip", "more tips",
                "go on", "continue", "and what about"
            };
            return followUpPhrases.Any(p => input.Contains(p));
        }

        private string HandleFollowUp(string input)
        {
            if (string.IsNullOrEmpty(_memory.LastTopic)) return "";

            // Find the entry for last topic and return another random tip
            var entry = _responses.FirstOrDefault(e =>
                e.TopicLabel.Equals(_memory.LastTopic, StringComparison.OrdinalIgnoreCase));

            if (entry == null) return "";

            return $"Sure! Here's more on {_memory.LastTopic}:\n\n" +
                   entry.Responses[_rng.Next(entry.Responses.Length)];
        }

        private string RecallMemory(string name)
        {
            var sb = new System.Text.StringBuilder();
            sb.AppendLine($"Here's what I remember about you, {name}:");
            if (!string.IsNullOrEmpty(_memory.FavouriteTopic))
                sb.AppendLine($"  ⭐ Favourite topic so far: {_memory.FavouriteTopic}");
            if (_memory.MentionedInterests.Count > 0)
                sb.AppendLine($"  📚 Topics we discussed: {string.Join(", ", _memory.MentionedInterests)}");
            if (_memory.MentionedInterests.Count == 0)
                sb.AppendLine("  We haven't discussed any topics yet — feel free to ask me anything!");
            return sb.ToString();
        }

        //Response table (OOP, dictionaries/lists)
        private List<ResponseEntry> BuildResponseTable()
        {
            return new List<ResponseEntry>
            {
                //PHISHING (random)
                new ResponseEntry("Phishing",
                    keywords: new[] { "phishing", "phish", "email scam", "fake email" },
                    responses: new[]
                    {
                        "Phishing is when criminals send fake emails pretending to be a trusted source to steal your information. " +
                        "Always check the sender's email address and never click suspicious links!\n\n" +
                        "🔑 Key tip: Your bank will NEVER ask for your password via email.",

                        "Watch out for these phishing red flags:\n" +
                        "  ⚠️ Urgent subject lines ('Your account is at risk!')\n" +
                        "  ⚠️ Spelling errors and generic greetings ('Dear Customer')\n" +
                        "  ⚠️ Links that look almost right (paypa1.com instead of paypal.com)\n" +
                        "  ⚠️ Requests for personal data or login credentials\n\nIf in doubt, don't click!",

                        "To avoid phishing: never click links in unexpected emails. " +
                        "Go directly to the official website by typing the URL yourself. " +
                        "Enable MFA on all accounts so even a stolen password can't get attackers in.",

                        "Be cautious of emails asking for personal information. " +
                        "Scammers often disguise themselves as trusted organisations — banks, PayPal, Netflix, even your employer. " +
                        "When in doubt, pick up the phone and call the company directly using their official number."
                    },
                    isRandom: true),

                //SPEAR PHISHING
                new ResponseEntry("Spear Phishing",
                    keywords: new[] { "spear phishing" },
                    responses: new[]
                    {
                        "Spear phishing is targeted phishing where attackers personalise the attack using your name, " +
                        "job title, or personal details to seem convincing. It's far more dangerous than regular phishing! " +
                        "Always verify unexpected requests, even if they appear to come from someone you know."
                    }),

                //PASSWORDS (random)
                new ResponseEntry("Passwords",
                    keywords: new[] { "password", "passphrase", "credentials" },
                    responses: new[]
                    {
                        "Password Safety Tips:\n" +
                        "  🔑 Use at least 12 characters\n" +
                        "  🔑 Mix uppercase, lowercase, numbers, and symbols\n" +
                        "  🔑 Never reuse the same password on different sites\n" +
                        "  🔑 Use a password manager to keep them safe\n" +
                        "  🔑 Avoid your name, birthday, or common words\n\n" +
                        "Would you like to know about two-factor authentication (2FA) for extra security?",

                        "A strong password is your first line of defence. Consider using a passphrase — " +
                        "a string of 4+ random words like 'Purple-Tiger-Cloud-Seven'. " +
                        "It's easier to remember and harder to crack than 'P@ssw0rd123'!",

                        "Password managers (like Bitwarden, 1Password, or Dashlane) store all your passwords " +
                        "in an encrypted vault. You only need one strong master password. " +
                        "This lets you have a unique, complex password for every site — a huge security win!"
                    },
                    isRandom: true),

                //2FA
                new ResponseEntry("Two-Factor Authentication",
                    keywords: new[] { "2fa", "two factor", "two-factor", "multi-factor", "mfa", "authenticator" },
                    responses: new[]
                    {
                        "Two-Factor Authentication (2FA) adds a second security layer to your accounts. " +
                        "Even if someone steals your password, they can't log in without the second step " +
                        "(a code sent to your phone or generated by an app like Google Authenticator). " +
                        "Always enable 2FA on email, banking, and social media — it's one of the most effective protections available!"
                    }),

                //MALWARE
                new ResponseEntry("Malware",
                    keywords: new[] { "malware", "what is malware" },
                    responses: new[]
                    {
                        "Malware is harmful software designed to damage your device or steal data. " +
                        "Types include: viruses, ransomware, spyware, trojans, worms, and adware. " +
                        "Protect yourself with a trusted antivirus, keep software updated, " +
                        "and never download files from unknown sources. Ask me about any specific type!"
                    }),

                //RANSOMWARE (random)
                new ResponseEntry("Ransomware",
                    keywords: new[] { "ransomware" },
                    responses: new[]
                    {
                        "Ransomware locks or encrypts your files and demands payment to restore access. " +
                        "It often spreads through malicious email attachments or infected downloads. " +
                        "💡 Best defence: regularly back up your data to an offline or cloud location!",

                        "Preventing ransomware: back up data (3-2-1 rule), keep software patched, " +
                        "use reputable security software, and be alert to phishing (the main delivery method). " +
                        "Enable MFA and restrict user permissions to limit damage if infection occurs.",

                        "If you're hit by ransomware: don't pay the ransom (no guarantee of recovery), " +
                        "disconnect the infected device from the network immediately, " +
                        "report to authorities (SAPS Cyber Crime Unit in SA), and restore from a clean backup."
                    },
                    isRandom: true),

                //VIRUSES
                new ResponseEntry("Viruses",
                    keywords: new[] { "virus", "viruses" },
                    responses: new[]
                    {
                        "A computer virus attaches to legitimate files and spreads when those files are opened or shared. " +
                        "It can corrupt data, slow your device, or give hackers access. " +
                        "Keep your antivirus updated and avoid opening attachments from unknown senders!"
                    }),

                //SPYWARE
                new ResponseEntry("Spyware",
                    keywords: new[] { "spyware" },
                    responses: new[]
                    {
                        "Spyware secretly installs itself on your device and monitors your activity — " +
                        "recording keystrokes, browsing habits, and even passwords, then sending the data to cybercriminals. " +
                        "Run regular antivirus scans and avoid downloading software from unofficial sources!"
                    }),

                //TROJANS
                new ResponseEntry("Trojans",
                    keywords: new[] { "trojan" },
                    responses: new[]
                    {
                        "A Trojan disguises itself as a harmless program to trick you into installing it. " +
                        "Once in, it can give hackers remote access or steal your data. " +
                        "Only download software from verified, official sources — never from random websites!"
                    }),

                //WORMS
                new ResponseEntry("Worms",
                    keywords: new[] { "worm" },
                    responses: new[]
                    {
                        "A worm spreads automatically across networks without any human interaction — " +
                        "unlike viruses, it doesn't need to attach to a file. " +
                        "Keeping your firewall active and OS patched is the best defence against worms!"
                    }),

                //VPN (random)
                new ResponseEntry("VPN",
                    keywords: new[] { "vpn", "virtual private network" },
                    responses: new[]
                    {
                        "A VPN (Virtual Private Network) encrypts your internet connection and hides your IP address, " +
                        "making it harder for hackers or ISPs to spy on your activity. " +
                        "Especially important on public Wi-Fi in cafes or airports!",

                        "Good free VPN options include Proton VPN (no data limit). " +
                        "For paid options, look at NordVPN, ExpressVPN, or Surfshark. " +
                        "Avoid 'free VPNs' that seem too good — they may sell your data instead of protecting it!"
                    },
                    isRandom: true),

                //FIREWALL
                new ResponseEntry("Firewall",
                    keywords: new[] { "firewall" },
                    responses: new[]
                    {
                        "A firewall monitors and controls network traffic coming in and going out of your device. " +
                        "It acts like a security guard — blocking suspicious connections. " +
                        "Keep your OS firewall ON at all times, and consider a router-level firewall for extra protection!"
                    }),

                //ENCRYPTION
                new ResponseEntry("Encryption",
                    keywords: new[] { "encryption", "encrypt", "end-to-end" },
                    responses: new[]
                    {
                        "Encryption converts your data into a coded format only readable by someone with the correct key. " +
                        "HTTPS encrypts your web traffic. End-to-end encryption (E2EE) in apps like WhatsApp means " +
                        "only you and the recipient can read your messages — not even the app company can."
                    }),

                //SOCIAL ENGINEERING (random)
                new ResponseEntry("Social Engineering",
                    keywords: new[] { "social engineering", "pretexting", "impersonation" },
                    responses: new[]
                    {
                        "Social engineering tricks people (not computers) into giving away sensitive info. " +
                        "Attackers may pose as IT support, managers, or bank officials — creating urgency or fear. " +
                        "Always verify who you're speaking to before sharing any information!",

                        "Common social engineering tactics:\n" +
                        "  🎭 Pretexting — fake scenario to gain your trust\n" +
                        "  😱 Urgency — 'Act now or your account will be closed!'\n" +
                        "  🎁 Baiting — 'You've won a prize, click here!'\n" +
                        "  🤝 Tailgating — following someone into a secure area\n\n" +
                        "Trust your instincts — if something feels off, it probably is!"
                    },
                    isRandom: true),

                //IDENTITY THEFT
                new ResponseEntry("Identity Theft",
                    keywords: new[] { "identity theft", "stolen identity", "identity fraud" },
                    responses: new[]
                    {
                        "Identity theft is when criminals steal your personal info to commit fraud in your name. " +
                        "Protect yourself: use strong unique passwords, enable 2FA, monitor bank statements, " +
                        "shred sensitive documents, and be cautious about what you share online. " +
                        "If you suspect theft, contact your bank and SAPS immediately!"
                    }),

                //PRIVACY (random)
                new ResponseEntry("Privacy",
                    keywords: new[] { "privacy", "privacy settings", "digital privacy" },
                    responses: new[]
                    {
                        "Managing your privacy is key to staying safe online:\n" +
                        "  👁️ Review social media privacy settings regularly\n" +
                        "  📍 Turn off location tracking for apps that don't need it\n" +
                        "  🎤 Check which apps access your camera/microphone/contacts\n" +
                        "  🔍 Use a privacy-focused browser (Brave, Firefox) and search engine (DuckDuckGo)",

                        "Your data is valuable — treat it like your wallet! " +
                        "Limit what personal info you share publicly, read app permissions before granting access, " +
                        "and regularly clear your cookies and browsing history."
                    },
                    isRandom: true),

                //SCAMS (random)
                new ResponseEntry("Online Scams",
                    keywords: new[] { "scam", "online scam", "fraud", "fake", "too good to be true" },
                    responses: new[]
                    {
                        "Online scams include fake job offers, lottery winnings, romance scams, and more. " +
                        "If it sounds too good to be true, it almost certainly is!\n\n" +
                        "Tips: Never send money to strangers online, research companies before applying for jobs, " +
                        "and report scams to your local cybercrime authority.",

                        "Watch for these scam warning signs:\n" +
                        "  🚩 Requests for upfront payment or gift cards\n" +
                        "  🚩 Unsolicited prize or lottery winnings\n" +
                        "  🚩 Romantic partners who won't video call\n" +
                        "  🚩 Urgent requests for personal information\n\n" +
                        "Staying sceptical online is a powerful defence!"
                    },
                    isRandom: true),

                //PUBLIC WI-FI
                new ResponseEntry("Public Wi-Fi",
                    keywords: new[] { "public wifi", "public wi-fi", "free wifi", "open wifi" },
                    responses: new[]
                    {
                        "Public Wi-Fi is risky! Hackers can set up fake hotspots or intercept traffic. " +
                        "Tips:\n  🔒 Always use a VPN\n  🏦 Avoid banking or sensitive logins\n" +
                        "  🔐 Make sure websites use HTTPS\n  📶 Turn off auto-connect to open networks"
                    }),

                //DATA BACKUP
                new ResponseEntry("Data Backup",
                    keywords: new[] { "backup", "back up", "data backup" },
                    responses: new[]
                    {
                        "Regular data backups are one of the most important cybersecurity habits!\n\n" +
                        "Follow the 3-2-1 rule:\n" +
                        "  3️⃣ Keep 3 copies of your data\n" +
                        "  2️⃣ On 2 different types of media\n" +
                        "  1️⃣ With 1 copy offsite or in the cloud\n\n" +
                        "Test your backups regularly to make sure they actually restore correctly!"
                    }),

                //ANTIVIRUS
                new ResponseEntry("Antivirus",
                    keywords: new[] { "antivirus", "anti-virus", "security software" },
                    responses: new[]
                    {
                        "Antivirus software is your first line of defence against malware and threats.\n" +
                        "  🛡️ Keep it updated (new threats emerge daily)\n" +
                        "  🔍 Run full scans at least weekly\n" +
                        "  ⚠️ Don't run two AV programs simultaneously (they can conflict)\n" +
                        "  🔒 Pair with a firewall for stronger protection!"
                    }),

                //MOBILE SECURITY
                new ResponseEntry("Mobile Security",
                    keywords: new[] { "mobile security", "phone security", "smartphone" },
                    responses: new[]
                    {
                        "Your smartphone holds a huge amount of personal data!\n" +
                        "  📱 Use a strong PIN, pattern, or biometric lock\n" +
                        "  🏪 Only download apps from official stores (Google Play, App Store)\n" +
                        "  🔄 Keep your phone's OS updated\n" +
                        "  🔍 Enable remote wipe in case of loss/theft\n" +
                        "  🔌 Avoid charging on public USB ports (juice jacking risk!)"
                    }),

                //SAFE BROWSING (random)
                new ResponseEntry("Safe Browsing",
                    keywords: new[] { "browsing", "safe browsing", "internet safety", "browse safely" },
                    responses: new[]
                    {
                        "Safe Browsing Tips:\n" +
                        "  🔒 Only visit HTTPS websites (padlock in address bar)\n" +
                        "  🚫 Avoid pop-up ads and suspicious links\n" +
                        "  🔄 Keep your browser and extensions updated\n" +
                        "  🌐 Use a VPN on public Wi-Fi\n" +
                        "  🍪 Clear cookies and browsing history regularly",

                        "For maximum browsing safety, consider using Brave or Firefox browsers. " +
                        "Install uBlock Origin to block ads and trackers. " +
                        "Use HTTPS Everywhere extension and a private search engine like DuckDuckGo."
                    },
                    isRandom: true),

                //HTTPS
                new ResponseEntry("HTTPS",
                    keywords: new[] { "https", "http", "secure website", "padlock" },
                    responses: new[]
                    {
                        "HTTPS encrypts the data you send and receive with a website. " +
                        "Always look for the padlock icon in your browser before entering personal information. " +
                        "HTTP (without the S) is NOT secure — avoid entering passwords or payment details on HTTP sites!"
                    }),

                //DARK WEB
                new ResponseEntry("Dark Web",
                    keywords: new[] { "dark web", "darkweb", "deep web" },
                    responses: new[]
                    {
                        "The dark web requires special software (Tor) to access and isn't indexed by search engines. " +
                        "It's used for both legitimate privacy purposes and criminal activity (selling stolen data, malware). " +
                        "Check if your data is exposed at haveibeenpwned.com — it's free and safe!"
                    }),

                //SOFTWARE UPDATES
                new ResponseEntry("Software Updates",
                    keywords: new[] { "software update", "update", "patch", "why update" },
                    responses: new[]
                    {
                        "Keeping software updated is one of the simplest yet most effective security habits! " +
                        "Updates patch vulnerabilities hackers could exploit. " +
                        "Enable automatic updates for your OS, browser, antivirus, and all apps. " +
                        "Many major attacks succeed simply because victims hadn't applied available patches!"
                    }),

                //CYBERBULLYING
                new ResponseEntry("Cyberbullying",
                    keywords: new[] { "cyberbullying", "online harassment", "cyber harassment" },
                    responses: new[]
                    {
                        "Cyberbullying uses technology to harass, threaten, or humiliate someone online.\n" +
                        "If you're being cyberbullied:\n" +
                        "  🚫 Don't respond to the bully\n" +
                        "  📸 Take screenshots as evidence\n" +
                        "  🔕 Block and report on the platform\n" +
                        "  👩‍👩‍👦 Tell a trusted adult or authority\n\n" +
                        "Remember — you are not alone and help is always available!"
                    }),

                //GENERAL TIPS (random)
                new ResponseEntry("Cybersecurity Tips",
                    keywords: new[] { "cybersecurity tips", "cyber tips", "stay safe", "general tips", "golden rules" },
                    responses: new[]
                    {
                        "🛡️ Golden Rules of Cybersecurity:\n" +
                        "  1. Use strong, unique passwords + a password manager\n" +
                        "  2. Enable 2FA on all important accounts\n" +
                        "  3. Keep all software and devices updated\n" +
                        "  4. Think before you click — be suspicious of unexpected links\n" +
                        "  5. Back up your data regularly (3-2-1 rule)\n" +
                        "  6. Use a VPN on public Wi-Fi\n" +
                        "  7. Protect your privacy settings on social media\n" +
                        "  8. Install reputable antivirus software\n" +
                        "  9. Learn to recognise phishing and social engineering\n" +
                        "  10. Report suspicious activity to the right authorities\n\n" +
                        "Cybersecurity is a habit, not a one-time action!",

                        "Quick cybersecurity wins you can do TODAY:\n" +
                        "  ✅ Turn on 2FA on your email\n" +
                        "  ✅ Check haveibeenpwned.com for breached passwords\n" +
                        "  ✅ Update your most important passwords\n" +
                        "  ✅ Enable automatic updates on your phone and PC\n" +
                        "  ✅ Back up your important files to the cloud"
                    },
                    isRandom: true)
            };
        }
    }
}
