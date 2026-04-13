using System;
using System.Speech.Synthesis;       // Needed for playing greeting
using System.Threading;             // Needed for the typing delay effect

class CybersecurityBot
{
    static void Main(string[] args)
    {

        DisplayLogo();
        PlayVoiceGreeting();
        //Ask the user for their name and greet them
        string userName = GetUserName();
        DisplayWelcomeMessage(userName);
        //Start the chat loop
        StartChat(userName);
    }
    static void PlayVoiceGreeting()
    {
        SpeechSynthesizer speaker = new SpeechSynthesizer();
        speaker.Volume = 100;
        speaker.Rate = 0;

        speaker.Speak("Hello! Welcome to the Cybersecurity Awareness Bot. I am here to help you stay safe online. How can I help you?");
    }

    static void DisplayLogo()
    {
        Console.Clear(); // Clear the screen first

        PrintDivider();
        Console.WriteLine("                                                        ");
        Console.WriteLine("        ██████╗██╗   ██╗██████╗ ███████╗██████╗         ");
        Console.WriteLine("       ██╔════╝╚██╗ ██╔╝██╔══██╗██╔════╝██╔══██╗        ");
        Console.WriteLine("       ██║      ╚████╔╝ ██████╔╝█████╗  ██████╔╝        ");
        Console.WriteLine("       ██║       ╚██╔╝  ██╔══██╗██╔══╝  ██╔══██╗        ");
        Console.WriteLine("       ╚██████╗   ██║   ██████╔╝███████╗██║  ██║        ");
        Console.WriteLine("        ╚═════╝   ╚═╝   ╚═════╝ ╚══════╝╚═╝  ╚═╝        ");
        Console.WriteLine("                                                        ");
        SetColour(ConsoleColor.Cyan);
        Console.WriteLine("         /\\ CYBERSECURITY AWARENESS BOT   /\\       ");
        Console.WriteLine("        /  \\   Keeping you safe online   /  \\      ");
        Console.WriteLine("       /    \\ ========================= /    \\     ");
        Console.WriteLine("      / SAFE \\ [shield] [lock] [alert] / SAFE \\    ");
        Console.WriteLine("     /________\\_______________________/________\\   ");
        Console.WriteLine("                                                     ");
        PrintDivider();

        ResetColour();

        Console.WriteLine();
    }

    static string GetUserName()
    {
        SetColour(ConsoleColor.Green);
        Console.Write("  Please enter your name: ");
        ResetColour();

        string name = Console.ReadLine();

        // Input validation - keep asking until they enter something
        while (string.IsNullOrWhiteSpace(name))
        {
            SetColour(ConsoleColor.Red);
            Console.WriteLine("  Name cannot be empty. Please try again.");
            ResetColour();

            SetColour(ConsoleColor.Green);
            Console.Write("  Please enter your name: ");
            ResetColour();

            name = Console.ReadLine();
        }

        return name.Trim(); // Remove extra spaces from the name
    }

    static void DisplayWelcomeMessage(string userName)
    {
        Console.WriteLine();
        PrintDivider();

        SetColour(ConsoleColor.Magenta);
        Console.WriteLine($"  Welcome, {userName}! I'm your Cybersecurity Awareness Bot.");
        Console.WriteLine("  I'm here to help you stay safe online.");
        ResetColour();

        PrintDivider();
        Console.WriteLine();
    }

    static void StartChat(string userName)
    {
        SetColour(ConsoleColor.White);
        Console.WriteLine($" Hey! {userName}, Type a question to get started, or type 'exit' to quit.");
        Console.WriteLine("  (I can offer assistance with Cybersecurity related questions like 'what is phishing', 'password tips', etc.)");
        ResetColour();

        Console.WriteLine();

        // This loop runs until the user types "exit"
        while (true)
        {
            // Show the input prompt
            SetColour(ConsoleColor.Green);
            Console.Write($"  {userName}: ");
            ResetColour();

            string userInput = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(userInput))
            {
                ShowBotResponse("I didn't receive any input. Could you please type something?");
                continue; // Go back to the top of the loop
            }

            // Convert to lowercase so we can compare easily
            string input = userInput.ToLower().Trim();

            // Check if user wants to exit
            if (input == "exit" || input == "quit" || input == "bye")
            {
                ShowBotResponse($"Goodbye, {userName}! Stay safe online. See you next time!");
                break; // Exit the loop
            }

            string response = GetResponse(input, userName);
            ShowBotResponse(response);

            Console.WriteLine(); // Space between conversations
        }
    }

    static string GetResponse(string input, string userName)
    {

        // Catchphrase 1 - How are you
        if (input.Contains("how are you"))
        {
            return $"I am great, thank you for asking, {userName}! I am fully charged and ready to help you stay cybersafe! " +
                   "How can I assist you with cybersecurity today? You could ask me about phishing, passwords, or malware!";
        }

        // Catchphrase 2 - Purpose / what do you do
        if (input.Contains("purpose") || input.Contains("what do you do") || input.Contains("what are you") || input.Contains("how can you help"))
        {
            return "My purpose is to educate you about cybersecurity. I can answer questions " +
                   "about various topics on cyber security! Feel free to ask anything and I will answer the best I can. " +
                   "Try asking me about phishing, ransomware, passwords, or safe browsing!";
        }

        // Catchphrase 3 - Topics / help menu
        if (input.Contains("what can i ask") || input.Contains("topics"))
        {
            return "You can ask me about:\n" +
                   "  - Phishing & email scams\n" +
                   "  - Password safety & managers\n" +
                   "  - Safe browsing & VPNs\n" +
                   "  - Malware, viruses, ransomware, spyware, worms & trojans\n" +
                   "  - Social engineering & identity theft\n" +
                   "  - Two-factor authentication (2FA)\n" +
                   "  - Firewalls, encryption & data backups\n" +
                   "  - Public Wi-Fi, updates & privacy\n" +
                   "  What would you like to know more about?";
        }

        // Catchphrase 4 - Help keyword
        if (input.Contains("help"))
        {
            return $"No worries, {userName}! I am here to guide you. You can ask me about phishing, " +
                   "passwords, malware, safe browsing, 2FA, encryption, and much more. " +
                   "Just type a topic and I will explain it to you!";
        }

        // Catchphrase 5 - Thank you
        if (input.Contains("thank") || input.Contains("thanks"))
        {
            return $"You are very welcome, {userName}! Staying informed is the first step to staying safe. " +
                   "Is there anything else you would like to know about cybersecurity?";
        }

        // Catchphrase 6 - What is phishing
        if (input.Contains("what is phishing") || input.Contains("explain phishing"))
        {
            return "Phishing is when criminals send fake emails or messages pretending to be " +
                   "a trusted company (like your bank) to steal your personal information. " +
                   "Always check the sender's email address and never click suspicious links!\n" +
                   "Tip: Ask me 'how can I prevent phishing?' to learn how to protect yourself!";
        }

        // Catchphrase 7 - Phishing (general mention)
        if (input.Contains("phishing"))
        {
            return "Phishing attacks are one of the most common cyber threats today! " +
                   "Criminals disguise themselves as trustworthy sources to steal your data. " +
                   "Would you like to know what phishing is, or how to prevent it? Just ask!";
        }

        // Catchphrase 8 - Prevent phishing
        if (input.Contains("prevent phishing") || input.Contains("safe from phishing") || input.Contains("avoid phishing"))
        {
            return "To avoid phishing, never click links or download attachments in unexpected emails or texts. " +
                   "Verify suspicious requests directly through official websites or phone numbers. Key defenses " +
                   "include enabling multi-factor authentication (MFA), keeping software updated, using strong unique " +
                   "passwords, and watching for spelling errors, generic greetings, and high-pressure threats. " +
                   "Remember: your bank will NEVER ask for your password via email!";
        }

        // Catchphrase 9 - Spear phishing
        if (input.Contains("spear phishing"))
        {
            return "Spear phishing is a targeted form of phishing where attackers personalise the attack " +
                   "using your name, job title, or other details to make it seem more convincing. " +
                   "It is much more dangerous than regular phishing! Always verify unexpected requests, " +
                   "even if they appear to come from someone you know. Want to know about regular phishing too?";
        }

        // Catchphrase 10 - General malware / virus
        if (input.Contains("malware") || input.Contains("what is malware"))
        {
            return "Malware is harmful software designed to damage your computer or steal data. " +
                   "There are many types: viruses, ransomware, spyware, trojans, worms, and adware. " +
                   "Protect yourself by installing a trusted antivirus program, " +
                   "keeping your system updated, and never downloading files from unknown sources. " +
                   "Ask me about any specific type of malware to learn more!";
        }

        // Catchphrase 11 - Viruses (what is)
        if (input.Contains("what is a virus") || input.Contains("what is virus") || input.Contains("explain virus"))
        {
            return "A computer virus is malware that attaches itself to legitimate files and " +
                   "spreads when those files are opened or shared. " +
                   "It can corrupt data, slow down your device, or give hackers access to your system. " +
                   "Keeping your antivirus updated is the best way to protect against viruses! " +
                   "Want to know how to prevent viruses? Just ask!";
        }

        // Catchphrase 12 - Viruses (general)
        if (input.Contains("viruses") || input.Contains("virus"))
        {
            return "Computer viruses are a type of malware that can spread between devices and cause serious damage. " +
                   "They can delete files, slow your device, or open backdoors for hackers. " +
                   "Ask me 'how can I prevent viruses?' to get protection tips!";
        }

        // Catchphrase 13 - Prevent viruses
        if (input.Contains("prevent virus") || input.Contains("safe from viruses") || input.Contains("avoid viruses"))
        {
            return "Prevent viruses and malicious code by using reputable antivirus software, keeping all software updated, " +
                   "avoiding suspicious links and attachments, and practicing safe browsing habits. Key actions include using strong " +
                   "unique passwords with multi-factor authentication (MFA), enabling firewalls, and backing up data regularly to " +
                   "prevent ransomware losses. Running weekly antivirus scans is a great habit to build!";
        }

        // Catchphrase 14 - Ransomware (what is)
        if (input.Contains("what is ransomware") || input.Contains("explain ransomware"))
        {
            return "Ransomware is a type of malware that locks or encrypts your files and demands a payment to restore access. " +
                   "It often spreads through malicious email attachments or infected downloads. " +
                   "Victims are usually asked to pay in cryptocurrency to remain untraceable. " +
                   "Regularly backing up your data is the best defence against ransomware attacks! " +
                   "Ask me 'how to prevent ransomware' for protection tips!";
        }

        // Catchphrase 15 - Ransomware (general or prevent)
        if (input.Contains("ransomware"))
        {
            return "Ransomware is one of the most damaging cyber threats out there! " +
                   "Preventing it requires a layered defence: regularly back up data (ideally offline), " +
                   "keep software patched, use reputable security software, and educate yourself against phishing, " +
                   "which is a primary infection vector. Enable multi-factor authentication (MFA) and restrict " +
                   "user permissions to prevent widespread infections. " +
                   "Want more detail? Ask me 'what is ransomware?' or 'how to prevent ransomware'!";
        }

        // Catchphrase 16 - Spyware
        if (input.Contains("spyware"))
        {
            return "Spyware secretly installs itself on your device and monitors your activity without your knowledge. " +
                   "It can record your keystrokes, browsing habits, and even your passwords. " +
                   "This stolen information is then sent back to cybercriminals. " +
                   "Running regular antivirus scans can help detect and remove spyware from your device. " +
                   "You should also avoid downloading software from unofficial or untrusted sources!";
        }

        // Catchphrase 17 - Adware
        if (input.Contains("adware"))
        {
            return "Adware is software that automatically displays unwanted advertisements on your device, often as pop-ups. " +
                   "While not always harmful, it can slow your device down and sometimes lead to more dangerous malware. " +
                   "It often gets installed alongside free software you download. " +
                   "You can remove adware using an antivirus or anti-malware tool. " +
                   "Always read the terms when installing free software to avoid bundled adware!";
        }

        // Catchphrase 18 - Trojans
        if (input.Contains("trojan"))
        {
            return "A Trojan is malware that disguises itself as a harmless or useful program to trick you into installing it. " +
                   "Once installed, it can give hackers remote access to your device or steal your data. " +
                   "The name comes from the ancient Greek story of the Trojan Horse! " +
                   "Never download software from untrusted websites or unofficial sources. " +
                   "Stick to verified app stores and official developer websites!";
        }

        // Catchphrase 19 - Worms
        if (input.Contains("worm"))
        {
            return "A worm is malware that spreads automatically across networks without needing any human interaction. " +
                   "Unlike viruses, it does not need to attach to a file — it travels on its own through network connections. " +
                   "Worms can cause massive damage by consuming network bandwidth and dropping other malware. " +
                   "Keeping your firewall active and your operating system patched helps stop worms from spreading!";
        }

        // Catchphrase 20 - Keylogger
        if (input.Contains("keylogger") || input.Contains("keystroke"))
        {
            return "A keylogger is malicious software or hardware that records every keystroke you type, " +
                   "including your passwords, credit card numbers, and private messages. " +
                   "This data is then secretly sent to a cybercriminal. " +
                   "Using two-factor authentication (2FA) can protect you even if your password is stolen via a keylogger. " +
                   "Anti-malware software can detect and remove software keyloggers!";
        }


        // Catchphrase 21 - Password tips
        if (input.Contains("password"))
        {
            return "Password Safety Tips:\n" +
                   "  - Use at least 12 characters\n" +
                   "  - Mix uppercase, lowercase, numbers, and symbols\n" +
                   "  - Never reuse the same password on different sites\n" +
                   "  - Use a password manager to keep them safe\n" +
                   "  - Avoid using your name, birthday, or common words\n" +
                   "Would you like to know about two-factor authentication (2FA) for extra security?";
        }

        // Catchphrase 22 - Two-Factor Authentication
        if (input.Contains("2fa") || input.Contains("two factor") || input.Contains("two-factor") || input.Contains("multi-factor") || input.Contains("mfa"))
        {
            return "Two-Factor Authentication (2FA) adds an extra layer of security to your accounts. " +
                   "Even if someone knows your password, they cannot log in without " +
                   "the second step (like a code sent to your phone or generated by an app). " +
                   "Always enable 2FA on important accounts like email, banking, and social media! " +
                   "It is one of the single most effective ways to protect your accounts.";
        }

        // Catchphrase 23 - Password manager
        if (input.Contains("password manager"))
        {
            return "A password manager is a secure app that stores all your passwords in an encrypted vault. " +
                   "You only need to remember one strong master password, and the manager remembers the rest. " +
                   "Popular options include Bitwarden, LastPass, and 1Password. " +
                   "Using a password manager means you can have a unique, complex password for every site " +
                   "without needing to memorise them all. It is a game changer for online security!";
        }

        // Catchphrase 24 - Credential stuffing
        if (input.Contains("credential stuffing") || input.Contains("account takeover"))
        {
            return "Credential stuffing is when cybercriminals take username and password combinations " +
                   "leaked from one data breach and try them on other websites automatically. " +
                   "This is why reusing passwords is so dangerous! " +
                   "If your password from one site is exposed, attackers will try it on your bank, email, and social media. " +
                   "Always use unique passwords for each site, and enable 2FA wherever possible!";
        }

        // Catchphrase 25 - Data breach / leaked password
        if (input.Contains("data breach") || input.Contains("leaked password") || input.Contains("hacked account"))
        {
            return "A data breach is when sensitive information (like passwords or credit card numbers) " +
                   "is stolen from a company's database by hackers. " +
                   "If your data is in a breach, change your password immediately and enable 2FA on that account. " +
                   "You can check if your email has been in a breach at https://haveibeenpwned.com — it is free and safe to use! " +
                   "This is exactly why using unique passwords for each site is so important.";
        }

        // Catchphrase 26 - Safe browsing / internet
        if (input.Contains("browsing") || input.Contains("safe browsing") || input.Contains("internet safety"))
        {
            return "Safe Browsing Tips:\n" +
                   "  - Only visit websites that start with HTTPS (look for the padlock icon)\n" +
                   "  - Avoid clicking on pop-up ads or suspicious links\n" +
                   "  - Keep your browser and extensions updated\n" +
                   "  - Use a VPN on public Wi-Fi\n" +
                   "  - Clear your cookies and browser history regularly\n" +
                   "Would you like to know more about VPNs or HTTPS?";
        }

        // Catchphrase 27 - VPN
        if (input.Contains("virtual private network") || input.Contains("vpn"))
        {
            return "A VPN (Virtual Private Network) encrypts your internet connection and hides your IP address, " +
                   "making it much harder for hackers or third parties to spy on your online activity. " +
                   "It is especially important to use a VPN when connecting to public Wi-Fi in places like cafes or airports. " +
                   "A good VPN protects your privacy and can also help you browse more securely on any network!";
        }

        // Catchphrase 28 - Firewall
        if (input.Contains("firewall"))
        {
            return "A firewall is a security system that monitors and controls the traffic coming in and " +
                   "going out of your network. It acts as a barrier between your device and potential threats from the internet. " +
                   "Most operating systems come with a built-in firewall that should always be kept on. " +
                   "Think of it like a security guard at the door — only letting safe traffic through!";
        }

        // Catchphrase 29 - Public Wi-Fi
        if (input.Contains("public wifi") || input.Contains("public wi-fi") || input.Contains("free wifi"))
        {
            return "Public Wi-Fi is convenient but very risky! Hackers can set up fake Wi-Fi hotspots " +
                   "or intercept your traffic on unsecured networks in cafes, airports, and malls. " +
                   "Tips for staying safe on public Wi-Fi:\n" +
                   "  - Always use a VPN\n" +
                   "  - Avoid logging into banking or sensitive accounts\n" +
                   "  - Make sure websites use HTTPS\n" +
                   "  - Turn off auto-connect to open networks on your device\n" +
                   "Would you like to know more about VPNs?";
        }

        // Catchphrase 30 - HTTPS / HTTP
        if (input.Contains("https") || input.Contains("http") || input.Contains("secure website") || input.Contains("padlock"))
        {
            return "HTTPS (HyperText Transfer Protocol Secure) means the website encrypts the data " +
                   "you send and receive, protecting it from eavesdroppers. " +
                   "Always look for the padlock icon in your browser's address bar before entering any personal information. " +
                   "HTTP (without the S) is NOT secure — avoid entering passwords or payment details on HTTP sites! " +
                   "Most modern browsers will warn you when a site is not secure.";
        }

        // Catchphrase 31 - Cookies and tracking
        if (input.Contains("cookies") || input.Contains("tracking") || input.Contains("browser tracking"))
        {
            return "Browser cookies are small files that websites store on your device to remember your preferences and login sessions. " +
                   "While many cookies are harmless, tracking cookies follow your browsing activity across multiple websites " +
                   "to build an advertising profile about you. " +
                   "To protect your privacy, regularly clear your cookies, use private browsing mode, " +
                   "and consider using a browser extension that blocks trackers. " +
                   "Your privacy matters — take control of who can track you!";
        }

        // Catchphrase 32 - Social engineering
        if (input.Contains("social engineering"))
        {
            return "Social engineering is when hackers trick people (not computers) into giving " +
                   "away sensitive information. They may pretend to be IT support, a friend, a manager, or a bank official. " +
                   "Common tactics include creating urgency, offering something free, or making you feel threatened. " +
                   "Always verify who you are speaking to before sharing any information! " +
                   "If something feels off, trust your instincts and hang up or close the message.";
        }

        // Catchphrase 33 - Vishing (voice phishing)
        if (input.Contains("vishing") || input.Contains("phone scam") || input.Contains("voice phishing") || input.Contains("scam call"))
        {
            return "Vishing (voice phishing) is when scammers call you on the phone pretending to be from " +
                   "a bank, government agency, or tech support company to steal your personal information. " +
                   "They often create a sense of urgency — for example, saying your account is frozen. " +
                   "Tips: Never give personal info to unexpected callers. Hang up and call the official number directly. " +
                   "Real organisations will never ask for your PIN or password over the phone!";
        }

        // Catchphrase 34 - Smishing (SMS phishing)
        if (input.Contains("smishing") || input.Contains("sms scam") || input.Contains("text scam") || input.Contains("fake sms"))
        {
            return "Smishing is SMS-based phishing where scammers send fake text messages to trick you " +
                   "into clicking a malicious link or calling a fraudulent number. " +
                   "Common examples include fake delivery notifications, prize winnings, or urgent bank alerts. " +
                   "Never click links in unexpected text messages! If you are unsure, go directly to the official website. " +
                   "Report suspicious texts to your network provider or local cybercrime authority.";
        }

        // Catchphrase 35 - Identity theft
        if (input.Contains("identity theft") || input.Contains("stolen identity") || input.Contains("identity fraud"))
        {
            return "Identity theft is when a criminal steals your personal information (like your ID number, " +
                   "bank details, or passwords) to commit fraud in your name. " +
                   "This can damage your credit record and take years to fix. " +
                   "Protect yourself by: using strong unique passwords, enabling 2FA, monitoring your bank statements, " +
                   "shredding documents with personal info, and being cautious about what you share online. " +
                   "Act fast if you suspect your identity has been stolen — contact your bank and authorities immediately!";
        }

        // Catchphrase 36 - Pretexting
        if (input.Contains("pretexting") || input.Contains("fake identity") || input.Contains("impersonation"))
        {
            return "Pretexting is a form of social engineering where an attacker creates a fabricated scenario " +
                   "(a pretext) to manipulate you into giving up sensitive information. " +
                   "For example, someone might pretend to be an IT technician who needs your login credentials to 'fix' something. " +
                   "Always verify someone's identity through official channels before sharing any sensitive information. " +
                   "Legitimate IT professionals will never need your password!";
        }

        // Catchphrase 37 - Encryption
        if (input.Contains("encryption") || input.Contains("encrypt") || input.Contains("what is encryption"))
        {
            return "Encryption is the process of converting your data into a coded format that can only be " +
                   "read by someone with the correct decryption key. " +
                   "It protects your files, messages, and data from being read by unauthorised parties. " +
                   "HTTPS uses encryption to protect your web traffic. " +
                   "End-to-end encryption in messaging apps like WhatsApp means only you and the recipient can read your messages. " +
                   "Encryption is one of the most powerful tools in cybersecurity!";
        }

        // Catchphrase 38 - Data backup
        if (input.Contains("backup") || input.Contains("back up") || input.Contains("data backup"))
        {
            return "Regular data backups are one of the most important cybersecurity habits you can build! " +
                   "If ransomware encrypts your files or your device is stolen, backups let you recover without paying a ransom. " +
                   "Best practices:\n" +
                   "  - Follow the 3-2-1 rule: 3 copies, 2 different media, 1 offsite (or cloud)\n" +
                   "  - Schedule automatic backups weekly at minimum\n" +
                   "  - Test your backups to make sure they actually work\n" +
                   "Would you like to know more about ransomware protection?";
        }

        // Catchphrase 39 - Cloud security
        if (input.Contains("cloud security") || input.Contains("cloud storage") || input.Contains("cloud"))
        {
            return "Cloud storage can be very convenient but it also comes with security risks. " +
                   "To keep your cloud data safe:\n" +
                   "  - Use a strong, unique password and enable 2FA on your cloud account\n" +
                   "  - Avoid sharing sensitive files publicly\n" +
                   "  - Check app permissions — only grant access to apps you trust\n" +
                   "  - Use cloud providers with end-to-end encryption for sensitive data\n" +
                   "The cloud is only as secure as the account protecting it!";
        }

        // Catchphrase 40 - Cybersecurity at work / insider threats
        if (input.Contains("insider threat") || input.Contains("workplace security") || input.Contains("cyber at work"))
        {
            return "Insider threats occur when someone within an organisation — intentionally or accidentally — " +
                   "compromises its security. This could be a disgruntled employee leaking data, " +
                   "or simply a staff member clicking a phishing link. " +
                   "Organisations protect against this by limiting access to sensitive data (least privilege principle), " +
                   "monitoring unusual activity, and training employees in cybersecurity awareness. " +
                   "Always follow your organisation's security policies and report suspicious behaviour!";
        }

        // Catchphrase 41 - Privacy settings
        if (input.Contains("privacy settings") || input.Contains("privacy") || input.Contains("digital privacy"))
        {
            return "Managing your privacy settings is a key part of staying safe online. " +
                   "Review the privacy settings on your social media, apps, and devices regularly. " +
                   "Tips:\n" +
                   "  - Limit what personal information you share publicly\n" +
                   "  - Turn off location tracking for apps that don't need it\n" +
                   "  - Review which apps have access to your camera, microphone, and contacts\n" +
                   "  - Use a privacy-focused browser or search engine\n" +
                   "Your data is valuable — protect it like you protect your wallet!";
        }


        // Catchphrase 42 - Software updates
        if (input.Contains("software update") || input.Contains("update software") || input.Contains("patch") || input.Contains("why update"))
        {
            return "Keeping your software updated is one of the simplest yet most effective cybersecurity habits! " +
                   "Updates patch security vulnerabilities that hackers could exploit to gain access to your device. " +
                   "This applies to your operating system, antivirus, browser, and ALL your apps. " +
                   "Enable automatic updates wherever possible so you never miss a critical patch. " +
                   "Many major cyberattacks have succeeded simply because victims had not applied available updates!";
        }

        // Catchphrase 43 - Antivirus
        if (input.Contains("antivirus") || input.Contains("anti-virus") || input.Contains("security software"))
        {
            return "Antivirus software is your first line of defence against malware, viruses, and other threats. " +
                   "It scans your device for malicious files and behaviour, and removes threats it finds. " +
                   "Tips for getting the most out of your antivirus:\n" +
                   "  - Keep it updated so it knows the latest threats\n" +
                   "  - Run full scans regularly (at least weekly)\n" +
                   "  - Don't install multiple antivirus programs — they can conflict\n" +
                   "  - Pair it with a firewall for stronger protection!";
        }

        // Catchphrase 44 - Mobile / smartphone security
        if (input.Contains("mobile security") || input.Contains("phone security") || input.Contains("smartphone security"))
        {
            return "Your smartphone holds a huge amount of personal data, so securing it is essential!\n" +
                   "Mobile security tips:\n" +
                   "  - Use a strong PIN, pattern, or biometric lock\n" +
                   "  - Only download apps from official stores (Google Play, Apple App Store)\n" +
                   "  - Keep your phone's OS updated\n" +
                   "  - Enable remote wipe in case your phone is lost or stolen\n" +
                   "  - Avoid charging your phone on public USB ports (juice jacking risk)\n" +
                   "Would you like to know about juice jacking? Just ask!";
        }

        // Catchphrase 45 - Juice jacking
        if (input.Contains("juice jacking") || input.Contains("usb charging") || input.Contains("public usb"))
        {
            return "Juice jacking is a cyberattack where hackers compromise public USB charging stations " +
                   "to install malware on or steal data from your device when you plug in to charge. " +
                   "It has been found in airports, shopping malls, and hotels. " +
                   "To stay safe:\n" +
                   "  - Use your own charger plugged into a regular power socket\n" +
                   "  - Carry a portable power bank\n" +
                   "  - Use a USB data blocker (also called a USB condom) if you must use public USB ports!";
        }

        // Catchphrase 46 - Zero-day vulnerability
        if (input.Contains("zero day") || input.Contains("zero-day") || input.Contains("vulnerability"))
        {
            return "A zero-day vulnerability is a software security flaw that is unknown to the software vendor, " +
                   "meaning there is no official patch or fix available yet. " +
                   "Hackers exploit these before developers can release an update. " +
                   "While you cannot patch a zero-day yourself, you can reduce your risk by:\n" +
                   "  - Keeping all other software updated\n" +
                   "  - Using security tools that detect unusual behaviour\n" +
                   "  - Following safe browsing habits\n" +
                   "Stay alert and keep everything else secured!";
        }


        // Catchphrase 47 - Online scams
        if (input.Contains("online scam") || input.Contains("internet scam") || input.Contains("cyber scam"))
        {
            return "Online scams come in many forms — fake job offers, lottery winnings, romance scams, and more! " +
                   "If something sounds too good to be true, it almost certainly is. " +
                   "Tips to avoid online scams:\n" +
                   "  - Never send money to someone you have not met in person\n" +
                   "  - Research companies before applying for jobs online\n" +
                   "  - Be wary of urgent requests for personal information\n" +
                   "  - Report scams to your local cybercrime authority\n" +
                   "Staying sceptical online is a powerful defence!";
        }

        // Catchphrase 48 - Cyberbullying
        if (input.Contains("cyberbullying") || input.Contains("online harassment") || input.Contains("cyber harassment"))
        {
            return "Cyberbullying is the use of technology to harass, threaten, or humiliate someone online. " +
                   "It can happen on social media, in gaming, via messages, or in online forums. " +
                   "If you are being cyberbullied:\n" +
                   "  - Do not respond to the bully\n" +
                   "  - Take screenshots as evidence\n" +
                   "  - Block and report the bully on the platform\n" +
                   "  - Tell a trusted adult or authority\n" +
                   "Remember, you are not alone — help is always available!";
        }

        // Catchphrase 49 - Dark web
        if (input.Contains("dark web") || input.Contains("darkweb") || input.Contains("deep web"))
        {
            return "The dark web is a part of the internet that is not indexed by regular search engines " +
                   "and requires special software like Tor to access. " +
                   "It is used both for legitimate privacy purposes (like journalism in dangerous countries) " +
                   "and by criminals to sell stolen data, malware, and illegal goods. " +
                   "Your stolen passwords or personal data could end up on the dark web after a data breach. " +
                   "You can check if your data is on the dark web at haveibeenpwned.com — it is free and safe!";
        }

        // Catchphrase 50 - General cybersecurity awareness / tips
        if (input.Contains("cybersecurity tips") || input.Contains("cyber tips") || input.Contains("stay safe online") || input.Contains("general tips"))
        {
            return $"Great question, {userName}! Here are the golden rules of cybersecurity:\n" +
                   "  1. Use strong, unique passwords and a password manager\n" +
                   "  2. Enable Two-Factor Authentication (2FA) on all important accounts\n" +
                   "  3. Keep all software and devices updated\n" +
                   "  4. Think before you click — be suspicious of unexpected links\n" +
                   "  5. Back up your data regularly\n" +
                   "  6. Use a VPN on public Wi-Fi\n" +
                   "  7. Protect your privacy settings on social media\n" +
                   "  8. Install reputable antivirus software\n" +
                   "  9. Learn to recognise phishing and social engineering\n" +
                   "  10. Report suspicious activity to the right authorities\n" +
                   "Cybersecurity is a habit, not a one-time action. Keep learning!";
        }

        // Default response for unrecognised input
        return $"I didn't quite understand that, {userName}. Could you rephrase? \n" +
               "Try asking about: phishing, passwords, malware, ransomware, safe browsing, VPN, encryption, 2FA, \n" +
               "social engineering, identity theft, data backups, antivirus, or type 'topics' for a full list!";
    }

    static void ShowBotResponse(string message)
    {
        Console.WriteLine();
        SetColour(ConsoleColor.Cyan);
        Console.Write("  Bot: ");
        ResetColour();

        SetColour(ConsoleColor.White);
        TypeText("  " + message); // TypeText gives a typing effect
        ResetColour();
    }

    static void PrintDivider()
    {
        SetColour(ConsoleColor.DarkCyan);
        Console.WriteLine("  ============================================================");
        ResetColour();
    }

    static void SetColour(ConsoleColor colour)
    {
        Console.ForegroundColor = colour;
    }

    static void ResetColour()
    {
        Console.ResetColor();
    }

    static void TypeText(string message)
    {
        foreach (char letter in message)
        {
            Console.Write(letter);
            Thread.Sleep(18); // Wait 18 milliseconds between each letter
        }
        Console.WriteLine(); // Move to the next line when done
    }
}