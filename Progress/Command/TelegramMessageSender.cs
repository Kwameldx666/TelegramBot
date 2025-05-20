using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;

namespace TelegramBot.Progress.Command
{
    //Receiver(получатель.Реальный испоонитель)
    public class TelegramMessageSender
    {
        private readonly ITelegramBotClient _botClient;

        public TelegramMessageSender(ITelegramBotClient botClient)
        {
            _botClient = botClient;
        }

        public async Task SendMessage(long chatId, string message, CancellationToken cancellationToken)
        {
            await _botClient.SendTextMessageAsync(chatId, message, cancellationToken: cancellationToken);
        }
    }
}
