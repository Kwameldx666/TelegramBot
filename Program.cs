using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Extensions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using static TelegramBot.Strategy.RomanticBotContext;
using Telegram.Bot.Polling;
using TelegramBot.Models;
using TelegramBot.Strategy;
using TelegramBot.Observer;
using TelegramBot.Observer.Intefaces;
using Microsoft.AspNetCore.Hosting.Server;

namespace RomanticTimerBot
{
    class Program
    {
        private static readonly TelegramBotClient Bot = new TelegramBotClient("7748670664:AAHNZJ8e22ptr-ZlX5NgKQtfeGY-vKhVGvo");
        private static readonly Dictionary<long, UserState> UserStates = new Dictionary<long, UserState>();
        private static readonly Dictionary<long, RomanticBot> UserBots = new Dictionary<long, RomanticBot>();

        static async Task Main()
        {
            Bot.StartReceiving(HandleUpdateAsync, HandleErrorAsync, new ReceiverOptions { AllowedUpdates = new[] { UpdateType.Message, UpdateType.CallbackQuery } });
            var me = await Bot.GetMeAsync();
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
            var text = message.Text;

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

                    await botClient.SendTextMessageAsync(chatId,
                        "Привет! Я твой романтический помощник. Чем могу помочь?",
                        replyMarkup: GetStartMenu(),
                        cancellationToken: cancellationToken);
                    break;

                case "/compliment":
                    bot.SetStrategy(new ComplimentGeneratorStrategy());
                    await bot.ExecuteStrategyAsync(botClient, chatId, state, null, cancellationToken);
                    break;

                case "/flirt":
                    bot.SetStrategy(new FlirtSparkStrategy());
                    await bot.ExecuteStrategyAsync(botClient, chatId, state, null, cancellationToken);
                    break;

                default:
                    if (state.Step == 2 && text != null) // Ручной ввод названия события
                    {
                        state.EventName = text;
                        state.Step = 3;
                        await SendConfirmation(botClient, chatId, state, cancellationToken);
                    }
                    break;
            }
        }

        private static async Task HandleCallbackQueryAsync(ITelegramBotClient botClient, CallbackQuery callbackQuery, CancellationToken cancellationToken)
        {
            var chatId = callbackQuery.Message.Chat.Id;
            var data = callbackQuery.Data;
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
                        await botClient.EditMessageText(chatId, callbackQuery.Message.MessageId,
                            "Выбери день:", replyMarkup: GetCalendarInline(state.CurrentMonth));
                        await botClient.AnswerCallbackQuery(callbackQuery.Id);
                        break;

                    case "compliment":
                        bot.SetStrategy(new ComplimentGeneratorStrategy());
                        await bot.ExecuteStrategyAsync(botClient, chatId, state, null, cancellationToken);
                        await botClient.AnswerCallbackQuery(callbackQuery.Id, "Вот тебе комплимент!");
                        break;

                    case "flirt":
                        bot.SetStrategy(new FlirtSparkStrategy());
                        await bot.ExecuteStrategyAsync(botClient, chatId, state, null, cancellationToken);
                        await botClient.AnswerCallbackQuery(callbackQuery.Id, "Лови искру флирта!");
                        break;

                    default:
                        switch (state.Step)
                        {
                            case 0:
                                if (data.StartsWith("month_"))
                                {
                                    state.CurrentMonth = DateTime.ParseExact(data.Split('_')[1], "yyyy-MM", null);
                                    await botClient.EditMessageText(chatId, callbackQuery.Message.MessageId,
                                        "Выбери день:", replyMarkup: GetCalendarInline(state.CurrentMonth));
                                    await botClient.AnswerCallbackQuery(callbackQuery.Id);
                                }
                                else if (int.TryParse(data, out int day))
                                {
                                    state.EventDate = new DateTime(state.CurrentMonth.Year, state.CurrentMonth.Month, day);
                                    state.Step = 1;
                                    await botClient.EditMessageText(chatId, callbackQuery.Message.MessageId,
                                        $"Дата: {state.EventDate:dd.MM.yyyy}\nВыбери время:", replyMarkup: GetTimeInline());
                                    await botClient.AnswerCallbackQuery(callbackQuery.Id);
                                }
                                break;

                            case 1:
                                state.EventTime = data;
                                state.Step = 2;
                                await botClient.EditMessageText(chatId, callbackQuery.Message.MessageId,
                                    $"Дата: {state.EventDate:dd.MM.yyyy}\nВремя: {state.EventTime}\nВыбери событие:",
                                    replyMarkup: GetEventInline());
                                await botClient.AnswerCallbackQuery(callbackQuery.Id);
                                break;

                            case 2:
                                if (data == "custom")
                                {
                                    await botClient.SendTextMessageAsync(chatId, "Введи свое название события:");
                                    await botClient.DeleteMessage(chatId, callbackQuery.Message.MessageId);
                                    await botClient.AnswerCallbackQuery(callbackQuery.Id);
                                }
                                else
                                {
                                    state.EventName = data;
                                    state.Step = 3;
                                    await SendConfirmation(botClient, chatId, state, cancellationToken);
                                    await botClient.AnswerCallbackQuery(callbackQuery.Id);
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
                                    Console.WriteLine($"Chat {chatId}: Таймер установлен, Step = {state.Step}");
                                    await botClient.AnswerCallbackQuery(callbackQuery.Id);
                                }
                                else if (data == "edit")
                                {
                                    state.Step = 0;
                                    await SendCalendar(botClient, chatId, state.CurrentMonth, cancellationToken);
                                    await botClient.AnswerCallbackQuery(callbackQuery.Id);
                                }
                                break;

                            case 4:
                                Console.WriteLine($"Chat {chatId}: 'mytime' вызван, Step = {state.Step}");
                                if (state.EventDate.HasValue && state.EventTime != null && state.EventName != null)
                                {
                                    Console.WriteLine($"Chat {chatId}: Данные таймера: {state.EventDate:dd.MM.yyyy}, {state.EventTime}, {state.EventName}");
                                    if (bot.CurrentStrategy is EventTimer timer)
                                    {
                                        Console.WriteLine($"Chat {chatId}: Существующий таймер найден");
                                        await timer.Notify(); // Просто уведомляем существующих наблюдателей
                                        Console.WriteLine($"Chat {chatId}: Notify выполнен для существующего таймера");
                                        await botClient.AnswerCallbackQuery(callbackQuery.Id);
                                    }
                                    else
                                    {

                                        bot.SetStrategy(new EventTimer());
                                        var user = new ConcreteObserver(chatId, botClient);
                                        ((EventTimer)bot.CurrentStrategy).Attach(user);
                                        Console.WriteLine($"Chat {chatId}: Новый таймер создан, наблюдатель присоединен");
                                        await bot.ExecuteStrategyAsync(botClient, chatId, state, null, cancellationToken);
                                        await ((EventTimer)bot.CurrentStrategy).Notify();
                                        Console.WriteLine($"Chat {chatId}: Notify выполнен для нового таймера");
                                        await botClient.AnswerCallbackQuery(callbackQuery.Id);
                                    }
                                }
                                else
                                {
                                    await botClient.SendTextMessageAsync(chatId,
                                        "Сначала установите таймер с помощью 'Таймер любви ⏳'",
                                        cancellationToken: cancellationToken);
                                    Console.WriteLine($"Chat {chatId}: 'mytime' вызван, но таймер не установлен");
                                    await botClient.AnswerCallbackQuery(callbackQuery.Id);
                                }
                                break;
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                await botClient.SendTextMessageAsync(chatId, $"Ошибка: {ex.Message}", cancellationToken: cancellationToken);
                await botClient.AnswerCallbackQuery(callbackQuery.Id, "Произошла ошибка!");
            }
        }

        private static InlineKeyboardMarkup GetStartMenu()
        {
            return new InlineKeyboardMarkup(new[]
            {
                new[] { InlineKeyboardButton.WithCallbackData("Таймер любви ⏳", "timer") },
                new[] { InlineKeyboardButton.WithCallbackData("Осталось времени на таймере ⏳", "mytime") },
                new[] { InlineKeyboardButton.WithCallbackData("Комплимент 💌", "compliment"), InlineKeyboardButton.WithCallbackData("Флирт 😘", "flirt") }
            });
        }

        private static async Task SendConfirmation(ITelegramBotClient botClient, long chatId, UserState state, CancellationToken cancellationToken)
        {
            var targetTime = state.EventDate.Value + TimeSpan.Parse(state.EventTime);
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
            var times = new[] { "00:00", "01:00", "02:00", "03:00", "04:00", "05:00", "06:00", "07:00", "08:00", "09:00", "10:00", "11:00", "12:00", "13:00", "14:00", "15:00", "16:00", "17:00", "18:00", "19:00", "20:00", "21:00", "22:00", "23:00" };
            var buttons = new List<List<InlineKeyboardButton>>();

            for (int i = 0; i < times.Length; i += 3)
            {
                var row = new List<InlineKeyboardButton>();
                for (int j = i; j < i + 3 && j < times.Length; j++)
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