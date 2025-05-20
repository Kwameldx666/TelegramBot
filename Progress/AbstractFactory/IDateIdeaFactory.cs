using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelegramBot.Progress.AbstractFactory.AbstractImplements;

namespace TelegramBot.Progress.AbstractFactory
{
    public interface IDateIdeaFactory
    {
        IDateIdeaGenerator CreateSunnyDateIdeaGenerator();
        IDateIdeaGenerator CreateRainyDateIdeaGenerator();
        IDateIdeaGenerator CreateColdDateIdeaGenerator();
    }
}
