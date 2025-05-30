﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using TelegramBot.Progress.Models;

namespace TelegramBot.Progress.Strategy
{
    public interface IStrategy
        {
            Task ExecuteAsync(ITelegramBotClient botClient, long chatId, UserState state, string input, CancellationToken cancellationToken);
        }

    
}
