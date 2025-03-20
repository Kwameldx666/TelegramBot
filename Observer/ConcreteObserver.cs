using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using TelegramBot.Factory_Method;
using TelegramBot.Observer.Intefaces;

namespace TelegramBot.Observer
{
    public class ConcreteObserver : IObserver
    {
        private readonly long _chatId;
        private readonly ITelegramBotClient _botClient;

        public ConcreteObserver(long chatId, ITelegramBotClient botClient)
        {
            _chatId = chatId;
            _botClient = botClient;
        }

        public async Task Update(string message)
        {
            try
            {
                await _botClient.SendTextMessageAsync(_chatId, message, replyMarkup: Program.GetStartMenu(), cancellationToken: CancellationToken.None);
                Console.WriteLine($"Chat {_chatId}: Отправлено сообщение: {message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Chat {_chatId}: Ошибка отправки: {ex.Message}");
            }
        }
    }

}
