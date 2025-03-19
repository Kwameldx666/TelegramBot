using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;

namespace TelegramBot.Command
{
    public class DateTask : ICommand
    {
        public async Task Execute(ITelegramBotClient botClient, long chatId, CancellationToken cancellationToken)
        {
            await botClient.SendTextMessageAsync(chatId, "Организуйте свидание: пикник под звёздами или уютный вечер с фильмом.", cancellationToken: cancellationToken);
        }
    }
}
