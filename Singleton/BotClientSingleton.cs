using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;

namespace TelegramBot.Singleton
{
    public sealed class BotClientSingleton
    {
        private static readonly Lazy<BotClientSingleton> _instance = new(() => new BotClientSingleton());
        public TelegramBotClient Client { get; }

        private BotClientSingleton()
        {
            Client = new TelegramBotClient("7581339703:AAHEAQ1NhPVL4UYcmFYTBrbwdqTx_pmwE7A"); 
        }

        public static BotClientSingleton Instance => _instance.Value;
    }
}
