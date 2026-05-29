# 🔐 Cybersecurity Awareness Chatbot — Part 2

**Student:** ST10496771 — Tanganyika S  
**Project:** PROG POE Part 2 — GUI, Dynamic Responses, Sentiment Detection & Memory

---

## 📋 Overview

Part 2 expands the console chatbot from Part 1 into a full **WinForms GUI application** built on **.NET 8**. The chatbot now features:

- ✅ **GUI Interface** (WinForms) with a professional dark-themed design
- ✅ **Keyword Recognition** — identifies 20+ cybersecurity topics
- ✅ **Random Responses** — multiple randomised replies for topics like phishing, passwords, scams, VPN, etc.
- ✅ **Conversation Flow** — follow-up phrases ("tell me more", "give me another tip") continue the current topic without restarting
- ✅ **Memory & Recall** — remembers user's name, favourite topic, and all discussed topics; personalises responses accordingly
- ✅ **Sentiment Detection** — detects worried, curious, or frustrated sentiment and prepends empathetic responses
- ✅ **Error Handling** — graceful default responses for unknown inputs
- ✅ **OOP Design** — `ChatbotEngine`, `UserMemory`, `ResponseEntry`, and `Sentiment` classes with clean separation of concerns

---

## 🗂️ Project Structure

```
CybersecurityChatbotGUI/
├── CybersecurityChatbotGUI.sln
└── CybersecurityChatbotGUI/
    ├── CybersecurityChatbotGUI.csproj
    ├── Program.cs            ← App entry point
    ├── ChatForm.cs           ← WinForms UI (layout, event handlers)
    ├── ChatbotEngine.cs      ← Core logic: keywords, memory, sentiment, random responses
    └── README.md
```

---

## 🚀 How to Run

### Requirements
- Visual Studio 2022 (or later) with the **.NET desktop development** workload installed
- .NET 8 SDK

### Steps
1. Open `CybersecurityChatbotGUI.sln` in Visual Studio
2. Press **F5** to build and run

---

## 🎯 Feature Walkthrough

### 1. GUI Design
- Dark cybersecurity-themed colour scheme
- ASCII art logo displayed in the chat window
- Quick-topic sidebar buttons (15 topics) for instant access
- Status bar shows current user and last topic discussed

### 2. Keyword Recognition
Recognises and responds to 20+ topics including:
`phishing`, `password`, `malware`, `ransomware`, `spyware`, `trojan`, `worm`, `vpn`, `firewall`, `encryption`, `2fa`, `privacy`, `scam`, `social engineering`, `identity theft`, `data backup`, `antivirus`, `mobile security`, `safe browsing`, `https`, `dark web`, and more.

### 3. Random Responses
Topics like **phishing**, **passwords**, **ransomware**, **scams**, **VPN**, **privacy**, **safe browsing**, and **general tips** randomly select from multiple predefined responses to keep interactions varied and engaging.

### 4. Conversation Flow
If the user types a follow-up phrase such as:
- "tell me more"
- "give me another tip"
- "more info"
- "what else"
- "another tip"

The bot continues on the **last discussed topic** without needing the user to re-enter the topic.

### 5. Memory & Recall
- The bot **stores the user's name** at startup
- When the user expresses interest ("I'm interested in privacy"), the bot **remembers** this and personalises future responses
- Type **"what do you remember"** or **"what do you know about me"** to see a memory summary
- After multiple topics are discussed, the bot occasionally reminds the user of related previous topics

### 6. Sentiment Detection
The bot detects three sentiment types from user input:

| Sentiment | Example keywords | Bot behaviour |
|-----------|-----------------|---------------|
| Worried | "worried", "scared", "hacked", "stolen" | Empathetic opener: "It's completely understandable..." |
| Curious | "curious", "interested", "what is", "explain" | Encouraging opener: "Great curiosity!" |
| Frustrated | "confused", "don't understand", "too hard" | Patient opener: "I'll break it down simply..." |

The sentiment is displayed as a note in the chat window above the bot's response.

### 7. Error Handling
- Empty inputs are ignored
- Unrecognised inputs return a helpful default message with topic suggestions
- No crashes on unexpected input

### 8. OOP Design
| Class | Responsibility |
|-------|---------------|
| `ChatbotEngine` | Core logic, response routing, sentiment, memory management |
| `UserMemory` | Stores user name, favourite topic, discussed interests |
| `ResponseEntry` | Encapsulates a topic's keywords and multiple response options |
| `Sentiment` (enum) | Categorises detected user sentiment |
| `ChatForm` | WinForms UI, event handling, display formatting |

---

## 🧪 Example Interactions

| User Input | Bot Behaviour |
|------------|--------------|
| `I'm worried about phishing` | Detects "worried" sentiment → empathetic prefix + phishing tip |
| `tell me more` | Continues on last topic (phishing) with a new random tip |
| `I'm interested in privacy` | Remembers interest, gives privacy response |
| `what do you remember` | Recalls stored topics and favourite interest |
| `2fa` | Explains two-factor authentication |
| `give me another tip` | Continues on last topic |
| `xyzabc` | Graceful default: suggests topics |

---

*© 2026 — Cybersecurity Awareness Chatbot POE Part 2*
