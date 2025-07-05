using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace TelegramBot.Progress.Singleton
{
    public sealed class BotClientSingleton
    {
        private static readonly Lazy<BotClientSingleton> _instance = new(() => new BotClientSingleton());
        public TelegramBotClient Client { get; }

        private BotClientSingleton()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            var token = configuration["TelegramBot:Token"];
            if (string.IsNullOrEmpty(token))
            {
                throw new InvalidOperationException("Telegram bot token not found in configuration. Please ensure appsettings.json contains TelegramBot:Token setting.");
            }

            Client = new TelegramBotClient(token); 
        }

        public static BotClientSingleton Instance => _instance.Value;
    }
}
