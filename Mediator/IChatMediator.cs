using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;

namespace TelegramBot.Mediator
{
    public interface IChatMediator
    {
        Task SendMessage(string message, ITelegramBotClient botClient, long chatId, CancellationToken cancellationToken);
    }
}
