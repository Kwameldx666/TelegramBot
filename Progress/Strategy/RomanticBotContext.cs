using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using TelegramBot.Progress.Models;

namespace TelegramBot.Progress.Strategy
{
    public class RomanticBotContext
    {

            private IStrategy _strategy;

            public IStrategy CurrentStrategy => _strategy;

            public void SetStrategy(IStrategy strategy)
            {
                _strategy = strategy;
            }

            public async Task ExecuteStrategyAsync(ITelegramBotClient botClient, long chatId, UserState state, string input, CancellationToken cancellationToken)
            {
                if (_strategy == null)
                {
                    await botClient.SendTextMessageAsync(chatId,
                        "Выбери действие через меню: 'Таймер любви ⏳', 'Комплимент 💌' или 'Флирт 😘'",
                        cancellationToken: cancellationToken);
                    return;
                }
                await _strategy.ExecuteAsync(botClient, chatId, state, input, cancellationToken);
            }
        }
    
}
