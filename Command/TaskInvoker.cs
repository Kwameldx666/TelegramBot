using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;

namespace TelegramBot.Command
{
    public class TaskInvoker
    {
        private ICommand? _command;

        public void SetCommand(ICommand command) => _command = command;

        public async Task ExecuteCommand(ITelegramBotClient botClient, long chatId, CancellationToken cancellationToken)
        {
            if (_command != null)
                await _command.Execute(botClient, chatId, cancellationToken);
        }
    }
}
