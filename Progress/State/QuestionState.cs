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
    public class QuestionState : IQuizState
    {
        public static readonly string[] Questions = {
                "Что важнее в любви? 1) Доверие 2) Страсть 3) Поддержка",
                "Как вы выражаете чувства? 1) Слова 2) Действия 3) Подарки",
                "Что вдохновляет в отношениях? 1) Общие цели 2) Романтика 3) Спонтанность"
            };

        public async Task Handle(QuizContext context, ITelegramBotClient botClient, long chatId, UserState state, CancellationToken cancellationToken)
        {
            if (state.QuizQuestionIndex < Questions.Length)
            {
                var text = botClient.GetUpdates().Result.Last().Message?.Text;
                if (text == "1" || text == "2" || text == "3")
                {
                    state.QuizAnswers!.Add(text);
                    state.QuizQuestionIndex++;
                }

                if (state.QuizQuestionIndex < Questions.Length)
                {
                    await botClient.SendTextMessageAsync(chatId, Questions[state.QuizQuestionIndex], cancellationToken: cancellationToken);
                }
                else
                {
                    context.SetState(new ResultState());
                    await context.Request(botClient, chatId, state, cancellationToken);
                }
            }
        }
    }
}
