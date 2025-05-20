using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;

namespace TelegramBot.Progress.Command
{
    //Concrete comand 
    public class SurpriseTask : ICommand
    {
        private readonly TelegramMessageSender _sender;

        public SurpriseTask(TelegramMessageSender sender)
        {
            _sender = sender;
        }
        public async Task Execute(long chatId, CancellationToken cancellationToken)
        {
            await _sender.SendMessage(chatId, "Сделайте сюрприз: оставьте милую записку или маленький подарок в неожиданном месте.", cancellationToken: cancellationToken);
        }
    }
}
