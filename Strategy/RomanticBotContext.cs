using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;

namespace TelegramBot.Strategy
{
    public class RomanticBotContext
    {
        public class RomanticBot
        {
            private IStrategy _strategy;

            public IStrategy CurrentStrategy => _strategy;

            public void SetStrategy(IStrategy strategy)
            {
                _strategy = strategy;
            }

            public async Task ExecuteStrategyAsync(ITelegramBotClient botClient, long chatId, Models.UserState state, string input, CancellationToken cancellationToken)
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
}
