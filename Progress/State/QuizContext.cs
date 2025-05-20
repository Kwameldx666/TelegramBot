using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using TelegramBot.Progress.Models;

namespace TelegramBot.State
{
    public class QuizContext
    {
        private IQuizState _state;

        public QuizContext(IQuizState state)
        {
            _state = state;
        }

        public void SetState(IQuizState state)
        {
            _state = state;
        }

        public async Task Request(ITelegramBotClient botClient, long chatId, UserState state, CancellationToken cancellationToken)
        {
            await _state.Handle(this, botClient, chatId, state, cancellationToken);
        }
    }

}