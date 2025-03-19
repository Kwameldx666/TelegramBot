using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using static TelegramBot.Strategy.RomanticBotContext;
using Telegram.Bot.Polling;
using TelegramBot.Models;
using TelegramBot.Strategy;
using TelegramBot.Observer;
using TelegramBot.Observer.Intefaces;
using TelegramBot.Mediator;
using TelegramBot.Command;
using TelegramBot.State;
using TelegramBot.Singleton;
using TelegramBot.Prototype;

namespace TelegramBot.Factory_Method
{


    class Program
    {
        private static readonly Dictionary<long, UserState> UserStates = new();
        private static readonly Dictionary<long, RomanticBot> UserBots = new();
        private static readonly Dictionary<long, QuizContext> QuizContexts = new();
        private static readonly Dictionary<long, Character> UserCharacters = new();

        static async Task Main()
        {
            var botClient = BotClientSingleton.Instance.Client;
            botClient.StartReceiving(HandleUpdateAsync, HandleErrorAsync, new ReceiverOptions { AllowedUpdates = new[] { UpdateType.Message, UpdateType.CallbackQuery } });
            var me = await botClient.GetMe();
            Console.WriteLine($"Романтический помощник @{me.Username} запущен!");
            await Task.Delay(-1);
        }

        private static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            if (update.Type == UpdateType.Message && update.Message?.Text != null)
                await HandleMessageAsync(botClient, update.Message, cancellationToken);
            else if (update.Type == UpdateType.CallbackQuery && update.CallbackQuery?.Data != null)
                await HandleCallbackQueryAsync(botClient, update.CallbackQuery, cancellationToken);
        }

        private static async Task HandleMessageAsync(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
        {
            var chatId = message.Chat.Id;
            var text = message.Text ?? string.Empty;

            if (!UserStates.ContainsKey(chatId))
                UserStates[chatId] = new UserState();
            if (!UserBots.ContainsKey(chatId))
                UserBots[chatId] = new RomanticBot();

            var state = UserStates[chatId];
            var bot = UserBots[chatId];

            switch (text.ToLower())
            {
                case "/start":
                    state.Step = 0;
                    state.EventDate = null;
                    state.EventTime = null;
                    state.EventName = null;
                    state.CurrentMonth = DateTime.Today;
                    state.IsChatStarted = false;
                    state.DateIdeaLocation = null;
                    state.DateIdeaCity = null;
                    state.Goals.Clear();
                    UserCharacters.Remove(chatId);
                    QuizContexts.Remove(chatId);
                    await botClient.SendTextMessageAsync(chatId,
                        "Привет! Я твой романтический помощник. Чем могу помочь?",
                        replyMarkup: GetStartMenu(),
                        cancellationToken: cancellationToken);
                    break;

                case "/compliment":
                    state.Step = 0;
                    bot.SetStrategy(new ComplimentGeneratorStrategy());
                    await bot.ExecuteStrategyAsync(botClient, chatId, state, null, cancellationToken);
                    break;

                case "/flirt":
                    state.Step = 0;
                    bot.SetStrategy(new FlirtSparkStrategy());
                    await bot.ExecuteStrategyAsync(botClient, chatId, state, null, cancellationToken);
                    break;

                case "/quiz":
                    state.Step = -1;
                    state.QuizAnswers = new List<string>();
                    state.QuizQuestionIndex = 0;
                    QuizContexts[chatId] = new QuizContext(new QuestionState());
                    await QuizContexts[chatId].Request(botClient, chatId, state, cancellationToken);
                    break;

                case "/goals":
                    state.GoalStep = 0;
                    await botClient.SendTextMessageAsync(chatId,
                        "Добро пожаловать в управление целями! Что хочешь сделать?",
                        replyMarkup: GetGoalsMainMenu(),
                        cancellationToken: cancellationToken);
                    break;

                case string s when s.StartsWith("/task"):
                    state.Step = 0;
                    var args = text.Split(' ');
                    string taskType = args.Length > 1 ? args[1].ToLower() : "random";
                    var invoker = new TaskInvoker();
                    var tasks = new Dictionary<string, ICommand>
                    {
                        { "poem", new PoemTask() },
                        { "letter", new LetterTask() },
                        { "date", new DateTask() },
                        { "surprise", new SurpriseTask() },
                        { "random", new RandomTask() }
                    };
                    invoker.SetCommand(tasks.TryGetValue(taskType, out var task) ? task : new RandomTask());
                    await invoker.ExecuteCommand(botClient, chatId, cancellationToken);
                    break;

                case string s when s.StartsWith("/chat"):
                    if (!UserCharacters.ContainsKey(chatId))
                        UserCharacters[chatId] = new Character();
                    var character = UserCharacters[chatId];
                    var mediator = new ChatMediator(character);
                    string chatMessage = text.Replace("/chat", "").Trim();
                    state.Step = -2;
                    if (!state.IsChatStarted)
                    {
                        if (string.IsNullOrEmpty(chatMessage))
                            chatMessage = "Привет!";
                        state.IsChatStarted = true;
                        await mediator.SendMessage(chatMessage, botClient, chatId, cancellationToken);
                        await botClient.SendTextMessageAsync(chatId, "Ты в чате! Пиши что угодно, а для выхода скажи 'пока'.", cancellationToken: cancellationToken);
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(chatMessage))
                            chatMessage = "Ты молчишь?";
                        await mediator.SendMessage(chatMessage, botClient, chatId, cancellationToken);
                    }
                    break;

                case "/dateidea":
                    state.Step = 10;
                    state.DateIdeaLocation = null;
                    state.DateIdeaCity = null;
                    await botClient.SendTextMessageAsync(chatId,
                        "Где ты хочешь провести свидание? Дома или на улице?",
                        replyMarkup: GetLocationInline(),
                        cancellationToken: cancellationToken);
                    break;

                default:
                    if (state.Step == -1 && text != null) // Викторина
                    {
                        if (!QuizContexts.ContainsKey(chatId))
                            QuizContexts[chatId] = new QuizContext(new QuestionState());
                        await QuizContexts[chatId].Request(botClient, chatId, state, cancellationToken);
                        if (state.QuizQuestionIndex >= QuestionState.Questions.Length)
                        {
                            state.Step = 0;
                            QuizContexts.Remove(chatId);
                        }
                    }
                    else if (state.Step == -2 && text != null) // Чат
                    {
                        if (!UserCharacters.ContainsKey(chatId))
                            UserCharacters[chatId] = new Character();
                        var chatCharacter = UserCharacters[chatId];
                        var chatMediator = new ChatMediator(chatCharacter);
                        string userMessage = text.Trim();
                        if (string.IsNullOrEmpty(userMessage))
                            userMessage = "Ты молчишь?";
                        if (userMessage.ToLower().Contains("пока") || userMessage.ToLower().Contains("до свидания"))
                        {
                            await chatMediator.SendMessage(userMessage, botClient, chatId, cancellationToken);
                            await botClient.SendTextMessageAsync(chatId, "Чат завершён. Возвращайся, когда захочешь!", replyMarkup: GetStartMenu(), cancellationToken: cancellationToken);
                            state.Step = 0;
                            state.IsChatStarted = false;
                            UserCharacters.Remove(chatId);
                        }
                        else
                        {
                            await chatMediator.SendMessage(userMessage, botClient, chatId, cancellationToken);
                        }
                    }
                    else if (state.Step == 2 && text != null) // Ввод названия события
                    {
                        state.EventName = text;
                        state.Step = 3;
                        await SendConfirmation(botClient, chatId, state, cancellationToken);
                    }
                    else if (state.Step == 11 && text != null) // Ввод города
                    {
                        state.DateIdeaCity = text.Trim();
                        IDateIdeaGeneratorFactory factory = state.DateIdeaLocation switch
                        {
                            "home" => new HomeDateIdeaFactory(state.DateIdeaCity),
                            "outdoor" => new OutdoorDateIdeaFactory(state.DateIdeaCity),
                            _ => throw new InvalidOperationException("Неподдерживаемое место")
                        };
                        var generator = factory.CreateGenerator();
                        string idea = generator.GenerateDateIdea();
                        await botClient.SendTextMessageAsync(chatId,
                            $"Вот идея для вашего свидания:\n\n{idea}",
                            replyMarkup: GetStartMenu(),
                            cancellationToken: cancellationToken);
                        state.Step = 0;
                        await SendChatReminder(botClient, chatId, state, cancellationToken);
                    }
                    else if (state.GoalStep == 2 && text != null) // Ввод описания цели
                    {
                        var builder = new GoalBuilder()
                            .SetDescription(text)
                            .SetDate(state.TempGoalDate)
                            .SetCategory(state.TempCategory ?? "Общее");
                        state.Goals.Add(builder.Build());
                        state.GoalStep = 0;
                        state.TempGoalDate = DateTime.MinValue;
                        await botClient.SendTextMessageAsync(chatId,
                            "Цель успешно добавлена! Что дальше?",
                            replyMarkup: GetGoalsMainMenu(),
                            cancellationToken: cancellationToken);
                    }
                    break;
            }
        }

        private static async Task HandleCallbackQueryAsync(ITelegramBotClient botClient, CallbackQuery callbackQuery, CancellationToken cancellationToken)
        {
            var chatId = callbackQuery.Message!.Chat.Id;
            var data = callbackQuery.Data ?? string.Empty;
            var state = UserStates[chatId];
            var bot = UserBots[chatId];

            try
            {
                switch (data)
                {
                    case "timer":
                        state.Step = 0;
                        state.EventDate = null;
                        state.EventTime = null;
                        state.EventName = null;
                        state.CurrentMonth = DateTime.Today;
                        await botClient.EditMessageTextAsync(chatId, callbackQuery.Message.MessageId,
                            "Выбери день:", replyMarkup: GetCalendarInline(state.CurrentMonth));
                        await botClient.AnswerCallbackQueryAsync(callbackQuery.Id);
                        break;

                    case "compliment":
                        state.Step = 0;
                        bot.SetStrategy(new ComplimentGeneratorStrategy());
                        await bot.ExecuteStrategyAsync(botClient, chatId, state, null, cancellationToken);
                        await botClient.AnswerCallbackQueryAsync(callbackQuery.Id, "Вот тебе комплимент!");
                        break;

                    case "flirt":
                        state.Step = 0;
                        bot.SetStrategy(new FlirtSparkStrategy());
                        await bot.ExecuteStrategyAsync(botClient, chatId, state, null, cancellationToken);
                        await botClient.AnswerCallbackQueryAsync(callbackQuery.Id, "Лови искру флирта!");
                        break;

                    case "quiz":
                        state.Step = -1;
                        state.QuizAnswers = new List<string>();
                        state.QuizQuestionIndex = 0;
                        QuizContexts[chatId] = new QuizContext(new QuestionState());
                        await QuizContexts[chatId].Request(botClient, chatId, state, cancellationToken);
                        await botClient.AnswerCallbackQueryAsync(callbackQuery.Id);
                        break;

                    case "task":
                        state.Step = 0;
                        var invoker = new TaskInvoker();
                        invoker.SetCommand(new RandomTask());
                        await invoker.ExecuteCommand(botClient, chatId, cancellationToken);
                        await botClient.AnswerCallbackQueryAsync(callbackQuery.Id, "Вот тебе задание!");
                        break;

                    case "chat":
                        if (!UserCharacters.ContainsKey(chatId))
                            UserCharacters[chatId] = new Character();
                        var character = UserCharacters[chatId];
                        var mediator = new ChatMediator(character);
                        state.IsChatStarted = true;
                        state.Step = -2;
                        await mediator.SendMessage("Привет! Чем могу очаровать тебя сегодня?", botClient, chatId, cancellationToken);
                        await botClient.SendTextMessageAsync(chatId, "Ты в чате! Пиши что угодно, а для выхода скажи 'пока'.", cancellationToken: cancellationToken);
                        await botClient.AnswerCallbackQueryAsync(callbackQuery.Id, "Чат начался!");
                        break;

                    case "quote":
                        var quotes = new[] { "Любовь — это когда хочешь состариться с кем-то одним.", "Настоящая любовь не имеет срока годности." };
                        var random = new Random();
                        await botClient.SendTextMessageAsync(chatId, quotes[random.Next(quotes.Length)], cancellationToken: cancellationToken);
                        await botClient.AnswerCallbackQueryAsync(callbackQuery.Id, "Вот тебе романтическая цитата!");
                        break;

                    case "dateidea":
                        state.Step = 10;
                        state.DateIdeaLocation = null;
                        state.DateIdeaCity = null;
                        await botClient.EditMessageTextAsync(chatId, callbackQuery.Message.MessageId,
                            "Где ты хочешь провести свидание? Дома или на улице?",
                            replyMarkup: GetLocationInline());
                        await botClient.AnswerCallbackQueryAsync(callbackQuery.Id);
                        break;

                    case "goals":
                        state.GoalStep = 0;
                        await botClient.EditMessageTextAsync(chatId, callbackQuery.Message.MessageId,
                            "Добро пожаловать в управление целями! Что хочешь сделать?",
                            replyMarkup: GetGoalsMainMenu());
                        await botClient.AnswerCallbackQueryAsync(callbackQuery.Id);
                        break;

                    case "goal_add":
                        state.GoalStep = 1;
                        state.CurrentGoalMonth = DateTime.Today;
                        await botClient.EditMessageTextAsync(chatId, callbackQuery.Message.MessageId,
                            "Выбери день для новой цели:",
                            replyMarkup: GetGoalCalendarInline(state.CurrentGoalMonth));
                        await botClient.AnswerCallbackQueryAsync(callbackQuery.Id);
                        break;

                    case "goal_view":
                        if (state.Goals.Any())
                        {
                            var goalsText = "Твои цели:\n\n" + string.Join("\n\n", state.Goals.Select((g, i) => $"{i + 1}. {g}"));
                            await botClient.EditMessageTextAsync(chatId, callbackQuery.Message.MessageId,
                                goalsText,
                                replyMarkup: GetGoalsMainMenu());
                        }
                        else
                        {
                            await botClient.EditMessageTextAsync(chatId, callbackQuery.Message.MessageId,
                                "У тебя пока нет целей. Добавим?",
                                replyMarkup: GetGoalsMainMenu());
                        }
                        await botClient.AnswerCallbackQueryAsync(callbackQuery.Id);
                        break;

                    case "goal_complete":
                        if (state.Goals.Any())
                        {
                            var buttons = state.Goals.Select((g, i) =>
                                new[] { InlineKeyboardButton.WithCallbackData($"{i + 1}. {g.Description}", $"complete_{i}") }).ToArray();
                            var keyboard = new InlineKeyboardMarkup(buttons);
                            await botClient.EditMessageTextAsync(chatId, callbackQuery.Message.MessageId,
                                "Выбери цель для отметки как выполненной:",
                                replyMarkup: keyboard);
                        }
                        else
                        {
                            await botClient.EditMessageTextAsync(chatId, callbackQuery.Message.MessageId,
                                "У тебя пока нет целей для завершения.",
                                replyMarkup: GetGoalsMainMenu());
                        }
                        await botClient.AnswerCallbackQueryAsync(callbackQuery.Id);
                        break;

                    case "goal_clone":
                        state.GoalStep = 3;
                        state.CurrentGoalMonth = DateTime.Today;
                        await botClient.EditMessageTextAsync(chatId, callbackQuery.Message.MessageId,
                            "Выбери день, с которого клонировать цели:",
                            replyMarkup: GetGoalCalendarInline(state.CurrentGoalMonth));
                        await botClient.AnswerCallbackQueryAsync(callbackQuery.Id);
                        break;

                    case "back_to_main":
                        state.GoalStep = 0;
                        state.TempGoalDate = DateTime.MinValue;
                        state.CloneSourceDate = null;
                        await botClient.EditMessageTextAsync(chatId, callbackQuery.Message.MessageId,
                            "Чем могу помочь?",
                            replyMarkup: GetStartMenu());
                        await botClient.AnswerCallbackQueryAsync(callbackQuery.Id);
                        break;

                    default:
                        switch (state.Step)
                        {
                            case 0:
                                if (data.StartsWith("month_"))
                                {
                                    state.CurrentMonth = DateTime.ParseExact(data.Split('_')[1], "yyyy-MM", null);
                                    await botClient.EditMessageTextAsync(chatId, callbackQuery.Message.MessageId,
                                        "Выбери день:", replyMarkup: GetCalendarInline(state.CurrentMonth));
                                    await botClient.AnswerCallbackQueryAsync(callbackQuery.Id);
                                }
                                else if (int.TryParse(data, out int day))
                                {
                                    state.EventDate = new DateTime(state.CurrentMonth.Year, state.CurrentMonth.Month, day);
                                    state.Step = 1;
                                    await botClient.EditMessageTextAsync(chatId, callbackQuery.Message.MessageId,
                                        $"Дата: {state.EventDate:dd.MM.yyyy}\nВыбери время:", replyMarkup: GetTimeInline());
                                    await botClient.AnswerCallbackQueryAsync(callbackQuery.Id);
                                }
                                break;

                            case 1:
                                state.EventTime = data;
                                state.Step = 2;
                                await botClient.EditMessageTextAsync(chatId, callbackQuery.Message.MessageId,
                                    $"Дата: {state.EventDate:dd.MM.yyyy}\nВремя: {state.EventTime}\nВыбери событие:",
                                    replyMarkup: GetEventInline());
                                await botClient.AnswerCallbackQueryAsync(callbackQuery.Id);
                                break;

                            case 2:
                                if (data == "custom")
                                {
                                    await botClient.SendTextMessageAsync(chatId, "Введи свое название события:");
                                    await botClient.DeleteMessageAsync(chatId, callbackQuery.Message.MessageId);
                                    await botClient.AnswerCallbackQueryAsync(callbackQuery.Id);
                                }
                                else
                                {
                                    state.EventName = data;
                                    state.Step = 3;
                                    await SendConfirmation(botClient, chatId, state, cancellationToken);
                                    await botClient.AnswerCallbackQueryAsync(callbackQuery.Id);
                                }
                                break;

                            case 3:
                                if (data == "confirm")
                                {
                                    if (!(bot.CurrentStrategy is EventTimer))
                                    {
                                        bot.SetStrategy(new EventTimer());
                                    }
                                    var user = new ConcreteObserver(chatId, botClient);
                                    ((EventTimer)bot.CurrentStrategy).Attach(user);
                                    await bot.ExecuteStrategyAsync(botClient, chatId, state, null, cancellationToken);
                                    state.Step = 4;
                                    await botClient.AnswerCallbackQueryAsync(callbackQuery.Id);
                                }
                                else if (data == "edit")
                                {
                                    state.Step = 0;
                                    await SendCalendar(botClient, chatId, state.CurrentMonth, cancellationToken);
                                    await botClient.AnswerCallbackQueryAsync(callbackQuery.Id);
                                }
                                break;

                            case 4:
                                if (data == "mytime")
                                {
                                    if (state.EventDate.HasValue && state.EventTime != null && state.EventName != null)
                                    {
                                        if (bot.CurrentStrategy is EventTimer timer)
                                        {
                                            await timer.Notify();
                                            await botClient.AnswerCallbackQueryAsync(callbackQuery.Id);
                                        }
                                        else
                                        {
                                            bot.SetStrategy(new EventTimer());
                                            var user = new ConcreteObserver(chatId, botClient);
                                            ((EventTimer)bot.CurrentStrategy).Attach(user);
                                            await bot.ExecuteStrategyAsync(botClient, chatId, state, null, cancellationToken);
                                            await ((EventTimer)bot.CurrentStrategy).Notify();
                                            await botClient.AnswerCallbackQueryAsync(callbackQuery.Id);
                                        }
                                    }
                                    else
                                    {
                                        await botClient.SendTextMessageAsync(chatId,
                                            "Сначала установите таймер с помощью 'Таймер любви ⏳'",
                                            cancellationToken: cancellationToken);
                                        await botClient.AnswerCallbackQueryAsync(callbackQuery.Id);
                                    }
                                }
                                break;

                            case 10:
                                if (data == "home" || data == "outdoor")
                                {
                                    state.DateIdeaLocation = data;
                                    state.Step = 11;
                                    await botClient.EditMessageTextAsync(chatId, callbackQuery.Message.MessageId,
                                        "В каком городе ты находишься? Напиши название (например, Moscow, London).",
                                        replyMarkup: null);
                                    await botClient.AnswerCallbackQueryAsync(callbackQuery.Id);
                                }
                                break;
                        }

                        // Обработка callback для целей
                        if (data.StartsWith("goal_month_"))
                        {
                            state.CurrentGoalMonth = DateTime.ParseExact(data.Split('_')[2], "yyyy-MM", null);
                            string message = state.GoalStep == 1 ? "Выбери день для новой цели:" : "Выбери день, с которого клонировать цели:";
                            await botClient.EditMessageTextAsync(chatId, callbackQuery.Message.MessageId,
                                message, replyMarkup: GetGoalCalendarInline(state.CurrentGoalMonth));
                            await botClient.AnswerCallbackQueryAsync(callbackQuery.Id);
                        }
                        else if (state.GoalStep == 1 && int.TryParse(data, out int goalDay))
                        {
                            state.TempGoalDate = new DateTime(state.CurrentGoalMonth.Year, state.CurrentGoalMonth.Month, goalDay);
                            await botClient.EditMessageTextAsync(chatId, callbackQuery.Message.MessageId,
                                $"Дата цели: {state.TempGoalDate:dd.MM.yyyy}\nВыбери категорию:",
                                replyMarkup: GetGoalCategories());
                            await botClient.AnswerCallbackQueryAsync(callbackQuery.Id);
                        }
                        else if (state.GoalStep == 1 && new[] { "Работа", "Личное", "Здоровье", "Общее" }.Contains(data))
                        {
                            state.TempCategory = data;
                            state.GoalStep = 2;
                            await botClient.EditMessageTextAsync(chatId, callbackQuery.Message.MessageId,
                                $"Дата: {state.TempGoalDate:dd.MM.yyyy}\nКатегория: {data}\nНапиши описание цели:");
                            await botClient.AnswerCallbackQueryAsync(callbackQuery.Id);
                        }
                        else if (state.GoalStep == 3 && int.TryParse(data, out int cloneDay))
                        {
                            state.CloneSourceDate = new DateTime(state.CurrentGoalMonth.Year, state.CurrentGoalMonth.Month, cloneDay);
                            state.GoalStep = 4;
                            await botClient.EditMessageTextAsync(chatId, callbackQuery.Message.MessageId,
                                $"Клонировать цели с {state.CloneSourceDate:dd.MM.yyyy}\nВыбери день для клонирования:",
                                replyMarkup: GetGoalCalendarInline(state.CurrentGoalMonth));
                            await botClient.AnswerCallbackQueryAsync(callbackQuery.Id);
                        }
                        else if (state.GoalStep == 4 && int.TryParse(data, out int targetDay))
                        {
                            DateTime targetDate = new DateTime(state.CurrentGoalMonth.Year, state.CurrentGoalMonth.Month, targetDay);
                            var sourceGoals = state.Goals.Where(g => g.Date.Date == state.CloneSourceDate.Value.Date).ToList();
                            foreach (var goal in sourceGoals)
                            {
                                var clonedGoal = goal.CreateModifiedClone(goal.Description, targetDate, goal.Category);
                                state.Goals.Add(clonedGoal);
                            }
                            state.GoalStep = 0;
                            state.CloneSourceDate = null;
                            await botClient.EditMessageTextAsync(chatId, callbackQuery.Message.MessageId,
                                $"Склонировано {sourceGoals.Count} целей на {targetDate:dd.MM.yyyy}!\nЧто дальше?",
                                replyMarkup: GetGoalsMainMenu());
                            await botClient.AnswerCallbackQueryAsync(callbackQuery.Id);
                        }
                        else if (data.StartsWith("complete_"))
                        {
                            int index = int.Parse(data.Split('_')[1]);
                            if (index >= 0 && index < state.Goals.Count)
                            {
                                state.Goals[index].IsCompleted = true;
                                await botClient.EditMessageTextAsync(chatId, callbackQuery.Message.MessageId,
                                    $"Цель '{state.Goals[index].Description}' отмечена как выполненная! ✅\nЧто дальше?",
                                    replyMarkup: GetGoalsMainMenu());
                            }
                            await botClient.AnswerCallbackQueryAsync(callbackQuery.Id);
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                await botClient.SendTextMessageAsync(chatId, $"Ошибка: {ex.Message}", cancellationToken: cancellationToken);
                await botClient.AnswerCallbackQueryAsync(callbackQuery.Id, "Произошла ошибка!");
            }
        }

        private static async Task SendChatReminder(ITelegramBotClient botClient, long chatId, UserState state, CancellationToken cancellationToken)
        {
            if (state.IsChatStarted && UserCharacters.ContainsKey(chatId))
            {
                await botClient.SendTextMessageAsync(chatId, "Ты всё ещё в чате! Продолжай писать или скажи 'пока' для выхода.", cancellationToken: cancellationToken);
            }
        }

        private static InlineKeyboardMarkup GetStartMenu()
        {
            return new InlineKeyboardMarkup(new[]
            {
                new[] { InlineKeyboardButton.WithCallbackData("Таймер любви ⏳", "timer") },
                new[] { InlineKeyboardButton.WithCallbackData("Осталось времени на таймере ⏳", "mytime") },
                new[] { InlineKeyboardButton.WithCallbackData("Комплимент 💌", "compliment"), InlineKeyboardButton.WithCallbackData("Флирт 😘", "flirt") },
                new[] { InlineKeyboardButton.WithCallbackData("Викторина о любви ❓", "quiz"), InlineKeyboardButton.WithCallbackData("Задание 💡", "task") },
                new[] { InlineKeyboardButton.WithCallbackData("Чат с романтиком 💬", "chat"), InlineKeyboardButton.WithCallbackData("Романтическая цитата 📜", "quote") },
                new[] { InlineKeyboardButton.WithCallbackData("Идея для свидания 🌹", "dateidea") },
                new[] { InlineKeyboardButton.WithCallbackData("Мои цели 🎯", "goals") }
            });
        }

        private static InlineKeyboardMarkup GetLocationInline()
        {
            return new InlineKeyboardMarkup(new[]
            {
                new[] { InlineKeyboardButton.WithCallbackData("Дома 🏡", "home"), InlineKeyboardButton.WithCallbackData("На улице 🌳", "outdoor") }
            });
        }

        private static InlineKeyboardMarkup GetGoalsMainMenu()
        {
            return new InlineKeyboardMarkup(new[]
            {
                new[] { InlineKeyboardButton.WithCallbackData("Добавить цель ➕", "goal_add") },
                new[] { InlineKeyboardButton.WithCallbackData("Посмотреть цели 📋", "goal_view") },
                new[] { InlineKeyboardButton.WithCallbackData("Отметить выполненной ✅", "goal_complete") },
                new[] { InlineKeyboardButton.WithCallbackData("Клонировать цели 📑", "goal_clone") },
                new[] { InlineKeyboardButton.WithCallbackData("Назад в главное меню ⬅️", "back_to_main") }
            });
        }

        private static InlineKeyboardMarkup GetGoalCategories()
        {
            return new InlineKeyboardMarkup(new[]
            {
                new[] { InlineKeyboardButton.WithCallbackData("Работа 💼", "Работа"), InlineKeyboardButton.WithCallbackData("Личное 🌟", "Личное") },
                new[] { InlineKeyboardButton.WithCallbackData("Здоровье 🏃", "Здоровье"), InlineKeyboardButton.WithCallbackData("Общее 📌", "Общее") }
            });
        }

        private static InlineKeyboardMarkup GetGoalCalendarInline(DateTime month)
        {
            var daysInMonth = DateTime.DaysInMonth(month.Year, month.Month);
            var buttons = new List<List<InlineKeyboardButton>>();
            var week = new List<InlineKeyboardButton>();

            for (int day = 1; day <= daysInMonth; day++)
            {
                week.Add(InlineKeyboardButton.WithCallbackData(day.ToString(), day.ToString()));
                if (week.Count == 7 || day == daysInMonth)
                {
                    buttons.Add(new List<InlineKeyboardButton>(week));
                    week.Clear();
                }
            }

            buttons.Add(new List<InlineKeyboardButton>
            {
                InlineKeyboardButton.WithCallbackData("◄", $"goal_month_{month.AddMonths(-1):yyyy-MM}"),
                InlineKeyboardButton.WithCallbackData($"{month:MMMM yyyy}", "noop"),
                InlineKeyboardButton.WithCallbackData("►", $"goal_month_{month.AddMonths(1):yyyy-MM}")
            });

            return new InlineKeyboardMarkup(buttons);
        }

        private static async Task SendConfirmation(ITelegramBotClient botClient, long chatId, UserState state, CancellationToken cancellationToken)
        {
            var targetTime = state.EventDate!.Value + TimeSpan.Parse(state.EventTime!);
            var message = $"Твое событие:\n" +
                          $"Дата: {state.EventDate:dd.MM.yyyy}\n" +
                          $"Время: {state.EventTime}\n" +
                          $"Название: {state.EventName}\n" +
                          "Все верно?";
            await botClient.SendTextMessageAsync(chatId, message, replyMarkup: GetConfirmationInline(), cancellationToken: cancellationToken);
        }

        private static async Task SendCalendar(ITelegramBotClient botClient, long chatId, DateTime month, CancellationToken cancellationToken)
        {
            await botClient.SendTextMessageAsync(chatId, "Выбери день:", replyMarkup: GetCalendarInline(month), cancellationToken: cancellationToken);
        }

        private static InlineKeyboardMarkup GetCalendarInline(DateTime month)
        {
            var daysInMonth = DateTime.DaysInMonth(month.Year, month.Month);
            var buttons = new List<List<InlineKeyboardButton>>();
            var week = new List<InlineKeyboardButton>();

            for (int day = 1; day <= daysInMonth; day++)
            {
                week.Add(InlineKeyboardButton.WithCallbackData(day.ToString(), day.ToString()));
                if (week.Count == 7 || day == daysInMonth)
                {
                    buttons.Add(new List<InlineKeyboardButton>(week));
                    week.Clear();
                }
            }

            buttons.Add(new List<InlineKeyboardButton>
            {
                InlineKeyboardButton.WithCallbackData("◄", $"month_{month.AddMonths(-1):yyyy-MM}"),
                InlineKeyboardButton.WithCallbackData($"{month:MMMM yyyy}", "noop"),
                InlineKeyboardButton.WithCallbackData("►", $"month_{month.AddMonths(1):yyyy-MM}")
            });

            return new InlineKeyboardMarkup(buttons);
        }

        private static InlineKeyboardMarkup GetTimeInline()
        {
            var times = new[] { "00:00", "06:00", "09:00", "12:00", "15:00", "18:00", "20:00", "22:00" };
            var buttons = new List<List<InlineKeyboardButton>>();

            for (int i = 0; i < times.Length; i += 4)
            {
                var row = new List<InlineKeyboardButton>();
                for (int j = i; j < i + 4 && j < times.Length; j++)
                    row.Add(InlineKeyboardButton.WithCallbackData(times[j], times[j]));
                buttons.Add(row);
            }

            return new InlineKeyboardMarkup(buttons);
        }

        private static InlineKeyboardMarkup GetEventInline()
        {
            return new InlineKeyboardMarkup(new[]
            {
                new[] { InlineKeyboardButton.WithCallbackData("Свидание 💕", "Свидание"), InlineKeyboardButton.WithCallbackData("Годовщина 🎉", "Годовщина") },
                new[] { InlineKeyboardButton.WithCallbackData("Ужин при свечах 🕯️", "Ужин при свечах"), InlineKeyboardButton.WithCallbackData("Прогулка 🌙", "Прогулка") },
                new[] { InlineKeyboardButton.WithCallbackData("Свое название ✍️", "custom") }
            });
        }

        private static InlineKeyboardMarkup GetConfirmationInline()
        {
            return new InlineKeyboardMarkup(new[]
            {
                new[] { InlineKeyboardButton.WithCallbackData("Подтвердить ✅", "confirm"), InlineKeyboardButton.WithCallbackData("Изменить ✏️", "edit") }
            });
        }

        private static Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            Console.WriteLine($"Ошибка: {exception.Message}");
            return Task.CompletedTask;
        }
    }
}