# 💕 Romantic Assistant Telegram Bot

[![.NET](https://img.shields.io/badge/.NET-8.0-blue.svg)](https://dotnet.microsoft.com/download)
[![C#](https://img.shields.io/badge/C%23-12.0-blue.svg)](https://docs.microsoft.com/en-us/dotnet/csharp/)
[![Telegram Bot API](https://img.shields.io/badge/Telegram%20Bot%20API-22.4.4-blue.svg)](https://core.telegram.org/bots/api)
[![Design Patterns](https://img.shields.io/badge/Design%20Patterns-10+-green.svg)](#design-patterns)

A sophisticated **Romantic Assistant Telegram Bot** built with C# and .NET 8.0, showcasing multiple **design patterns** and providing a comprehensive romantic companion experience. This bot serves as both a functional romantic assistant and an educational example of design pattern implementation in modern C# development.

## ✨ Features

### 🎯 Core Functionality
- **💖 Love Timer**: Set and track romantic events with countdown notifications
- **💌 Compliment Generator**: AI-powered romantic compliments with personalization
- **😘 Flirt Mode**: Interactive flirting conversations and responses
- **❓ Love Quiz**: Engaging romantic personality quizzes
- **💬 Chat System**: Interactive conversations with romantic AI character
- **📜 Quote System**: Beautiful romantic quotes and inspirational messages
- **🌹 Date Ideas**: Location and weather-based romantic date suggestions
- **🎯 Goal Management**: Personal relationship goals tracking and management

### 🎨 Task System
Execute various romantic tasks with the **Command Pattern**:
- **📝 Poem Generation**: Create personalized romantic poems
- **💕 Love Letters**: Generate heartfelt love letters
- **🌙 Date Planning**: Comprehensive date planning assistance
- **🎁 Surprise Ideas**: Creative romantic surprise suggestions

### 🌤️ Smart Location Features
- **🏠 Home Date Ideas**: Indoor romantic activities
- **🌳 Outdoor Adventures**: Weather-aware outdoor date suggestions
- **🌦️ Weather Integration**: Adaptive suggestions based on current weather
- **🗺️ City-Specific**: Localized recommendations for different cities

## 🏗️ Design Patterns

This project demonstrates **10+ design patterns** in practical application:

| Pattern | Implementation | Purpose |
|---------|---------------|---------|
| **🔒 Singleton** | `BotClientSingleton` | Single Telegram Bot instance management |
| **🎯 Strategy** | `ComplimentGeneratorStrategy`, `FlirtSparkStrategy` | Interchangeable romantic response algorithms |
| **⚡ Command** | `PoemTask`, `LetterTask`, `DateTask`, `SurpriseTask` | Encapsulated task execution system |
| **👁️ Observer** | `ConcreteObserver`, `EventTimer` | Event notification and timer alerts |
| **🏭 Abstract Factory** | `HomeDateIdeaFactory`, `OutdoorDateIdeaFactory` | Location-based idea generation |
| **🤝 Mediator** | `ChatMediator` | Character communication coordination |
| **🎨 Decorator** | `WeatherPredictionDecorator` | Enhanced date ideas with weather data |
| **🔄 State** | `QuizQuestionState`, `UserState` | Quiz and conversation state management |
| **🏗️ Builder** | `GoalBuilder` | Complex goal object construction |
| **📋 Prototype** | Goal cloning system | Efficient goal duplication |

## 🚀 Quick Start

### Prerequisites
- **.NET 8.0 SDK** or later
- **Telegram Bot Token** (obtain from [@BotFather](https://t.me/botfather))
- **Visual Studio 2022** or **VS Code** (recommended)

### Installation

1. **Clone the repository**
   ```bash
   git clone https://github.com/Kwameldx666/TelegramBot.git
   cd TelegramBot
   ```

2. **Configure Bot Token**
   ```csharp
   // Update in Progress/Singleton/BotClientSingleton.cs
   Client = new TelegramBotClient("YOUR_BOT_TOKEN_HERE");
   ```

3. **Install Dependencies**
   ```bash
   dotnet restore
   ```

4. **Build the Project**
   ```bash
   dotnet build
   ```

5. **Run the Bot**
   ```bash
   dotnet run
   ```

## 🎮 Usage Examples

### Basic Commands
```
/start          - Initialize bot and set location
/menu           - Return to main menu
/compliment     - Get a romantic compliment
/flirt          - Start flirting mode
/quiz           - Take the love personality quiz
/chat [message] - Start romantic chat session
/dateidea       - Get date suggestions
/goals          - Manage relationship goals
```

### Task Commands
```
/task poem      - Generate a romantic poem
/task letter    - Create a love letter
/task date      - Plan a romantic date
/task surprise  - Get surprise ideas
/task random    - Random task with delays
```

### Interactive Features
- **📅 Calendar Selection**: Visual date picker for events
- **⏰ Time Selection**: Multiple time slots for planning
- **📍 Location Choice**: Home vs outdoor preferences
- **🎨 Category Selection**: Goal categorization (Work, Personal, Health, General)

## 📁 Project Structure

```
TelegramBot/
├── 📄 Program.cs                    # Main entry point and bot logic
├── 📄 TelegramBot.csproj           # Project configuration
├── 📁 Mediator/                    # Mediator Pattern
│   ├── Character.cs                # Chat character implementation
│   ├── ChatMediator.cs            # Communication mediator
│   └── IChatMediator.cs           # Mediator interface
├── 📁 Observer/                    # Observer Pattern
│   ├── ConcreteObserver.cs        # Event observer implementation
│   ├── EventTimer.cs              # Timer with notifications
│   └── Intefaces/                 # Observer interfaces
├── 📁 Progress/                    # Core design patterns
│   ├── 📁 AbstractFactory/        # Abstract Factory Pattern
│   │   ├── HomeDateIdeaFactory.cs
│   │   ├── OutdoorDateIdeaFactory.cs
│   │   └── AbstractImplements/
│   ├── 📁 Command/                 # Command Pattern
│   │   ├── PoemTask.cs
│   │   ├── LetterTask.cs
│   │   ├── DateTask.cs
│   │   ├── SurpriseTask.cs
│   │   └── TaskInvoker.cs
│   ├── 📁 Strategy/                # Strategy Pattern
│   │   ├── ComplimentGeneratorStrategy.cs
│   │   ├── FlirtSparkStrategy.cs
│   │   └── RomanticBotContext.cs
│   ├── 📁 Singleton/               # Singleton Pattern
│   │   └── BotClientSingleton.cs
│   ├── 📁 State/                   # State Pattern
│   │   ├── QuizQuestionState.cs
│   │   └── UserState.cs
│   ├── 📁 Decorator/               # Decorator Pattern
│   ├── 📁 Models/                  # Data models
│   └── 📁 Prototype/               # Prototype Pattern
└── 📄 README.md                    # This file
```

## 📦 Dependencies

| Package | Version | Purpose |
|---------|---------|---------|
| `Telegram.Bot` | 22.4.4 | Telegram Bot API integration |
| `Microsoft.EntityFrameworkCore` | 9.0.3 | Data persistence (future use) |
| `Newtonsoft.Json` | 13.0.3 | JSON serialization |
| `System.Net.Http` | 4.3.4 | HTTP client for weather API |

## 🔧 Configuration

### Environment Variables (Recommended)
```bash
export TELEGRAM_BOT_TOKEN="your_bot_token_here"
export WEATHER_API_KEY="your_weather_api_key" # Optional
```

### Configuration Options
- **Bot Token**: Telegram Bot API token
- **Weather API**: OpenWeatherMap API for enhanced date suggestions
- **Default City**: Fallback city for location-based features
- **Timer Intervals**: Customizable notification frequencies

## 🛡️ Security Considerations

⚠️ **Important Security Notes:**

1. **Bot Token Security**
   - Never commit bot tokens to version control
   - Use environment variables or secure configuration
   - Rotate tokens regularly

2. **User Data Protection**
   - User states are stored in memory (not persistent)
   - No personal data is logged permanently
   - Consider implementing data encryption for production

3. **Input Validation**
   - All user inputs are sanitized
   - SQL injection protection through parameterized queries
   - Rate limiting should be implemented for production

## 🚀 Development Guidelines

### Adding New Features
1. **Follow Design Patterns**: Implement new features using appropriate design patterns
2. **Maintain Consistency**: Use existing code style and naming conventions
3. **Documentation**: Add XML comments for public methods
4. **Testing**: Consider unit tests for complex logic

### Code Style
- **Naming**: PascalCase for public members, camelCase for private
- **Async/Await**: Use async patterns for I/O operations
- **Error Handling**: Implement proper exception handling
- **Logging**: Use structured logging for debugging

### Performance Considerations
- **Memory Management**: Dispose of resources properly
- **Caching**: Consider caching for frequently accessed data
- **Rate Limiting**: Implement rate limiting for API calls
- **Concurrent Access**: Handle concurrent user interactions

## 🐛 Known Issues

- ⚠️ Deprecated Telegram.Bot API methods (warnings during build)
- 🔧 Hardcoded bot token in source code
- 💾 Non-persistent user state storage
- 🌐 Limited weather API integration

## 🤝 Contributing

1. **Fork** the repository
2. **Create** a feature branch (`git checkout -b feature/amazing-feature`)
3. **Commit** your changes (`git commit -m 'Add amazing feature'`)
4. **Push** to the branch (`git push origin feature/amazing-feature`)
5. **Open** a Pull Request

### Contribution Areas
- 🎨 New romantic features
- 🏗️ Additional design pattern implementations
- 🌍 Internationalization support
- 🧪 Unit testing infrastructure
- 📱 UI/UX improvements

## 📄 License

This project is licensed under the **MIT License** - see the [LICENSE](LICENSE) file for details.

## 🙏 Acknowledgments

- **Telegram Bot API** for the excellent bot framework
- **Design Patterns Community** for implementation guidance
- **Open Source Contributors** who inspire better code

## 📞 Support

- 🐛 **Issues**: [GitHub Issues](https://github.com/Kwameldx666/TelegramBot/issues)
- 💬 **Discussions**: [GitHub Discussions](https://github.com/Kwameldx666/TelegramBot/discussions)
- 📧 **Email**: Create an issue for contact information

---

<div align="center">

**Made with 💕 for romantic developers**

*"Code is poetry, and every commit is a love letter to the future."*

[![GitHub stars](https://img.shields.io/github/stars/Kwameldx666/TelegramBot?style=social)](https://github.com/Kwameldx666/TelegramBot/stargazers)
[![GitHub forks](https://img.shields.io/github/forks/Kwameldx666/TelegramBot?style=social)](https://github.com/Kwameldx666/TelegramBot/network/members)

</div>