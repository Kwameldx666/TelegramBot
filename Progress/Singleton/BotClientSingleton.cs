using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;

namespace TelegramBot.Progress.Singleton
{
    public sealed class BotClientSingleton
    {
        private static readonly Lazy<BotClientSingleton> _instance = new(() => new BotClientSingleton());
        public TelegramBotClient Client { get; }

        private BotClientSingleton()
        {
            Client = new TelegramBotClient("7599150748:AAElk8GNCc_Su9w_qZqQihQW06f4mz5Hkww"); 
        }

        public static BotClientSingleton Instance => _instance.Value;
    }
}
