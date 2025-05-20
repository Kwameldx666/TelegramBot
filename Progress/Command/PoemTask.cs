using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;

namespace TelegramBot.Progress.Command
{
    //Concrete comand 
    public class PoemTask : ICommand
    {
        private readonly TelegramMessageSender _sender;

        public PoemTask(TelegramMessageSender sender)
        {
            _sender = sender;
        }
        public async Task Execute(long chatId, CancellationToken cancellationToken)
        {
            await _sender.SendMessage(chatId, "Напишите короткое стихотворение о любви и отправьте его своему партнёру!", cancellationToken);
        }
    }
}
