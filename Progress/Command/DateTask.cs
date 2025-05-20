using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;

namespace TelegramBot.Progress.Command
{
    //Concrete comand 
    public class DateTask(TelegramMessageSender _sender) : ICommand
    {
        public async Task Execute(long chatId, CancellationToken cancellationToken)
        {
            await _sender.SendMessage(chatId, "Организуйте свидание: пикник под звёздами или уютный вечер с фильмом.", cancellationToken: cancellationToken);
        }
    }
}
