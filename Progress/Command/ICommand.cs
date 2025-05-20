using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;

namespace TelegramBot.Progress.Command
{
    public interface ICommand
    {
        Task Execute(long chatId, CancellationToken cancellationToken);
    }
}
