﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramBot.Factory_Method
{
    public interface IDateIdeaGeneratorFactory
    {
        IDateIdeaGenerator CreateGenerator();
    }
}
