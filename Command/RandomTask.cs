using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;

namespace TelegramBot.Command
{
    public class RandomTask : ICommand
    {
        private readonly ICommand[] _tasks = { new PoemTask(), new LetterTask(), new DateTask(), new SurpriseTask() };

        public async Task Execute(ITelegramBotClient botClient, long chatId, CancellationToken cancellationToken)
        {
            var random = new Random();
            await _tasks[random.Next(_tasks.Length)].Execute(botClient, chatId, cancellationToken);
        }
    }
}
