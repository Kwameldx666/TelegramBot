using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;

namespace TelegramBot.Command
{
    public interface ICommand
    {
        Task Execute(ITelegramBotClient botClient, long chatId, CancellationToken cancellationToken);
    }
}
