using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;

namespace TelegramBot.Progress.Command
{
    //Concrete comand 
    public class LetterTask : ICommand
    {
        private readonly TelegramMessageSender _sender;

        public LetterTask(TelegramMessageSender sender)
        {
            _sender = sender;
        }
        public async Task Execute( long chatId, CancellationToken cancellationToken)
        {
            await _sender.SendMessage(chatId, "Напишите письмо с признанием в любви и спрячьте его в неожиданном месте.", cancellationToken: cancellationToken);
        }
    }
}
