using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot;
using TelegramBot.Models;

namespace TelegramBot.State
{
    public class ResultState : IQuizState
    {
        public async Task Handle(QuizContext context, ITelegramBotClient botClient, long chatId, UserState state, CancellationToken cancellationToken)
        {
            var answers = state.QuizAnswers!;
            string result = answers switch
            {
                _ when answers.Count(a => a == "1") >= 2 => "Вы прагматичный романтик: цените стабильность и доверие.",
                _ when answers.Count(a => a == "2") >= 2 => "Вы страстный идеалист: любите яркие эмоции и романтику.",
                _ => "Вы спонтанный мечтатель: обожаете сюрпризы и свободу в любви."
            };
            await botClient.SendTextMessageAsync(chatId, $"Ваш тип в любви: {result}", cancellationToken: cancellationToken);
            state.QuizAnswers = null;
            state.QuizQuestionIndex = 0;
        }
    }
}
