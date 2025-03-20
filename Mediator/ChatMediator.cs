using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;

namespace TelegramBot.Mediator
{
    public class ChatMediator : IChatMediator
    {
        private readonly Character _character;

        public ChatMediator(Character character) => _character = character;

        public async Task SendMessage(string message, ITelegramBotClient botClient, long chatId, CancellationToken cancellationToken)
        {
            string response = await _character.Respond(message);
            await botClient.SendTextMessageAsync(chatId, response, cancellationToken: cancellationToken);
        }
    }
}
