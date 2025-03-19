using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;

namespace TelegramBot.Command
{
    public class PoemTask : ICommand
    {
        public async Task Execute(ITelegramBotClient botClient, long chatId, CancellationToken cancellationToken)
        {
            await botClient.SendTextMessageAsync(chatId, "Напишите короткое стихотворение о любви и отправьте его своему партнёру!", cancellationToken: cancellationToken);
        }
    }
}
