using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;

namespace TelegramBot.Command
{
    public class SurpriseTask : ICommand
    {
        public async Task Execute(ITelegramBotClient botClient, long chatId, CancellationToken cancellationToken)
        {
            await botClient.SendTextMessageAsync(chatId, "Сделайте сюрприз: оставьте милую записку или маленький подарок в неожиданном месте.", cancellationToken: cancellationToken);
        }
    }
}
