using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;

namespace TelegramBot.Command
{
    public class LetterTask : ICommand
    {
        public async Task Execute(ITelegramBotClient botClient, long chatId, CancellationToken cancellationToken)
        {
            await botClient.SendTextMessageAsync(chatId, "Напишите письмо с признанием в любви и спрячьте его в неожиданном месте.", cancellationToken: cancellationToken);
        }
    }
}
