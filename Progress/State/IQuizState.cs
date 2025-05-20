using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot;
using TelegramBot.Progress.Models;

namespace TelegramBot.State
{
    public interface IQuizState
    {
        Task Handle(QuizContext context, ITelegramBotClient botClient, long chatId, UserState state, CancellationToken cancellationToken);
    }
}
